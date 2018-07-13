using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using CsvHelper;
using Dapper;
using ExtensionMinder;
using Serilog;
using Vita.Contracts;

namespace Vita.Domain.Infrastructure.Importers
{
    public class BankVisionImporter
    {
      public static IEnumerable<BankVision> Import()
      {
        var list = new List<BankVision>();
        using (StreamReader sr = new StreamReader("D:/downloads/bankvision-keywords-2016-02-19_04-05-05-PM.csv"))
        {
          var csv = new CsvReader(sr);
          var records = csv.GetRecords<BankVision>();
          int count = 0;
          using (var conn = new SqlConnection(Constant.ConnectionString))
          {

            foreach (var rec in records)
            {
             // SaveToDatabase(conn, rec);
             list.Add(rec.TrimAllStrings());
            }
            Log.Information("bank vision {record}",count++.ToString());
            conn.Close();
          }
        }

        return list;
      }

      private static void SaveToDatabase(SqlConnection conn, BankVision rec)
      {
        string sqlQuery = @"INSERT INTO [dbo].[bankvision-keywords] (
            [BankName]
            ,[TranType]
            ,[CatName]
            ,[CatKeyword]
            ,[Description]
            ,[Amount]
            ,[DateTrans]
            ,[LoanStage]
            ,[AccountId]) 
            VALUES (@BankName,@TranType,@Catname,@CatKeyword, @Description, @Amount, @DateTrans, @LoanStage, @AccountId)";
        conn.Execute(sqlQuery,
          new
          {
            rec.BankName,
            rec.TranType,
            rec.CatName,
            rec.CatKeyword,
            Description = rec.Desc,
            rec.Amount,
            rec.DateTrans,
            rec.LoanStage,
            rec.AccountId,
          });
      }
    }
}
