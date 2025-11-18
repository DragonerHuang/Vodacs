insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
values(newid(), 0, N'期限管理文件_图片',1,getdate(),'Deadline_Management_Img','\Deadline_Management\Img')

insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
values(newid(), 0, N'期限管理文件_文件',1,getdate(),'Deadline_Management_Doc','\Deadline_Management\Doc')

insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
values(newid(), 0, N'提交文件_资料文件',1,getdate(),'Submission_Information','\Submission\Information')

insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
values(newid(), 0, N'提交文件_编辑文件',1,getdate(),'Submission_Edit','\Submission\Edit')

insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
values(newid(), 0, N'提交文件_提交文件',1,getdate(),'Submission_Submit','\Submission\Submit')

insert Sys_File_Config(id, delete_status, remark,create_id,create_date,file_code,folder_path)
values(newid(), 0, N'提交文件_客户评语文件',1,getdate(),'Submission_Comments','\Submission\Comments')

ALTER TABLE Biz_Project_Files ADD file_version INT
ALTER TABLE Sys_File_Config  alter column  file_code  nvarchar(100) 