-- ----------------------------------------
-- 新增Biz_Site_Work_Record表
-- 新增Biz_Site_Work_Record_Item_Check表
-- 新增Biz_Site_Work_Record_Sign表
-- 新增Biz_Site_Work_Record_Worker表
-- 新增Sys_Site_Work_Check_Item表
-- 新增Sys_Training_Item表
-- 新增Sys_Training_Que_Item表
-- 新增Biz_Quotation_Interview表
-- 新增Biz_Quotation_Site表
-- 新增Biz_Contract_Contact表
-- 新增Biz_Project_Files表
-- 新增Sys_User_Relation表
-- 报价内页-期限管理新增预计完成时间
-- 新增工地记录配置
-- 数字字典添加值更
-- 新增文件夹配置记录
-- ----------------------------------------

-- ----------------------------
-- Table structure for Sys_User_Relation
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Sys_User_Relation]') AND type IN ('U'))
	DROP TABLE [dbo].[Sys_User_Relation]
GO

CREATE TABLE [dbo].[Sys_User_Relation] (
  [id] uniqueidentifier  NOT NULL,
  [create_id] int  NULL,
  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_date] datetime DEFAULT getdate() NULL,
  [modify_id] int  NULL,
  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [modify_date] datetime  NULL,
  [delete_status] int DEFAULT 0 NULL,
  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [relation_type] int  NULL,
  [user_register_Id] uniqueidentifier  NULL,
  [relation_id] uniqueidentifier  NULL
)
GO

ALTER TABLE [dbo].[Sys_User_Relation] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_User_Relation',
'COLUMN', N'delete_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'Sys_User_Relation',
'COLUMN', N'remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'关联类型：0：用户工种',
'SCHEMA', N'dbo',
'TABLE', N'Sys_User_Relation',
'COLUMN', N'relation_type'
GO

EXEC sp_addextendedproperty
'MS_Description', N'用户注册id',
'SCHEMA', N'dbo',
'TABLE', N'Sys_User_Relation',
'COLUMN', N'user_register_Id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'关联id',
'SCHEMA', N'dbo',
'TABLE', N'Sys_User_Relation',
'COLUMN', N'relation_id'
GO


-- ----------------------------
-- Primary Key structure for table Sys_User_Relation
-- ----------------------------
ALTER TABLE [dbo].[Sys_User_Relation] ADD CONSTRAINT [PK__Sys_User__3213E83F1FB0790E] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO



-- ----------------------------
-- Table structure for Biz_Project_Files
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Biz_Project_Files]') AND type IN ('U'))
	DROP TABLE [dbo].[Biz_Project_Files]
GO

CREATE TABLE [dbo].[Biz_Project_Files] (
  [id] uniqueidentifier  NOT NULL,
  [create_id] int  NULL,
  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_date] datetime DEFAULT getdate() NULL,
  [modify_id] int  NULL,
  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [modify_date] datetime  NULL,
  [delete_status] int DEFAULT 0 NULL,
  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [relation_id] uniqueidentifier  NULL,
  [file_type] int  NULL,
  [file_index] int  NULL,
  [file_name] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [file_thumbnail_name] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [file_path] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [file_thumbnail_path] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [file_ext] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [file_size] int  NULL,
  [upload_status] int  NULL,
  [project_id] uniqueidentifier  NULL,
  [version] int  NOT NULL,
  [inner_status] int  NOT NULL,
  [submit_date] datetime  NULL,
  [submit_status] int  NULL,
  [check_list] nvarchar(300) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [approved_date] datetime  NULL,
  [approved_id] int  NULL
)
GO

ALTER TABLE [dbo].[Biz_Project_Files] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'delete_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'关联id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'relation_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件类型（0：主文件，1：编辑文件，2：客户评语，3：参考文件，4：内部验收凭证，5：客户验收凭证，6：开工前，7：施工中，8：完工后）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'file_type'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件排序号',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'file_index'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件名称',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'file_name'
GO

EXEC sp_addextendedproperty
'MS_Description', N'图片缩略图',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'file_thumbnail_name'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件路径',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'file_path'
GO

EXEC sp_addextendedproperty
'MS_Description', N'图片缩略图路径',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'file_thumbnail_path'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件后缀名',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'file_ext'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件大小、单位是字节',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'file_size'
GO

EXEC sp_addextendedproperty
'MS_Description', N'上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'upload_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'项目ID',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'project_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'版本',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'version'
GO

EXEC sp_addextendedproperty
'MS_Description', N'内部状态（0：编辑中；1：审核中；2：已批核；）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'inner_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'提交时间',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'submit_date'
GO

EXEC sp_addextendedproperty
'MS_Description', N'提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'submit_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'检查清单',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'check_list'
GO

EXEC sp_addextendedproperty
'MS_Description', N'审核日期',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'approved_date'
GO

EXEC sp_addextendedproperty
'MS_Description', N'审核人',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Project_Files',
'COLUMN', N'approved_id'
GO


-- ----------------------------
-- Primary Key structure for table Biz_Project_Files
-- ----------------------------
ALTER TABLE [dbo].[Biz_Project_Files] ADD CONSTRAINT [PK__Sys_Site__3213E83F3E51A56B] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO



-- 文件夹配置
INSERT INTO Sys_File_Config ( id, delete_status, remark, file_code, folder_path )
VALUES
  ( NEWID( ), 0, N'预审问答完成文件', 'Preliminary_Enquiry_QA_Finsih_Documents', '\Preliminary_Enquiry_QA_Documents\' ),
  ( NEWID( ), 0, N'投标问答完成文件', 'Tender_QA_Finish_Documents', '\Tender_QA_Documents\' ),
  ( NEWID( ), 0, N'提交完成文件', 'Submit_Finish_Documents', '\03 Tender document\'),
  ( NEWID( ), 0, N'招标面试完成文件', 'Tender_Interview_Finish_Documents', '\03 Tender document\Tender Interview'),
  ( NEWID( ), 0, N'现场考察完成文件', 'Site_Visit_Documents', '\02 Site visit'),
  ( NEWID( ), 0, N'完工后', 'Site_Work_Photo_After_Work', '2_Post_Contract\07_Site Work\3_Site Work Record\1_Daily Site Work Record\'),
  ( NEWID( ), 0, N'施工中', 'Site_Work_Photo_Working', '2_Post_Contract\07_Site Work\3_Site Work Record\1_Daily Site Work Record\'),
  ( NEWID( ), 0, N'开工前', 'Site_Work_Photo_Before_Work', '2_Post_Contract\07_Site Work\3_Site Work Record\1_Daily Site Work Record\'),
  ( NEWID( ), 0, N'开工前', 'Site_Work_Record', '2_Post_Contract\07_Site Work\3_Site Work Record\1_Daily Site Work Record\')
  

-- 2025-11-6数字字典添加值更

DECLARE @dic_id INT;

-- 插入数字字典表（Sys_Dictionary）
INSERT INTO Sys_Dictionary (
    parent_id,
    dic_name,
    dic_no,
    enable,
    remark,
    delete_status
) VALUES (
    0,                -- parent_id：0
    N'值更',          -- dic_name：值更
    N'work_shift',    -- dic_no：work_shift
    1,                -- enable：1
    N'值更',          -- remark：值更
    0                 -- delete_status：0
);

-- 获取刚插入的自增 dic_id
SELECT @dic_id = CAST(SCOPE_IDENTITY() AS INT);

-- 插入数字字典列表（Sys_DictionaryList），三条班次
INSERT INTO Sys_Dictionary_List (
    dic_name,
    dic_value,
    dic_id,
    enable,
    order_no,
    delete_status
) VALUES
    (N'早班', N'Morning Shift', @dic_id, 1, 1, 0),
    (N'中班', N'Afternoon Shift', @dic_id, 1, 2, 0),
    (N'晚班', N'Night Shift', @dic_id, 1, 3, 0);


-- 2025-11-05新增工地记录配置
INSERT INTO Sys_Config ( id, config_type, config_key, config_value, delete_status )
VALUES
  ( NEWID( ), 5, 'FM', 'FM', 0 ),
  ( NEWID( ), 5, 'CPNT', 'CP(NT)', 0 ),
  ( NEWID( ), 5, 'CPT', 'CP(T)', 0 )

-- 报价内页-期限管理新增预计完成时间
ALTER TABLE Biz_Quotation_Deadline ADD exp_complete_date datetime NULL; -- 添加预计完成时间

-- ----------------------------
-- Table structure for Biz_Contract_Contact
-- ----------------------------
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
'MS_Description', N'报价的id',
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



-- ----------------------------
-- Table structure for Biz_Quotation_Site
-- ----------------------------
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
  [site_visit] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [reply_date] datetime  NULL
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

EXEC sp_addextendedproperty
'MS_Description', N'回复日期',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Quotation_Site',
'COLUMN', N'reply_date'
GO


-- ----------------------------
-- Primary Key structure for table Biz_Quotation_Site
-- ----------------------------
ALTER TABLE [dbo].[Biz_Quotation_Site] ADD CONSTRAINT [PK__Biz_Quot__3213E83FE476ADCD] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO



-- ----------------------------
-- Table structure for Biz_Quotation_Interview
-- ----------------------------
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
  [contact_id] uniqueidentifier  NULL,
  [reply_date] datetime  NULL
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

EXEC sp_addextendedproperty
'MS_Description', N'回复日期',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Quotation_Interview',
'COLUMN', N'reply_date'
GO


-- ----------------------------
-- Primary Key structure for table Biz_Quotation_Interview
-- ----------------------------
ALTER TABLE [dbo].[Biz_Quotation_Interview] ADD CONSTRAINT [PK__Biz_Quot__3213E83F463CFFF9] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO




-- ----------------------------
-- Table structure for Sys_Training_Que_Item
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Sys_Training_Que_Item]') AND type IN ('U'))
	DROP TABLE [dbo].[Sys_Training_Que_Item]
GO

CREATE TABLE [dbo].[Sys_Training_Que_Item] (
  [id] uniqueidentifier  NOT NULL,
  [delete_status] int DEFAULT 0 NULL,
  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_id] int  NULL,
  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_date] datetime DEFAULT getdate() NULL,
  [modify_id] int  NULL,
  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [modify_date] datetime  NULL,
  [level] int  NOT NULL,
  [item_code] int  IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
  [root_id] int  NULL,
  [master_id] int  NULL,
  [global_code] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [correct_ans] int  NULL,
  [type_id] int  NULL,
  [name_cht] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [name_eng] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Sys_Training_Que_Item] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Que_Item',
'COLUMN', N'delete_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Que_Item',
'COLUMN', N'remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'层级（1，2，3）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Que_Item',
'COLUMN', N'level'
GO

EXEC sp_addextendedproperty
'MS_Description', N'行级代码（自曾列）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Que_Item',
'COLUMN', N'item_code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'主层级id',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Que_Item',
'COLUMN', N'root_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'父层级（item_code）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Que_Item',
'COLUMN', N'master_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'业务全局编码',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Que_Item',
'COLUMN', N'global_code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'中文',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Que_Item',
'COLUMN', N'name_cht'
GO

EXEC sp_addextendedproperty
'MS_Description', N'英文',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Que_Item',
'COLUMN', N'name_eng'
GO


-- ----------------------------
-- Records of Sys_Training_Que_Item
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_Training_Que_Item] ON
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'81B0E3C3-DA15-4C9E-A7A0-000D7107C2A7', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'48', N'4', N'15', N'Q1040101', N'0', N'0', N'A.肝臟', N'A. Liver')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'8E8C0947-CDEE-4241-8FCE-005CCCE27E18', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'26', N'1', N'7', N'Q1010303', N'0', N'0', N'C.二氧化碳', N'C. Carbon dioxide')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'2C9DA1BE-AE72-4F6B-ABCE-02098C3F6DAC', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.797', NULL, NULL, NULL, N'1', N'3', N'3', NULL, N'Q103', N'1', N'0', N'高空工作', N'Work at Height')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'A9764B8B-84A9-4BAF-AC6F-02EF1F2163DC', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'38', N'2', N'11', N'Q1020403', N'1', N'0', N'C.16千克', N'C.16 kg')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'6F7EFBB9-7322-415B-BB23-04C9F3CB0DA1', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'25', N'1', N'7', N'Q1010302', N'1', N'0', N'B.泡劑', N'B. Foaming agent')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'20DE00F2-41D2-426D-9D93-08E59CCAFEDA', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'9', N'2', N'2', N'Q10202', N'0', N'0', N'5.下列哪一種情況下，是造成令物料吊運期間鬆散的原因?', N'5. Which of the following situations is the cause of the loosening of materials during lifting?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'4DEEEF60-D23E-4002-A26B-0FC939BCCEFD', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'41', N'3', N'12', N'Q1030103', N'0', N'0', N'C.5米', N'C.5 meters')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'76E96A0D-DFCC-4276-B8DA-1265EA4DED0E', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'42', N'3', N'13', N'Q1030201', N'0', N'0', N'A.7天', N'A.7 days')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'FF0D688B-2E4E-4C4B-AB89-17864F7A4AEB', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'28', N'2', N'8', N'Q1020102', N'0', N'0', N'B.盡量將搬運動作速度放緩。', N'B. Try to slow down the moving process.')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'90CD569A-6913-430D-9580-1E9AA0A332DD', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'20', N'1', N'5', N'Q1010103', N'0', N'0', N'C.彎腰及伸直雙手', N'C. Bend down and stretch your arms')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'D26C0BE9-AAD0-41C1-AC65-2B9572C3026C', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'51', N'4', N'16', N'Q1040201', N'0', N'0', N'A.電荷超負', N'A. Overcharge')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'D92D1F01-B728-467E-8F45-350FC4E68032', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'45', N'3', N'14', N'Q1030301', N'1', N'0', N'A.安全的工作平台', N'A. Safe working platform')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'D214280B-B545-4AEC-A6F5-35BE3C395F54', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'46', N'3', N'14', N'Q1030302', N'0', N'0', N'B.安全帶', N'B. Seat belt')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'51FA5876-6BE9-4C34-A7E6-395D1159F45A', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.797', NULL, NULL, NULL, N'1', N'1', N'1', NULL, N'Q101', N'1', N'0', N'基本安全', N'Basic Safety')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'99A8BEED-F4F4-47A1-83FA-396DDCBC96BF', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'23', N'1', N'6', N'Q1010203', N'1', N'0', N'C.按逃生路線圖行', N'C. Follow the escape route map')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'E4BFED7C-74AC-4BA5-8621-3E9793F9EB57', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'16', N'4', N'4', N'Q10402', N'0', N'0', N'12.電力安全裝置中，漏電斷路器的使用是防止下列哪一種危險事故的發生?', N'12. In power safety devices, the use of leakage circuit breakers is to prevent which of the following dangerous accidents?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'6C26056E-7A1A-4BCD-935D-4115B69C7465', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'27', N'2', N'8', N'Q1020101', N'0', N'0', N'A.盡量每次提取多點貨物，減少搬運次數。', N'A.Try to pick up more goods each time to reduce the number of times they are moved.')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'6723DF06-594D-4168-8B5A-488E3349DE9F', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'50', N'4', N'15', N'Q1040103', N'0', N'0', N'C.腎臟', N'C. Kidney')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'EF30A9EF-6E6B-445C-807A-4CC049D86A87', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'24', N'1', N'7', N'Q1010301', N'0', N'0', N'A.乾粉劑', N'A. Dry powder agent
')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'B76A48CC-F51E-4CC2-9F44-4FED1B5387B1', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'15', N'4', N'4', N'Q10401', N'0', N'0', N'11.人體觸電致死，主要是下列哪一個器官受到嚴重傷害?', N'11. When a person dies from electric shock, which of the following organs is most seriously damaged?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'1B1E4738-3C2F-4F50-BFDA-53826564A2AE', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'29', N'2', N'8', N'Q1020103', N'1', N'0', N'C.盡量採用器具輔助，減少人力提舉。', N'C. Use equipment to assist as much as possible to reduce manual lifting.')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'C59528CA-2A0C-4CC2-A41C-587691E7AC92', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'36', N'2', N'11', N'Q1020401', N'0', N'0', N'A.12千克', N'A.12 kg')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'0D64C1CE-6E09-4101-8C6C-5EB17B354234', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'31', N'2', N'9', N'Q1020202', N'0', N'0', N'B.材料被牢固地縳好及吊勾的安全扣完全關上。', N'B. The materials are securely fastened and the safety buckle of the hook is fully closed.')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'73F6FBF8-7F58-41CB-9086-626B0E264CD5', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'40', N'3', N'12', N'Q1030102', N'0', N'0', N'B.2米以下', N'B. Below 2 meters')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'1152A232-AE00-4289-BF19-6DA2BF507C43', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.797', NULL, NULL, NULL, N'1', N'4', N'4', NULL, N'Q104', N'1', N'0', N'電力', N'Electricity')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'86872F27-3412-4A51-8943-6E03837BA6EE', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'53', N'4', N'16', N'Q1040203', N'1', N'0', N'C.觸電', N'C. Electric shock')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'166463B5-480F-450B-8D21-714020ED8933', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'13', N'3', N'3', N'Q10302', N'0', N'0', N'9.棚架每隔多少天由合資格人士檢查一次?', N'9. How often should the scaffolding be inspected by a qualified person?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'07A8FC9A-186A-41F6-8116-7203ACA01EBB', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'54', N'4', N'17', N'Q1040301', N'0', N'0', N'A.待工作完畢，才報告當值管工', N'A. Wait until the work is completed before reporting to the on-duty supervisor')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'81F23BED-BF18-47F5-99C1-76530E78368F', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'6', N'1', N'1', N'Q10102', N'0', N'0', N'2.火警時應如何逃生?', N'2. How to escape in case of fire?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'41726CF5-86C4-4491-B581-7D2E636FDC24', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'52', N'4', N'16', N'Q1040202', N'0', N'0', N'B.電器突然啟動', N'B. Electrical appliances start suddenly')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'F3D9123C-3496-4450-8B9E-8132509E9980', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'35', N'2', N'10', N'Q1020303', N'0', N'0', N'C.沒有遮蓋的地方', N'C. No covered area')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'28CC55FB-937F-4E25-A569-87C3D61AF375', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'55', N'4', N'17', N'Q1040302', N'0', N'0', N'B.傷勢輕微，自行處理不向管工報告', N'B. The injury is minor and the patient handles it on his own without reporting it to the supervisor')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'05B56334-9A92-4D48-B15A-937CE77D9A61', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'21', N'1', N'6', N'Q1010201', N'0', N'0', N'A.見路就行', N'A. Go wherever you see ')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'4420FEB7-8451-45F4-B2FA-9804B63B1189', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'56', N'4', N'17', N'Q1040303', N'1', N'0', N'C.即時通知管工，待管工再通知值日站長', N'C. Notify the supervisor immediately, who will then notify the stationmaster on duty')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'B1112C1E-54B4-492C-A9C2-A544167BBA3D', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'37', N'2', N'11', N'Q1020402', N'0', N'0', N'B.14千克', N'B.14 kg')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'8793448B-18E9-4748-86EC-A66919DCE670', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'18', N'1', N'5', N'Q1010101', N'1', N'0', N'A.蹲下時兩腿分開，屈膝及腰背要直', N'A. When squatting, keep your legs apart, bend your knees and keep your back straight.')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'5CA0BC8D-4117-4397-A8BB-A82D0E882FCD', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'44', N'3', N'13', N'Q1030203', N'0', N'0', N'C.30天', N'C. 30 days')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'651BB201-D24E-44EF-B253-AA91B45077E6', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'22', N'1', N'6', N'Q1010202', N'0', N'0', N'B.見人行便行', N'B. Go wherever you see people')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'963A1E3D-7ECA-411C-B95A-AD76A3932CEA', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'5', N'1', N'1', N'Q10101', N'0', N'0', N'1.如果貨物放在地上而需要把它提起前，正確的步驟是:', N'1. If the cargo is placed on the ground and needs to be lifted, the correct steps are:')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'42A9EEE7-AE99-4E4A-A0CC-ADF0147F3E42', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'32', N'2', N'9', N'Q1020203', N'0', N'0', N'C.吊運的威也、鍊鉻及帆布帶已配有有效証書', N'C. The hoisting yoke, chain and canvas belt are equipped with valid certificates')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'E546FD44-28EB-4EB7-8D35-B191480C9DA3', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'8', N'2', N'2', N'Q10201', N'0', N'0', N'4.搬運重物時，應先考慮下列哪一項以減少員工發生工傷意外?', N'4. When carrying heavy objects, which of the following should be considered first to reduce the risk of workplace accidents?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'DCBD304B-75B2-4391-AB9F-BCB4B460DE1B', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'17', N'4', N'4', N'Q10403', N'0', N'0', N'13. 在港鐵範圍內，如遇到緊急意外時，如何處理?', N'13. What should I do if I encounter an emergency in the MTR area?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'271D07F2-54BF-44DA-9AFC-C2B963BDDA50', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'33', N'2', N'10', N'Q1020301', N'0', N'0', N'A.沒有空氣調節的地方', N'A. Places without air conditioning')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'74B0BBF2-CE2A-4523-BDE7-C46589792AE3', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'10', N'2', N'2', N'Q10203', N'0', N'0', N'6.在下列哪一個環境中工作，會較容易使工友中暑?', N'6. In which of the following working environments would workers be more likely to suffer from heat stroke?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'97740F99-16FF-45BD-B1C3-C5288747664B', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'30', N'2', N'9', N'Q1020201', N'1', N'0', N'A."埋碼"的工人在起吊前，沒有檢查纜索及塞古有否破壞。', N'A. The workers who were engaged in the "burying of codes" did not check whether the cables and segu were damaged before lifting.')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'483E65D4-8302-47DE-8DF5-C71C385D5C6E', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'11', N'2', N'2', N'Q10204', N'0', N'0', N'7.根據勞工處的「體力處理操作指引」資料，一個人在站立時不應操控多重的物件，否則會增加背部受傷的機會:', N'7. According to the Labour Department''s "Guidelines on Manual Handling Operations", a person should not manipulate heavy objects while standing, otherwise it will increase the chance of back injury:')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'78B80208-15FB-4F76-9A14-D555F3DB91FD', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'19', N'1', N'5', N'Q1010102', N'0', N'0', N'B.深呼吸及全神貫注', N'B. Take a deep breath and concentrate')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'B7D3D3F1-7C3A-4553-9012-D59FA04FB80C', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'49', N'4', N'15', N'Q1040102', N'1', N'0', N'B.心臟', N'B. Heart')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'8D631A25-27EA-4CF1-855D-D74BBF7467A1', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'14', N'3', N'3', N'Q10303', N'0', N'0', N'10.為防止工人從2米或以上的地方墮下，應首先考慮提供甚麼安全設施?', N'10. To prevent workers from falling from a height of 2 meters or more, what safety facilities should be considered first?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'38CAAA9B-7811-42E1-AEE5-D94219879DF2', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'43', N'3', N'13', N'Q1030202', N'1', N'0', N'B.14天', N'B.14 days')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'84E3D986-F445-448F-87BB-E2F7AAC29F58', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'39', N'3', N'12', N'Q1030101', N'1', N'0', N'A.2米或以上', N'A.2 meters or above')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'412DF4FE-DA62-480E-A1EC-EF1D23975E90', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'34', N'2', N'10', N'Q1020302', N'1', N'0', N'B.酷熱及潮濕的環境', N'B. Hot and humid environment')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'ADC6B5B8-5477-477F-AFBF-F157F3D8B58A', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.797', NULL, NULL, NULL, N'1', N'2', N'2', NULL, N'Q102', N'1', N'0', N'體力處理操作', N'Physical Processing Operation')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'9E845DFD-70E2-474C-A2A7-F20317B10673', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.810', NULL, NULL, NULL, N'3', N'47', N'3', N'14', N'Q1030303', N'0', N'0', N'C.救生繩', N'C. Life rope')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'6C6C6BEF-55E2-4964-891E-F513D491FD9B', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'7', N'1', N'1', N'Q10103', N'0', N'0', N'3.下列那一種滅火劑,不適用撲救電器故障所引致的火警?', N'3. Which of the following fire extinguishing agents is not suitable for extinguishing fires caused by electrical failures?')
GO

