IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = '[ApplicationRef]'))
BEGIN



CREATE TABLE [dbo].[ApplicationRef](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationOrDeviceType] [nvarchar](100) NULL,
	[ApplicationRootName] [nvarchar](100) NULL,
	[ApplicationInitPage] [nvarchar](100) NULL,
 CONSTRAINT [PK_ApplicationRef] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


END