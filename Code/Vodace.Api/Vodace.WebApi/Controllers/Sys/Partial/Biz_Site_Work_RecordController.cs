
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Site_Work_RecordController
    {
        private readonly IBiz_Site_Work_RecordService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Site_Work_RecordController(
            IBiz_Site_Work_RecordService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 分页查询工地工作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetSiteWorkRecordPage")]
        [ApiActionPermission]
        public async Task<IActionResult> GetSiteWorkRecordPage([FromBody] PageInput<SiteWorkRecordSearchDto> input)
        {
            var res = await _service.GetSiteWorkRecordPageAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 根据时间查询工地工作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetSiteWorkRecordByDate")]
        [ApiActionPermission]
        public async Task<IActionResult> GetSiteWorkRecordByDateAsync([FromBody] SiteWorkRecordSearchDto input)
        {
            var res = await _service.GetSiteWorkRecordByDateAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 根据id获取工地记录详情
        /// </summary>
        /// <param name="id">工地记录id</param>
        /// <returns></returns>
        [HttpGet, Route("GetSiteWorkRecordById")]
        [ApiActionPermission]
        public async Task<IActionResult> GetSiteWorkRecordById(Guid id)
        {
            var res = await _service.GetSiteWorkRecordByIdAsync(id);
            return Json(res);
        }

        /// <summary>
        /// 编辑工地工作记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("EditSiteWorkRecord")]
        [ApiActionPermission]
        public async Task<IActionResult> EditSiteWorkRecordAsync([FromBody] EditSiteWorkRecordDto input)
        {
            var res = await _service.EditSiteWorkRecordAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 删除工地工作记录
        /// </summary>
        /// <param name="id">工地工作记录id</param>
        /// <returns></returns>
        [HttpDelete, Route("EditSiteWorkRecord")]
        [ApiActionPermission]
        public async Task<IActionResult> DeleteSiteWorkRecordByIdAsync(Guid id)
        {
            var res = await _service.DeleteSiteWorkRecordByIdAsync(id);
            return Json(res);
        }

        /// <summary>
        /// 设置工程进度完成百分比
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("SetProjectProgress")]
        [ApiActionPermission]
        public async Task<IActionResult> SetProjectProgressAsync([FromBody]SetProgressDto input)
        {
            var res = await _service.SetProjectProgressAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 获取工程进度完成百分比
        /// </summary>
        /// <param name="id">工地工作记录id</param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectProgress")]
        [ApiActionPermission]
        public async Task<IActionResult> GetProjectProgressAsync(Guid id)
        {
            var res = await _service.GetProjectProgressAsync(id);
            return Json(res);
        }


        /// <summary>
        /// 根据工地记录id获取图片列表
        /// </summary>
        /// <param name="id">工地记录id</param>
        /// <returns></returns>
        [HttpGet, Route("GetPhotoListByRecord")]
        [ApiActionPermission]
        public async Task<IActionResult> GetPhotoListByRecordAsync(Guid record_id)
        {
            var res = await _service.GetPhotoListByRecordAsync(record_id);
            return Json(res);
        }

        /// <summary>
        /// 根据工地记录id和图片类型获取图片列表
        /// </summary>
        /// <param name="record_id">工地记录id</param>
        /// <param name="type">图片类型（6：开工前，7：施工中，8：完工后）</param>
        /// <returns></returns>
        [HttpGet, Route("GetPhotoListByType")]
        [ApiActionPermission]
        public async Task<IActionResult> GetPhotoListByTypeAsync(Guid record_id, int type)
        {
            var res = await _service.GetPhotoListByTypeAsync(record_id, type);
            return Json(res);
        }

        /// <summary>
        /// 根据工地记录id和图片类型获取图片列表
        /// </summary>
        /// <param name="record_id">工地记录id</param>
        /// <param name="type">图片类型（6：开工前，7：施工中，8：完工后）</param>
        /// <returns></returns>
        [HttpPost, Route("UploadPhoto")]
        [ApiActionPermission]
        public async Task<IActionResult> UploadPhotoAsync([FromForm] Guid record_id, [FromForm] int type, List<IFormFile> files)
        {
            var res = await _service.UploadPhotoAsync(record_id, type, files);
            return Json(res);
        }


        /// <summary>
        /// 获取滚动计划任务进度数据
        /// </summary>
        /// <param name="record_id">合约id</param>
        /// <param name="type">任务id</param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectProgressTableDataAsync")]
        [ApiActionPermission]
        public async Task<IActionResult> GetProjectProgressTableDataAsync(Guid contractId, Guid taskId)
        {
            var res = await _service.GetProjectProgressTableDataAsync(contractId, taskId);
            return Json(res);
        }

        /// <summary>
        /// 获取滚动计划进度数据
        /// </summary>
        /// <param name="record_id">合约id</param>
        /// <param name="type">任务id</param>
        /// <returns></returns>
        [HttpGet, Route("GetRollingProgramScheduleDataAsync")]
        [ApiActionPermission]
        public async Task<IActionResult> GetRollingProgramScheduleDataAsync(Guid contractId, Guid taskId)
        {
            var res = await _service.GetRollingProgramScheduleDataAsync(contractId, taskId);
            return Json(res);
        }
    }
}