INSERT INTO [dbo].[Sys_Training_Que_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [correct_ans], [type_id], [name_cht], [name_eng]) VALUES (N'6FCC9583-6DD5-4C08-B84E-F94CA892B8C8', N'0', NULL, NULL, NULL, N'2025-11-04 14:36:50.803', NULL, NULL, NULL, N'2', N'12', N'3', N'3', N'Q10301', N'0', N'0', N'8.何謂高空工作?', N'8.What is working at height?')
GO

SET IDENTITY_INSERT [dbo].[Sys_Training_Que_Item] OFF
GO


-- ----------------------------
-- Auto increment value for Sys_Training_Que_Item
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Sys_Training_Que_Item]', RESEED, 56)
GO


-- ----------------------------
-- Primary Key structure for table Sys_Training_Que_Item
-- ----------------------------
ALTER TABLE [dbo].[Sys_Training_Que_Item] ADD CONSTRAINT [PK__Sys_Trai__3213E83F34929B37] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO



-- ----------------------------
-- Table structure for Sys_Training_Item
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Sys_Training_Item]') AND type IN ('U'))
	DROP TABLE [dbo].[Sys_Training_Item]
GO

CREATE TABLE [dbo].[Sys_Training_Item] (
  [id] uniqueidentifier  NOT NULL,
  [delete_status] int DEFAULT 0 NULL,
  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_id] int  NULL,
  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_date] datetime DEFAULT getdate() NULL,
  [modify_id] int  NULL,
  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [modify_date] datetime  NULL,
  [level] int  NOT NULL,
  [item_code] int  IDENTITY(1,1) NOT NULL,
  [root_id] int  NULL,
  [master_id] int  NULL,
  [global_code] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [type_id] int  NULL,
  [is_qly] int  NULL,
  [is_others] int  NULL,
  [name_cht] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [name_eng] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Sys_Training_Item] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Item',
'COLUMN', N'delete_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Item',
'COLUMN', N'remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'层级（1，2，3）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Item',
'COLUMN', N'level'
GO

EXEC sp_addextendedproperty
'MS_Description', N'行级代码（自曾列）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Item',
'COLUMN', N'item_code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'主层级id',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Item',
'COLUMN', N'root_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'父层级（item_code）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Item',
'COLUMN', N'master_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'业务全局编码',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Item',
'COLUMN', N'global_code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'中文',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Item',
'COLUMN', N'name_cht'
GO

EXEC sp_addextendedproperty
'MS_Description', N'英文',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Training_Item',
'COLUMN', N'name_eng'
GO


-- ----------------------------
-- Records of Sys_Training_Item
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_Training_Item] ON
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'0699A203-72EE-42BA-9C78-041B12CE1737', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'12', N'2', N'2', N'T10206', N'0', N'1', N'0', N'熱工序許可證及工地防火', N'Hot work permit and fire prevention')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'C14C7576-52EB-42CF-8504-1DDCD9E303F6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'1', N'5', N'5', NULL, N'T105', N'0', NULL, NULL, N'重溫課程', N'Refresher Training')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'3272C3CF-1B3D-4A9F-BECB-2407720CED48', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'1', N'3', N'3', NULL, N'T103', N'0', NULL, NULL, N'工具箱培訓', N'Toolbox Talk')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'FDCA970F-311C-4C68-9019-3532442AA5E7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'1', N'2', N'2', NULL, N'T102', N'0', NULL, NULL, N'專門的安全培訓', N'Specific Safety Training')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'C5AD6C78-7AD9-4C9A-A0B8-4EB802CC6AC5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'16', N'2', N'2', N'T10210', N'0', N'1', N'0', N'軌道手推車的使用', N'Use of Track Trolley')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'C0A207EC-73EA-47C7-B717-6B0C1C6321B1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'6', N'1', N'1', N'T10101', N'0', N'1', N'0', N'入職培訓內容', N'Induction Training Matorials')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'4C89300B-52C0-464D-95A1-72543E9A6A9C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'1', N'1', N'1', NULL, N'T101', N'0', NULL, NULL, N'地盤入職培訓內容', N'Induction Training')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'5AF2B5EB-6B72-4A0F-8D14-82E2C6409B66', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'18', N'2', N'2', N'T10212', N'0', N'1', N'0', N'其他:', N'Other')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'4FE660C1-F99F-442B-B8F1-9032CBC91769', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'1', N'4', N'4', NULL, N'T104', N'0', NULL, NULL, N'PN Licensing', N'PN Licensing')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'CF13708C-5A39-4EEA-8737-9EE33DFBACDC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'14', N'2', N'2', N'T10208', N'0', N'1', N'0', N'酷熱/惡劣天氣下工作及預防中暑', N'Work in hot/adyerse weather and heat stroke prevevntion')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'EDB3604A-C7A2-4194-B6B6-A8141C30AF2C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'13', N'2', N'2', N'T10207', N'0', N'1', N'0', N'高空工作安全要點及梯具及工作台使用安全', N'Working at height safety and safe use of ladders & working platform')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'D52707DE-9BC2-42E7-AF5B-AE4C23544EB3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'17', N'2', N'2', N'T10211', N'0', N'1', N'0', N'第二位獨立檢查員(軌道)', N'SIC(Track)')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'5A6C3A94-5E66-4980-8911-AECC983E7598', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'9', N'2', N'2', N'T10203', N'0', N'1', N'0', N'工地整潔', N'Site housekeeping')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'F551F813-79B1-48CD-836A-AF4C63462C80', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'7', N'2', N'2', N'T10201', N'0', N'1', N'0', N'個人防護裝備應用', N'Use of PPEs')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'643364BA-3522-4680-B378-C823DC106DD6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'10', N'2', N'2', N'T10204', N'0', N'1', N'0', N'體力處理安全要點', N'Manual handling')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'16EA3B04-F76D-4569-9798-DFE848F31CD0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'11', N'2', N'2', N'T10205', N'0', N'1', N'0', N'密閉空間作業安全', N'Work in confined space')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'38EC97C0-025F-4FAD-8939-EA87E95A16AE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'8', N'2', N'2', N'T10202', N'0', N'1', N'0', N'電力安全', N'Electrical safety')
GO

INSERT INTO [dbo].[Sys_Training_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng]) VALUES (N'D289DEFF-0BDA-4F1D-A2DC-F0FD7E457EBE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:19.000', NULL, NULL, NULL, N'2', N'15', N'2', N'2', N'T10209', N'0', N'1', N'0', N'吊運安全', N'Specific Lifting')
GO

SET IDENTITY_INSERT [dbo].[Sys_Training_Item] OFF
GO


-- ----------------------------
-- Auto increment value for Sys_Training_Item
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Sys_Training_Item]', RESEED, 18)
GO


-- ----------------------------
-- Primary Key structure for table Sys_Training_Item
-- ----------------------------
ALTER TABLE [dbo].[Sys_Training_Item] ADD CONSTRAINT [PK__Sys_Trai__3213E83F2F63CCC5] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO




-- ----------------------------
-- Table structure for Sys_Site_Work_Check_Item
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Sys_Site_Work_Check_Item]') AND type IN ('U'))
	DROP TABLE [dbo].[Sys_Site_Work_Check_Item]
GO

CREATE TABLE [dbo].[Sys_Site_Work_Check_Item] (
  [id] uniqueidentifier  NOT NULL,
  [delete_status] int DEFAULT 0 NULL,
  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_id] int  NULL,
  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_date] datetime DEFAULT getdate() NULL,
  [modify_id] int  NULL,
  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [modify_date] datetime  NULL,
  [level] int  NOT NULL,
  [item_code] int  IDENTITY(1,1) NOT NULL,
  [root_id] int  NULL,
  [master_id] int  NULL,
  [global_code] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [type_id] int  NULL,
  [is_qly] int  NULL,
  [is_others] int  NULL,
  [name_cht] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [name_eng] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [order_no] int  NULL,
  [enable] tinyint  NULL
)
GO

ALTER TABLE [dbo].[Sys_Site_Work_Check_Item] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'delete_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'层级（1，2，3）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'level'
GO

EXEC sp_addextendedproperty
'MS_Description', N'行级代码（自曾列）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'item_code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'主层级id',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'root_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'父层级（item_code）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'master_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'业务全局编码',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'global_code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'中文',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'name_cht'
GO

EXEC sp_addextendedproperty
'MS_Description', N'英文',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'name_eng'
GO

EXEC sp_addextendedproperty
'MS_Description', N'排序列',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'order_no'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否启用（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Sys_Site_Work_Check_Item',
'COLUMN', N'enable'
GO


