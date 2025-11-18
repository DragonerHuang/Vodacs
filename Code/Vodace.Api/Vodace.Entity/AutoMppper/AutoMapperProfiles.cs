using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vodace.Entity.DomainModels;

namespace Vodace.Entity.AutoMppper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Sys_User_Register, UserBaseDto>().ReverseMap();
            CreateMap<Sys_User_Register, UserAllDto>().ReverseMap();
            CreateMap<Sys_Worker_Register, WorkerDto>().ReverseMap();

            CreateMap<Biz_Project, ProjectDto>().ReverseMap();
            CreateMap<Biz_Contract, ContractDto>().ReverseMap();
            CreateMap<Sys_Contact, ContactDto>().ReverseMap();
            CreateMap<Sys_Contact, ContactDetailDto>().ReverseMap();
            CreateMap<Sys_Company, CompanyDto>().ReverseMap();
            CreateMap<Biz_Site_Relationship, SiteRelationshipDto>().ReverseMap();
            CreateMap<Biz_Site, SiteDto>().ReverseMap();
            CreateMap<Sys_Role, RoleDto>().ReverseMap();
            CreateMap<Sys_Role, EditRoleDto>().ReverseMap();
            CreateMap<Sys_Menu, MenuDto>().ReverseMap();

            CreateMap<Biz_Contract, ContractByvwDto>().ReverseMap();
            CreateMap<Biz_Quotation, QuotationByvwDto>().ReverseMap();
            CreateMap<Biz_Various_Work_Order, VariousWorkByvwDto>().ReverseMap();

            CreateMap<Sys_Dictionary, DictionaryDto>().ReverseMap();

            //CreateMap<Biz_Task, TaskDto>().ReverseMap();
            CreateMap<Biz_Task_Detail, TaskDetailDto>().ReverseMap();
            CreateMap<Biz_Task_Detail_Work_Type, TaskWorkTypeDto>().ReverseMap();
            CreateMap<Biz_Quotation_QA, QuotationQADto>().ReverseMap();
            CreateMap<Biz_Quotation_QA, QuotationQAWithFileAddDto>().ReverseMap();

            CreateMap<Biz_Upcoming_Events, Upcoming_EventsDto>().ReverseMap();
            CreateMap<Biz_Upcoming_Events, Upcoming_Events_OptionDto>().ReverseMap();

            CreateMap<Biz_Quotation, Biz_Quotation>().ReverseMap();
            CreateMap<Biz_Contract, Biz_Contract>().ReverseMap();
            CreateMap<Sys_Department, Department_Old_Dto>().ReverseMap();
            CreateMap<Biz_Project, ProjectDetailDto>().ReverseMap();

            CreateMap<Biz_Quotation_Record, QuotationRecordEditDto>().ReverseMap();
            CreateMap<Biz_Quotation_Record, QuotationRecordAddDto>().ReverseMap();
            CreateMap<Biz_Quotation_Record, SearchQuotationRecordDto>().ReverseMap();

            CreateMap<Sys_File_Config, AddFileConfigDto>().ReverseMap();
            CreateMap<Sys_File_Config, EditFileConfigDto>().ReverseMap();

            CreateMap<Biz_Quotation_Record_Excel, QuotationRecordExcelDto>().ReverseMap();
            CreateMap<Biz_Quotation_Record_Excel, QuotationRecordExcelEditDto>().ReverseMap();

            CreateMap<Sys_Config, ConfigAddDto>().ReverseMap();
            CreateMap<Sys_Config, ConfigEditDto>().ReverseMap();

            CreateMap<Sys_QuartzOptions, TaskAddDto>().ReverseMap();
            CreateMap<Sys_QuartzOptions, TaskAddDto>().ReverseMap();
            CreateMap<Sys_QuartzOptions, TaskListDto>().ReverseMap();

            CreateMap<Biz_Contact_Relationship, ContactRelationshipDto>().ReverseMap();

            CreateMap<Sys_Attendance_Record, AttendanceRecordDto>().ReverseMap();

            CreateMap<Sys_Leave_Type, LeaveTypeAddDto>().ReverseMap();
            CreateMap<Sys_Leave_Type, LeaveTypeEditDto>().ReverseMap();

            CreateMap<Sys_Leave_Holiday, LeaveHolidayAddDto>().ReverseMap();
            CreateMap<Sys_Leave_Holiday, LeaveHolidayEditDto>().ReverseMap();

            CreateMap<EditQuotationQADto, Biz_Quotation_QA>().ReverseMap();

            CreateMap<Biz_Submission_Files, SubmissionFilesAddDto>().ReverseMap();

            CreateMap<Sys_Message_Notification, MessageNotificationAddDto>().ReverseMap();

            CreateMap<Biz_Completion_Acceptance, CompletionAcceptanceAddDto>().ReverseMap();
            CreateMap<Biz_Completion_Acceptance, CompletionAcceptanceEditDto>().ReverseMap();

            CreateMap<Biz_Deadline_Management, DeadlineManagementAddDto>().ReverseMap();
            CreateMap<Biz_Deadline_Management, DeadlineManagementEditDto>().ReverseMap();

            CreateMap<Biz_Site_Work_Record_Worker, AddWorkerDto>().ReverseMap();

            //员工管理编辑或创建
            CreateMap<SysEmpMentInputDto, Sys_Employee_Management>().ReverseMap();
            ///员工管理查询输出
            CreateMap<SysEmpMentWebDto, Sys_Employee_Management>().ReverseMap();
            //编辑输出
            CreateMap<SysEmpMentOutputDto, Sys_Employee_Management>().ReverseMap();
        }
    }
}