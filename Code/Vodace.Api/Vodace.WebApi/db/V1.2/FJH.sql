ALTER TABLE [dbo].[Biz_Site_Work_Record] ALTER COLUMN [check_config] nvarchar(3000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL

ALTER TABLE [dbo].[Biz_Contract_Contact] ADD [mail_to] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
GO

EXEC sp_addextendedproperty
'MS_Description', N'抄送邮件群组，Gen，Pay ，Safe ，PD， ABWF ，EL， FS（多选，用,号隔开）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Contract_Contact',
'COLUMN', N'mail_to'