-- ----------------------------
-- Records of Sys_Site_Work_Check_Item
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_Site_Work_Check_Item] ON
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AB0E0AA2-E2CC-4697-B780-002A03E8EF21', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'71', N'18', N'18', N'B11801', N'0', N'1', N'0', N'嚴格執行密閉空間許可證制度', N'Rigorously Implement the Confined Space Entry Permit System', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1DC0CC49-014C-47B4-A941-00572CF33D70', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'222', N'22', N'80', N'S1010603', N'1', N'0', N'0', N'不適用', N'N/A', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B8A97A5F-D41E-4E34-B6B8-007236AF1168', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'223', N'22', N'75', N'S1010101', N'1', N'0', N'0', N'照明不足', N'Insufficient Lighting', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0415D514-8AA1-4C1F-AF50-00C542E4544B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'29', N'29', NULL, N'C102', N'0', NULL, NULL, N'施工期間', N'During construction', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1B40BCB8-6C57-4A72-B4DF-00F1E31CBE38', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'224', N'22', N'219', N'S1011301', N'1', N'0', N'0', N'已清除說明工地位置(上/下行線、公里數等)', N'Clearly specify the work site location(Up/Down Line, Kilometers, etc.)', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'661E9276-FAC8-4718-8A47-017E32BB0127', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'141', N'36', N'36', N'S10105', N'2', N'0', N'0', N'工作前注意事項', N'Pre-Work Precautions', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EC324893-3719-460E-BBDE-02BEC0671D82', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'225', N'47', N'196', N'SI1040101', N'2', NULL, N'0', N'在離開軌道現場，使用 SafeTrack App 掃瞄 QR code, 確認已撤離軌逍道及有關軌道回復正常。', N'At the scene of track access, apply SafeTrack App to scan the QR code for cancelling the track access and confirmed track status has resumed normal. ', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9F3D2300-EAC2-455D-8CCE-0347A61C42B3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'226', N'36', N'138', N'S4010203', N'2', N'0', N'0', N'雷暴*', N'Thunderstorms*', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0916842A-C118-4020-9B2F-03610F285135', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'227', N'22', N'218', N'S1011203', N'1', N'0', N'0', N'不適用', N'N/A', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1C6F4718-5C4B-4AAA-A79D-03AC45CA7B7B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'228', N'22', N'81', N'S1010708', N'1', N'0', N'0', N'口罩', N'Mask', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1B8C11BC-353D-439E-B978-03DBD2B59D82', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'229', N'36', N'140', N'S4010407', N'2', N'0', N'0', N'阻礙物*', N'Obstacle*', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'72C6D7B2-E773-4B7B-9F0A-03E35480D1CE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'230', N'36', N'141', N'S4010513', N'2', N'1', N'0', N'進行熱工序/鑽探/連接時, 須通知港鐵工程監督進行檢查, 並取得工作許可證', N'When carrying out hot work/drilling/connection, the MTR Engineering Supervisor must be notified for inspection and a work permit must be obtained', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'486959B4-CD0A-4E72-BF38-03E4E4A71BD6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'231', N'37', N'157', N'S4020902', N'2', N'0', N'0', N'使用前先檢查，如有損壞，應停止使用及安排維修', N'Check before use. If there is any damage, stop using it and arrange for repair.', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D64408FA-202A-41E9-82D1-040C577C17C7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'232', N'22', N'221', N'S1011502', N'1', N'0', N'0', N'架空電線上或附近', N'Overhead Line or nearby', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1D7160D4-CF67-475D-9192-041BED953505', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'233', N'46', N'194', N'SI1030202', N'2', NULL, N'0', N'已移除紅閃燈數目:', N'No. of RFL removed:', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'ECBA7978-E434-4422-8150-04238304444E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'234', N'36', N'147', N'S4011104', N'2', N'0', N'0', N'與上司或有關人仕保持聯絡(如出入工地,開始及完成工作)', N'stay in communication with your supervisor or related persons (e.g go in/out the site, start/complete work)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'36F9102E-4EAE-4338-9B1F-042394158230', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'119', N'29', N'29', N'C10203', N'0', N'0', N'0', N'完工後的清掃及安全檢查已完成', N' The post-completion cleaning and safety inspection have been successfully carried out.', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0BE8F37B-5B60-4F8B-9CC5-04279B89F37F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'175', N'39', N'39', N'S10209', N'2', N'0', N'0', N'工地整潔', N'Clean construction site', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A5CF0903-D418-478D-A655-042AFBE599A3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'235', N'22', N'79', N'S1010510', N'1', N'0', N'1', N'制房隔電許可證', N'Electrical insulation permit for manufacturing room', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C2C244BE-D0B9-4CAD-BA75-04772A37D5C4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'64', N'11', N'11', N'B11101', N'0', N'1', N'0', N'嚴格執行熱工序及禁煙制度', N'Strictly Enforce Hot Work Procedures and No-Smoking Policy', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C7E7F527-8E2C-49E7-B5EA-04A7937F7896', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'187', N'44', N'44', N'SI10107', N'2', N'0', N'0', N'7', N'7', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2DB42E44-4A4D-4396-AFFD-0539DF937B03', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'84', N'23', N'23', N'S10201', N'1', N'0', N'0', N'個人防護裝備', N'Individual Safety Gear', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'35C3CACA-789E-4937-AB84-055DCBA2397E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'155', N'37', N'37', N'S10207', N'2', N'0', N'0', N'用電安全', N'Electricity safety', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D90EAFD2-8415-47D3-8DDF-057CF078C9B9', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'236', N'22', N'79', N'S1010506', N'1', N'0', N'0', N'車站內高空工作的控制記錄表 (OPM858):', N'Control Record Sheet for Working at Height in Stations (OPM858):', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3DE79F93-035C-4212-905E-069A302A7B5D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'237', N'38', N'163', N'S5010503', N'2', N'0', N'0', N'已貼檢查標籤', N'Inspection label attached', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6E150979-2D75-4251-8D8A-07AB5B40A868', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'81', N'22', N'22', N'S10107', N'1', N'0', N'0', N'個人防護裝備', N'Individual Safety Gear', N'10', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'16106BBB-1D6B-4B58-BE93-07F05B08AF52', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'238', N'22', N'75', N'S1010104', N'1', N'0', N'0', N'地面不平/斜路', N'Uneven ground/ramp', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6D5DD824-3E56-432D-9AB9-08D0B2D18839', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'104', N'27', N'27', N'S10604', N'1', N'0', N'0', N'工具妥善放回工具箱', N'Please return the tools to the toolbox appropriately.', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'CE37843C-9190-4725-907E-0973A693CD05', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'239', N'36', N'145', N'S4010908', N'2', N'0', N'0', N'電筒', N'Torch', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AF2C6F82-F561-4250-AD1A-09AC1A2E3E60', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'22', N'22', NULL, N'S101', N'1', NULL, NULL, N'安全早會', N'Safety Morning Meeting', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0BE17BF5-0C6A-4C16-A5E5-0A20865EB857', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'13', N'13', NULL, N'B113', N'0', NULL, NULL, N'狹窄位置施工撞傷', N'Collision Injury while working in a Limited space', N'13', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'993DEC93-375A-4DD2-9DF0-0A2953A99A92', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'240', N'22', N'75', N'S1010109', N'1', N'0', N'0', N'中暑', N'Heatstroke', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'15018CAA-055D-4CE7-A428-0A5028CEDBC1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'241', N'22', N'219', N'S1011302', N'1', N'0', N'0', N'安全駕駛及提點', N'Drive safety and give reminders', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2CD9C0AA-AF02-455D-AE31-0AAF3BCA8020', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'242', N'36', N'137', N'S4010102', N'2', N'0', N'0', N'建造業工人註冊證', N'CIC', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9A8FFA82-3DCD-4529-B6F7-0AE9527A262F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'243', N'52', N'216', N'S1070101', N'1', N'0', N'0', N'注意人力抬舉', N'Pay attention to Manual Handling', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B768B8C7-E954-41A9-B66F-0B1ECD06C615', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'150', N'37', N'37', N'S10202', N'2', N'0', N'0', N'物料堆放', N'Material stacking', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A86EA7AE-094F-4E04-A425-0D0F0B7D5ED2', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'244', N'22', N'76', N'S1010208', N'1', N'0', N'0', N'已替工作會涉及的港鐵站系統進行隔離及掛牌  消防', N'The MTR station systems involved in the work have been isolated and marked. Fire Control', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5BD3348C-9376-40F7-88C5-0D478E7FB286', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'167', N'39', N'39', N'S10201', N'2', N'0', N'0', N'工作整潔及安全通道暢通', N'Clean workspace and clear safety passages', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'54D2D9B0-0F67-49B0-B4A0-0E083B7ED5E1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'93', N'24', N'24', N'S10304', N'1', N'0', N'0', N'正確使用工具及裝備', N'Properly Operate Tools And Gear', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'CE56F974-B36B-457C-873F-0E62A3F73E0A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'153', N'37', N'37', N'S10205', N'2', N'0', N'0', N'吊運', N'Lifting', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'34894F14-E77D-4E2A-9335-0E85748C8F0E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'136', N'35', N'35', N'C20502', N'0', N'0', N'0', N'品質問題(如有) 已向相關工程師匯報', N'Quality issues (if any) have been reported to the appropriate engineers.', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'01784450-5A90-4359-A06C-0EFD823E1D05', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'89', N'23', N'23', N'S10206', N'1', N'0', N'0', N'工作環境', N'Site Environment', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'14668AE4-FA12-4954-865B-0FDCE4FF01E4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'18', N'18', NULL, N'B118', N'0', NULL, NULL, N'窒息(密閉空間)', N'Suffocation (Confined Space)', N'18', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'86B8CD37-6457-4CF9-AACA-100AADD87A57', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'139', N'36', N'36', N'S10103', N'2', N'0', N'0', N'工作內容', N'Work Description', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'08FBF307-1963-4E00-9A9D-109D961E10D6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'245', N'36', N'138', N'S4010202', N'2', N'0', N'0', N'雨天*', N'Raining*', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'72DB726D-1CE3-431F-9CB0-118EDBB64953', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'246', N'36', N'143', N'S4010709', N'2', N'0', N'0', N'其他:____________', N'Others:_____________________________', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'21294852-7BD5-41CE-A023-1196E6037A32', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'247', NULL, NULL, NULL, NULL, NULL, NULL, N'', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9D84371F-0071-4940-9323-11A5932F65C8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'248', N'22', N'215', N'S1011107', N'1', N'0', N'0', N'寒冷', N'Frigid', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8B1FC591-42DC-4F70-AC90-1281885BE258', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'57', N'4', N'4', N'B10401', N'0', N'1', N'0', N'保持正確人力提舉姿勢', N'Maintain Proper Manual Lifting Posture', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'67BB8417-B25D-4A56-9D33-132BEF0F660D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'162', N'38', N'38', N'S10104', N'2', N'0', N'0', N'機械', N'Mechanical Equipment', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'80B963F5-8606-445B-BEAE-1340C7203C74', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'66', N'13', N'13', N'B11301', N'0', N'1', N'0', N'瞭解逃生路線及集合點', N'Understand Escape Routes and Assembly Points', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4EC419DC-381E-4728-8A7F-139370514A52', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'173', N'39', N'39', N'S10207', N'2', N'0', N'0', N'防火 (滅火筒、煙霧頭)', N'Fire protection (fire extinguisher, smoke detector)', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'97CBEF1B-5EF1-4679-B1A1-13B48730A352', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'249', N'36', N'138', N'S4010204', N'2', N'0', N'0', N'酷熱*', N'Very Hot*', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'955429CE-7599-40A4-B304-141A3B44C487', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'126', N'32', N'32', N'C20202', N'0', N'0', N'0', N'最新批准的施工方案/ 有關規則/程序/工作指示/圖則的副本是否已存放在工地? ', N'Are copies of the latest approved construction plans/rules/procedures/job instructions/drawings stored on the construction site?', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0819D72F-E919-431B-BF78-142D4453D0E2', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'250', N'42', N'179', N'S5050103', N'2', N'0', N'0', N'3. 確認點齊工作隊人數及離開現場', N'3. Confirm the number of team members and leave the scene', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7A568F17-58ED-43A6-A62C-152DB8423114', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'251', N'48', N'203', N'SI2010301', N'2', NULL, N'0', N'CP(T)已確認在進入軌道前，取得調車場主管(YM)/行車控制主任(TC)/工程主管(EPIC)授權。(如有需要時) (例子 檢査通話記錄等)', N'CP(T) confirmed that track access authorization was given by Yard Master(YM) / Traffic Controller(TC) / Engineer''s Person-in-Charge(EPIC) before track access. (if necessary) (eg. Check telephone record etc.)', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F2AA84A5-FB6E-42ED-B4DF-15B5AC2A661A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'252', N'22', N'220', N'S1011403', N'1', N'0', N'0', N'軌道上或附件工作', N'Work on or near Track', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'589DB75B-D104-4967-8457-16148939E028', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'32', N'32', NULL, N'C202', N'1', NULL, NULL, N'現場施工文件情況', N'Condition of Construction Site Documentation', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0B1695BC-EA7B-4C0F-B4C8-16345D8DC339', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'20', N'20', NULL, N'B120', N'0', NULL, NULL, N'小心地面不平', N'Beware of Uneven Ground', N'20', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'95E1794E-A1E9-4931-901C-1644F70CCE92', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'253', N'22', N'75', N'S1010108', N'1', N'0', N'0', N'缺氧', N'Hypoxia', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3B9FA566-FDC2-4D40-8462-1657F8350117', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'254', N'41', N'178', N'S5040106', N'2', N'0', N'0', N'地盤出入口安全及沒有阻塞', N'Site entrances and exits are safe and unobstructed', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BDBD41C7-9868-4097-BD06-16621A045207', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'255', N'36', N'146', N'S4011002', N'2', N'0', N'0', N'逃生路線', N'Evacuation Routes ', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'04FC4B9E-DF2D-44C6-8FCC-1665E1C1EE34', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'114', N'28', N'28', N'C10104', N'0', N'0', N'0', N'已向值日站長(車站)/調車場主管(車廠)報告工作詳情，工作隊，工作地點和時段及獲批准開工', N' Reported the specifics of the work, work team, location, schedule to the duty station master (station)/shunting yard supervisor (depot), and obtained approval to commence work.', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7A6B6A3E-543F-43D4-8F6B-169425F0099F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'256', N'22', N'215', N'S1011106', N'1', N'0', N'0', N'酷熱', N'Very Hot', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F7921122-CB7A-4DBC-8E83-16A340D6F8C4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'257', N'22', N'75', N'S1010112', N'1', N'0', N'1', N'需要向 TC 攞路 Transaction Number: ', N'Need to ask TC for Transaction Number:', N'12', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6EAB9965-E69F-4808-AE54-16C47B34BEAD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'258', N'42', N'179', N'S5050101', N'2', N'0', N'0', N'1. 清理現場工作時產生的垃圾', N'1. Clean up the garbage generated during on-site work', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'09A1D22D-0C4B-4044-8093-1896ED730B55', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'259', N'52', N'216', N'S1070103', N'1', N'0', N'0', N'其他:', N'Others:', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F432EF83-D0C7-43E4-AE6A-1922CC7CEC4A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'260', N'22', N'219', N'S1011305', N'1', N'0', N'0', N'工具/設備齊備及良好', N'All tools/equipment are ready and in good condition', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0D8AF327-2A61-41CB-9C5B-199783C04FE0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'261', N'36', N'144', N'S4010808', N'2', N'0', N'0', N'工作許可證(熱作) Hot Work', N'Work Permit (Hot Work)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'75575BAE-955B-4492-A1B8-19B1F3BC97B1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'172', N'39', N'39', N'S10206', N'2', N'0', N'0', N'電氣 (開關掣、電箱、電線)', N'Electrical (switches, wires, cables)', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7D5E22DF-FA6D-44FB-A51A-1AAE4F06B4AC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'262', N'22', N'218', N'S1011202', N'1', N'0', N'0', N'符合「建築噪音許可證」要求', N'Comply with the requirements of Construction Noise Permit', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8ADB1CD5-9339-4997-8AF0-1BF19CDFED4F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'182', N'44', N'44', N'SI10102', N'2', N'0', N'0', N'2', N'2', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D2AE1389-F526-4DAE-A2AF-1C3450A10C62', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'263', N'22', N'221', N'S1011504', N'1', N'0', N'0', N'密閉空間', N'Confined Space', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7586F5CD-D9EE-4B3B-AAD7-1C5ECA05BBCD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'264', N'52', N'217', N'S1070203', N'1', N'0', N'0', N'其他:', N'Others:', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'58DF3ADA-906D-4388-9AB0-1C7BF871AD97', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'73', N'20', N'20', N'B12001', N'0', N'1', N'0', N'將貼黃、黑膠紙及作出適當圍封', N'Apply Yellow and Black Tape and provide Appropriate Enclosure', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0A9C45DC-29C2-4D1B-A5E7-1CA342C65747', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'265', N'36', N'144', N'S4010802', N'2', N'0', N'0', N'電路隔離證書CIC/ 電路狀況證CSC', N'Circuit Isolation Certificate/Circuit State Certificate', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B30A5DFF-F4EF-43DD-A9FE-1DD8377D425B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'108', N'27', N'27', N'S10608', N'1', N'0', N'0', N'所有火種已熄滅', N'All sources of fire have been extinguished.', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'79BC2198-8B31-4C94-B3A5-1DDA406E3516', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'266', N'23', N'87', N'S1020402', N'1', N'0', N'0', N'損毀(即時停用)', N'Damaged (immediate deactivation)', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B6C410A1-FECD-4141-A810-1E618E3531E9', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'267', N'22', N'76', N'S1010201', N'1', N'0', N'0', N'*簡述工作危害(JHA)', N'*Brief description of job hazards (JHA)', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'630A0B69-20E6-4BE2-962E-1EEA7B43E94C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'268', N'37', N'149', N'S4020102', N'2', N'0', N'0', N'圍封工作範圍及張貼足夠安全標誌，保持地方整潔，搬運物料時小心行人安全', N'Seal off the work area and post sufficient safety signs to keep the area clean and tidy, and pay attention to pedestrian safety when moving materials', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F68B8649-62D4-486C-A9D6-20015FAEFD97', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'120', N'30', N'30', N'C10301', N'0', N'0', N'0', N'已邀請值日站長(車站)/調車場主管(車廠)共同視察工地', N'The on-duty station master (station)/shunting yard supervisor (depot) has been invited to jointly inspect the construction site.', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'656185A5-79EE-4B55-ABAA-2042B7524FFD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'269', N'41', N'178', N'S5040101', N'2', N'0', N'0', N'物料適當擺放', N'Proper placement of materials', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'56BEECC5-863B-4B9B-B81D-211131E0AF96', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'159', N'38', N'38', N'S10101', N'2', N'0', N'0', N'個人保護裝備', N'Personal protective equipment', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EBF5A6CB-1FB8-44D6-8D1E-21F234E9321E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'113', N'28', N'28', N'C10103', N'0', N'0', N'0', N'已用中文向工人講解施工方案及施工前因應已評估風險所需要安全措施的安排', N'The construction plans have been communicated in Chinese to the workers, and safety measures required for addressing pre-construction risk assessments have been arranged.', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6BD443D1-BDA8-45CB-9989-22CC4354548C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'270', N'22', N'221', N'S1011508', N'1', N'0', N'0', N'高空工作', N'Work at Height', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0EEC3602-9B48-473F-8BD2-22DB73BA087F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'271', N'38', N'165', N'S5010705', N'2', N'0', N'1', N'其他:', N'Others:', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D0B8AA24-6825-48A6-A5AC-234A3D830888', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'152', N'37', N'37', N'S10204', N'2', N'0', N'0', N'高處工作', N'Working at heights', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'566E39FF-716A-47D4-AFAE-2360CB07B40A', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'272', N'45', N'192', N'SI1020201', N'2', NULL, N'0', N'其他(如有需要時):', N'Other (if necessary):', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'15616194-E61A-4F6D-8404-246E8D5318BE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'100', N'26', N'26', N'S10501', N'1', N'0', N'0', N'整理工地 (收拾物料,垃圾)', N'Clean Construction Site (Remove Materials, Waste)', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6FD6FCB2-732C-41C2-9FC3-24C1890064EE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'273', N'36', N'143', N'S4010702', N'2', N'0', N'0', N'紅色閃燈', N'Red Flashing Light', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'86804B7D-F295-4C29-B4F4-25CE4B4A7E34', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'274', N'52', N'217', N'S1070202', N'1', N'0', N'0', N'個人防護裝備', N'Personal protective equipment', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A16C0513-E09D-4730-9067-25CECDF300B0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'17', N'17', NULL, N'B117', N'0', NULL, NULL, N'被鋒利物割傷', N'Injury from a Sharp Object', N'17', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E5BB94DC-20BF-4DB2-A4E3-265DC19AF3C5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'85', N'23', N'23', N'S10202', N'1', N'0', N'0', N'電工具', N'Electrical Equipment', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8CCDCE08-DDB4-47B9-8D6B-27FCD282B365', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'275', N'36', N'145', N'S4010907', N'2', N'0', N'0', N'護目鏡', N'Goggles', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E42F25F0-5F27-44D6-BFE6-288251C43441', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'133', N'34', N'34', N'C20403', N'0', N'0', N'0', N'工作是否由合資格的人定期檢查確保符合施工程序及質量要求? ', N'Is the work periodically inspected by qualified individuals to ensure adherence to construction procedures and quality standards?', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'74B815CB-094B-4874-8CD9-296846D23401', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'276', N'22', N'218', N'S1011205', N'1', N'0', N'0', N'化學廢料', N'Chemical Waste', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0DEF8743-2BF6-4A12-A86D-29CDFC5FA687', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'277', N'38', N'164', N'S5010601', N'2', N'0', N'0', N'照明充足', N'Adequate lighting', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'68C076E2-3E71-4E14-BAAB-29EFA0B7DA65', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'278', N'22', N'83', N'S1010904', N'1', N'0', N'0', N'與上司或有關人仕保持聯絡(如出入工地，開始及完成工作)', N'stay in communication with your supervisor or related persons (e.g go in/out the site, start/complete work)', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E096FA2F-26AE-4ABB-A362-2A2F5EDBBFEE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'279', N'22', N'78', N'S1010404', N'1', N'0', N'0', N'號角+燈/旗', N'Horn + Lamp/Flag', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'82F90567-2F12-46D3-B629-2A46F331AB23', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'90', N'24', N'24', N'S10301', N'1', N'0', N'0', N'工地整潔及安全通道暢通', N'Worksite Tidiness And Unobstructed Safety Pathways', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'CC38318F-51D4-461C-A3EA-2A6152A8D02E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'280', N'22', N'220', N'S1011402', N'1', N'0', N'0', N'非工程領域', N'Non-Possession', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'826A1927-8513-432D-AA86-2AB96F2DA4F7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'281', N'22', N'81', N'S1010704', N'1', N'0', N'0', N'耳罩/耳塞', N'Ear Muff/Plugs', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8C9778F6-9833-46EB-9C53-2B0E3127E073', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'282', N'23', N'89', N'S1020604', N'1', N'0', N'0', N'防止墜下措施狀態良好', N'Fall prevention measures are in good condition', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A7B509CA-6635-418E-9483-2C1FE006227C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'283', N'22', N'82', N'S1010802', N'1', N'0', N'1', N'*進出工地路線:', N'*Access routes to the work site', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'72C9F306-26E5-455C-9A96-2CCCEB74ECE0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'39', N'39', NULL, N'S202', N'2', NULL, NULL, N'工作中安全巡查記錄', N'Safety Inspection Record during Work', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5473FDCF-39B3-42A9-BDBF-2D58C1D0F78A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'115', N'28', N'28', N'C10105', N'0', N'0', N'0', N'批准工人開工前，已提供及確保所有安全設備及措施齊備', N'Prior to authorizing the commencement of work, all necessary safety equipment and measures have been provided and confirmed to be in place.', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'26A42E3C-97F8-41B3-B792-2E71A1029F15', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'284', N'23', N'88', N'S1020507', N'1', N'0', N'0', N'損毀(即時停用)', N'Damaged (immediate deactivation)', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A144CFE6-171D-46B8-99A0-2E9C49106A22', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'128', N'33', N'33', N'C20301', N'0', N'0', N'0', N'所有現場材料均符合批准清單的要求', N'All on-site materials comply with the requirements of the approved list.', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2C12DD99-2927-4960-81F8-2ED2E799190F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'11', N'11', NULL, N'B111', N'0', NULL, NULL, N'火警(熱工序)', N'Fire Alarm (during Hot work)', N'11', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'666EF758-5190-4A15-B32C-2F0C7A75F7FE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'285', N'22', N'81', N'S1010714', N'1', N'0', N'1', N'其他:', N'Others:', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'243CCF09-E944-416F-B1DA-2F1A37196E95', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'286', N'45', N'191', N'SI1020104', N'2', NULL, N'0', N'使用接地棒數目:', N'No. of Earthing Rod used:', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DCDAD685-A5B6-4F90-8BA2-2FCD386F67CC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'287', N'37', N'156', N'S4020802', N'2', N'0', N'0', N'安排足夠人手提舉重物，儘量以機械輔助', N'Arrange enough people to lift heavy objects manually, and use mechanical assistance as much as possible', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F2E5B84B-934C-433D-8786-30027B3EFC71', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'288', N'38', N'159', N'S5010101', N'2', N'0', N'0', N'裝備足夠', N'Sufficient equipment', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'920E0B1F-D7DD-44A5-BE9D-30DC55250DFB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'58', N'5', N'5', N'B10501', N'0', N'1', N'0', N'保持工地整潔及安全通道暢通', N'Keep the Construction Site Clean and Ensure Clear Safety Passages', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'70C9690B-FD67-4439-81C7-316EDB6E69A6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'289', N'38', N'160', N'S5010206', N'2', N'0', N'0', N'急停掣操作正常(如有)', N'Emergency stop button operates normally (if any)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DAAF1FC6-A9AF-4036-BE3A-320175E5B54A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'290', N'36', N'141', N'S4010504', N'2', N'1', N'0', N'注意對公眾的影響', N'Be aware of the impact on the public', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'937C4EC0-44FB-411A-BBC9-3239363ADE06', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'291', N'38', N'163', N'S5010502', N'2', N'0', N'0', N'梯台狀況良好', N'The landing is in good condition', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'59EF5E41-AA6C-4FEB-A145-3265E257AE06', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'131', N'34', N'34', N'C20401', N'0', N'0', N'0', N'工人是否明白施工方案的程序及要求?', N'Are the workers knowledgeable about the procedures and requirements outlined in the construction plan?', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4FF8A19D-66FA-418E-AD42-32975EF152A5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'112', N'28', N'28', N'C10102', N'0', N'0', N'0', N'已有批核的施工方案', N'The construction plans have been approved.', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6D270D48-9404-4296-A371-32C81C6AE8EB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'206', N'50', N'50', N'SI20302', N'2', N'0', N'0', N'6', N'6', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BAEAE32A-1837-4CE2-9C9B-3365F13BC47B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'292', N'23', N'84', N'S1020101', N'1', N'0', N'0', N'裝備足夠', N'Sufficient equipment', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'97310FCC-40C3-4A78-B4B8-343B64DEA62A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'293', N'22', N'220', N'S1011407', N'1', N'0', N'0', N'軌道車輛運行安排', N'Operation Arrangement of On-track vehicle', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6FE7DF0B-3FAC-42A2-B4F3-34C70AD0B122', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'294', N'22', N'81', N'S1010713', N'1', N'0', N'1', N'電筒', N'Torch', N'13', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'57BD5CDB-803C-4B18-BD6F-34CD912FA38E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'174', N'39', N'39', N'S10208', N'2', N'0', N'0', N'健康 (塵埃、噪音、危險物質)', N'Health (dust, noise, hazardous substances)', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'96DD9585-80B8-4E9F-9F1C-3563589A4CE2', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'295', N'43', N'180', N'S5060103', N'2', N'0', N'0', N'人力提舉', N'Manual lifting', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EE231BF4-42B8-4B52-815A-35737C5A3DC1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'37', N'37', NULL, N'S102', N'2', NULL, NULL, N'工作危害識別活動(HIA)', N'Workplace Hazard Identification Activity (HIA)', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'19840287-0985-4723-B8BC-3599A275FAB9', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'296', N'44', N'187', N'SI1010701', N'2', NULL, N'0', N'開工前，已通知有關的值日站長(SC)/調車場主管(YM)。', N'Informed all related SC/YM before work starts.', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7133B384-0B1E-4525-9527-35AF567AD360', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'297', N'22', N'80', N'S1010602', N'1', N'0', N'0', N'工作證書(密閉空間) CFW(CS)', N'Certificate-For-Work(Confined Space) CFW(CS)', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EEF54D7C-F43D-43A2-9444-3674D83F759A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'142', N'36', N'36', N'S10106', N'2', N'0', N'0', N'保護措施(一般工作)', N'Protective Measures (General Work)', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'903925DC-5091-48D1-A047-369B89FA4131', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'185', N'44', N'44', N'SI10105', N'2', N'0', N'0', N'5', N'5', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B3690613-E886-4159-B432-36D0F124783B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'116', N'28', N'28', N'C10106', N'0', N'0', N'0', N'於檢查以上項目 1-5 後，已通知當值工務督察開始施工', N' Following the inspection of the aforementioned items 1-5, the on-duty engineering inspector has been informed to commence construction.', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D0EDB62B-FA05-4D5E-BC60-36D163D245E0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'298', N'37', N'151', N'S4020301', N'2', N'0', N'0', N'油污、廢料及雜物必須清理/垃圾存放在廢物箱內並定期清理', N'Oil, waste and debris must be cleaned up/garbage should be stored in waste bins and cleaned up regularly', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8920D05A-1DFD-4A2F-92E9-3A2F1EA34C37', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'299', N'22', N'78', N'S1010401', N'1', N'0', N'0', N'紅色閃燈', N'Red Flashing Light', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DAD65AE2-0AEE-4769-8F98-3A3F9758EEE0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'16', N'16', NULL, N'B116', N'0', NULL, NULL, N'刺激皮膚(化學物品處理)', N'Skin Irritation (due to Chemical handling)', N'16', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2B6E3E1A-CFB0-45F9-8907-3A49EB0F5E06', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'144', N'36', N'36', N'S10108', N'2', N'0', N'0', N'安全文件(許可證)', N'Safety Documentation (Authorization)', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7BFB18F5-FCF9-4B17-9F24-3AB0BF446ECA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'300', N'22', N'76', N'S1010205', N'1', N'0', N'0', N'注意對公眾的影響', N'Be aware of the impact on the public', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E5B3DC92-1DE3-4D5C-800D-3B56F5231BC6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'156', N'37', N'37', N'S10208', N'2', N'0', N'0', N'人力提舉', N'Manual lifting', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A14BF85A-DC9F-44BF-A35A-3C1076FDFF85', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'78', N'22', N'22', N'S10104', N'1', N'0', N'0', N'保護措施-保護設備(路軌工作)', N'Safety Measures - Safety Gear (Track Tasks)', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EB09D93A-83F7-4825-B24E-3C509B3623B6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'301', N'22', N'76', N'S1010202', N'1', N'0', N'0', N'使用合適工作台進行高空工作', N'Use a suitable work platform for working at height', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3383CDE0-5DE4-48CC-BCC7-3C691EE8F666', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'219', N'22', N'22', N'S10113', N'1', N'0', N'0', N'其他注意事項', N'Other Notes', N'14', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E972B4B6-D6BB-4D69-9A16-3CB6EF2EFA3D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'302', N'22', N'78', N'S1010403', N'1', N'0', N'0', N'手提燈/信號燈', N'Handlamp/Telltale lamp', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9066E01A-7964-4BE4-835B-3CC4750F6AE7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'67', N'14', N'14', N'B11401', N'0', N'1', N'0', N'正確使用眼罩及口罩', N'Proper Use of Protective Eyewear and Masks
', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0C9BE5E0-41B5-40AC-A877-3EB39500BF0D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'303', N'52', N'216', N'S1070102', N'1', N'0', N'0', N'其他:', N'Others:', N'100', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B09011C7-DC88-46C4-915D-3EFD6970554F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'88', N'23', N'23', N'S10205', N'1', N'0', N'0', N'工作台', N'Working Platform', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'ED3CE6D9-8241-4F78-BE24-3F0F8C5AC857', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'304', N'38', N'165', N'S5010703', N'2', N'0', N'0', N'已檢查撞針無損毀', N'The firing pin has been checked for damage', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'436F4723-1A8E-4F57-8945-3F5A66C0180D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'118', N'29', N'29', N'C10202', N'0', N'0', N'0', N'於當值工務督察巡查時發現的不當行為/偏差事項已修正', N'The improper behaviors/deviations identified during the inspection by the on-duty engineering inspector have been rectified.', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6C5D3FCB-710A-4D8A-ACAD-3FE4DA355279', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'305', N'36', N'141', N'S4010507', N'2', N'1', N'0', N'P/N牌員工須由有經驗的指導員陪伴工作', N'P/N employees must be accompanied by an experienced instructor', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2CB459E0-A104-4C9C-A7B1-40366A98AFD5', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'306', N'44', N'189', N'SI1010901', N'2', NULL, N'0', N'已由行車控制主任(TC)/調車場主管(YM)/工程王管(EPIC)取得授權進人軌道。(如有需要時)', N'Obtained track access authorization from TC/YM/EPIC. (if necessary)', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8C087DB7-D316-4939-B24C-407CC7F43F96', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'307', N'22', N'214', N'S1011002', N'1', N'0', N'0', N'機房', N'Engine Room', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BFB592F4-7E8A-4FFA-90D5-40B0172E5F86', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'308', N'36', N'138', N'S4010201', N'2', N'0', N'0', N'正常', N'Normal', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'01B746B6-B1CA-47EA-86D9-41D60A3173B4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'309', N'52', N'217', N'S1070201', N'1', N'0', N'0', N'注意人力抬舉', N'Pay attention to Manual Handling', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5B36BAAA-B4D8-4EA9-A1FD-4207D8DCB80A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'310', N'22', N'81', N'S1010712', N'1', N'0', N'0', N'雨衣', N'Raincoat', N'12', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'22EE0790-2EA2-4CCF-A042-42E4F4522C55', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'311', N'23', N'89', N'S1020605', N'1', N'0', N'0', N'滅火器具正常', N'Fire extinguisher is normal', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'26A362DB-3D34-4B86-84FB-43762D59B1BE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'312', N'36', N'145', N'S4010904', N'2', N'0', N'0', N'保護手套', N'Protective Gloves', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2CFB2395-831E-432F-B122-43CA36B31C5D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'143', N'36', N'36', N'S10107', N'2', N'0', N'0', N'保護措施(路軌工作)', N'Safety Precautions (Track Work)', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EBE4C365-4D03-45B5-89EF-4426ED3D32A8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'218', N'22', N'22', N'S10112', N'1', N'0', N'0', N'環境影響', N'Environmental Impact', N'13', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'465E3178-54AB-4D82-9D1B-457FC6572E26', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'9', N'9', NULL, N'B109', N'0', NULL, NULL, N'需要在機電房間工作', N'Require working in the Mechanical and Electrical room', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9322E014-2C7C-4B37-AF7D-45A23C17FDA5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'313', N'36', N'142', N'S4010605', N'2', N'0', N'0', N'提供/使用吹風機', N'Hair dryer available/used', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9C15EEDB-36A9-4761-8094-46602705575B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'314', N'22', N'75', N'S1010105', N'1', N'0', N'0', N'墮下', N'Fall', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5579FAEB-4AE7-475E-9023-4662F1384E82', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'315', N'36', N'141', N'S4010512', N'2', N'1', N'0', N'本人確定CP(NT)、FM及進入路軌工作人士持有有效RSI的資歷', N'I confirm that CP(NT), FM and those entering track work have valid RSI qualifications', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D1682D8F-F823-4550-8FFB-4758254FF312', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'97', N'24', N'24', N'S10308', N'1', N'0', N'0', N'健康(塵埃、噪音、危險物質)', N'Health Safety (Dust, Noise, Toxic Substances)', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7DFED454-E8C8-466C-A197-47E7BF77D619', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'316', N'46', N'195', N'SI1030301', N'2', NULL, N'0', N'其他(如有需要時):', N'Other (if necessary): ', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'91EDCA61-D44D-4885-AE5D-4850292FA6CC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'215', N'22', N'22', N'S10111', N'1', N'0', N'0', N'天氣狀況', N'Weather conditions', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3F6E0992-45DD-4BF8-9A87-4890218C7B1E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'317', N'36', N'141', N'S4010502', N'2', N'1', N'0', N'正確使用工具及個人防護裝備', N'Correct use of tools and personal protective equipment', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FCE365C5-CBB8-4C5D-A59D-489492731A61', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'318', N'36', N'145', N'S4010902', N'2', N'0', N'0', N'螢光衣', N'High Visibility Vest', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'22B6817A-E531-40B2-AA4F-49179057C6CD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'319', N'22', N'220', N'S1011406', N'1', N'0', N'0', N'非行車時間', N'Non-Traffic Hours', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'11AE3F78-1A68-44B8-BC60-4923BD5A45D0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'204', N'49', N'49', N'SI20201', N'2', N'0', N'0', N'4', N'4', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C01EE30B-A42D-46E7-B1DE-493A5630F692', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'94', N'24', N'24', N'S10305', N'1', N'0', N'0', N'電氣(開關掣、電箱、電線)', N'Electrical Components (Switch, Panel Box, Cables)', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F3DF2363-427A-4B62-91F1-494DA0466D48', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'320', N'41', N'178', N'S5040104', N'2', N'0', N'0', N'通道保持暢通', N'Keep the channel open', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C2E37539-D8D4-4424-9487-4A0126C2232E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'321', N'38', N'160', N'S5010207', N'2', N'0', N'0', N'損毀(即時停用.更換)', N'Damage (immediate deactivation and replacement)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'299BD716-6DEC-49E8-ABB1-4A0404029A23', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'322', N'47', N'199', N'SI1040401', N'2', NULL, N'0', N'已通知值日站長(SC)/調車場主管(YM)將有關的道岔調回正常。(如有需要時)', N'Informed SC/YM to throw the point where the point has been scotched during replacement work. (if necessary)', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'76054B2E-5E4C-4C2C-B1D4-4A701AF69E21', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'323', N'22', N'220', N'S1011405', N'1', N'0', N'0', N'行車時間', N'Traffic Hours', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0BE56E53-0C7E-4D7C-9D33-4A825D0D6F36', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'324', N'22', N'218', N'S1011201', N'1', N'0', N'0', N'噪音', N'Noise', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3D47F008-3AAA-4CCA-9F05-4BB4544E7BA5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'325', N'23', N'85', N'S1020204', N'1', N'0', N'0', N'合適護罩', N'Suitable shield', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'962AA66E-7D30-4AD5-9844-4BC6140E2E9C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'82', N'22', N'22', N'S10108', N'1', N'0', N'0', N'應急安排', N'Emergency Procedures', N'11', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8D791010-00B7-4E5F-843F-4BCC3A0BFA58', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'326', N'45', N'191', N'SI1020103', N'2', NULL, N'0', N'使用紅閃燈數目:', N'No. of RFL used:', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'74568AA4-8956-4FAB-89B9-4BF9F016C43E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'327', N'36', N'139', N'S4010302', N'2', N'1', N'0', N'相關施工方案#', N'Related construction plans#', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3D355FE2-91BF-40C8-A630-4C5FED2B772F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'328', N'36', N'141', N'S4010508', N'2', N'1', N'0', N'提供清晰工作指示予P/N員工', N'Provide clear work instructions to P/N employees', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'81732B67-ED46-4E62-B623-4C9290F11F75', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'329', N'22', N'81', N'S1010706', N'1', N'0', N'0', N'面盾', N'Face shield', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'96BCA034-4655-4036-A90C-4CB4293D09FA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'330', N'22', N'75', N'S1010113', N'1', N'0', N'1', N'需要向 EPIC 攞路 Transaction Number: ', N'Need to submit Transaction Number to EPIC:', N'13', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'31EBCB73-EFEC-41AA-A4AA-4D1BE6E7CABE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'158', N'37', N'37', N'S10210', N'2', N'0', N'0', N'酷熱天氣', N'Very Hot Weather', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EFAC20DD-72C1-4399-8C69-4D6825DF685D', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'331', N'44', N'183', N'SI1010301', N'2', NULL, N'0', N'已與行車控制主任(TC)/值日站長(SC)/調車場主管(YM)/工程主管(EPIC)協定/進入軌道的方法、在軌道停留的時間、保護措施、特別指示和通訊方法。', N'Agreed with TC/SC/YM/EPIC on the means for track access, duration on track, protection arrangements, special instructions and means of communication', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'83749799-694A-4304-8A56-4E6021E29057', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'106', N'27', N'27', N'S10606', N'1', N'0', N'0', N'排水系統保持暢通無阻', N'Ensure the drainage system remains unobstructed and flowing smoothly.', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A6353A4E-AABB-4867-8EC1-4EA0DB837E44', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'202', N'48', N'48', N'SI20102', N'2', N'0', N'0', N'2', N'2', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2BBF1760-154B-4B69-8438-4EFA7E6D8BE4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'86', N'23', N'23', N'S10203', N'1', N'0', N'0', N'手工具', N'Manual Equipment', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4800E911-D2CB-4161-A326-505923CE013A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'332', N'38', N'160', N'S5010203', N'2', N'0', N'0', N'機身沒有損毀', N'No damage to the fuselage', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C59E617C-35A0-4601-887A-50CB648F6AC8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'333', N'22', N'77', N'S1010301', N'1', N'0', N'0', N'密閉空間器具', N'Confined Space Equipment', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A5CC50F4-C379-4007-920C-511EA811B019', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'334', N'22', N'82', N'S1010801', N'1', N'0', N'0', N'*緊急集合地點:', N'*Emergency Assembly Point:', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6AB3F42F-7471-4F28-949B-52325859CE1A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'335', N'22', N'215', N'S1011108', N'1', N'0', N'0', N'其他:', N'Others:', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6D207288-815F-4301-97BA-5257B5EB2365', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'336', N'23', N'88', N'S1020504', N'1', N'0', N'0', N'安全上落通道', N'Safe access', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'97944D7D-19D9-41ED-803E-538440A193A2', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'337', N'22', N'76', N'S1010209', N'1', N'0', N'0', N'已替工作會涉及的港鐵站系統進行隔離及掛牌  其他:', N'The MTR station systems involved in the works have been isolated and marked Others:', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'17013C49-726D-4FBF-9FDC-53F0FBEA346A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'10', N'10', NULL, N'B110', N'0', NULL, NULL, N'照明不足', N'Insufficient Lighting', N'10', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EB73A8B7-CB77-448A-BCC8-5425EED8926D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'2', N'2', NULL, N'B102', N'0', NULL, NULL, N'物件下墮', N'Fall of Object from height', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E33C214C-A04A-465B-A3D7-54A1C3E1F6E2', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'5', N'5', NULL, N'B105', N'0', NULL, NULL, N'滑倒/絆倒(工地欠整理)', N'Slip/Trip(due to poor housekeeping on the construction site)', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A6CFA900-C2A1-48B6-835B-5533900DBF06', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'338', N'47', N'198', N'SI1040301', N'2', NULL, N'0', N'已向值日站長(SC)/調車場主管(YM)報告，已完成撤離軌道及所有相關保護措施已移除。', N'Informed SC/YM the track status has resumed normal and all related safety protections have been removed.', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3638B6BD-881A-4DBF-8539-55A86EC86DAC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'339', N'36', N'141', N'S4010505', N'2', N'1', N'0', N'使用合適工作台進行高空工作', N'Use a suitable work platform for working at height', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7C9F630D-6311-4499-BF30-55FE667DB3AA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'4', N'4', NULL, N'B104', N'0', NULL, NULL, N'進行體力處理時扭傷', N'Sprain during physical handling', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7E896874-CABF-4BFF-AE4F-56EA060E49B6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'340', N'38', N'165', N'S5010701', N'2', N'0', N'0', N'已檢查有效日期', N'Checked validity date', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'76EEE402-13CE-4076-9DAD-57BC3A6B133F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'341', N'23', N'85', N'S1020202', N'1', N'0', N'0', N'機身沒有缺損', N'No damage to the fuselage', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BBFAAC3A-3A4E-4D99-ADF5-57D878FDB460', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'342', N'22', N'81', N'S1010705', N'1', N'0', N'0', N'眼罩', N'Goggles', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'94C0F0AA-3929-4258-8158-5819FF2A5F79', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'343', N'36', N'145', N'S4010905', N'2', N'0', N'0', N'耳罩/耳塞', N'Ear Muff/Plugs', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4B4BE088-9E5E-479F-A807-587CC7677239', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'344', N'38', N'163', N'S5010506', N'2', N'0', N'0', N'正確安裝圍欄及踢腳板', N'Correct installation of fences and skirting boards', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7496F8C2-F4CF-451A-90A8-59333AB89E62', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'345', N'36', N'144', N'S4010807', N'2', N'0', N'0', N'鑽探/連接工程許可證', N'Drilling/Connection Works Permit', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'93CDCD38-584D-4745-9F25-5933FC606AE7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'346', N'23', N'85', N'S1020206', N'1', N'0', N'0', N'損毀(即時停用)', N'Damaged (immediate deactivation)', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E30FE9ED-FCDD-4F4C-8064-59432B67F271', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'347', N'38', N'159', N'S5010101', N'2', N'0', N'0', N'損毀(即時停用.更換)', N'Damage (immediate deactivation and replacement)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'33BE88E7-E2DB-4543-BCD0-59D00F4E12B1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'48', N'48', NULL, N'SI201', N'2', NULL, NULL, N'進入軌道前', N'Prior to Track Access', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6FAE5DD6-0CA5-421F-B7BC-5A7CE4505EB5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'19', N'19', NULL, N'B119', N'0', NULL, NULL, N'窒息(自動消防系統房間)', N'Suffocation (Automatic Fire Suppression system room)', N'19', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D468A068-2AFD-4AA2-AA22-5ACC1BDFCBA1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'348', N'36', N'146', N'S4011003', N'2', N'0', N'0', N'滅火筒', N'Fire Extinguisher', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'54B56DD9-289D-4414-8A91-5B8294522720', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'349', N'23', N'84', N'S1020103', N'1', N'0', N'0', N'損毀(即時停用)', N'Damaged (immediate deactivation)', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8DF62F00-447E-4C21-9443-5BDB36EFDDC6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'350', N'22', N'215', N'S1011105', N'1', N'0', N'0', N'颱風', N'Typhoon', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0FF0438D-DEC6-428F-97B8-5CBDA94EC2CB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'351', N'22', N'80', N'S1010601', N'1', N'0', N'0', N'*建造業工人註冊證/建造業安全訓練證明書(安全卡)', N'*CIC/Green Card', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A9C10006-3D9C-4280-BFBC-5CF521D76E81', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'163', N'38', N'38', N'S10105', N'2', N'0', N'0', N'工作台', N'Working Platform', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DFE27174-3809-4B16-9296-5CF98B173A51', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'352', N'37', N'158', N'S4021002', N'2', N'0', N'0', N'多飲水，每小時小休10分鐘，如有不適，應停止工作', N'Drink plenty of water, take a 10-minute break every hour, and stop working if you feel unwell.', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'55B06255-CCAD-42FC-8B76-5D0836998F5E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'353', N'36', N'143', N'S4010706', N'2', N'0', N'0', N'止輪器', N'Scotch Block', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F6B73BC8-8342-4AB1-BB1F-5D5D2E939791', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'354', N'46', N'194', N'SI1030201', N'2', NULL, N'0', N'已確認所有保護措施(包括 紅閃燈、接地棒及固定道岔等)，已撤離軌道。', N'Confirmed all Red Flashing Lights (RFL) and Earthing Rods have been removed from track and removed point secured.', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'135CF897-864D-4D62-AACE-5D6911CF8CAD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'30', N'30', NULL, N'C103', N'0', NULL, NULL, N'完工後', N'Post construction', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'38AAED88-358E-4E54-8493-5D79A7D87D56', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'355', N'22', N'79', N'S1010503', N'1', N'0', N'0', N'鑽探工程許可證PTW (Drill):', N'Drilling Works Permit PTW (Drill):', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'71F2F0FA-2F70-4CDC-8156-5DDC50591C9E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'356', N'22', N'78', N'S1010408', N'1', N'0', N'0', N'不適用', N'N/A', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7AEB09C1-2B46-4893-9343-5E26EABB57A3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'357', N'37', N'152', N'S4020402', N'2', N'0', N'0', N'必須使用合規格工作台，配戴及扣上安全帶', N'Must use a workbench that meets the standards and wear and fasten a safety belt', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C4138DA4-E205-494F-B1F4-5E42385FD3FD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'8', N'8', NULL, N'B108', N'0', NULL, NULL, N'需要隔離電力', N'Require Electrical Isolation', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'478329DE-66FD-4024-9C63-5E9D5C74E2CB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'166', N'38', N'38', N'S10108', N'2', N'0', N'0', N'急救箱', N'First aid kit', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C9C366A5-B65E-4344-88B3-5F2309867D7D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'358', N'36', N'143', N'S4010707', N'2', N'0', N'0', N'接地捧(必須先用試電捧)', N'Earthing Rod(Must use Live Line Tester first)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C9C57C9E-D81A-4CB3-AB7A-5FD8ECBB5527', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'125', N'32', N'32', N'C20201', N'0', N'0', N'0', N'施工前，工序的施工方案/ 有關規則/程序/工作指示/圖則是否已獲批准? ', N'Has the construction plan/rules/procedures/job instructions/drawings for the construction process been approved before construction?', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AA8A62CF-9B3D-44B2-B34B-5FF29B714FE8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'359', N'23', N'88', N'S1020502', N'1', N'0', N'0', N'已貼檢查標籤', N'Inspection label attached', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B185B657-7AD1-4B71-B1FD-6080041D186A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'105', N'27', N'27', N'S10605', N'1', N'0', N'0', N'電源及電掣板已關上', N'The power and switches have been properly shut off.', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C2BE6B72-E92A-45D4-BEC0-60922A0E6F23', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'360', N'41', N'178', N'S5040105', N'2', N'0', N'0', N'所有地洞圍好/封蓋及有告示', N'All holes are enclosed/capped and posted', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1D0461DD-3F7A-49FB-9D02-60D49ABB8A0A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'361', N'36', N'145', N'S4010911', N'2', N'0', N'0', N'工作保護服', NULL, NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B07D1983-CAB2-4E1F-AC97-60D779B0BB7B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'189', N'44', N'44', N'SI10109', N'2', N'0', N'0', N'9', N'9', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C75AAEEF-052A-4BC7-8A1C-61DC7B6CC6F1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'362', N'36', N'148', N'S4011201', N'2', N'0', N'0', N'技能/經驗/人手不足*', N'Skills/Experience/Manpower Shortage*', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5125DF84-770B-4C33-9B3E-61E645B378E2', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'183', N'44', N'44', N'SI10103', N'2', N'0', N'0', N'3', N'3', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BAC5AFA8-3A17-48D9-9327-61F683B6B720', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'76', N'22', N'22', N'S10102', N'1', N'0', N'0', N'工作前注意事項', N'Pre-Work Safety Precautions', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8CC6E3F0-981B-4AEE-91F3-630788580B07', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'190', N'44', N'44', N'SI10110', N'2', N'0', N'0', N'10', N'10', N'10', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BBA8E7E9-0E26-4EDF-BF10-632C370C2139', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'363', N'37', N'154', N'S4020602', N'2', N'0', N'0', N'申請及遵循熱工序許可証，使用合規格焊接工具，擺放有效滅火器', N'Apply for and comply with hot work permits, use qualified welding tools, and place effective fire extinguishers', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'18B63A03-FEFD-43A4-866F-63B28AD50C99', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'364', N'23', N'88', N'S1020503', N'1', N'0', N'0', N'棚架表格五有效', N'Scaffolding Form 5 Valid', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E3CF1D21-BB85-482D-98AA-63F8C6B12F37', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'365', N'38', N'166', N'S5010801', N'2', N'0', N'0', N'狀態良好', N'Good condition', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BFC2FC74-0166-43AC-A2F7-6486DED212C1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'102', N'27', N'27', N'S10602', N'1', N'0', N'0', N'工具及物料放回儲存區並鎖好', N'Restore Tools And Materials To The Storage Space And Secure With Locking Mechanism', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C15D5A63-E87B-4B3E-B21E-663AF804E943', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'366', N'43', N'180', N'S5060104', N'2', N'0', N'0', N'許可證工序', N'Permit Process', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'86A14185-8D4C-4D73-9291-6648A79694A8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'367', N'36', N'144', N'S4010803', N'2', N'0', N'0', N'試驗許可證SFT', N'Sanction-For-Test', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'904AEEDF-07B0-45D2-BDD3-67469F19557A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'368', N'22', N'75', N'S1010106', N'1', N'0', N'0', N'狹窄空間', N'Limited Space', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6DC1F21B-F7FB-45AB-95B4-675A827DF4AA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'47', N'47', NULL, N'SI104', N'2', NULL, NULL, N'撤離軌道後', N'After Track Clear', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B5C13722-4AA4-42CF-9D8D-67962B961A85', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'184', N'44', N'44', N'SI10104', N'2', N'0', N'0', N'4', N'4', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B36B597F-D1CA-4CFC-9FFE-67B225A5AC70', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'369', N'22', N'219', N'S1011307', N'1', N'0', N'0', N'簡述工作內容(程序號碼/工作指引/施工方法說明):', N'Briefly describe the work(Procedure/WI/Method Statement No.):', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D8C092C2-1BD2-433C-8D7E-67C47BFB1F62', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'140', N'36', N'36', N'S10104', N'2', N'0', N'0', N'工作地點注意事項', N'Workplace Precautions', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C0D07B5C-DF8B-4FBB-BAC1-681E592C66AA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'370', N'38', N'160', N'S5010204', N'2', N'0', N'0', N'合適護罩', N'Suitable shield', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2982152F-2D34-4324-A2EE-68354C34F729', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'371', N'22', N'77', N'S1010302', N'1', N'0', N'0', N'圍欄', N'Railing', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'759CAD02-9F48-44B2-9173-68EA6BF8149E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'186', N'44', N'44', N'SI10106', N'2', N'0', N'0', N'6', N'6', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A41C23F1-3459-49B1-9E00-6958BAFDB5AC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'372', N'23', N'89', N'S1020603', N'1', N'0', N'0', N'足夠圍封', N'Enclosed enough', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5FCC49A0-04A2-4582-B5DD-6965365543D0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'373', N'22', N'75', N'S1010102', N'1', N'0', N'0', N'通風欠佳', N'Poor Ventilation', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FB128872-D40F-4CE4-929C-69D0530D2792', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'148', N'36', N'36', N'S10112', N'2', N'0', N'0', N'工作人員', N'Staff', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5AFB95F9-B2E2-4788-9AA8-6B15146CCBA8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'374', N'36', N'145', N'S4010909', N'2', N'0', N'0', N'雨衣', N'Raincoat', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'872A203C-E4FD-471F-B7FE-6B6D6B9C48FB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'31', N'31', NULL, N'C201', N'1', NULL, NULL, N'工作人員資格及訓練', N'Workers Qualifications and Training', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DA65599F-C0CF-4E06-BC24-6C4DACB0FA20', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'154', N'37', N'37', N'S10206', N'2', N'0', N'0', N'熱工序工作', N'Hot process work', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3ECB1033-6734-41D3-8261-6DE4FE20AE28', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'23', N'23', NULL, N'S102', N'1', NULL, NULL, N'開工前檢查記錄', N'Pre-Work Inspection Record', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C97C6143-DB24-4959-BCD8-6DF35C26CEC8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'52', N'52', NULL, N'S107', N'1', NULL, NULL, N'安全檢討會議', N'Safety Review Meeting', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'02F20F9F-70E4-443E-8AB9-6EC16694C0DC', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'375', N'48', N'202', N'SI2010201', N'2', NULL, N'0', N'已確認CP(T)在進入軌道現場，使用SafeTrack App掃瞄QR code，取得授權進入軌道。(包括 在現場確認及SafeTrack App授權畫面截圖等)', N'Confirmed at the scene of track access, CP(T) has applied SafeTrack App to scan the QR code for obtaining track access authorization. (Included confirmed at the scene and record of SafeTrack App screen capture etc.)', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'69000E7B-6565-4023-AB0B-6F08E8E6C5DA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'376', N'23', N'89', N'S1020606', N'1', N'0', N'0', N'其他:', N'Others:', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BEFBEF5B-2385-49B3-94BF-6F7AF44E039D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'214', N'22', N'22', N'S10110', N'1', N'0', N'0', N'工前熱身操', N'Warm-up exercises before work', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'247D5F65-5C77-481C-A279-6FA5F18AAC9F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'377', N'36', N'141', N'S4010511', N'2', N'1', N'0', N'已替工作會涉及的港鐵站系統(電力/消防/其他:            )進行隔離及掛牌', N'The MTR station systems (electricity/fire protection/others:            ) involved in the work have been isolated and marked', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C00EE074-7494-49B8-B9A8-6FF8B5DFA8C8', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'378', N'44', N'184', N'SI1010401', N'2', NULL, N'0', N'已向工作隊進行開工前安全簡報、危害分析及保護措施(包括講述 Track Diagrams), 並完成相關記錄( 包括開工前安全簡報記錄及錄音等)。', N'Delivered pre-work safety briefing & hazards identification & protection arrangement (included: Track Diagrams explanation) and completed related record (included Pre-work safety briefing record and voice recording etc.)', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B85FE6A4-3C08-4CB2-904A-7025B8AAC56F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'379', N'22', N'221', N'S1011510', N'1', N'0', N'0', N'空氣污染', N'Air Pollution', N'10', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5FCAED42-0A6F-490D-BA6A-70822DA05E4B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'380', N'22', N'79', N'S1010507', N'1', N'0', N'0', N'工作範圍限製證 LOA', N'Limitation of Access (LOA)', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FF19D2E5-2FD3-4503-8CCF-72D74A7BBF3F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'381', N'22', N'78', N'S1010405', N'1', N'0', N'0', N'止輪器', N'Scotch Block', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'CB177FA2-2853-4AE5-BF34-732D8719F480', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'382', N'38', N'164', N'S5010603', N'2', N'0', N'0', N'足夠圍封', N'Enclosed enough', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'395ED13F-0B24-4434-9D64-73C33853910A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'383', N'22', N'81', N'S1010711', N'1', N'0', N'0', N'防電弧保護服', N'Arc Flash Suit', N'11', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'63BED4D6-76AA-4ABF-9AE6-73E5621E724D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'384', N'36', N'143', N'S4010704', N'2', N'0', N'0', N'手提燈/信號燈', N'Handlamp /Telltale lamp', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'710BC6DF-AB26-49B1-B1D1-747B9850F55C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'385', N'36', N'140', N'S4010403', N'2', N'0', N'0', N'通風欠佳*', N'Poor Ventilation*', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4FB44F20-F33F-4197-9F76-75BA6653087B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'386', N'36', N'142', N'S4010603', N'2', N'0', N'0', N'已配備照明系統', N'Equipped with lighting system', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C2D7DF9E-E9C9-4B33-98C1-76145CD67E1F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'387', N'22', N'78', N'S1010406', N'1', N'0', N'0', N'接地棒(必須先用試電棒)', N'Earthing Rod(Must use Live Line Tester first)', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'97F3AFEB-6AC9-49E8-A46C-76638B9B2209', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'388', N'22', N'76', N'S1010213', N'1', N'0', N'0', N'避免安排新到場 P/N 牌員工進行高風險工作', N'Avoid assigning new P/N badge employees to high-risk tasks', N'13', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'80F0F187-3F46-406F-9F65-76C82A74CE98', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'75', N'22', N'22', N'S10101', N'1', N'0', N'0', N'工作地點注意事項', N'Work Site Safety Precautions', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4053547E-0947-4A87-9977-76DE2E6D01D5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'389', N'36', N'143', N'S4010703', N'2', N'0', N'0', N'路軌夾', N'Rail Clamp', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'13D55D69-3622-4327-9AF2-771743FAFED3', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'390', N'47', N'197', N'SI1040201', N'2', NULL, N'0', N'已通知行車控制主任(TC)/調車場主管(YM)/工程主管(EPIC)，撤離軌道及有關軌道回復正常。(如有需要時)', N'Informed TC/YM/EPIC to cancel the track access and confirmed track status has resumed normal. (if necessary)', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E7941185-91B0-4C65-BC94-77B8215F4C00', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'391', N'22', N'221', N'S1011511', N'1', N'0', N'0', N'交通車輛', N'Traffic Vehicle', N'11', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5A8E1F15-3903-4B7F-B942-77FA6AE83977', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'392', N'22', N'221', N'S1011509', N'1', N'0', N'0', N'酷熱環境', N'Very Hot Environment', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'267C3544-1C7D-4D84-8C7F-782F50A377DA', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'393', N'45', N'191', N'SI1020102', N'2', NULL, N'0', N'在軌道上工作隊人數:', N'No. of Person on Track:', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'87AE9DC8-B352-4287-BAD1-783EE94EB2B3', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'394', N'44', N'190', N'SI1011001', N'2', NULL, N'0', N'其他(如有需要時):', N'Other (if necessary): ', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4BC63F0A-2917-45B4-A84B-785426669B4D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'395', N'23', N'87', N'S1020401', N'1', N'0', N'0', N'合資格人士已檢查起重裝置(包括升降台)及簽署每週檢查表格一', N'A qualified person has inspected the lifting gear (including lifting platform) and signed the weekly inspection form.', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1E3D4B7A-62F4-4482-837C-785B451E1169', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'396', N'41', N'178', N'S5040107', N'2', N'0', N'0', N'沒有影響公眾環境', N'No impact on the public environment', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F3B797FC-8EC7-4C37-8456-7860B071BDAC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'397', N'22', N'221', N'S1011501', N'1', N'0', N'0', N'軌道上或附近', N'Track or Trackside', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'458A48AC-8BC1-43D8-B671-7911660E6293', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'207', N'51', N'51', N'SI20401', N'2', N'0', N'0', N'7', N'7', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BC0DA05C-1668-4F76-9385-7A0685081051', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'192', N'45', N'45', N'SI10202', N'2', N'0', N'0', N'12', N'12', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D236F89E-D373-4452-80EB-7A3B7E9F0B44', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'398', N'22', N'77', N'S1010303', N'1', N'0', N'1', N'交通指示牌&指示燈', N'Traffic signs & lights', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'94A6855C-F3A6-4531-A152-7A3E827E9C8F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'399', N'22', N'76', N'S1010212', N'1', N'0', N'0', N'指導員應提醒 P/N 牌員工地盤的特別危險及相關工作要求', N'Instructors should remind P/N employees of the special hazards and related work requirements of the site', N'12', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'561ACA4B-33C4-4B24-99B4-7A58B4412A75', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'129', N'33', N'33', N'C20302', N'0', N'0', N'0', N'首次使用物料前，已與港鐵人員進行驗收', N' Prior to the initial use of the materials, verification was conducted with MTR personnel.
', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A5171392-0B3D-44B7-8BE5-7ABC50AA0136', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'400', N'38', N'166', N'S5010803', N'2', N'0', N'0', N'已檢查急救手冊', N'Checked the first aid manual', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4DDAA61F-D5E7-45B1-9123-7B333A75DA5C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'401', N'22', N'77', N'S1010304', N'1', N'0', N'1', N'其他:', N'Others:', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A91D2360-8521-4CC9-9FDF-7BC08A334E2A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'178', N'41', N'41', N'S10401', N'2', N'0', N'0', N'工地整潔', N'Clean construction site', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2EF58CC0-F3D2-4777-B035-7BC47CFAD559', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'161', N'38', N'38', N'S10103', N'2', N'0', N'0', N'手工具', N'Manual Equipment', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'82E5F4CC-9460-41D3-AF21-7C18C59AA7FA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'402', N'22', N'76', N'S1010203', N'1', N'0', N'0', N'正確人力提舉姿勢', N'Correct manual lifting posture', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'13ABCD71-3995-4531-B09A-7CAC5631A32B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'403', N'22', N'83', N'S1010903', N'1', N'0', N'0', N'觀察四周環境以辨識潛在風險', N'Observe surroundings to identify potential risks', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2CC0FF44-B0CA-4B1A-8CD7-7CE1143B8164', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'404', N'22', N'75', N'S1010110', N'1', N'0', N'0', N'電器設備跳火', N'Electrical equipment sparks', N'10', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FD0845FB-2F7F-4092-96CC-7D2B1B9FDFD5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'405', N'38', N'163', N'S5010507', N'2', N'0', N'0', N'工作台使用手提工具已繫上手繩', N'The hand tools used on the workbench are tied with hand straps', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A9A2F4C5-FBD9-4E86-ACB0-7D446D287A27', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'68', N'15', N'15', N'B11501', N'0', N'1', N'0', N'佩戴合適保護耳塞及耳罩', N'Wear Appropriate Protective Earplugs and Earmuffs', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'218D1741-0FED-4D37-9700-7D95F1502AC3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'145', N'36', N'36', N'S10109', N'2', N'0', N'0', N'個人保護裝備', N'Personal Protective Equipment ', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B7B8353A-035F-403F-AAF3-7E20C7256EC6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'171', N'39', N'39', N'S10205', N'2', N'0', N'0', N'使用合規格工作台進行高空工作', N'Use a qualified work platform for working at height', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'074D68F1-8801-4AE3-BB85-7F0230A9018E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'51', N'51', NULL, N'SI204', N'2', NULL, NULL, N'撤離軌道後', N'After Track Clear', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'01F0C98E-AC6D-477D-ABE9-808668DE2713', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'406', N'22', N'76', N'S1010204', N'1', N'0', N'0', N'*正確使用工具及個人防護裝備', N'* Correct use of tools and personal protective equipment', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'ED524D15-08B7-49B9-94CD-80CF40F65E1C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'407', N'51', N'207', N'SI2040101', N'2', NULL, N'0', N'已確認CP(T)在離開軌道現場，使用SafeTrack App掃瞄QR code，確認已撤離軌道及有關軌道回復正常。( 包括 在現場確認及SafeTrack App 授權畫面截圖等)', N'Confirmed at the scene of track access, CP(T) has applied SafeTrack App to scan the QR code for cancelling the track access and confirmed track status has resumed normal. (Included confirmed at the scene and record of SafeTrack App screen capture etc.)', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'43ECE9D3-22AE-4EAE-A882-80DA73C29701', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'408', N'36', N'148', N'S4011202', N'2', N'0', N'0', N'精神欠佳/疲勞/治療中/身體不適/濫用毒品或酒精*', N'Low spirits/fatigue/medical treatment/physical discomfort/drug or alcohol abuse*', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2C9BC106-AC25-4859-8833-80FC9DA27EC4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'409', N'36', N'141', N'S4010501', N'2', N'1', N'0', N'簡述工作危害(HIA)', N'Brief description of Hazards at Work (HIA)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D1A6AD13-71C1-4867-A003-81135A0879EE', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'410', N'45', N'191', N'SI1020105', N'2', NULL, N'0', N'固定道岔位置:', N'Point secured at: ', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7E789D64-441A-436E-B96A-813DCF232025', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'188', N'44', N'44', N'SI10108', N'2', N'0', N'0', N'8', N'8', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4D301FE4-6D2C-45CD-92CB-82454A7A5A6F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'411', N'43', N'180', N'S5060101', N'2', N'0', N'0', N'高空工作', N'Working at height', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5233FB63-C7BA-4929-B783-8287643B5D6D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'199', N'47', N'47', N'SI10404', N'2', N'0', N'0', N'19', N'19', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'368FA6D4-734F-4049-9F2D-82A6F7F4ABE3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'412', N'41', N'178', N'S5040109', N'2', N'0', N'0', N'其他:_____________________________', N'Others:_____________________________', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E8D2558B-C3D7-4A1F-976F-838EEB6AEE6A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'194', N'46', N'46', N'SI10302', N'2', N'0', N'0', N'14', N'14', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C267FC3A-BE83-4F3E-B96B-8432F28822E7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'413', N'51', N'208', N'SI2040201', N'2', NULL, N'0', N'CP(T)已確認在完工後，當所有人員已攜同所有工具和物料撤離軌道後，通知值日站長(SC)/調車場主管(YM)，以及通知行車控制主任 (TC)/工程主管(EPIC)。(如有需要時) (例子 檢査通話記錄等)', N'CP(T) confirmed that on completion of work, he/she has reported to SC/YM and TC/EPIC when all persons, tools and materials have been cleared off the track. (if necessary)(e.g. Check telephone record etc.)', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3C9955A6-888B-4138-AE97-844E5885ABFC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'72', N'19', N'19', N'B11901', N'0', N'1', N'0', N'進入自動消防系統房間前須確保已隔離', N'Ensure Isolation before entering the Automatic Fire Suppression System Room', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FD1C2F86-94AC-44DD-A1CD-8530D640362C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'414', N'36', N'141', N'S4010506', N'2', N'1', N'0', N'正確人力提舉', N'Correct manpower lifting', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7BC0B44E-F862-4C98-A588-85A48D613F56', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'180', N'43', N'43', N'S10601', N'2', N'0', N'0', N'下一個工作日工序及注意事項', N'Next working day procedures and precautions', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'CD45A9AD-1DAC-43B9-A208-85F1C3012A81', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'415', N'38', N'166', N'S5010804', N'2', N'0', N'1', N'其他:', N'Others:', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AC18885D-8EED-46A0-8C4E-86397E1D35C5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'12', N'12', NULL, N'B112', N'0', NULL, NULL, N'火警(易燃品及可燃雜物)', N'Fire Alarm (Due to Flammable materials and Combustible Debris)', N'12', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7F8A1AA5-6705-401B-B210-86A9223B050B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'110', N'27', N'27', N'S10610', N'1', N'0', N'0', N'已向 站長 / TC / EPIC 匯報收工', N' I have reported to the station head/ TC / EPIC that I have finished work.', N'11', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'26071850-DD9A-4DE3-B8DD-873F26543F57', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'416', N'50', N'206', N'SI2030201', N'2', NULL, N'0', N'CP(T)已確認所有保護措施(包括 紅閃燈及接地棒等)，已撤離軌道。', N'CP(T) confirmed that "Track Clear" was conducted and all Red Flashing Lights and Earthing Rods have been removed.', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'838BB448-6143-4456-8A92-87B6E536B2A8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'124', N'31', N'31', N'C20102', N'0', N'0', N'0', N'所有管理人員及工作人員已接受相關訓練並有足夠經驗及由承辦商委任', N'All management personnel and staff have undergone relevant training, possess adequate experience, and are appointed by the contractor.', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7DC60520-4C17-43F5-AE37-87CF9BD4C481', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'417', N'36', N'140', N'S4010405', N'2', N'0', N'0', N'中暑*', N'Heatstroke', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'186C7B2F-AC97-464E-B46C-88D95CD5F930', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'177', N'40', N'40', N'S10301', N'2', N'0', N'0', N'施工安全措施', N'Construction safety measures', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E8F4841C-4AE7-4FA6-A68E-88F7256C1845', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'418', N'36', N'145', N'S4010903', N'2', N'0', N'0', N'安全帽連下頷帶', N'Safety Helmet-Chin Strap', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'986C1FA8-F212-47CA-BB0D-8947D78580AE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'419', N'22', N'79', N'S1010513', N'1', N'0', N'1', N'其他:', N'Others:', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EE790EFE-5A44-4EFA-8645-8A356CB9A07D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'420', N'36', N'144', N'S4010805', N'2', N'0', N'0', N'IRF隔電紀錄表', N'Isolation Record Form', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7D18CEF6-6980-4387-936F-8B1C0E60715E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'160', N'38', N'38', N'S10102', N'2', N'0', N'0', N'電工具', N'Electrical Equipment', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4B1BA7BA-4410-4BC9-B376-8B5D520F516B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'212', NULL, NULL, N'', N'2', N'0', N'0', N'', N'', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FDE245ED-DD39-4F32-AA39-8BE394FD0B80', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'61', N'8', N'8', N'B10801', N'0', N'1', N'0', N'註冊電工隔離電力 姓名:', N'Registered Electrician to Isolate Electricity, Name:', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'05BED13B-CB43-4A5A-8C58-8C96BB303788', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'421', N'22', N'221', N'S1011512', N'1', N'0', N'0', N'禁止進入地點', N'No Entry Area', N'12', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'51F6E2BE-C913-4974-B68D-8D15215D5C15', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'15', N'15', NULL, N'B115', N'0', NULL, NULL, N'聽覺受損(噪音)', N'Hearing Damage (due to Noise)', N'15', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5C308AFE-0E09-4DCD-AC3D-8D24AA738A14', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'170', N'39', N'39', N'S10204', N'2', N'0', N'0', N'使用安全梯台進行離地工作', N'Use a safety ladder to work above ground', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'337410F3-4AB6-418E-9960-8D368DDABC8E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'422', N'36', N'137', N'S4010101', N'2', N'0', N'0', N'建造業安全訓練證明書(安全卡)#', N'Green Card#', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'46A77E2F-DBE1-4DEE-9334-8D9F4B94B421', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'423', N'36', N'145', N'S4010901', N'2', N'0', N'0', N'安全鞋', N'Safety Shoes', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'43AFBE62-0AAD-403A-A074-8E143481B7C3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'424', N'22', N'79', N'S1010508', N'1', N'0', N'0', N'電路隔離證書/電路狀況燈', N'Circuit Isolation Certificate / Circuit State Certificate', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F3633034-2FC5-4676-8423-8E97F6D42F92', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'134', N'34', N'34', N'C20404', N'0', N'0', N'0', N'檢查記錄是否由合資格的人發出?', N'Are the inspection records issued by qualified personnel?', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'357890A7-2CFB-4A9E-94E7-8EBA01A43888', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'221', N'22', N'22', N'S10115', N'1', N'0', N'0', N'工地環境', N'Site Environment', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C8B0B181-7F6D-4A8A-8813-8EF20E33D9EC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'217', N'52', N'52', N'S10702', N'1', N'0', N'0', N'下一個工作天注意事項', N'Things to note on the next working day', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'71CCE5A0-9510-4EA9-89AF-8F8BBFE753A4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'425', N'22', N'80', N'S1010603', N'1', N'0', N'0', N'RSI 資格', N'RSI Qualification', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0D75C442-278D-4A25-9C00-8FD0C86353D5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'426', N'22', N'76', N'S1010210', N'1', N'0', N'0', N'P/N 牌員工須由有經驗的指導員陪伴工作', N'P/N employees must be accompanied by an experienced instructor', N'10', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'44D05DDA-079D-4F3F-8B12-916282116AE9', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'69', N'16', N'16', N'B11601', N'0', N'0', N'0', N'佩戴合適保護手套', N'Wear Appropriate Protective Gloves', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'38222C12-4A66-4A06-A1EA-919B81CCE586', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'91', N'24', N'24', N'S10302', N'1', N'0', N'0', N'嚴格執行熱工序許可證制度', N'Rigorously Implement The Hot Work Permit Protocol', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8BDBC9BF-05BB-4BBE-A5D1-9260E3214549', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'427', N'37', N'150', N'S4020201', N'2', N'0', N'0', N'物料擺放沒有堆疊過高及阻塞通道', N'Materials are not stacked too high or blocking the passage.', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B9634116-B361-4DB9-AEB6-9287EE3938CE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'65', N'12', N'12', N'B11201', N'0', N'1', N'0', N'設置足夠有效滅火筒', N'Provide Sufficient and Effective Fire Extinguishers', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'51626D32-DF4C-46EC-AC32-92996BFD348F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'428', N'22', N'83', N'S1010902', N'1', N'0', N'0', N'簡述及清楚了解「單獨工作的安全指引」卡的安全指引', N'Brief and understand the safety instructions of "Safety Guildenes on Lone Working"Card', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'57CFB1FC-4B2D-45E1-82EE-931A895A829C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'429', N'22', N'81', N'S1010709', N'1', N'0', N'0', N'保護手套', N'Protective Gloves', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6A73F370-B14C-406C-8E2F-931BBE56D660', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'35', N'35', NULL, N'C205', N'1', NULL, NULL, N'其他', N'Others', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E917DAF1-B18F-4290-9FB2-93506BEBF19E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'430', N'36', N'143', N'S4010708', N'2', N'0', N'0', N'標誌牌(Target)', N'Target', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0DABBBDE-ADFF-4D4E-8FD3-9420FB5906F0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'176', N'39', N'39', N'S10210', N'2', N'0', N'1', N'其他:', N'Others:', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BE2BFB62-265E-4A8D-AA66-944BABB5B298', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'46', N'46', NULL, N'SI103', N'2', NULL, NULL, N'撤離軌道前', N'Before Track Clear', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'851EBB9C-9B30-4246-A452-946F6FEA6265', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'431', N'22', N'80', N'S1010604', N'1', N'0', N'0', N'已向沒有鐵路安全資格的訪客或承辦商, 進行鐵路安全簡報 (如適用)', N'Conducted a railway safety briefing to visitors or contractors who do not have railway safety qualifications (If applicable)', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7EED55FC-BD74-4B80-B834-94F99EEDAEA8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'432', N'36', N'139', N'S4010301', N'2', N'1', N'0', N'簡述工作內容#', N'Brief description of work content#', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EA64FCDE-18A6-40E8-8369-95341E6A0803', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'99', N'25', N'25', N'S10401', N'1', N'0', N'0', N'施工安全措施', N'Site Safety Safeguards', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C2713F0A-5617-4DAF-A7B7-9580E2B278CE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'213', N'30', N'30', N'C10304', N'0', N'0', N'0', N'已通知當值工務督察完工(用電話通知)', N'The on-duty Works Inspector has been notified of the completion of the work (notified by phone)', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'93E723DB-A1D6-4FDA-9DAE-95D3BFE04B30', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'433', N'38', N'166', N'S5010802', N'2', N'0', N'0', N'已檢查及貼上2名負責人資料', N'The information of 2 responsible persons has been checked and posted', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3ED0A1DE-0DE6-4E1E-B194-9670FA4B5211', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'135', N'35', N'35', N'C20501', N'0', N'0', N'0', N'出現品質問題 (如有)', N'Quality issues have arisen (if any).', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AE319967-8963-4DEB-8E05-96C2CD6ACD4C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'146', N'36', N'36', N'S10110', N'2', N'0', N'0', N'應急安排', N'Emergency Arrangements', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'46BA50B4-ECE5-4A50-8B70-96EB73D6BF5B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'434', N'22', N'214', N'S1011003', N'1', N'0', N'0', N'其他:', N'Others:', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DF86897C-C011-48CD-8CC6-976D59250EB1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'138', N'36', N'36', N'S10102', N'2', N'0', N'0', N'天氣狀況', N'Weather Condition', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'CCE0FDC4-CD61-442E-A71D-98106AC6BA98', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'435', N'36', N'144', N'S4010804', N'2', N'0', N'0', N'工作範圍限制證LOA', N'Linitation of Access', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7D008663-E6C3-475E-B482-9834B8CF9641', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'27', N'27', NULL, N'S106', N'1', NULL, NULL, N'最後檢查', N'Final Inspection', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5DDB7F12-13CD-479E-B15E-98D4480E4EB0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'436', N'43', N'180', N'S5060105', N'2', N'0', N'0', N'其他:______________________________________________', N'Others:______________________________________________', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'318324E4-D11B-440F-8210-9901B0429AFE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'1', N'1', NULL, N'B101', N'0', NULL, NULL, N'人體下墮', N'Fall of Person from height', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'69F1C04F-6F59-4826-A36B-9990B416510A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'437', N'36', N'142', N'S4010602', N'2', N'0', N'0', N'圍欄', N'Railing', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'81AF9D55-E843-4EF3-AEB6-9A9D95D36ABF', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'103', N'27', N'27', N'S10603', N'1', N'0', N'0', N'機械設備', N'Mechanical Machinery', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'741629D2-29D1-45C4-9E47-9B8026322118', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'438', N'36', N'141', N'S4010509', N'2', N'1', N'0', N'避免安排新到場P/N牌員工進行高風險工作', N'Avoid assigning new P/N employees to high-risk tasks', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'727CC19B-CF31-41CD-8FE4-9CC0C643571D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'169', N'39', N'39', N'S10203', N'2', N'0', N'0', N'正確使用工具及裝備', N'Proper use of tools and equipment', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'539B1D24-4C0C-4A67-8F08-9CDC57FF8866', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'439', N'36', N'146', N'S4011001', N'2', N'0', N'0', N'進出工地路線', N'Access routes to the work site', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'49F37186-83EF-49C8-9B11-9CE5720458EF', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'33', N'33', NULL, N'C203', N'1', NULL, NULL, N'物料品質管理', N'Material Quality Management', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C4961C2D-81FA-4D16-AC0F-9D33FB2170DB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'440', N'22', N'218', N'S1011206', N'1', N'0', N'0', N'燈光滋擾', N'Light nuisance', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'36DF3E34-FDB6-4ECA-B1DC-9D99F84B4350', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'203', N'48', N'48', N'SI20103', N'2', N'0', N'0', N'3', N'3', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'52D2299C-D368-4817-9237-9DA192DB69B3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'210', NULL, NULL, N'', N'2', N'0', N'0', N'', N'', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E0308F24-8C9F-4C61-96BA-9F1D0BD69268', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'77', N'22', N'22', N'S10103', N'1', N'0', N'0', N'保護措施-保護設備(一般工作)', N'Safety Measures - Safety Gear (General Work)', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'985D52AC-4BD7-41EB-A033-9F79AF5FA892', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'164', N'38', N'38', N'S10106', N'2', N'0', N'0', N'工作環境', N'Site Environment', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B36E3582-57EF-4446-BA71-9F87AF395BAA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'95', N'24', N'24', N'S10306', N'1', N'0', N'0', N'防火(滅火筒、煙霧頭)', N'Fire prevention (fire extinguishers, smoke heads).', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7239D21E-3362-4A4C-A46C-9FBD5B228E70', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'441', N'22', N'83', N'S1010905', N'1', N'0', N'0', N'不適用', N'N/A', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1ADFBB9F-45D7-4709-9362-A0A2D267874A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'442', N'22', N'81', N'S1010710', N'1', N'0', N'0', N'高壓絕緣手套', N'Electrical Insulated Gloves', N'10', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F7EDC4EF-4216-4A83-BFF8-A0C9401904B1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'443', N'23', N'84', N'S1020102', N'1', N'0', N'0', N'佩戴方法正確', N'Correct wearing method', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A223093C-7D64-47CC-9CBE-A28087664FA8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'444', N'22', N'81', N'S1010703', N'1', N'0', N'0', N'安全鞋/水鞋', N'Safety Shoes / Water-proof boots', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'33F4913E-2F79-44FA-A4F7-A293DFD04D36', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'36', N'36', NULL, N'S101', N'2', NULL, NULL, N'施工前會議', N'Pre-Construction Meeting', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1DE6BE3E-E5C7-4BF5-8F8B-A34BF4E2A473', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'445', N'22', N'75', N'S1010107', N'1', N'0', N'0', N'隙縫(裂口) ', N'Gap(Crack)', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'376D339E-16FF-4F23-B16C-A3A649ECA872', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'26', N'26', NULL, N'S105', N'1', NULL, NULL, N'收工前清掃', N'Clean-Up before Finishing Work', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'63167119-1371-4660-9290-A3E7C0F6DABF', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'220', N'22', N'22', N'S10114', N'1', N'0', N'0', N'保護措施-地點及時間', N'Protective measures - location and time', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1CDE1BDA-1B7E-46B5-B739-A44AE18516C9', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'446', N'22', N'220', N'S1011404', N'1', N'0', N'0', N'非軌道上或附件工作', N'Work on Non-Track area', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3BCB9643-2F02-4566-96B2-A50F39414A38', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'117', N'29', N'29', N'C10201', N'0', N'0', N'0', N'施工時發現的不安全/不當行為及/或與施工方案/圖則/規格的偏差已修正', N'Any unsafe/improper behaviors or deviations from the construction plans/drawings/specifications identified during construction have been rectified.', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'96D46C54-B5BE-4AD4-8EDD-A57974F61AB2', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'447', N'22', N'82', N'S1010806', N'1', N'0', N'0', N'召集緊急支援', N'Call for Emergency Support', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BADB62D0-8176-4CD5-A338-A59642A6D25F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'62', N'9', N'9', N'B10901', N'0', N'1', N'0', N'註冊電工監察機電房間施行工序', N'Registered Electrician to Supervise the Implementation of Procedures in the Mechanical and Electrical Room', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A26DC386-3DD4-4277-86C6-A5E2BE98D600', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'38', N'38', NULL, N'S201', N'2', NULL, NULL, N'開工前安全巡查記錄', N'Pre-work Safety Inspection Record', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A17EAB03-F192-4C2B-BD6B-A5EC2F52978F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'448', N'22', N'79', N'S1010504', N'1', N'0', N'0', N'挖掘工作許可證PTW (Dig):', N'Excavation Work Permit PTW (Dig):', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EFE75D67-7ECD-44D0-83CA-A635F4FFC981', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'74', N'21', N'21', N'B12101', N'0', N'1', N'0', N'安排定時休息、保充水份及通風設備', N'Arrange Regular Breaks, Ensure Hydration, and Provide Ventilation Equipment', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BE4FBF67-41CA-4BCB-B95F-A6BD0D31DB8E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'449', N'37', N'155', N'S4020702', N'2', N'0', N'0', N'所有電工具使用前，先由合資格電工檢查，貼上安全標誌，電線掛離地面', N'Before using all electrical tools, they must be inspected by a qualified electrician, have safety signs affixed, and have their wires hung off the ground.', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'36C33EC8-82DA-42C9-A435-A6E9553E9596', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'450', N'36', N'142', N'S4010606', N'2', N'0', N'0', N'密閉空間器具', N'Confined Space Equipment', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F9DF71D6-A321-499A-8D95-A7F166BF2192', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'451', N'22', N'75', N'S1010111', N'1', N'0', N'0', N'不需要向 TC 攞路', N'No need to ask TC for directions', N'11', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D60289A1-85C1-4D69-9D65-A888057FB5AB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'195', N'46', N'46', N'SI10303', N'2', N'0', N'0', N'15', N'15', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E21B39DB-C720-4946-AB28-A93F12B7C0E3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'198', N'47', N'47', N'SI10403', N'2', N'0', N'0', N'18', N'18', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7D19A83B-9245-453E-8A86-A981AC9C1D55', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'452', N'22', N'218', N'S1011204', N'1', N'0', N'0', N'利用相關的「消滅噪音措施查核表」完成核實', N'Complete the verification using the relevant "Checklist for Noise Mitigation Measures"', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E6EB076A-DEA3-4F26-ACC0-A9B5B71839CD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'453', N'22', N'75', N'S1010114', N'1', N'0', N'1', N'其他:', N'Others:', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'35D33436-3835-4F5B-BF13-AA8FA1D63F35', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'454', N'23', N'85', N'S1020201', N'1', N'0', N'0', N'已貼檢查標籤', N'Inspection label attached', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'48C489D5-F74B-4433-95E9-AAAEBA1DDDE1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'179', N'42', N'42', N'S10501', N'2', N'0', N'0', N'最後檢查', N'Final Check', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BE5AE3D1-88D5-4324-A69A-AB005FF69CCD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'80', N'22', N'22', N'S10106', N'1', N'0', N'0', N'保護措施-安全文件(證書)', N'Safety Measures - Safety Documentation (Certificates)', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'74013F53-7DAF-46D6-896A-AB6FD07C003F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'101', N'27', N'27', N'S10601', N'1', N'0', N'0', N'工作台上沒有工具或物料', N'Work Platform Free Of Tools Or Materials', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9C527FEA-3F58-4BF2-A6D7-ABF149199912', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'455', N'47', N'200', N'SI1040501', N'2', NULL, N'0', N'其他(如有需要時):', N'Other (if necessary): ', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0FFA2C65-F6DB-4E51-89F7-AD0A4E5BD1F4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'456', N'22', N'221', N'S1011506', N'1', N'0', N'0', N'挖掘工程', N'Excavation Work', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'EF183086-62B0-4CCD-A928-AD10FC2F14C1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'457', N'23', N'89', N'S1020602', N'1', N'0', N'0', N'通道無阻礙', N'Unobstructed passage', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7D9B17C7-2E41-47EF-9FCA-ADBA5812C371', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'458', N'48', N'201', N'SI2010101', N'2', NULL, N'0', N'已確認CP(T)已向工作隊進行開工前安全簡報、危害分析及保護措施(包括 講述Track Diagrams)， 並完成相關記錄(包括 開工前安全簡報記錄及錄音等)。', N'Confirmed CP(T) has delivered pre-work safety briefing & Hazards identification & protection arrangement (included: Track Diagrams explanation) and completed related record (included Pre-work safety briefing record and voice recording etc.)', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A742407D-4B08-4540-B31B-AE056E1A5397', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'459', N'46', N'194', N'SI1030203', N'2', NULL, N'0', N'已移除接地棒數目:', N'No. of Earthing Rod removed:', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'88F79684-F5C9-4428-B838-AE42B7D36D2C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'63', N'10', N'10', N'B11001', N'0', N'1', N'0', N'提供足夠照明', N'Ensure Sufficient Illumination', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AB316EE2-5A8A-4401-B251-AE982D0B8B65', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'79', N'22', N'22', N'S10105', N'1', N'0', N'0', N'保護措施-安全文件(許可證)', N'Safety Measures - Safety Documentation (Permits)', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'724860A5-1D8E-4553-9EB3-AEC4224B399E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'460', N'22', N'75', N'S1010115', N'1', N'0', N'0', N'阻礙物', N'Obstacle', N'14', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'58828BB2-2CDA-40F0-B915-AF0D269E9D82', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'461', N'22', N'219', N'S1011304', N'1', N'0', N'0', N'對訪客之安全提示', N'Safety Reminders for Visitors', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6CFBDC79-B0EC-4EEC-8C5B-AFB624499FEA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'462', N'22', N'77', N'S1010305', N'1', N'0', N'1', N'不適用', N'N/A', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0EA0EF5E-C59F-46A2-9277-AFE6023170C7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'463', N'38', N'160', N'S5010202', N'2', N'0', N'0', N'已檢查及貼上檢查標籤', N'Inspected and labeled', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C8C5A2F7-0D5D-46C3-9F64-B112CF3DC7F3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'464', N'36', N'140', N'S4010408', N'2', N'0', N'0', N'隙縫(裂口)*', N'Gap(Check)*', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9B7BF6D6-F0DC-4E42-927E-B1BC4C03CB57', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'465', N'22', N'82', N'S1010804', N'1', N'0', N'0', N'*滅火筒位置', N'*Fire Extinguisher location', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8C792850-9047-439F-8361-B1C6B4E4B179', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'466', N'50', N'205', N'SI2030101', N'2', NULL, N'0', N'CP(T)已確認所有人員、工具及物料已撤離軌道。', N'CP(T) confirmed that all persons, tools and materials have been cleared of the track.', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DEC756BD-08D7-47EE-AF52-B1F06035C41C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'111', N'28', N'28', N'C10101', N'0', N'0', N'0', N'已呈交檢查申請表格及工作許可證 (#)', N'The inspection application form and work permit (#) have been duly submitted.', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6604E041-26BC-4246-B39F-B2261280A760', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'467', N'36', N'142', N'S4010604', N'2', N'0', N'0', N'提供及使用個人防護裝備', N'Provision and use of personal protective equipment', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9D634311-42C4-4C39-88EA-B273DB506DCB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'468', N'22', N'79', N'S1010505', N'1', N'0', N'0', N'熱作工作許可證 (Hot Work):', N'PTW (Hot Work):', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'93BF7F55-3C3A-4FBD-B77D-B3439939768D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'42', N'42', NULL, N'S205', N'2', NULL, NULL, N'完工前確認及檢查', N'Pre-Completion Verification and Checks', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'97549DE5-4CB1-49C6-9D6A-B48E5974BF23', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'469', N'38', N'164', N'S5010605', N'2', N'0', N'1', N'其他:', N'Others:', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8FAEA78A-547C-4B5E-A23C-B4A044171261', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'137', N'36', N'36', N'S10101', N'2', N'0', N'0', N'有效證件', N'Valid Certificate', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'73801459-4BDB-418D-B653-B57E9F265964', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'470', N'41', N'178', N'S5040108', N'2', N'0', N'0', N'清理/妥善處理現場工作時產生的垃圾', N'Clean up/properly dispose of trash generated during on-site work', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'54D37F14-5EE3-4D39-AE89-B7C323F94CBF', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'34', N'34', NULL, N'C204', N'1', NULL, NULL, N'施工過程及品質控制', N'Construction Process and Quality Control', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4818CD4E-6885-4321-ACD5-B7F1F2417E6C', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'471', N'46', N'193', N'SI1030101', N'2', NULL, N'0', N'已確認所有人員、工具及物料已撤離軌道。/已撤離軌道上工作隊人數:', N'Confirmed that all persons, tools and materials have been cleared of the track. (No. of Person left Track:', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'32395262-9B9E-451D-8220-B8E9AEE12029', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'472', N'44', N'186', N'SI1010601', N'2', NULL, N'0', N'已與值日站長(SC)/調車場主管(YM)確定有關行車綫已關斷牽引電流。(如有需要時)', N'Confirmed with SC/YM that traction current has been switched off on lines (if necessary)', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4E7A619C-577A-411C-AFDD-B9124E8E1066', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'14', N'14', NULL, N'B114', N'0', NULL, NULL, N'吸入粉塵', N'Dust Inhalation', N'14', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9498E63E-FF95-4051-AF93-B9BB84E3C2B4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'56', N'3', N'3', N'B10302', N'0', N'1', N'0', N'注意出入升降機開關情況及站內設施，安排足夠看守員開路、按開門掣及注意公眾安全', N'Ensure attention to the operation of elevator switches and on-site facilities, arrange sufficient personnel to clear pathways, operate door controls, and prioritize public safety.', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2D9AF610-1643-409A-84C2-B9D61CFF10A7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'473', N'41', N'178', N'S5040102', N'2', N'0', N'0', N'工具及設備沒有損毀', N'Tools and equipment are not damaged', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'08E1CE34-E268-4705-917E-BA0A88088027', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'196', N'47', N'47', N'SI10401', N'2', N'0', N'0', N'16', N'16', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2A796F13-090F-4D10-8914-BA3099E29E3F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'474', N'38', N'163', N'S5010504', N'2', N'0', N'0', N'棚架表格五有效', N'Form 5 :Scaffolding Valid', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FCF6241F-2157-4843-8CA3-BA8E28E08F24', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'151', N'37', N'37', N'S10203', N'2', N'0', N'0', N'工地整潔', N'Clean construction site', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C09603E8-FFBD-44D5-8AC4-BAEF80B2F9FC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'147', N'36', N'36', N'S10111', N'2', N'0', N'0', N'單獨工作', N'Individual Work', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BA4B4B8E-EE07-4534-825E-BB6A0857BD1D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'121', N'30', N'30', N'C10302', N'0', N'0', N'0', N'為即時跟進不當行為/偏差事項，已作出特別安排 ', N'Special arrangements have been established for the immediate monitoring of improper conduct/deviations.', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0ED96284-D211-4A93-8FB8-BBDA78A3589A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'475', N'36', N'141', N'S4010503', N'2', N'1', N'0', N'提醒員工在沒有CP及管工的情況下須暫停手上工作', N'Remind employees to stop working if there is no CP or supervisor', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5CEAE160-DFAF-4C4A-923B-BC13FA4F2EC5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'98', N'24', N'24', N'S10309', N'1', N'0', N'1', N'其他:', N'Others:', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'012B9C9F-70FD-4A20-BF1D-BCE1D859C5F7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'45', N'45', NULL, N'SI102', N'2', NULL, NULL, N'取得授權進入軌道後', N'After authorized for track access', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DBA2A46D-B45B-4123-A18D-BD44EAFA401F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'476', N'36', N'140', N'S4010402', N'2', N'0', N'0', N'照明不足', N'Insufficient lighting', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'02C0FE7F-3794-46D4-A0E3-BD4B11C47481', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'477', N'36', N'147', N'S4011102', N'2', N'1', N'0', N'簡述及清楚了解「單獨工作的安全指引」卡的安全指引', N'Brief and understand the safety instructions of "Safety Guildenes on Lone Working"Card', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'941CA83B-E46E-4BD1-844F-BE287FDF3C99', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'478', N'36', N'144', N'S4010801', N'2', N'0', N'0', N'工作許可證PTW', N'Permit-To-Work', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6F91EB56-B882-43FB-933C-BE3987D1DA84', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'3', N'3', NULL, N'B103', N'0', NULL, NULL, N'使用升降台或運送物料時', N'When using a lift platform or transporting materials', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'7D859443-36E5-496B-B67A-BEA6FAF5FB6B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'479', N'23', N'86', N'S1020302', N'1', N'0', N'0', N'損毀(即時停用)', N'Damaged (immediate deactivation)', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B3ED5740-3941-4549-B5B5-BEAC83E70789', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'480', N'38', N'165', N'S5010704', N'2', N'0', N'0', N'狀態良好', N'Good condition', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C11C917A-F9F3-445F-A632-BF3E00B3598D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'481', N'38', N'164', N'S5010602', N'2', N'0', N'0', N'通道無阻礙', N'Unobstructed passage', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'476D97D9-72B8-4BF0-AAE5-BF66CCC234AC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'28', N'28', NULL, N'C101', N'0', NULL, NULL, N'開工前', N'Before construction', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'382C2871-BAE9-401F-9A13-C014939C4238', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'482', N'42', N'179', N'S5050104', N'2', N'0', N'0', N'4. 恢復現場環境', N'4. Restore the on-site environment', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1E32090A-114D-4FC5-A248-C114BB04171E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'483', N'38', N'165', N'S5010702', N'2', N'0', N'0', N'已檢查氣壓無異', N'The air pressure has been checked and is normal', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'787C190A-E42A-434B-A5BE-C11A77C8C1E4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'25', N'25', NULL, N'S104', N'1', NULL, NULL, N'工作中指導及監督', N'Guidance and Supervision during Work', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D48D9F72-EC60-411E-85CA-C17C72859883', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'484', N'22', N'220', N'S1011401', N'1', N'0', N'0', N'工程領域', N'Possession', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F583BE57-2AE2-4C7A-B04C-C17DF231C308', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'485', N'38', N'163', N'S5010508', N'2', N'0', N'0', N'損毀(即時停用.更換)', N'Damage (immediate deactivation and replacement)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'72D2F3F5-0793-4F9D-9301-C2137DE7E90A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'211', NULL, NULL, N'', N'2', N'0', N'0', N'', N'', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'54B5362B-0FD8-4973-B639-C30D4324BF19', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'44', N'44', NULL, N'SI101', N'2', NULL, NULL, N'進入軌道前', N'Prior to Track Access', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'87B1BB2C-ADAA-432F-81F8-C47AB3CDAB18', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'486', N'36', N'148', N'S4011203', N'2', N'0', N'0', N'沒有上述情況出現(所有工作人員身體及精神狀態良好,合適是日工作)', N'None of the above situations occur (all staff are in good physical and mental condition and suitable for work on that day)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6BEB8EE4-9762-4DE5-A5AB-C4E5E0CFF649', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'40', N'40', NULL, N'S203', N'2', NULL, NULL, N'工作中指導及監督', N'Guidance and Supervision during Work', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3D429FCF-8BB1-431B-837B-C4EC5E7C9D39', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'54', N'2', N'2', N'B10201', N'0', N'1', N'0', N'高空工作使用手工具須繫上手繩', N'During elevated operations, all hand tools shall be secured with a safety lanyard', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FC02F0E1-9F83-404E-809D-C58F98AABBBB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'487', N'22', N'215', N'S1011102', N'1', N'0', N'0', N'暴雨', N'Heavy Rain', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'486B2E60-1BDE-4BBB-A744-C5FEE0A37189', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'488', N'36', N'140', N'S4010401', N'2', N'0', N'0', N'工作環境安全', N'Safe working environment', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0DDCEBC6-F09D-432C-8E74-C6A2852A32F2', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'489', N'44', N'188', N'SI1010801', N'2', NULL, N'0', N'在進入軌道現場，使用 SafeTrack App 掃瞄 QR code，取得授權進入軌道。包括SafeTrack App授權畫面截圖等) ', N'At the scene of track access, apply SafeTrack App to scan the QR code for ontaining track access authorization. (included: capture screen record of SafeTrack App, etc.)', N'8', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F42C1E13-4583-4D29-92A7-C7EE50874B60', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'50', N'50', NULL, N'SI203', N'2', NULL, NULL, N'撤離軌道前', N'Before Track Clear', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AEA8D6B0-480A-415D-A80B-C82927EA9DF0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'490', N'37', N'153', N'S4020502', N'2', N'0', N'0', N'起重機械及吊具持有有效証書，圍封及劃分吊運區，未經授權人士不可進入，委任合資格埋碼員進行埋碼工作，埋碼後吊離地面確定穩固方可進行吊運', N'Heavy Machinery and Lifting Gear Hold Current Certificates, Enclose and Label Lifting Zone, Bar Unauthorized Persons from Entering, Engage Qualified Rigger for Rigging, Ensure Secure Rigging before Lifting', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AB346172-B344-4567-9D2B-C8447F0DD7EB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'92', N'24', N'24', N'S10303', N'1', N'0', N'0', N'使用安全工作台進行高空工作', N'Utilize Safety Working Platform For Height Operations', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'07BA37EA-BAF1-45C7-8F4E-C8B64B6C4768', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'87', N'23', N'23', N'S10204', N'1', N'0', N'0', N'機械', N'Mechanical Equipment', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0BFD41CE-BA32-4BD1-B38D-C8C89CA0A6CA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'209', NULL, NULL, N'', N'2', N'0', N'0', N'', N'', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'759D2B47-9FFA-4D2D-925A-C9F37093191A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'83', N'22', N'22', N'S10109', N'1', N'0', N'0', N'單獨工作', N'Individual Task', N'12', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C976F448-24F7-47F1-B8F6-CA33C5AE473A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'491', N'22', N'221', N'S1011513', N'1', N'0', N'0', N'其他:', N'Other:', N'99', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'178D7CAA-1467-434F-A22D-CABBA97B37BE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'165', N'38', N'38', N'S10107', N'2', N'0', N'0', N'消防', N'fire control', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'962E86E9-AA7C-4B32-9C27-CB3307E0011D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'492', N'23', N'87', N'S1020403', N'1', N'0', N'0', N'不適用', N'N/A', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1111A6C8-610E-4B71-82C7-CB33DD2F61C4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'493', N'22', N'76', N'S1010211', N'1', N'0', N'0', N'提供清晰工作指示予 P/N 牌員工', N'Provide clear work instructions to P/N employees', N'11', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5FF30D66-095A-4CB4-8F6E-CBD813BA8A9C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'494', N'38', N'162', N'S5010402', N'2', N'0', N'0', N'合資格人士已檢查及簽署每週檢查表格一', N'Weekly Inspection Form 1 has been checked and signed by a qualified person', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'34E31273-98FB-47E8-BE43-CE76E4181ECF', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'495', N'22', N'76', N'S1010206', N'1', N'0', N'0', N'*已報站', N'*Stop reported', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A9610C0B-277B-4860-9070-CEA041F75FFE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'130', N'33', N'33', N'C20303', N'0', N'0', N'0', N'使用物料時合格人員(CP)/工地監督(SS) 需要檢查並確認物料已獲港鐵批准', N'The authorized personnel (CP)/site supervisor (SS) are required to inspect and confirm that the materials have obtained approval from MTR before utilization.
', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A9B1D3A5-017F-4C5F-9226-CEAF49E224B1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'496', N'42', N'179', N'S5050105', N'2', N'0', N'0', N'離場前已確認以上項目', N'The above items have been confirmed before leaving', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8A868A1F-685E-4D63-9528-CEC71BC4558F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'497', N'36', N'145', N'S4010910', N'2', N'0', N'0', N'安全帶', N'Safety Belt', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B5A0A3C2-C8AF-4155-B2D8-CF1BC934A6DD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'168', N'39', N'39', N'S10202', N'2', N'0', N'0', N'使用合適個人防護裝備', N'Use appropriate personal protective equipment', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'493C3D3C-A3DC-494D-BE31-CF9930D7F922', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'498', N'38', N'162', N'S5010403', N'2', N'0', N'0', N'損毀(即時停用.更換)', N'Damage (immediate deactivation and replacement)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B35CA6DA-EBA2-458E-A57E-D042ACF6CB3C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'499', N'22', N'79', N'S1010514', N'1', N'0', N'0', N'不適用', N'N/A', N'12', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3ABADEA2-BBBE-4BAA-A4FF-D13D7A4C9938', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'107', N'27', N'27', N'S10607', N'1', N'0', N'0', N'防火(煙霧頭已還原)', N'Fire prevention measures have been taken (smoke heads have been restored)', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E8D42D22-1A78-4043-AF3B-D193D9B41E86', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'60', N'7', N'7', N'B10701', N'0', N'1', N'0', N'由註冊電工進行電力監察施行工序', N'Electrical Supervision and Execution of Procedures by a Registered Electrician', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1956A7CC-4206-4497-9BF8-D2AB095514A8', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'500', N'38', N'160', N'S5010205', N'2', N'0', N'0', N'電線及插頭沒有缺損', N'The wire and plug are not damaged.', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'33AB7D17-6B40-4124-AD65-D2F6A48CE9A4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'205', N'50', N'50', N'SI20301', N'2', N'0', N'0', N'5', N'5', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'30EF73FD-3014-4469-B804-D334DD764D13', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'501', N'36', N'145', N'S4010906', N'2', N'0', N'0', N'口罩', N'Mask', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AC80205E-28B0-4515-B3A7-D4837A2980F1', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'502', N'22', N'221', N'S1011507', N'1', N'0', N'0', N'狹窄空間', N'Limited Space', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'23762635-D71B-4B42-AC4E-D87D84B1A871', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'503', N'38', N'165', N'S5010704', N'2', N'0', N'0', N'狀態良好', N'Good condition', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'795E8003-5CD2-4DA0-8CE2-D949A7355E07', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'504', N'46', N'194', N'SI1030204', N'2', NULL, N'0', N'已移除固定迫岔位置:', N'Removed point secured at: ', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3F57A00B-C270-4251-A1DD-D997A1C851B0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'197', N'47', N'47', N'SI10402', N'2', N'0', N'0', N'17', N'17', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D7E214F2-BFA7-4201-9011-D9C4FBE6366A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'191', N'45', N'45', N'SI10201', N'2', N'0', N'0', N'11', N'11', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D0F7EAAD-B23D-4D43-9184-D9E8C0E97DEE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'505', N'23', N'88', N'S1020506', N'1', N'0', N'0', N'正確安裝圍欄及踢腳板', N'Correct installation of fences and skirting boards', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'841A56B9-E36D-4765-86A3-D9EC5F4C212F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'7', N'7', NULL, N'B107', N'0', NULL, NULL, N'電擊(電力設備故障)', N'Electric shock (due to electrical equipment failure)', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8C89E6E7-83C9-4AE0-8A0C-DA3043D807B4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'506', N'22', N'75', N'S1010116', N'1', N'0', N'0', N'漏電/觸電', N'Electric shock/leakage', N'15', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2DFBE0DD-6592-4A6F-A434-DA94088341C5', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'507', N'22', N'76', N'S1010207', N'1', N'0', N'0', N'已替工作會涉及的港鐵站系統進行隔離及掛牌  電力', N'The MTR station systems involved in the works have been isolated and marked out. Electricity', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8AA9BFC5-7D3E-4A89-80F6-DABD59C44B8B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'149', N'37', N'37', N'S10201', N'2', N'0', N'0', N'公眾地方/港鐵大堂工作安全', N'Work Safety in Public Places/MTR Concourses', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BA5A4A1E-CB22-428E-944D-DB082EDD66CB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'21', N'21', NULL, N'B121', N'0', NULL, NULL, N'天氣酷熱', N'Very Hot Weather', N'21', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B3FD90C8-5679-4066-A06C-DB1960FCFF0C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'96', N'24', N'24', N'S10307', N'1', N'0', N'0', N'使用合適個人防護裝備', N'Utilize Adequate Protective Personal Gear', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6098D2A8-2E20-441D-B5F1-DB679153C1A7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'6', N'6', NULL, N'B106', N'0', NULL, NULL, N'觸電(使用電工具)', N'Electric shock (while using power tools)', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C9E83DF4-2164-491C-9A6D-DCEFD20CA879', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'508', N'44', N'181', N'SI1010101', N'2', NULL, N'0', N'已出席工程主管(EPIC)主持的工程領域安全簡報會。(如有需要時)', N'Attended possession safety briefing by Engineer''s Person-in-Charge(EPIC) (if necessary)', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5266F719-7099-4816-82E0-DD58C7615A61', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'509', N'22', N'219', N'S1011306', N'1', N'0', N'0', N'調車工作期間將會全程錄音', N'Audio recording throughout the shunting', N'6', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DE55F030-24C0-49C9-B768-DD63AC692FD0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'53', N'1', N'1', N'B10101', N'0', N'1', N'0', N'設置工作台/梯台進行高空工作', N'setting up a work platform/ladder for working at height', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B92B1054-23A2-4AF4-B1B3-DD74E6754962', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'510', N'23', N'88', N'S1020505', N'1', N'0', N'0', N'工作台使用手提工具已繫上手繩', N'The hand tools used on the workbench are tied with hand straps', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'01F93705-8B85-4081-83D2-DDE08D2F4C9A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'511', N'36', N'142', N'S4010607', N'2', N'0', N'0', N'其他:_____________________________', N'Others:_____________________________', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A7D43568-DD4D-4CC8-B781-DE973BA7B35E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'512', N'22', N'221', N'S1011505', N'1', N'0', N'0', N'帶電導體上或附件', N'Live Conductors or nearby', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'679DC6BC-958D-4B50-9643-DED0651DD6F3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'24', N'24', NULL, N'S103', N'1', NULL, NULL, N'工作中安全巡查記錄', N'Safety Inspection Record during Work', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F72B562A-6DE7-4DFA-B70B-DED1D50E0BA7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'513', N'23', N'89', N'S1020601', N'1', N'0', N'0', N'照明充足', N'Adequate lighting', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'89F8BDD4-A7C8-4EF9-9E0D-E1113F2F1930', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'514', N'38', N'163', N'S5010505', N'2', N'0', N'0', N'安全上落通道', N'Safe access', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'046CD61F-77FA-4818-8D3E-E16E6B4857A9', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'515', N'22', N'78', N'S1010407', N'1', N'0', N'0', N'架空電纜保護物料', N'Overhead cable protection materials', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BC9BECCE-8816-423C-94C0-E24150F1CFFB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'41', N'41', NULL, N'S204', N'2', NULL, NULL, N'完工後清理現場', N'Post-Construction Site Cleaning', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'BF60CAD7-97EC-4DA3-A4A9-E297BEC277C7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'201', N'48', N'48', N'SI20101', N'2', N'0', N'0', N'1', N'1', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'64312C6D-CA9A-449C-859E-E2B599F8EC38', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'49', N'49', NULL, N'SI202', N'2', NULL, NULL, N'取得授權進入軌道後', N'After authorized for track access', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5E50002E-9092-4112-853A-E2CE85D066E6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'516', N'38', N'161', N'S5010301', N'2', N'0', N'0', N'狀況良好', N'Good condition', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AD174540-88E6-47B9-B120-E300492A3920', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'517', N'36', N'147', N'S4011103', N'2', N'0', N'0', N'觀察四周環境以辦識潛在風險', N'Observe surroundings to identify potential risks', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5D4FD362-4B8C-4D4E-9BF9-E389F9307F95', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'518', N'22', N'219', N'S1011303', N'1', N'0', N'0', N'危險物料', N'Dangerous Material', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'50EDFE5C-CC99-4B6F-9DA5-E3B8851D1F01', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'200', N'47', N'47', N'SI10405', N'2', N'0', N'0', N'20', N'20', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'914ACAEE-7AE9-4350-BE3F-E4288D1F7E7E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'519', N'22', N'79', N'S1010512', N'1', N'0', N'1', N'試驗許可證 SFT', N'Sanction-For-Test', N'11', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E26FF2AD-326C-450C-AE9D-E4A091A0FB36', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'520', N'22', N'215', N'S1011103', N'1', N'0', N'0', N'有雨 ', N'Drizzle', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E4F3550C-5EF0-43B4-84D2-E5065378F09F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'521', N'38', N'164', N'S5010604', N'2', N'0', N'0', N'防止墜下措施狀態良好', N'Fall prevention measures are in good condition', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1AE91B75-33EC-4DCA-8B2F-E5C81AA59F40', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'522', N'22', N'78', N'S1010402', N'1', N'0', N'0', N'路軌夾', N'Rail Clamp', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1F02E850-F1A2-4159-9294-E5CDECE552CC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'523', N'36', N'143', N'S4010705', N'2', N'0', N'0', N'號角+燈/旗', N'Horn +Lamp/Flag', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'A33FAAA5-C354-4535-8D8F-E73C4C06DBA6', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'524', N'38', N'159', N'S5010102', N'2', N'0', N'0', N'佩帶方法正確', N'Correct wearing method', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'989E13DD-0B04-4FB9-B747-E8DC38A0CEE7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'525', N'23', N'86', N'S1020301', N'1', N'0', N'0', N'狀況良好', N'Good condition', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5A7C7D2C-D2D8-4B5B-BEF7-E8F3865837A9', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'109', N'27', N'27', N'S10609', N'1', N'0', N'0', N'閘門已上鎖', N'The gate has been locked.', N'9', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0E658660-239A-4547-BACE-EB53C81B59DE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'526', N'22', N'81', N'S1010707', N'1', N'0', N'0', N'安全帶&防墮下扣', N'Safety Belt & Fall arrest Buckle', N'7', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'C2721681-D3DF-48F8-8D02-EB568D606C8B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'527', N'22', N'82', N'S1010805', N'1', N'0', N'0', N'*急救箱位置', N'*First Aid Kit Location', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B0B48947-7760-4FA1-9F2B-EC6B64514171', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'123', N'31', N'31', N'C20101', N'0', N'0', N'0', N'所有建築工人已持有有效工人註冊證及相應工作訓練', N'All construction workers are in possession of valid worker registration cards and have undergone appropriate job training.', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4C8A9505-9479-45D2-8C01-ECABD7EAAC02', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'122', N'30', N'30', N'C10303', N'0', N'0', N'0', N'已通知當值工務督察完工(於現場通知)', N'The on-duty construction inspector has been informed of the completion (notified on-site).
', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2BA7D900-7C6A-47DE-8B70-ED51F487701F', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'528', N'42', N'179', N'S5050102', N'2', N'0', N'0', N'2. 確認沒有遺留帶來的物品和工具', N'2. Make sure that no items or tools are left behind', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B2C8FD74-6934-4F83-95F4-EDA1ED9728A7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'529', N'36', N'140', N'S4010404', N'2', N'0', N'0', N'地面濕滑*', N'Slippery Floor*', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'51701C5D-B33B-4D34-AAC0-EE5B8EF981EA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'1', N'43', N'43', NULL, N'S206', N'2', NULL, NULL, N'安全檢討會議', N'Safety Review Meeting', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6D26B989-37DE-44C6-8737-EF6CFDC4C735', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'216', N'52', N'52', N'S10701', N'1', N'0', N'0', N'每天工作安全檢討內容', N'Daily Work Safety Review Content', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FF95FF4D-D88B-4D06-92E3-F1716283DAE4', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'70', N'17', N'17', N'B11701', N'0', N'1', N'0', N'保護鋒利物件', N'Safeguard Sharp Objects', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'9AE3A2A7-B121-4366-BB62-F24275475D85', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'530', N'23', N'85', N'S1020203', N'1', N'0', N'0', N'防水電線及插頭沒有缺損', N'The waterproof wire and plug are not damaged.', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'3C230812-1E30-4F4D-AB0F-F24279C7139D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'531', N'22', N'75', N'S1010103', N'1', N'0', N'0', N'地面濕滑', N'Slippery Floor', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'59CF8166-C4B9-44DE-9012-F24B2D9319FB', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'532', N'22', N'79', N'S1010501', N'1', N'0', N'0', N'高壓工作許可證PTW (High Voltage):', N'PTW (High Voltage):', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FA845B48-318A-43FF-9B0C-F29AAA51B11E', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'533', N'36', N'141', N'S4010510', N'2', N'1', N'0', N'指導員應提醒P/N牌員工工地的特別危險及相關工作要求', N'Instructors should remind P/N employees of special hazards and related work requirements on the site', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'32875CAA-97D5-462B-BCBA-F37605B24FC3', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'59', N'6', N'6', N'B10601', N'0', N'1', N'0', N'使用前需由註冊電工檢查及貼上標籤', N'Must be Inspected and Labeled by a Registered Electrician before Use', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'D82521C0-4AB5-4752-A7F0-F387FB2664F7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'157', N'37', N'37', N'S10209', N'2', N'0', N'0', N'使用手工具/機械', N'Use of hand tools/machinery', NULL, N'0')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'1750AF19-37C1-4578-9B4F-F40F24D2B4B9', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'534', N'22', N'82', N'S1010803', N'1', N'0', N'1', N'*逃生路線', N'*Evacuation Routes', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E509B60B-54EF-4769-8435-F433E26CC39B', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'535', N'44', N'185', N'SI1010501', N'2', NULL, N'0', N'已接收安全文件(例如隔離記錄表(IRF))。(如有需要時)', N'Received safety document (e.g.Isolation Record Form (IRF)) (if necessary)', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'FD330B43-8EBC-4D40-B416-F526FBB5DF86', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'536', N'41', N'178', N'S5040103', N'2', N'0', N'0', N'沒有積水', N'No water accumulation', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'45C76F31-CFF5-4337-8727-F62F61520580', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'537', N'22', N'79', N'S1010511', N'1', N'0', N'0', N'隔離記錄表 IRF', N'Isolation Record Form IRF', N'10', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'CEB3C170-CB5D-4A7B-85EF-F6368BE0F367', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'538', N'23', N'85', N'S1020205', N'1', N'0', N'0', N'急停掣操作正常', N'Emergency stop button operates normally', N'5', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'46AE48D7-21FF-42CB-BF79-F691671D860C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'181', N'44', N'44', N'SI10101', N'2', N'0', N'0', N'1', N'1', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'6EADB95D-31DE-4956-9B38-F8317CA7380B', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'208', N'51', N'51', N'SI20402', N'2', N'0', N'0', N'8', N'8', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'40DD8A31-C20A-4BA9-B2FB-F8E841CD5A68', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'539', N'22', N'214', N'S1011001', N'1', N'0', N'0', N'大堂', N'Lobby', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4C021207-5602-455F-B507-F8ECD8791A68', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'132', N'34', N'34', N'C20402', N'0', N'0', N'0', N'工人是否跟從已批准的施工方案,圖則, 工具, 設備, 程序工作? ', N'Are the workers adhering to the approved construction plans, drawings, tools, equipment, and procedures?
', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5BF7C681-0582-47A6-8F7F-F91304127911', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'540', N'44', N'182', N'SI1010201', N'2', NULL, N'0', N'已向值日站長(SC)/調車場主管(YM)報到及呈交 OP290A (承判商工作記錄)。', N'Reported duty and submitted OP290A (Contractor Works Control Sheet) to related Station Controller (SC) / Yard Master (YM).', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'8F6AABF4-9A4B-4F6E-9D8D-F92F364F52BE', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'127', N'32', N'32', N'C20203', N'0', N'0', N'0', N'檢查記錄是否已存放在工地並保持更新?', N'Have inspection records been stored on the construction site and maintained up to date?', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'DA70885D-850A-44BA-8B6A-F9604529B631', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'541', N'22', N'215', N'S1011104', N'1', N'0', N'0', N'雷暴', N'Thuderstorms', N'4', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'F7FF5E4E-32A3-4075-9212-FA6F0980BBBA', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'542', N'38', N'161', N'S5010302', N'2', N'0', N'0', N'損毀(即時停用.更換)', N'Damage (immediate deactivation and replacement)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E5BEFDC9-F92B-4A53-95CB-FA9865592CB0', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'543', N'45', N'191', N'SI1020101', N'2', NULL, N'0', N'根據 RSR設置保護措施(例如 設置紅閃燈、接地棒、固定道岔)', N'Set up protection arrangements in accordance with the requirements of RSR. (e.g. use of Red Flashing Lights (RFL), Earthing Rods, point scotch.)', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'213B4D33-9D15-4F24-B1A4-FB028BA46329', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'544', N'22', N'81', N'S1010702', N'1', N'0', N'0', N'反光衣', N'High Visibility Vest', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'5B67D80C-17F1-4D75-BBF3-FB40E0308579', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'545', N'36', N'140', N'S4010406', N'2', N'0', N'0', N'地面不平/斜路', N'Uneven ground/ramp', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AD222191-0EBE-4E1C-AB6E-FCBC21498EAD', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'546', N'22', N'215', N'S1011101', N'1', N'0', N'0', N'正常', N'Normal', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0CD70AED-12FA-4EF2-98C3-FD54513DD8AD', N'0', N'', NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'547', N'49', N'204', N'SI2020101', N'2', NULL, N'0', N'CP(T)已確認在軌道工地範圍，設置保護措施(包括 放置紅閃燈及或裝設接地棒等)。', N'CP(T) confirmed that the worksite on track has been arranged protections (included Red Flashing Lights and/or Earthing Rods etc.)', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'948B1890-5AF7-438D-A722-FDEB935AFB90', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'193', N'46', N'46', N'SI10301', N'2', N'0', N'0', N'13', N'13', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0D3B946D-D426-4EBE-AE3E-FE4FC463D805', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'2', N'55', N'3', N'3', N'B10301', N'0', N'1', N'0', N'穿著安全帶尾繩須扣在指定位置', N'When wearing a safety harness, the lanyard shall be attached to the designated anchorage point', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'2C02C570-AB3A-4D32-9AE1-FE979F2F00C0', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'548', N'36', N'144', N'S4010806', N'2', N'0', N'0', N'工作證書(密閉空間) CFW(CS)', N'Cetificate-For-Work(Confined Space)', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'B9545460-2C6D-4D01-9060-FEA440A0272D', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'549', N'36', N'146', N'S4011004', N'2', N'0', N'0', N'急救箱位置#', N'First Aid kit location#', NULL, NULL)
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'0EF92742-9B17-4BA9-96B5-FEC6F5DC3CE7', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'550', N'23', N'88', N'S1020501', N'1', N'0', N'0', N'輕便工作台狀況良好', N'Portable workbench in good condition', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'E2D8281C-A3E7-41F5-98F7-FF1661A1D13C', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'551', N'22', N'81', N'S1010701', N'1', N'0', N'0', N'安全帽連下頷帶', N'Safety Helmet-Chin Strap', N'1', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'32CB547B-2E08-4AD2-9A36-FF8F092D0F5A', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'552', N'22', N'79', N'S1010502', N'1', N'0', N'0', N'低壓工作許可證PTW (Low Voltage):', N'PTW (Low Voltage):', N'2', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'AD3B0427-B829-4643-9BEB-FF9C18FE0EC2', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'553', N'22', N'221', N'S1011503', N'1', N'0', N'0', N'電力裝置', N'Electrical Installation', N'3', N'1')
GO

INSERT INTO [dbo].[Sys_Site_Work_Check_Item] ([id], [delete_status], [remark], [create_id], [create_name], [create_date], [modify_id], [modify_name], [modify_date], [level], [item_code], [root_id], [master_id], [global_code], [type_id], [is_qly], [is_others], [name_cht], [name_eng], [order_no], [enable]) VALUES (N'4085AC5D-3B3F-4397-8173-FFA140E5FFEC', N'0', NULL, NULL, NULL, N'2025-11-04 14:23:33.000', NULL, NULL, NULL, N'3', N'554', N'43', N'180', N'S5060102', N'2', N'0', N'0', N'電力安全', N'Power safety', NULL, NULL)
GO

SET IDENTITY_INSERT [dbo].[Sys_Site_Work_Check_Item] OFF
GO


-- ----------------------------
-- Auto increment value for Sys_Site_Work_Check_Item
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Sys_Site_Work_Check_Item]', RESEED, 554)
GO


-- ----------------------------
-- Primary Key structure for table Sys_Site_Work_Check_Item
-- ----------------------------
ALTER TABLE [dbo].[Sys_Site_Work_Check_Item] ADD CONSTRAINT [PK__Sys_Site__3213E83F33D74894] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO




-- ----------------------------
-- Table structure for Biz_Site_Work_Record_Worker
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Biz_Site_Work_Record_Worker]') AND type IN ('U'))
	DROP TABLE [dbo].[Biz_Site_Work_Record_Worker]
