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
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AggregateId] [nvarchar](64) NOT NULL,
	[CreatedUtcDate] [datetimeoffset](7) NOT NULL,
	[ModifiedUtcDate] [datetimeoffset](7) NOT NULL,
	[LastAggregateSequenceNumber] [int] NOT NULL,
	-------------------------------------------------
	[RequestId] [nvarchar](150) NULL,
	[Category] [nvarchar](100)  NULL,
	[SubCategory] [nvarchar](100)  NULL,
	[Description] [nvarchar](4000) NULL,
	[Amount]  [decimal](18, 2) NULL,
	[Method] [nvarchar](100)  NULL,

	CONSTRAINT [PK_BankStatementReadModel] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO
