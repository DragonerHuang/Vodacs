--2025-11-05 创建提交管理2个表
		CREATE TABLE [dbo].[Biz_Completion_Acceptance] (
	  [id] uniqueidentifier  NOT NULL,
	  [acceptance_no] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
	  [version] int  NOT NULL,
	  [inner_status] int  NOT NULL,
	  [describe] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [approved_date] datetime  NULL,
	  [file_publish_date] date  NULL,
	  [actual_inspection_date] datetime  NULL,
	  [submit_status ] int  NULL,
	  [contract_id] uniqueidentifier  NULL,
	  [test_result] int  NULL,
	  [inspector] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [engineer_permit_date] datetime  NULL,
	  [remark] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [delete_status] int DEFAULT 0 NULL,
	  [create_id] int  NULL,
	  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [create_date] datetime DEFAULT getdate() NULL,
	  [modify_id] int  NULL,
	  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [modify_date] datetime  NULL,
	  [producer_id] int  NULL,
	  [approved_id] int  NULL
	)
	GO

	ALTER TABLE [dbo].[Biz_Completion_Acceptance] SET (LOCK_ESCALATION = TABLE)
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'验收编号',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'acceptance_no'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'版本',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'version'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'内部状态（0：编辑中；1：审核中；2：已提交；3：取消；）',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'inner_status'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'描述',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'describe'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'审核日期',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'approved_date'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'文件发行日期',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'file_publish_date'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'实际检验日期',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'actual_inspection_date'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'submit_status '
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'合约Id',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'contract_id'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'检验结果（0：不及格；1：及格）',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'test_result'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'验收人',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'inspector'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'客户工程师许可日期',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'engineer_permit_date'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'备注',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'remark'
	GO

	EXEC sp_addextendedproperty
	'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
	'SCHEMA', N'dbo',
	'TABLE', N'Biz_Completion_Acceptance',
	'COLUMN', N'delete_status'
	GO


	-- ----------------------------
	-- Primary Key structure for table Biz_Completion_Acceptance
	-- ----------------------------
	ALTER TABLE [dbo].[Biz_Completion_Acceptance] ADD CONSTRAINT [PK__Biz_Subm__3213E83F059592E0] PRIMARY KEY CLUSTERED ([id])
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
	ON [PRIMARY]
	GO


	CREATE TABLE [dbo].[Biz_Deadline_Management] (
	  [id] uniqueidentifier  NOT NULL,
	  [deadline_type] int  NULL,
	  [subject] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [estimated_date] date  NULL,
	  [deadline_date] date  NULL,
	  [actual_date] date  NULL,
	  [customer_contact] nvarchar(16) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [director_user_id] int  NULL,
	  [approved_id] int  NULL,
	  [approved_date] datetime  NULL,
	  [status] int  NULL,
	  [describe] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [remark] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [delete_status] int DEFAULT 0 NULL,
	  [create_id] int  NULL,
	  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [create_date] datetime DEFAULT getdate() NULL,
	  [modify_id] int  NULL,
	  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
	  [modify_date] datetime  NULL,
	  [contract_id] uniqueidentifier  NULL
	)
	GO

		ALTER TABLE [dbo].[Biz_Deadline_Management] SET (LOCK_ESCALATION = TABLE)
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'期限类型（3：预审；4：现场考察；5：招标；6：公开招标；7：预审问答；8：邀请招标；9：招标问答；10：面试	）',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'deadline_type'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'主题',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'subject'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'预计日期',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'estimated_date'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'截至日期',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'deadline_date'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'实际日期',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'actual_date'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'客户联系人',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'customer_contact'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'负责人',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'director_user_id'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'审批人',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'approved_id'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'审核日期',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'approved_date'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'状态（0：待审核；1：已批准；2：驳回）',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'status'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'描述',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'describe'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'备注',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'remark'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'delete_status'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'合约Id',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Deadline_Management',
		'COLUMN', N'contract_id'
		GO


		-- ----------------------------
		-- Primary Key structure for table Biz_Deadline_Management
		-- ----------------------------
		ALTER TABLE [dbo].[Biz_Deadline_Management] ADD CONSTRAINT [PK__Biz_Dead__3213E83F3F86E196] PRIMARY KEY CLUSTERED ([id])
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
		ON [PRIMARY]
		GO

		CREATE TABLE [dbo].[Biz_Submission_Files] (
		  [id] uniqueidentifier  NOT NULL,
		  [file_no] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
		  [version] int  NOT NULL,
		  [inner_status] int  NOT NULL,
		  [describe] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
		  [approved_date] datetime  NULL,
		  [expected_upload_date] date  NULL,
		  [actual_upload_date] datetime  NULL,
		  [submit_status ] int  NULL,
		  [contract_id] uniqueidentifier  NULL,
		  [delete_status] int DEFAULT 0 NULL,
		  [brand] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
		  [remark] nvarchar(1000) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
		  [create_id] int  NULL,
		  [create_name] nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
		  [create_date] datetime DEFAULT getdate() NULL,
		  [modify_id] int  NULL,
		  [modify_name] nvarchar(60) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
		  [modify_date] datetime  NULL,
		  [submit_date] datetime  NULL,
		  [producer_id] int  NULL,
		  [approved_id] int  NULL
		)
		GO

		ALTER TABLE [dbo].[Biz_Submission_Files] SET (LOCK_ESCALATION = TABLE)
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'提交编号',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'file_no'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'版本',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'version'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'内部状态（0：编辑中；1：审核中；2：已批核；）',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'inner_status'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'描述',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'describe'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'审核日期',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'approved_date'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'预计上传日期',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'expected_upload_date'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'实际上传日期',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'actual_upload_date'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'submit_status '
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'合约Id',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'contract_id'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'delete_status'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'品牌/型号',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'brand'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'提交时间',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'submit_date'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'制作人id',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'producer_id'
		GO

		EXEC sp_addextendedproperty
		'MS_Description', N'审核人id',
		'SCHEMA', N'dbo',
		'TABLE', N'Biz_Submission_Files',
		'COLUMN', N'approved_id'
		GO


		-- ----------------------------
		-- Primary Key structure for table Biz_Submission_Files
		-- ----------------------------
		ALTER TABLE [dbo].[Biz_Submission_Files] ADD CONSTRAINT [PK__Biz_Subm__3213E83F9C262113] PRIMARY KEY CLUSTERED ([id])
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
		ON [PRIMARY]
		GO

	--2025-11-11 增加文件配置
	insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
	values(newid(), 0, N'提交文件',1,getdate(),'Submission_Files','\Submission_Files\')

	insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
	values(newid(), 0, N'竣工验收文件',1,getdate(),'Completion_Acceptance','\Completion_Acceptance\')

	insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
	values(newid(), 0, N'期限管理文件',1,getdate(),'Deadline_Management','\Deadline_Management\')