GO

CREATE TABLE [dbo].[Biz_Site_Work_Record_Worker] (
  [id] uniqueidentifier  NOT NULL,
  [create_id] int  NULL,
  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_date] datetime DEFAULT getdate() NULL,
  [modify_id] int  NULL,
  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [modify_date] datetime  NULL,
  [delete_status] int DEFAULT 0 NULL,
  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [record_id] uniqueidentifier  NULL,
  [contact_id] uniqueidentifier  NULL,
  [is_valid] int  NULL,
  [work_type_id] uniqueidentifier  NULL,
  [is_wpic] int  NULL,
  [is_cp] int  NULL,
  [is_nt] int  NULL,
  [is_qr] int  NULL,
  [is_sick] int  NULL,
  [is_sign] int  NULL,
  [work_remark] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [traffic_allowance] decimal(18,2)  NULL,
  [salary_adj] decimal(18,2)  NULL,
  [time_in] datetime  NULL,
  [time_in_adj] datetime  NULL,
  [time_out] datetime DEFAULT NULL NULL,
  [time_out_adj] datetime  NULL,
  [green_card_no] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [green_card_exp] datetime  NULL,
  [cic_no] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [cic_exp] datetime  NULL,
  [company_id] uniqueidentifier  NULL,
  [base_salary] decimal(18,2)  NULL,
  [cic_card_no] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT '' NULL,
  [is_sic] int  NULL
)
GO

