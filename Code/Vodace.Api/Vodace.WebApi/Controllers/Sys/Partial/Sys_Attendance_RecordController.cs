
using log4net.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_Attendance_RecordController
    {
        private readonly ISys_Attendance_RecordService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Attendance_RecordController(
            ISys_Attendance_RecordService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取打卡记录
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userNo">用户编号</param>
        /// <param name="punchTime">打卡时间(yyyy-MM-dd)</param>
        /// <returns></returns>
        [HttpGet,Route("GetRecord"),ApiActionPermission]
        public async Task<IActionResult> GetRecord(int userId, string userNo, string punchTime) 
        {
            return Json(await _service.GetRecord(userId, userNo, punchTime));
        }
        /// <summary>
        /// 根据时间范围获取用户打卡记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost,Route("GetRecordListByPage"),ApiActionPermission]
        public async Task<IActionResult> GetRecordList([FromBody]PageInput<AttendanceRecordQuery> query) 
        {
            return Json( await _service.GetRecordList(query));
        }
        /// <summary>
        /// 添加打卡记录
        /// </summary>
        /// <param name="recordDto"></param>
        /// <returns></returns>
        [HttpPost,Route("AddData"),ApiActionPermission]
        public async Task<IActionResult> AddData([FromBody]AttendanceRecordDto recordDto)
        {
            return Json(await _service.AddData(recordDto));
        }
        //[HttpPost, Route("upRecord"), AllowAnonymous]
        //public IActionResult upRecord([FromBody] AttendanceRecordDto1 recordDto) 
        //{
        //    return Json("上传成功" );
        //}
        [HttpPost, Route("ClockIn"), AllowAnonymous]
        public async Task<IActionResult> ClockIn([FromBody] AttendanceRecordWebDto webDto) 
        {
            Log4NetHelper.Info($"打卡机数据:{JsonConvert.SerializeObject(webDto)}");
            return Json(await _service.ClockIn(webDto));
        }


        /// <summary>
        /// 根据时间范围获取用户最早最晚打卡记录
        /// </summary>
        /// <param name="query">传入实体</param>
        /// <returns></returns>
        [HttpPost, Route("GetMaxAndMinTimeRecordList"), ApiActionPermission]
        public async Task<IActionResult> GetMaxAndMinTimeAllRecordList([FromBody] AtdRecordMaxMinTimeQuery input)
        {
            return Json(await _service.GetMaxAndMinTimeRecordListAsync(input));
        }

        
        /// <summary>
        /// 根据时间范围获取用户最早最晚打卡记录
        /// </summary>
        /// <param name="query">传入实体</param>
        /// <returns></returns>
        [HttpPost, Route("GetDayRecordDetailsList"), ApiActionPermission]
        public async Task<IActionResult> GetDayRecordDetailsList([FromBody] AtdDayRecordDetailsQuery input)
        {
            return Json(await _service.GetDayRecordDetailsListAsync(input));
        }
        
    }
}
