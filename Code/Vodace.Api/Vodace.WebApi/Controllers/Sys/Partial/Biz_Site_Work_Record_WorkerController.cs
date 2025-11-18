
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Site_Work_Record_WorkerController
    {
        private readonly IBiz_Site_Work_Record_WorkerService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Site_Work_Record_WorkerController(
            IBiz_Site_Work_Record_WorkerService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 根据工地记录获取工人列表（分页）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetWorkerListPage")]
        [ApiActionPermission]
        public async Task<IActionResult> GetWorkerListPageAsync([FromBody] PageInput<SiteWorkerSearchDto> input)
        {
            var res = await _service.GetWorkerListPageAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 根据工地记录获取工人出勤列表（分页）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetCICAttendancePage")]
        [ApiActionPermission]
        public async Task<IActionResult> GetCICAttendancePageAsync([FromBody] PageInput<SiteWorkerSearchDto> input)
        {
            var res = await _service.GetCICAttendancePageAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 地盘出勤记录（完成CIC记录）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetSiteAttendanceRecords")]
        [ApiActionPermission]
        public async Task<IActionResult> GetSiteAttendanceRecords([FromBody] PageInput<SiteAttendanceRecordDto> input)
        {
            var res = await _service.GetSiteAttendanceRecords(input);
            return Json(res);
        }

        /// <summary>
        /// 工人出勤资料(实际薪资)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetContactAttendanceActualSalary")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactAttendanceActualSalary([FromBody] PageInput<SiteAttendanceRecordDto> input)
        {
            var res = await _service.GetContactAttendanceActualSalary(input);
            return Json(res);
        }

        /// <summary>
        /// 工人出勤资料（计薪标准）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetContactAttendanceSalaryCalculationStandard")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactAttendanceSalaryCalculationStandard([FromBody] PageInput<SiteAttendanceRecordDto> input)
        {
            var res = await _service.GetContactAttendanceSalaryCalculationStandard(input);
            return Json(res);
        }

        /// <summary>
        /// 地盘出勤统计（人天统计）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetSiteAttendanceTotal")]
        [ApiActionPermission]
        public async Task<IActionResult> GetSiteAttendanceTotal([FromBody] PageInput<SiteAttendanceRecordDto> input)
        {
            var res = await _service.GetSiteAttendanceTotal(input);
            return Json(res);
        }

        /// <summary>
        /// 地盘出勤统计（工资统计）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetSiteAttendanceSalaryTotal")]
        [ApiActionPermission]
        public async Task<IActionResult> GetSiteAttendanceSalaryTotal([FromBody] PageInput<SiteAttendanceRecordDto> input)
        {
            var res = await _service.GetSiteAttendanceSalaryTotal(input);
            return Json(res);
        }

        /// <summary>
        /// 编辑工人信息（app端）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("EditWorkerData")]
        [ApiActionPermission]
        public async Task<IActionResult> EditWorkerDataAsync([FromBody]EditWorkerDto input)
        {
            var res = await _service.EditWorkerDataAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 编辑工人信息（web端）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("EditWorkerDataByWeb")]
        [ApiActionPermission]
        public async Task<IActionResult> EditWorkerDataByWebAsync([FromBody] EditWorkByWebDto input)
        {
            var res = await _service.EditWorkerDataByWebAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 根据二维码信息添加工人
        /// </summary>
        /// <param name="record_id">工地记录id</param>
        /// <param name="contect_id">联系人id</param>
        /// <returns></returns>
        [HttpPost, Route("AddWorkerByQRCode")]
        [ApiActionPermission]
        public async Task<IActionResult> AddWorkerByQRCodeAsync(Guid record_id, Guid contect_id)
        {
            var res = await _service.AddWorkerByQRCodeAsync(record_id, contect_id);
            return Json(res);
        }

        /// <summary>
        /// 批量添加工人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("AddWorkerByContactList")]
        [ApiActionPermission]
        public async Task<IActionResult> AddWorkerByContactListAsync([FromBody] AddWorkerByContactDto input)
        {
            var res = await _service.AddWorkerByContactListAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 根据工地工作记录删除工人
        /// </summary>
        /// <param name="id">工人id</param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteWorkerById")]
        [ApiActionPermission]
        public async Task<IActionResult> DeleteWorkerByIdAsync(Guid id)
        {
            var res = await _service.DeleteWorkerByIdAsync(id);
            return Json(res);
        }

        /// <summary>
        /// 根据工人id下班打卡
        /// </summary>
        /// <param name="id">工人id</param>
        /// <returns></returns>
        [HttpPost, Route("WorkerTimeOutById")]
        [ApiActionPermission]
        public async Task<IActionResult> WorkerTimeOutByIdAsync(Guid id)
        {
            var res = await _service.WorkerTimeOutByIdAsync(id);
            return Json(res);
        }

        /// <summary>
        /// 根据工地工作记录id将未下班的工人进行下班打卡
        /// </summary>
        /// <param name="record_id">工地工作记录id</param>
        /// <returns></returns>
        [HttpPost, Route("WorkerTimeOutByAll")]
        [ApiActionPermission]
        public async Task<IActionResult> WorkerTimeOutByAllAsync(Guid record_id)
        {
            var res = await _service.WorkerTimeOutByAllAsync(record_id);
            return Json(res);
        }

        /// <summary>
        /// 工人上下班时间调整
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("WorkerTimeAdjustment")]
        [ApiActionPermission]
        public async Task<IActionResult> WorkerTimeAdjustmentAsync([FromBody]WorkerTimeChangeDto input)
        {
            var res = await _service.WorkerTimeAdjustmentAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 手动调整工地工作记录工人薪资
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("WorkerSalaryChange")]
        [ApiActionPermission]
        public async Task<IActionResult> WorkerSalaryChangeAsync([FromBody] WorkerSalaryChangeDto input)
        {
            var res = await _service.WorkerSalaryChangeAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 设置当值值更管理人员
        /// </summary>
        /// <param name="id">工人id</param>
        /// <param name="record_id">工地工作记录表id</param>
        /// <returns></returns>
        [HttpPost, Route("SetSiteWorkRecordCP")]
        [ApiActionPermission]
        public async Task<IActionResult> SetSiteWorkRecordCPAsync(Guid id, Guid record_id)
        {
            var res = await _service.SetSiteWorkRecordCPAsync(id, record_id);
            return Json(res);
        }

        /// <summary>
        /// 根据工地工作记录id获取工人管工列表
        /// </summary>
        /// <param name="record_id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetSiteWorkCP")]
        [ApiActionPermission]
        public async Task<IActionResult> GetSiteWorkCPAsync(Guid record_id)
        {
            var res = await _service.GetSiteWorkCPAsync(record_id);
            return Json(res);
        }

        /// <summary>
        /// 编辑工地工作记录工人考勤记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("EditWorkerAttendance")]
        [ApiActionPermission]
        public async Task<IActionResult> EditWorkerAttendance([FromBody] EditWorkerAttendanceDto input)
        {
            var res = await _service.EditWorkerAttendanceAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 设置工人资料工种对应的薪资
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("SetContactWorkTypeSalary")]
        [ApiActionPermission]
        public async Task<IActionResult> SetContactWorkTypeSalary([FromBody] SetContactWorkTypeSalaryDto input)
        {
            var res = await _service.SetContactWorkTypeSalaryAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 分页查询工人资料-计薪标准
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost, Route("GetContactWorkTypeSalary")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactWorkTypeSalaryAsync([FromBody] PageInput<ContactWorkTypeSearchDto> search)
        {
            var res = await _service.GetContactWorkTypeSalaryAsync(search);
            return Json(res);
        }

        /// <summary>
        /// 分页查询工人资料-计薪标准
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost, Route("GetContactWorkTypeSalaryByMonth")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactWorkTypeSalaryByMonthAsync([FromBody] PageInput<ContactWorkTypeSearchDto> search)
        {
            var res = await _service.GetContactWorkTypeSalaryByMonthAsync(search);
            return Json(res);
        }
    }
}
