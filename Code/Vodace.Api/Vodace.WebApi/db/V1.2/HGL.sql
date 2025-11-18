


alter table Sys_User_Relation add day_salary decimal(18,2)
alter table Sys_User_Relation add night_salary decimal(18,2)
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_User_Relation', @level2name='day_salary', @value=N'白班薪资';
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Sys_User_Relation', @level2name='night_salary', @value=N'晚班薪资';



alter table Biz_Rolling_Program add percentage decimal(18,2)
EXEC sp_addextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA', @level0name=dbo, @level1type=N'TABLE', @level2type=N'COLUMN', @level1name='Biz_Rolling_Program', @level2name='percentage', @value=N'完成百分比';


alter table Biz_Rolling_Program alter column duty nvarchar(20)
















