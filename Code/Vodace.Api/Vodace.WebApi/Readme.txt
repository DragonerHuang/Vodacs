发版记录：
	
	数据库更新记录：
	更新日期：	2025-10-31 16：04
    更新内容：
	
	-- 报价内页-期限管理新增预计完成时间
	ALTER TABLE Biz_Quotation_Deadline ADD exp_complete_date datetime NULL; -- 添加预计完成时间

	-- 系统-文件上传路径配置
	INSERT INTO Sys_File_Config ( id, delete_status, remark, file_code, folder_path )
	VALUES
	( NEWID( ), 0, '预审问答完成文件', 'Preliminary_Enquiry_QA_Finsih_Documents', '\Preliminary_Enquiry_QA_Documents\' ),
	( NEWID( ), 0, '投标问答完成文件', 'Tender_QA_Finish_Documents', '\Tender_QA_Documents\' ),
	( NEWID( ), 0, '提交完成文件', 'Submit_Finish_Documents', '\03 Tender document\'),
	( NEWID( ), 0, '招标面试完成文件', 'Tender_Interview_Finish_Documents', '\03 Tender document\Tender Interview'),
    ( NEWID( ), 0, '现场考察完成文件', 'Site_Visit_Documents', '\02 Site visit')

	-- 2025-10-31新增合同联系人表
	IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Biz_Contract_Contact]') AND type IN ('U'))
		DROP TABLE [dbo].[Biz_Contract_Contact]
	GO
	
	CREATE TABLE [dbo].[Biz_Contract_Contact] (
	  [id] uniqueidentifier  NOT NULL,
	  [delete_status] int DEFAULT 0 NULL,
	  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [create_id] int  NULL,
	  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [create_date] datetime  NULL,
	  [modify_id] int  NULL,
	  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [modify_date] datetime  NULL,
	  [contract_id] uniqueidentifier  NULL,
	  [contact_name] nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [contact_tel] nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [contact_fax] nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [contact_title] nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [contact_email] nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
	)
	GO
	
	ALTER TABLE [dbo].[Biz_Contract_Contact] SET (LOCK_ESCALATION = TABLE)
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Contract_Contact',
	'COLUMN', N'delete_status'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'备注',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Contract_Contact',
	'COLUMN', N'remark'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'合同id',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Contract_Contact',
	'COLUMN', N'contract_id'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'姓名',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Contract_Contact',
	'COLUMN', N'contact_name'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'电话',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Contract_Contact',
	'COLUMN', N'contact_tel'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'传真',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Contract_Contact',
	'COLUMN', N'contact_fax'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'示题',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Contract_Contact',
	'COLUMN', N'contact_title'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'邮箱',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Contract_Contact',
	'COLUMN', N'contact_email'
	GO
	
	
	-- ----------------------------
	-- Primary Key structure for table Biz_Contract_Contact
	-- ----------------------------
	ALTER TABLE [dbo].[Biz_Contract_Contact] ADD CONSTRAINT [PK__Biz_Cont__3213E83FF217C548] PRIMARY KEY CLUSTERED ([id])
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
	ON [PRIMARY]
	GO

	更新备注：	报价内页-期限管理新增预计完成时间, 系统-文件上传路径配置，新增合同联系人表
	发版本号：	1.1.0.0

	
	数据库更新记录：
	更新日期：	2025-10-31 18：58
    更新内容：

	insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
	values(newid(), 0, N'工程内容初始化文件',1,getdate(),'Construction_Content_Init','\Construction_Content_Init\')
	
	更新备注：	工程文档初始经内容上传文件配置
	发版本号：	1.1.0.0


	

	数据库更新记录：
	更新日期：	2025-11-03 15：32
    更新内容：
		


    数据库更新记录：
	更新日期：	2025-10-04 10：23
    更新内容：

	-- 2025-11-3报价-招标面试
	IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Biz_Quotation_Interview]') AND type IN ('U'))
		DROP TABLE [dbo].[Biz_Quotation_Interview]
	GO
	
	CREATE TABLE [dbo].[Biz_Quotation_Interview] (
	  [id] uniqueidentifier  NOT NULL,
	  [delete_status] int DEFAULT 0 NULL,
	  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [create_id] int  NULL,
	  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [create_date] datetime  NULL,
	  [modify_id] int  NULL,
	  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [modify_date] datetime  NULL,
	  [qn_id] uniqueidentifier  NULL,
	  [interview_time] datetime  NULL,
	  [meeting_point] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [contact_id] uniqueidentifier  NULL
	)
	GO
	
	ALTER TABLE [dbo].[Biz_Quotation_Interview] SET (LOCK_ESCALATION = TABLE)
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Interview',
	'COLUMN', N'delete_status'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'备注',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Interview',
	'COLUMN', N'remark'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'报价的id',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Interview',
	'COLUMN', N'qn_id'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'面试时间',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Interview',
	'COLUMN', N'interview_time'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'集合点',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Interview',
	'COLUMN', N'meeting_point'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'联系人',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Interview',
	'COLUMN', N'contact_id'
	GO
	
	
	-- ----------------------------
	-- Primary Key structure for table Biz_Quotation_Interview
	-- ----------------------------
	ALTER TABLE [dbo].[Biz_Quotation_Interview] ADD CONSTRAINT [PK__Biz_Quot__3213E83F463CFFF9] PRIMARY KEY CLUSTERED ([id])
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
	ON [PRIMARY]
	GO

	-- 报价-现场考察
	IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Biz_Quotation_Site]') AND type IN ('U'))
		DROP TABLE [dbo].[Biz_Quotation_Site]
	GO
	
	CREATE TABLE [dbo].[Biz_Quotation_Site] (
	  [id] uniqueidentifier  NOT NULL,
	  [delete_status] int DEFAULT 0 NULL,
	  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [create_id] int  NULL,
	  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [create_date] datetime  NULL,
	  [modify_id] int  NULL,
	  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [modify_date] datetime  NULL,
	  [qn_id] uniqueidentifier  NULL,
	  [site_visit_time] datetime  NULL,
	  [contact_id] uniqueidentifier  NULL,
	  [meeting_point] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [site_visit] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
	)
	GO
	
	ALTER TABLE [dbo].[Biz_Quotation_Site] SET (LOCK_ESCALATION = TABLE)
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Site',
	'COLUMN', N'delete_status'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'备注',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Site',
	'COLUMN', N'remark'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'报价的id',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Site',
	'COLUMN', N'qn_id'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'考察日期',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Site',
	'COLUMN', N'site_visit_time'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'联系人',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Site',
	'COLUMN', N'contact_id'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'集合点',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Site',
	'COLUMN', N'meeting_point'
	GO
	
	EXEC sp_addextendedproperty
	'MS_Description', N'考察地点',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Quotation_Site',
	'COLUMN', N'site_visit'
	GO
	
	
	-- ----------------------------
	-- Primary Key structure for table Biz_Quotation_Site
	-- ----------------------------
	ALTER TABLE [dbo].[Biz_Quotation_Site] ADD CONSTRAINT [PK__Biz_Quot__3213E83FE476ADCD] PRIMARY KEY CLUSTERED ([id])
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
	ON [PRIMARY]
	GO
	
	更新备注：	新增报价内页-现场考察和招标面试
	发版本号：	1.1.0.0

	
    数据库更新记录：
	更新日期：2025-11-04 16：36
    更新内容：alter table Biz_Contact_Relationship add org_id uniqueidentifier
	更新备注：	新增报价内页-联系人添加组织id
	发版本号：	1.1.0.0




