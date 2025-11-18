insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
values(newid(), 0, N'员工管理文件_图片',1,getdate(),'Employee_Management','\Employee_Management\img')

create table Sys_Employee_Management
(
    id                              uniqueidentifier default newid() not null
        primary key,
    delete_status                   int,
    create_id                       int,
    create_name                     nvarchar(60),
    create_date                     datetime,
    modify_id                       int,
    modify_name                     nvarchar(60),
    modify_date                     datetime,
    sys_employee_management_number  nvarchar(50)                     not null,
    chinese_name                    nvarchar(100),
    english_name                    nvarchar(100),
    card_no                         nvarchar(50),
    phone                           nvarchar(50),
    email                           nvarchar(100),
    record_date                     date,
    leave_date                      date,
    leave_settlement_date           date,
    marital_status                  int,
    trial_period                    int,
    leave_reason                    nvarchar(400),
    user_gender                     int,
    section                         nvarchar(100),
    department                      uniqueidentifier,
    position                        nvarchar(50),
    leave_type_id                   uniqueidentifier,
    work_hour                       int,
    work_day                        int,
    leave_group                     nvarchar(100),
    basic_salary                    decimal(18, 2),
    salary_type                     int,
    salary_amount                   decimal(18, 2),
    adjustment_amount               decimal(18, 2),
    effective_date                  date,
    mpf_plan                        int,
    mpf_type                        int,
    contribution_mpf_account        nvarchar(100),
    employee_mpf_account            nvarchar(100),
    voluntary_mpf_contribution      int,
    voluntary_mpf_contribution_type int,
    employee_voluntary_contribution decimal(18, 2),
    employer_voluntary_contribution decimal(18, 2),
    bank_info                       nvarchar(1000)
)
go

exec sp_addextendedproperty 'MS_Description', N'员工管理系统表', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management'
go

exec sp_addextendedproperty 'MS_Description', N'主键ID', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management', 'COLUMN',
     'id'
go

exec sp_addextendedproperty 'MS_Description', N'是否删除（0：正常；1：删除；2：数据库手删除）', 'SCHEMA', 'dbo', 'TABLE',
     'Sys_Employee_Management', 'COLUMN', 'delete_status'
go

exec sp_addextendedproperty 'MS_Description', N'创建人ID', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'create_id'
go

exec sp_addextendedproperty 'MS_Description', N'创建人姓名', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'create_name'
go

exec sp_addextendedproperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'create_date'
go

exec sp_addextendedproperty 'MS_Description', N'修改人ID', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'modify_id'
go

exec sp_addextendedproperty 'MS_Description', N'修改人姓名', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'modify_name'
go

exec sp_addextendedproperty 'MS_Description', N'修改时间', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'modify_date'
go

exec sp_addextendedproperty 'MS_Description', N'员工编号', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'sys_employee_management_number'
go

exec sp_addextendedproperty 'MS_Description', N'中文名', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management', 'COLUMN',
     'chinese_name'
go

exec sp_addextendedproperty 'MS_Description', N'英文名', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management', 'COLUMN',
     'english_name'
go

exec sp_addextendedproperty 'MS_Description', N'身份证号', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'card_no'
go

exec sp_addextendedproperty 'MS_Description', N'电话', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management', 'COLUMN',
     'phone'
go

exec sp_addextendedproperty 'MS_Description', N'邮箱', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management', 'COLUMN',
     'email'
go

exec sp_addextendedproperty 'MS_Description', N'入职日期', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'record_date'
go

exec sp_addextendedproperty 'MS_Description', N'离职日期', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'leave_date'
go

exec sp_addextendedproperty 'MS_Description', N'休假结算日期', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'leave_settlement_date'
go

exec sp_addextendedproperty 'MS_Description', N'婚姻状态', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'marital_status'
go

exec sp_addextendedproperty 'MS_Description', N'试用期(月)', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'trial_period'
go

exec sp_addextendedproperty 'MS_Description', N'离职原因', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'leave_reason'
go

exec sp_addextendedproperty 'MS_Description', N'性别', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management', 'COLUMN',
     'user_gender'
go

exec sp_addextendedproperty 'MS_Description', N'科室', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management', 'COLUMN',
     'section'
go

exec sp_addextendedproperty 'MS_Description', N'部门', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management', 'COLUMN',
     'department'
go

exec sp_addextendedproperty 'MS_Description', N'职位(工种)', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'position'
go

exec sp_addextendedproperty 'MS_Description', N'假期类型 连接Sys_Leave_Type', 'SCHEMA', 'dbo', 'TABLE',
     'Sys_Employee_Management', 'COLUMN', 'leave_type_id'
go

exec sp_addextendedproperty 'MS_Description', N'工作时长(日/小时)', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'work_hour'
go

exec sp_addextendedproperty 'MS_Description', N'工作天数(周/天)', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'work_day'
go

exec sp_addextendedproperty 'MS_Description', N'假期组别', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'leave_group'
go

exec sp_addextendedproperty 'MS_Description', N'基本工资', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'basic_salary'
go

exec sp_addextendedproperty 'MS_Description', N'工资类型', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'salary_type'
go

exec sp_addextendedproperty 'MS_Description', N'工资金额', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'salary_amount'
go

exec sp_addextendedproperty 'MS_Description', N'调整金额', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'adjustment_amount'
go

exec sp_addextendedproperty 'MS_Description', N'生效日期', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'effective_date'
go

exec sp_addextendedproperty 'MS_Description', N'强基金计划', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'mpf_plan'
go

exec sp_addextendedproperty 'MS_Description', N'强基金类型', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'mpf_type'
go

exec sp_addextendedproperty 'MS_Description', N'雇主强积金', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'contribution_mpf_account'
go

exec sp_addextendedproperty 'MS_Description', N'雇员强积金', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'employee_mpf_account'
go

exec sp_addextendedproperty 'MS_Description', N'自愿性供款强积金', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'voluntary_mpf_contribution'
go

exec sp_addextendedproperty 'MS_Description', N'自愿性供款强积金类型', 'SCHEMA', 'dbo', 'TABLE',
     'Sys_Employee_Management', 'COLUMN', 'voluntary_mpf_contribution_type'
go

exec sp_addextendedproperty 'MS_Description', N'员工自愿性供款强积金', 'SCHEMA', 'dbo', 'TABLE',
     'Sys_Employee_Management', 'COLUMN', 'employee_voluntary_contribution'
go

exec sp_addextendedproperty 'MS_Description', N'雇主自愿性供款强积金', 'SCHEMA', 'dbo', 'TABLE',
     'Sys_Employee_Management', 'COLUMN', 'employer_voluntary_contribution'
go

exec sp_addextendedproperty 'MS_Description', N'支付信息', 'SCHEMA', 'dbo', 'TABLE', 'Sys_Employee_Management',
     'COLUMN', 'bank_info'
go

