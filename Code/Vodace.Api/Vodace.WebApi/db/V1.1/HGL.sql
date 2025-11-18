

insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
values(newid(), 0, N'工程内容初始化文件',1,getdate(),'Construction_Content_Init','\Construction_Content_Init\')
	
alter table Biz_Contact_Relationship add org_id uniqueidentifier


alter table Sys_User_New add doj datetime
alter table Sys_User_New add is_current int
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_User_New', @level2name='doj', @value=N'入职时间（Date of joining）';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_User_New', @level2name='is_current', @value=N'是否在职 1-是 0-否';


alter table Sys_Work_Type add day_salary decimal(18,2)
alter table Sys_Work_Type add night_salary decimal(18,2)
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Work_Type', @level2name='day_salary', @value=N'白天薪资';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Work_Type', @level2name='night_salary', @value=N'夜班薪资';

	
if object_id('Sys_Construction_Content_Init', 'U') is not null
begin
	drop table Sys_Construction_Content_Init;
end
create table Sys_Construction_Content_Init(
	id uniqueidentifier primary key,
	line_number int,
	level int,
	master_id uniqueidentifier,
	external_link_id uniqueidentifier,
	item_code nvarchar(250),	--site survey里面的2.1.8下面的key有超长的存在
	content nvarchar(2000),
	work_type varchar(20),
	point_type varchar(20),
	extend_attr varchar(2000),
	exp_type nvarchar(20),
	exp_value nvarchar(500),

	delete_status int default(0),
	remark nvarchar(500),
	create_id int,
	create_name nvarchar(30),
	create_date datetime default(getdate()),
	modify_id int,
	modify_name nvarchar(30),
	modify_date datetime
);
create index Sys_Construction_Content_Init_work_type_index on Sys_Construction_Content_Init(work_type);
create index Sys_Construction_Content_Init_item_code_index on Sys_Construction_Content_Init(item_code);
create index Sys_Construction_Content_Init_master_id_index on Sys_Construction_Content_Init(master_id);
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level1name='Sys_Construction_Content_Init', @value=N'施工具体内容初始化';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='delete_status', @value=N'是否删除（0：正常；1：删除；2：数据库手删除）';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='level', @value=N'内容级别：1级、2级、3级，目前最高三级数据';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='master_id', @value=N'上级数据id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='external_link_id', @value=N'外链接id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='line_number', @value=N'内容行号';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='item_code', @value=N'施工内容编码';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='content', @value=N'施工内容';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='work_type', @value=N'工程类型：Pre Work, Site Work, Site Survey, Sub-C.Work, T&C, O&M';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='point_type', @value=N'工作类型：Check Point, Whole Point, Daily Point';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='extend_attr', @value=N'扩展属性';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='exp_type', @value=N'扩展类型：text,radio,checkbox,file....';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Construction_Content_Init', @level2name='exp_value', @value=N'扩展类型值';


if object_id('Biz_Rolling_Program_Task', 'U') is not null
begin
	drop table Biz_Rolling_Program_Task;
end
create table Biz_Rolling_Program_Task(
	id uniqueidentifier primary key,
	project_id uniqueidentifier,
	contract_id uniqueidentifier,
	customer nvarchar(20),
	category nvarchar(20),
	task_name nvarchar(20),
	version nvarchar(10),

	delete_status int default(0),
	remark nvarchar(500),
	create_id int,
	create_name nvarchar(30),
	create_date datetime default(getdate()),
	modify_id int,
	modify_name nvarchar(30),
	modify_date datetime
);
create index Biz_Rolling_Program_Task_project_id_index on Biz_Rolling_Program_Task(project_id);
create index Biz_Rolling_Program_Task_contract_id_index on Biz_Rolling_Program_Task(contract_id);
create index Biz_Rolling_Program_Task_customer_index on Biz_Rolling_Program_Task(customer);
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level1name='Biz_Rolling_Program_Task', @value=N'滚动计划任务';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Task', @level2name='project_id', @value=N'项目id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Task', @level2name='contract_id', @value=N'合约id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Task', @level2name='customer', @value=N'客户';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Task', @level2name='category', @value=N'工程类型';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Task', @level2name='task_name', @value=N'滚动计划任务名称';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Task', @level2name='version', @value=N'版本号';



if object_id('Biz_Rolling_Program_Site_Content', 'U') is not null
begin
	drop table Biz_Rolling_Program_Site_Content;
end
create table Biz_Rolling_Program_Site_Content(
	id uniqueidentifier primary key,
	line_number int,
	project_id uniqueidentifier,
	contract_id uniqueidentifier,
	task_id uniqueidentifier,
	site_id uniqueidentifier,
	cc_id uniqueidentifier,
	item_code nvarchar(250),
	content nvarchar(500),
	work_type nvarchar(60),
	point_type nvarchar(60),
	number int,
	is_generate int,

	delete_status int default(0),
	remark nvarchar(500),
	create_id int,
	create_name nvarchar(30),
	create_date datetime default(getdate()),
	modify_id int,
	modify_name nvarchar(30),
	modify_date datetime
);
create index Biz_Rolling_Program_Site_Content_project_id_index on Biz_Rolling_Program_Site_Content(project_id);
create index contract_id_index on Biz_Rolling_Program_Site_Content(contract_id);
create index task_id_index on Biz_Rolling_Program_Site_Content(task_id);
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level1name='Biz_Rolling_Program_Site_Content', @value='滚动计划工地内容';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='project_id', @value=N'项目id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='contract_id', @value=N'合约id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='line_number', @value=N'序号';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='task_id', @value=N'滚动计划任务id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='site_id', @value=N'工地id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='cc_id', @value=N'施工具体内容初始化id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='item_code', @value=N'内容编码';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='content', @value=N'内容';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='work_type', @value=N'工作类型';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='point_type', @value=N'节点类型';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='number', @value=N'数量';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program_Site_Content', @level2name='is_generate', @value=N'是否生成任务 1-是 0-否';




if object_id('Biz_Rolling_Program', 'U') is not null
begin
	drop table Biz_Rolling_Program;
end
create table Biz_Rolling_Program(
	id uniqueidentifier primary key,
	project_id uniqueidentifier,
	contract_id uniqueidentifier,
	task_id uniqueidentifier,
	sc_id uniqueidentifier,
	cc_id uniqueidentifier,

	line_number int,
	level int,
	org_id varchar(500),
	site_id varchar(500),
	master_id uniqueidentifier,
	item_code nvarchar(20),
	content nvarchar(2000),
	work_type nvarchar(60),
	point_type nvarchar(60),
	start_date datetime,
	end_date datetime,

	director int,
	quotation decimal(18,2),
	duty nvarchar(10),
	color nvarchar(20),
	track_scope int,	
	exp_type nvarchar(20),
	exp_value nvarchar(500),


	delete_status int default(0),
	remark nvarchar(500),
	create_id int,
	create_name nvarchar(30),
	create_date datetime default(getdate()),
	modify_id int,
	modify_name nvarchar(30),
	modify_date datetime
);
create index Biz_Rolling_Program_project_id_index on Biz_Rolling_Program(project_id);
create index Biz_Rolling_Program_contract_id_index on Biz_Rolling_Program(contract_id);
create index Biz_Rolling_Program_task_id_index on Biz_Rolling_Program(task_id);
create index sc_id_index on Biz_Rolling_Program(sc_id);
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level1name='Biz_Rolling_Program', @value='滚动计划';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='project_id', @value=N'项目id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='contract_id', @value=N'合约id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='task_id', @value=N'滚动计划任务id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='sc_id', @value=N'滚动计划工地内容id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='cc_id', @value=N'施工具体内容初始化id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='level', @value=N'内容级别：1级、2级、3级，目前最高三级数据';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='master_id', @value=N'上级数据id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='org_id', @value=N'内容所属组织ids，多个之间以逗号分割';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='site_id', @value=N'工地ids，多个之间以逗号之间分割';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='line_number', @value=N'内容行号';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='item_code', @value=N'施工内容编码';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='content', @value=N'施工内容';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='work_type', @value=N'工作类型';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='point_type', @value=N'节点类型';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='start_date', @value=N'开始时间';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='end_date', @value=N'结束时间';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='director', @value=N'负责人';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='quotation', @value=N'相差报价细分金额';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='duty', @value=N'值更：早班、中班、晚班';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='color', @value=N'色块';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='track_scope', @value=N'是否路轨范围工作，0-否 1-是';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='exp_type', @value=N'扩展类型：text,radio,checkbox,file....';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='exp_value', @value=N'扩展类型值';

if object_id('Sys_Department', 'U') is not null
begin
	drop table Sys_Department;
end
create table Sys_Department(
	id uniqueidentifier primary key,
	master_id uniqueidentifier,
	name_eng nvarchar(200),
	name_cht nvarchar(200),
	name_sho nvarchar(200),
	name_ali nvarchar(200),
	enable int default(0),

	delete_status int default(0),
	remark nvarchar(500),
	create_id int,
	create_name nvarchar(30),
	create_date datetime default(getdate()),
	modify_id int,
	modify_name nvarchar(30),
	modify_date datetime
);
create index Sys_Department_work_enable_index on Sys_Department(enable);
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level1name='Sys_Department', @value=N'部门';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Department', @level2name='delete_status', @value=N'是否删除（0：正常；1：删除；2：数据库手删除）';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Department', @level2name='master_id', @value=N'上级数据id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Department', @level2name='name_eng', @value=N'英文名称';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Department', @level2name='name_cht', @value=N'中文名称';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Department', @level2name='name_sho', @value=N'简称';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Department', @level2name='name_ali', @value=N'别名';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Department', @level2name='enable', @value=N'是否启用（1：启用；0：禁用）';




if object_id('Sys_Organization', 'U') is not null
begin
	drop table Sys_Organization;
end
create table Sys_Organization(
	id uniqueidentifier primary key,
	master_id uniqueidentifier,
	name_eng nvarchar(200),
	name_cht nvarchar(200),
	name_sho nvarchar(200),
	name_ali nvarchar(200),
	enable int default(0),

	delete_status int default(0),
	remark nvarchar(500),
	create_id int,
	create_name nvarchar(30),
	create_date datetime default(getdate()),
	modify_id int,
	modify_name nvarchar(30),
	modify_date datetime
);
create index Sys_Department_work_enable_index on Sys_Organization(enable);
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level1name='Sys_Organization', @value=N'组织';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Organization', @level2name='delete_status', @value=N'是否删除（0：正常；1：删除；2：数据库手删除）';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Organization', @level2name='master_id', @value=N'上级数据id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Organization', @level2name='name_eng', @value=N'英文名称';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Organization', @level2name='name_cht', @value=N'中文名称';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Organization', @level2name='name_sho', @value=N'简称';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Organization', @level2name='name_ali', @value=N'别名';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Organization', @level2name='enable', @value=N'是否启用（1：启用；0：禁用）';



if object_id('Biz_ContractOrg', 'U') is not null
begin
	drop table Biz_ContractOrg;
end
create table Biz_ContractOrg(
	id uniqueidentifier primary key,
	master_id uniqueidentifier,
	contract_id uniqueidentifier,
	is_special int,
	org_id uniqueidentifier,
	user_id int,
	submit_file_code nvarchar(60),

	delete_status int default(0),
	remark nvarchar(500),
	create_id int,
	create_name nvarchar(30),
	create_date datetime default(getdate()),
	modify_id int,
	modify_name nvarchar(30),
	modify_date datetime
);
create index Sys_Department_work_contract_id_index on Biz_ContractOrg(contract_id);
create index Sys_Department_work_org_id_index on Biz_ContractOrg(org_id);
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level1name='Biz_ContractOrg', @value=N'合约组织';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_ContractOrg', @level2name='delete_status', @value=N'是否删除（0：正常；1：删除；2：数据库手删除）';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_ContractOrg', @level2name='master_id', @value=N'上级数据id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_ContractOrg', @level2name='contract_id', @value=N'合约id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_ContractOrg', @level2name='org_id', @value=N'组织id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_ContractOrg', @level2name='user_id', @value=N'用户id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_ContractOrg', @level2name='is_special', @value=N'特殊职位（一人之下万人之上）1-是 0-否';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_ContractOrg', @level2name='submit_file_code', @value=N'提交文件编码';





if object_id('Sys_Leave_Balance_Record', 'U') is not null
begin
	drop table Sys_Leave_Balance_Record;
end
create table Sys_Leave_Balance_Record(
	id uniqueidentifier primary key,
	user_id int,
	user_no nvarchar(30),
	year int,
	leave_type_code nvarchar(30),
	leave_type_name nvarchar(120),
	is_leave int,
	pay_type int,
	spend decimal(18,2),
	remaing_leave decimal(18,2),
	leave_balance_id uniqueidentifier,

	delete_status int default(0),
	remark nvarchar(500),
	create_id int,
	create_name nvarchar(30),
	create_date datetime default(getdate()),
	modify_id int,
	modify_name nvarchar(30),
	modify_date datetime
);
create index Sys_Leave_Balance_Record_year_index on Sys_Leave_Balance_Record(year);
create index Sys_Leave_Balance_Record_user_id_index on Sys_Leave_Balance_Record(user_id);
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level1name='Sys_Leave_Balance_Record', @value=N'假期消费记录';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='delete_status', @value=N'是否删除（0：正常；1：删除；2：数据库手删除）';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='remaing_leave', @value=N'剩余假期';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='spend', @value=N'消费天数';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='is_leave', @value=N'是否假期类型 0:不是假期，1：是假期';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='pay_type', @value=N'0：有工资，1：没有工资';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='leave_type_name', @value=N'假期名字';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='leave_type_code', @value=N'假期代号';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='year', @value=N'年份';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='user_no', @value=N'工号';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='user_id', @value=N'员工id';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_Leave_Balance_Record', @level2name='leave_balance_id', @value=N'请假id';