ALTER TABLE [dbo].[Biz_Site_Work_Record_Worker] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'delete_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工地工作记录id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'record_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'联系人id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'contact_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'valid（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'is_valid'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工种id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'work_type_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'wpic（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'is_wpic'
GO

EXEC sp_addextendedproperty
'MS_Description', N'cp（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'is_cp'
GO

EXEC sp_addextendedproperty
'MS_Description', N'NT/T（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'is_nt'
GO

EXEC sp_addextendedproperty
'MS_Description', N'二维码打卡（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'is_qr'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否生病（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'is_sick'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否签名（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'is_sign'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工作备注',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'work_remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'交通津贴',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'traffic_allowance'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工资调整（调整后的）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'salary_adj'
GO

EXEC sp_addextendedproperty
'MS_Description', N'上班时间',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'time_in'
GO

EXEC sp_addextendedproperty
'MS_Description', N'上班时间调整',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'time_in_adj'
GO

EXEC sp_addextendedproperty
'MS_Description', N'下班时间',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'time_out'
GO

EXEC sp_addextendedproperty
'MS_Description', N'下班时间调整',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'time_out_adj'
GO

EXEC sp_addextendedproperty
'MS_Description', N'绿卡编号',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'green_card_no'
GO

EXEC sp_addextendedproperty
'MS_Description', N'绿卡过期时间',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'green_card_exp'
GO

