SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = N'BankStatementReadModel')
BEGIN
  DROP TABLE [dbo].[BankStatementReadModel]
END

GO

CREATE TABLE [dbo].BankStatementReadModel(
	[AggregateId] [nvarchar](64) NOT NULL,
	[CreatedUtcDate] [datetime] NOT NULL,
	[ModifiedUtcDate] [datetime] NOT NULL,
	[LastAggregateSequenceNumber] [int] NOT NULL,
	-------------------------------------------------
	[RequestId] [nvarchar](150) NOT NULL default newsequentialid(),
	[Category] [nvarchar](100)  NULL,
	[SubCategory] [nvarchar](100)  NULL,
	[Description] [nvarchar](4000) NULL,
	[Amount]  [decimal](18, 2) NULL,
	[Method] [nvarchar](100)  NULL,
	[TransactionUtcDate] [datetime] NULL

	CONSTRAINT [PK_BankStatementReadModel_RequestId] PRIMARY KEY CLUSTERED 
	(
		[RequestId] ASC
	)
)
GO


IF EXISTS (SELECT * FROM sys.indexes WHERE name='UQ_BankStatementReadModel' AND object_id = OBJECT_ID(N'BankStatementReadModel'))
BEGIN
  DROP INDEX [UQ_BankStatementReadModel] ON [dbo].[BankStatementReadModel]
END

CREATE UNIQUE NONCLUSTERED INDEX [UQ_BankStatementReadModel] ON [dbo].[BankStatementReadModel]
(
	[SubCategory] ASC,
	[Amount] ASC,
	[TransactionUtcDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


