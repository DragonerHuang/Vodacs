
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Site_Work_Record_WorkerService
    {
        /// <summary>
        /// 根据工地记录获取工人列表（分页）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetWorkerListPageAsync(PageInput<SiteWorkerSearchDto> search);

        /// <summary>
        /// 根据工地记录获取工人出勤列表（分页）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetCICAttendancePageAsync(PageInput<SiteWorkerSearchDto> search);

        /// <summary>
        /// 编辑工人信息（app端）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditWorkerDataAsync(EditWorkerDto input);

        /// <summary>
        /// 编辑工人信息（web端）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditWorkerDataByWebAsync(EditWorkByWebDto input);

        /// <summary>
        /// 根据二维码添加工人
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="contectId"></param>
        /// <returns></returns>
        Task<WebResponseContent> AddWorkerByQRCodeAsync(Guid recordId, Guid contectId);

        /// <summary>
        /// 批量添加工人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> AddWorkerByContactListAsync(AddWorkerByContactDto input);

        /// <summary>
        /// 根据工地工作记录删除工人
        /// </summary>
        /// <param name="id">工人id</param>
        /// <returns></returns>
        Task<WebResponseContent> DeleteWorkerByIdAsync(Guid id);

        /// <summary>
        /// 根据工人id下班打卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> WorkerTimeOutByIdAsync(Guid id);

        /// <summary>
        /// 根据工地工作记录id将未下班的工人进行下班打卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> WorkerTimeOutByAllAsync(Guid recordId);

        /// <summary>
        /// 工人上下班时间调整
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> WorkerTimeAdjustmentAsync(WorkerTimeChangeDto input);

        /// <summary>
        /// 手动调整工地工作记录工人薪资
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> WorkerSalaryChangeAsync(WorkerSalaryChangeDto input);

        /// <summary>
        /// 设置当前值更管理人员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        Task<WebResponseContent> SetSiteWorkRecordCPAsync(Guid id, Guid recordId);

        /// <summary>
        /// 根据工地工作记录id获取工人管工列表
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetSiteWorkCPAsync(Guid recordId);

        /// <summary>
        /// 编辑工地工作记录工人考勤记录
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditWorkerAttendanceAsync(EditWorkerAttendanceDto inputDto);

        /// <summary>
        /// 地盘出勤记录（完成CIC记录）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetSiteAttendanceRecords(PageInput<SiteAttendanceRecordDto> search);
        /// <summary>
        /// 工人出勤资料(实际薪资)
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetContactAttendanceActualSalary(PageInput<SiteAttendanceRecordDto> search);
        /// <summary>
        /// 工人出勤资料（计薪标准）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetContactAttendanceSalaryCalculationStandard(PageInput<SiteAttendanceRecordDto> search);
        /// <summary>
        /// 地盘出勤统计（人天统计）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetSiteAttendanceTotal(PageInput<SiteAttendanceRecordDto> search);
        /// <summary>
        /// 地盘出勤统计（工资统计）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetSiteAttendanceSalaryTotal(PageInput<SiteAttendanceRecordDto> search);

        /// <summary>
        /// 设置工人资料工种对应的薪资
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> SetContactWorkTypeSalaryAsync(SetContactWorkTypeSalaryDto input);

        /// <summary>
        /// 分页查询工人资料-计薪标准
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetContactWorkTypeSalaryAsync(PageInput<ContactWorkTypeSearchDto> searchDto);

        /// <summary>
        /// 查询工人资料-实际薪资（分页）
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetContactWorkTypeSalaryByMonthAsync(PageInput<ContactWorkTypeSearchDto> searchDto);
    }
 }