EXEC sp_addextendedproperty
'MS_Description', N'cic编号',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'cic_no'
GO

EXEC sp_addextendedproperty
'MS_Description', N'cic过期时间',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'cic_exp'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工人当前所属公司id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'company_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工人当前开工基础工资',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'base_salary'
GO

EXEC sp_addextendedproperty
'MS_Description', N'cic序列号',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'cic_card_no'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否SIC（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Worker',
'COLUMN', N'is_sic'
GO


-- ----------------------------
-- Primary Key structure for table Biz_Site_Work_Record_Worker
-- ----------------------------
ALTER TABLE [dbo].[Biz_Site_Work_Record_Worker] ADD CONSTRAINT [PK__Biz_Site__3213E83FD5A47C08] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO



-- ----------------------------
-- Table structure for Biz_Site_Work_Record_Sign
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Biz_Site_Work_Record_Sign]') AND type IN ('U'))
	DROP TABLE [dbo].[Biz_Site_Work_Record_Sign]
GO

CREATE TABLE [dbo].[Biz_Site_Work_Record_Sign] (
  [id] uniqueidentifier  NOT NULL,
  [create_id] int  NULL,
  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_date] datetime DEFAULT getdate() NULL,
  [modify_id] int  NULL,
  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [modify_date] datetime  NULL,
  [delete_status] int DEFAULT 0 NULL,
  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [record_id] uniqueidentifier  NULL,
  [relation_id] uniqueidentifier  NULL,
  [relation_type] int  NULL,
  [file_path] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [file_blurry_path] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [sign_name] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Biz_Site_Work_Record_Sign] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Sign',
