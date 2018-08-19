using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using Vita.Domain.Infrastructure;

namespace Vita.Setup.Util
{
    public class DatabaseUtil
    {
        /// <summary>
        /// Executes the given SQL query
        /// </summary>
        /// <param name="connString">Connection string to the database</param>
        /// <param name="providerName">Db provider invariant name</param>
        /// <param name="query">The SQL query to be executed</param>
        /// <param name="Parameters">Query parameters and their values</param>
        /// <returns>Number of rows affected</returns>
        public static int ExecuteCommand(string connString, string providerName, string query, Dictionary<string, object> Parameters = null)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);

            //Create Query
            using (DbConnection conn = factory.CreateConnection())
            {
                conn.ConnectionString = connString;
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;

                    //Add Parameters
                    if (Parameters != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in Parameters)
                        {
                            DbParameter parameter = factory.CreateParameter();
                            parameter.ParameterName = kvp.Key;
                            parameter.Value = kvp.Value;
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    //Execute Query
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool CheckDatabaseExists(string connectionString, string databaseName)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand($"SELECT db_id('{databaseName}')", connection))
                    {
                        connection.Open();
                        return command.ExecuteScalar() != DBNull.Value;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Read a script file and optionally replace scriptVariables
        /// </summary>
        /// <param name="scriptFileName"></param>
        /// <param name="scriptVarReplacePairs">e.g. "@dbname", "myDbName", "@var2", "replaceVar2", ...</param>
        public static string GetScriptText(string scriptFileName, params string[] scriptVarReplacePairs)
        {
            var scriptText = File.ReadAllText(scriptFileName);
            if (scriptVarReplacePairs != null)
            {
                var n = scriptVarReplacePairs.Length;
                if (n % 2 != 0)
                    throw new ArgumentException($"Length of {nameof(scriptVarReplacePairs)} must be even",
                        nameof(scriptVarReplacePairs));
                for (var i = 0; i < n; i += 2)
                    scriptText = scriptText.Replace(scriptVarReplacePairs[i], scriptVarReplacePairs[i + 1] ?? "");
            }

            return scriptText;
        }

        /// <summary>
        ///     Split script text with multiple GO statements to individual statments
        /// </summary>
        /// <param name="scriptText"></param>
        /// <returns></returns>
        public static string[] ScriptToSqls(string scriptText)
        {
            scriptText = scriptText.Replace("\r", "");
            // reomve comment blocks
            scriptText = new Regex(@"/\*.*?\*/\n", RegexOptions.Singleline).Replace(scriptText, "");
            var queries = scriptText.Split(new[] {"\nGO\n"}, StringSplitOptions.RemoveEmptyEntries);
            return queries;
        }

        public static void DeleteAllTables(string connectionString)
        {
            try
            {
                ExecuteSqlsWithTrans(connectionString, DropTables);
                ExecuteSqlsWithTrans(connectionString, DropViews);
            }
            catch (Exception ex)
            {
                Consoler.Warn(ex.ToString());
            }

            try
            {
                SetupMsSpForEach(connectionString);
                const string sql =
                    @"DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR SET @Cursor = CURSOR FAST_FORWARD FOR SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + ']' FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1 LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql WHILE (@@FETCH_STATUS = 0) BEGIN Exec SP_EXECUTESQL @Sql FETCH NEXT FROM @Cursor INTO @Sql END CLOSE @Cursor DEALLOCATE @Cursor EXEC sp_MSForEachTable 'DROP TABLE ?' ";
                ExecuteSqlsWithTrans(connectionString, sql);
            }
            catch (Exception ex)
            {
                Consoler.Error(ex.ToString());
            }
        }

        //http://stackoverflow.com/questions/11689557/delete-all-views-from-sql-server
        public const string DropViews = @"
DECLARE @sql VARCHAR(MAX) = ''
        , @crlf VARCHAR(2) = CHAR(13) + CHAR(10) ;

SELECT @sql = @sql + 'DROP VIEW ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(v.name) +';' + @crlf
FROM   sys.views v
WHERE SCHEMA_NAME(schema_id) = 'dbo'

EXEC(@sql);
";

        public const string DropTables = @"
DECLARE @sql nvarchar(MAX) 
SET @sql = '' 

SELECT @sql = @sql + 'ALTER TABLE ' + QUOTENAME(RC.CONSTRAINT_SCHEMA) 
    + '.' + QUOTENAME(KCU1.TABLE_NAME) 
    + ' DROP CONSTRAINT ' + QUOTENAME(rc.CONSTRAINT_NAME) + '; ' 
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1 
    ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
    AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
    AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
GROUP BY RC.CONSTRAINT_SCHEMA, KCU1.TABLE_NAME, rc.CONSTRAINT_NAME


-- PRINT @sql 
EXECUTE(@sql) 


DECLARE @sqlDrop nvarchar(max) = '';

SELECT @sqlDrop += 'DROP TABLE ' + QUOTENAME([TABLE_SCHEMA]) + '.' + QUOTENAME([TABLE_NAME]) + ';'
FROM [INFORMATION_SCHEMA].[TABLES]
WHERE [TABLE_TYPE] = 'BASE TABLE';

EXEC SP_EXECUTESQL @sqlDrop;";

        /// <summary>
        ///     Needed as AZURE does not support sp_MSforeachtable  - used to delete all tables etc in the database
        ///     https://gist.github.com/metaskills/893599
        /// </summary>
        /// <param name="connectionString"></param>
        private static void SetupMsSpForEach(string connectionString)
        {
            const string sql1 =
                "IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_MSforeachtable') exec('CREATE PROCEDURE [dbo].[sp_MSforeachtable] AS BEGIN SET NOCOUNT ON; END') ";

            ExecuteSqlsWithTrans(connectionString, sql1);

            const string sql2 =
                @"alter proc [dbo].[sp_MSforeachtable]
	@command1 nvarchar(2000), @replacechar nchar(1) = N'?', @command2 nvarchar(2000) = null,
  @command3 nvarchar(2000) = null, @whereand nvarchar(2000) = null,
	@precommand nvarchar(2000) = null, @postcommand nvarchar(2000) = null
AS
	declare @mscat nvarchar(12)
	select @mscat = ltrim(str(convert(int, 0x0002)))
	if (@precommand is not null)
		exec(@precommand)
   exec(N'declare hCForEachTable cursor global for select ''['' + REPLACE(schema_name(syso.schema_id), N'']'', N'']]'') + '']'' + ''.'' + ''['' + REPLACE(object_name(o.id), N'']'', N'']]'') + '']'' from dbo.sysobjects o join sys.all_objects syso on o.id = syso.object_id '
         + N' where OBJECTPROPERTY(o.id, N''IsUserTable'') = 1 ' + N' and o.category & ' + @mscat + N' = 0 '
         + @whereand)
	declare @retval int
	select @retval = @@error
	if (@retval = 0)
		exec @retval = dbo.sp_MSforeach_worker @command1, @replacechar, @command2, @command3, 0
	if (@retval = 0 and @postcommand is not null)
		exec(@postcommand)
	return @retval
";

            ExecuteSqlsWithTrans(connectionString, sql2);

            const string sql3 =
                @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_MSforeach_worker')
                    exec('CREATE PROCEDURE [dbo].[sp_MSforeach_worker] AS BEGIN SET NOCOUNT ON; END')
               ";

            ExecuteSqlsWithTrans(connectionString, sql3);

            const string sql4 =
                @" alter proc [dbo].[sp_MSforeach_worker]
	@command1 nvarchar(2000), @replacechar nchar(1) = N'?', @command2 nvarchar(2000) = null, @command3 nvarchar(2000) = null, @worker_type int =1
as

	create table #qtemp (	/* Temp command storage */
		qnum				int				NOT NULL,
		qchar				nvarchar(2000)	COLLATE database_default NULL
	)

	set nocount on
	declare @name nvarchar(517), @namelen int, @q1 nvarchar(2000), @q2 nvarchar(2000)
   declare @q3 nvarchar(2000), @q4 nvarchar(2000), @q5 nvarchar(2000)
	declare @q6 nvarchar(2000), @q7 nvarchar(2000), @q8 nvarchar(2000), @q9 nvarchar(2000), @q10 nvarchar(2000)
	declare @cmd nvarchar(2000), @replacecharindex int, @useq tinyint, @usecmd tinyint, @nextcmd nvarchar(2000)
   declare @namesave nvarchar(517), @nametmp nvarchar(517), @nametmp2 nvarchar(258)

	declare @local_cursor cursor
	if @worker_type=1	
		set @local_cursor = hCForEachDatabase
	else
		set @local_cursor = hCForEachTable
	
	open @local_cursor
	fetch @local_cursor into @name

	while (@@fetch_status >= 0) begin

      select @namesave = @name
		select @useq = 1, @usecmd = 1, @cmd = @command1, @namelen = datalength(@name)
		while (@cmd is not null) begin		/* Generate @q* for exec() */
			select @replacecharindex = charindex(@replacechar, @cmd)
			while (@replacecharindex <> 0) begin

            /* 7.0, if name contains ' character, and the name has been single quoted in command, double all of them in dbname */
            /* if the name has not been single quoted in command, do not doulbe them */
            /* if name contains ] character, and the name has been [] quoted in command, double all of ] in dbname */
            select @name = @namesave
            select @namelen = datalength(@name)
            declare @tempindex int
            if (substring(@cmd, @replacecharindex - 1, 1) = N'''') begin
               /* if ? is inside of '', we need to double all the ' in name */
               select @name = REPLACE(@name, N'''', N'''''')
            end else if (substring(@cmd, @replacecharindex - 1, 1) = N'[') begin
               /* if ? is inside of [], we need to double all the ] in name */
               select @name = REPLACE(@name, N']', N']]')
            end else if ((@name LIKE N'%].%]') and (substring(@name, 1, 1) = N'[')) begin
               /* ? is NOT inside of [] nor '', and the name is in [owner].[name] format, handle it */
               /* !!! work around, when using LIKE to find string pattern, can't use '[', since LIKE operator is treating '[' as a wide char */
               select @tempindex = charindex(N'].[', @name)
               select @nametmp  = substring(@name, 2, @tempindex-2 )
               select @nametmp2 = substring(@name, @tempindex+3, len(@name)-@tempindex-3 )
               select @nametmp  = REPLACE(@nametmp, N']', N']]')
               select @nametmp2 = REPLACE(@nametmp2, N']', N']]')
               select @name = N'[' + @nametmp + N'].[' + @nametmp2 + ']'
            end else if ((@name LIKE N'%]') and (substring(@name, 1, 1) = N'[')) begin
               /* ? is NOT inside of [] nor '', and the name is in [name] format, handle it */
               /* j.i.c., since we should not fall into this case */
               /* !!! work around, when using LIKE to find string pattern, can't use '[', since LIKE operator is treating '[' as a wide char */
               select @nametmp = substring(@name, 2, len(@name)-2 )
               select @nametmp = REPLACE(@nametmp, N']', N']]')
               select @name = N'[' + @nametmp + N']'
            end
            /* Get the new length */
            select @namelen = datalength(@name)

            /* start normal process */
				if (datalength(@cmd) + @namelen - 1 > 2000) begin
					/* Overflow; put preceding stuff into the temp table */
					if (@useq > 9) begin
						close @local_cursor
						if @worker_type=1	
							deallocate hCForEachDatabase
						else
							deallocate hCForEachTable
						return 1
					end
					if (@replacecharindex < @namelen) begin
						/* If this happened close to beginning, make sure expansion has enough room. */
						/* In this case no trailing space can occur as the row ends with @name. */
						select @nextcmd = substring(@cmd, 1, @replacecharindex)
						select @cmd = substring(@cmd, @replacecharindex + 1, 2000)
						select @nextcmd = stuff(@nextcmd, @replacecharindex, 1, @name)
						select @replacecharindex = charindex(@replacechar, @cmd)
						insert #qtemp values (@useq, @nextcmd)
						select @useq = @useq + 1
						continue
					end
					/* Move the string down and stuff() in-place. */
					/* Because varchar columns trim trailing spaces, we may need to prepend one to the following string. */
					/* In this case, the char to be replaced is moved over by one. */
					insert #qtemp values (@useq, substring(@cmd, 1, @replacecharindex - 1))
					if (substring(@cmd, @replacecharindex - 1, 1) = N' ') begin
						select @cmd = N' ' + substring(@cmd, @replacecharindex, 2000)
						select @replacecharindex = 2
					end else begin
						select @cmd = substring(@cmd, @replacecharindex, 2000)
						select @replacecharindex = 1
					end
					select @useq = @useq + 1
				end
				select @cmd = stuff(@cmd, @replacecharindex, 1, @name)
				select @replacecharindex = charindex(@replacechar, @cmd)
			end

			/* Done replacing for current @cmd.  Get the next one and see if it's to be appended. */
			select @usecmd = @usecmd + 1
			select @nextcmd = case (@usecmd) when 2 then @command2 when 3 then @command3 else null end
			if (@nextcmd is not null and substring(@nextcmd, 1, 2) = N'++') begin
				insert #qtemp values (@useq, @cmd)
				select @cmd = substring(@nextcmd, 3, 2000), @useq = @useq + 1
				continue
			end

			/* Now exec() the generated @q*, and see if we had more commands to exec().  Continue even if errors. */
			/* Null them first as the no-result-set case won't. */
			select @q1 = null, @q2 = null, @q3 = null, @q4 = null, @q5 = null, @q6 = null, @q7 = null, @q8 = null, @q9 = null, @q10 = null
			select @q1 = qchar from #qtemp where qnum = 1
			select @q2 = qchar from #qtemp where qnum = 2
			select @q3 = qchar from #qtemp where qnum = 3
			select @q4 = qchar from #qtemp where qnum = 4
			select @q5 = qchar from #qtemp where qnum = 5
			select @q6 = qchar from #qtemp where qnum = 6
			select @q7 = qchar from #qtemp where qnum = 7
			select @q8 = qchar from #qtemp where qnum = 8
			select @q9 = qchar from #qtemp where qnum = 9
			select @q10 = qchar from #qtemp where qnum = 10
			truncate table #qtemp
			exec (@q1 + @q2 + @q3 + @q4 + @q5 + @q6 + @q7 + @q8 + @q9 + @q10 + @cmd)
			select @cmd = @nextcmd, @useq = 1
		end
    fetch @local_cursor into @name
	end /* while FETCH_SUCCESS */
	close @local_cursor
	if @worker_type=1	
		deallocate hCForEachDatabase
	else
		deallocate hCForEachTable
		
	return 0
";

            ExecuteSqlsWithTrans(connectionString, sql4);
        }

        /// <summary>
        ///     Execute sql statments without transaction
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqls"></param>
        /// <remarks>Required when performing database level operations like drop create databases</remarks>
        public static void ExecuteSqlsNoTrans(string connectionString, params string[] sqls)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.Connection = connection;
                foreach (var query in sqls)
                {
                    command.CommandText = query.Replace("\r", "").Replace("\n", "").Replace("GO", "");
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///     Execute multiple sql statments in transaction (use <see cref="ScriptToSqls" /> to split script first)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqls"></param>
        public static void ExecuteSqlsWithTrans(string connectionString, params string[] sqls)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    command.Connection = connection;
                    command.Transaction = transaction;
                    try
                    {
                        foreach (var query in sqls)
                        {
                            command.CommandText = query;
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        Consoler.Write("DB sqls executed ...");
                    }
                    catch (Exception ex)
                    {
                        Consoler.ShowError(ex);
                        try
                        {
                            transaction.Rollback();
                            throw;
                        }
                        catch (Exception ex2)
                        {
                            // This catch block will handle any errors that may have occurred 
                            // on the server that would cause the rollback to fail, such as 
                            // a closed connection.
                            Consoler.ShowError(ex2, true);
                            throw;
                        }
                    }
                }
            }
        }
    }
    }
