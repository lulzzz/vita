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
	[RequestId] [nvarchar](150) NOT NULL,
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