'COLUMN', N'delete_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Sign',
'COLUMN', N'remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工地工作记录id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Sign',
'COLUMN', N'record_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'对应的表ID',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Sign',
'COLUMN', N'relation_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'签名类型（0：工人签名，1：scr的执行合资格人员签署，2：scr的WPIC检查后签署，3：cpd的执行合资格人员签署，4：cpd的工务督察人员签署；5：qdc的WPIC检查后签署，6：cp的SIC CP(T)签署；7：sic的SIC签署）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Sign',
'COLUMN', N'relation_type'
GO

EXEC sp_addextendedproperty
'MS_Description', N'图片路径',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Sign',
'COLUMN', N'file_path'
GO

EXEC sp_addextendedproperty
'MS_Description', N'模糊图片的路径',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Sign',
'COLUMN', N'file_blurry_path'
GO

EXEC sp_addextendedproperty
'MS_Description', N'签名人',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Sign',
'COLUMN', N'sign_name'
GO


-- ----------------------------
-- Primary Key structure for table Biz_Site_Work_Record_Sign
-- ----------------------------
ALTER TABLE [dbo].[Biz_Site_Work_Record_Sign] ADD CONSTRAINT [PK__Biz_Site__3213E83FE7EE7E95] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

-- ----------------------------
-- 2025-11-11 Table structure for Biz_Site_Work_Record_Item_Check
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Biz_Site_Work_Record_Item_Check]') AND type IN ('U'))
	DROP TABLE [dbo].[Biz_Site_Work_Record_Item_Check]
GO

CREATE TABLE [dbo].[Biz_Site_Work_Record_Item_Check] (
  [id] uniqueidentifier  NOT NULL,
  [create_id] int  NULL,
  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_date] datetime DEFAULT getdate() NULL,
  [modify_id] int  NULL,
  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [modify_date] datetime  NULL,
  [delete_status] int DEFAULT 0 NULL,
  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [record_id] uniqueidentifier  NULL,
  [check_code] int  NULL,
  [check_sub_code] int  NULL,
  [radio_result] int  NULL,
  [check_result] int  NULL,
  [text_result] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [check_type] int  NULL,
  [time_result] datetime  NULL
)
GO

ALTER TABLE [dbo].[Biz_Site_Work_Record_Item_Check] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'delete_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工地工作记录id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'record_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'代码（指向表中的type_code）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'check_code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'子代码（指向表中的type_code）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'check_sub_code'
GO

EXEC sp_addextendedproperty
'MS_Description', N'单选框结果（1：第一个，2：第二个以此类推）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'radio_result'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否勾选（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'check_result'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文本框填写内容',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'text_result'
GO

EXEC sp_addextendedproperty
'MS_Description', N'选项类型（0：brf；1：scr；2：cpd；3：qdc；4：cp；5：sic，6：cp的备注，7：sic备注）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'check_type'
GO

EXEC sp_addextendedproperty
'MS_Description', N'检查时间（选择类型4，5使用）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record_Item_Check',
'COLUMN', N'time_result'
GO


-- ----------------------------
-- Primary Key structure for table Biz_Site_Work_Record_Item_Check
-- ----------------------------
ALTER TABLE [dbo].[Biz_Site_Work_Record_Item_Check] ADD CONSTRAINT [PK__Biz_Site__3213E83FDC3433E6] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO



-- ----------------------------
-- 2025-11-11 Table structure for Biz_Site_Work_Record
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Biz_Site_Work_Record]') AND type IN ('U'))
	DROP TABLE [dbo].[Biz_Site_Work_Record]
GO

CREATE TABLE [dbo].[Biz_Site_Work_Record] (
  [id] uniqueidentifier  NOT NULL,
  [create_id] int  NULL,
  [create_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [create_date] datetime DEFAULT getdate() NULL,
  [modify_id] int  NULL,
  [modify_name] nvarchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [modify_date] datetime  NULL,
  [delete_status] int DEFAULT 0 NULL,
  [remark] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [contract_id] uniqueidentifier  NULL,
  [rolling_program_id] uniqueidentifier  NULL,
  [site_id] uniqueidentifier  NULL,
  [sub_site_id] uniqueidentifier  NULL,
  [job_duties] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [shift] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [is_track] int  NULL,
  [work_date] datetime  NULL,
  [finish_briefing] int  NULL,
  [check_config] nvarchar(600) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [current_duty_cp_id] uniqueidentifier DEFAULT '' NULL,
  [duty_cp_id] uniqueidentifier  NULL,
  [check_ref] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[Biz_Site_Work_Record] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'delete_status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'所属合同id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'contract_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'所属工程进度id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'rolling_program_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'站点id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'site_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'子站点id（暂无）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'sub_site_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工作内容',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'job_duties'
GO

EXEC sp_addextendedproperty
'MS_Description', N'值更',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'shift'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否进入轨道（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'is_track'
GO

EXEC sp_addextendedproperty
'MS_Description', N'工作日期',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'work_date'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否完成安全简介（0：否；1：是）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'finish_briefing'
GO

EXEC sp_addextendedproperty
'MS_Description', N'选择的配置（Sys_Site_Work_Chk_Item）',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'check_config'
GO

EXEC sp_addextendedproperty
'MS_Description', N'当前值更管工id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'current_duty_cp_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'管工id',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'duty_cp_id'
GO

EXEC sp_addextendedproperty
'MS_Description', N'检查表参考编号',
'SCHEMA', N'dbo',
'TABLE', N'Biz_Site_Work_Record',
'COLUMN', N'check_ref'
GO


-- ----------------------------
-- Primary Key structure for table Biz_Site_Work_Record
-- ----------------------------
ALTER TABLE [dbo].[Biz_Site_Work_Record] ADD CONSTRAINT [PK__Biz_Site__3213E83F017C156D] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO



