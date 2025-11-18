
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
    public partial class Sys_Leave_RecordController
    {
        private readonly ISys_Leave_RecordService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Leave_RecordController(
            ISys_Leave_RecordService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("AddLeaveRecord")]
        [ApiActionPermission]
        public IActionResult AddLeaveRecord([FromForm] LeaveRecordDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelLeaveRecord")]
        [ApiActionPermission]
        public IActionResult DelLeaveRecord(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditLeaveRecord")]
        [ApiActionPermission]
        public IActionResult EditLeaveRecord([FromForm] LeaveRecordEditDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPut, Route("ReviewLeaveRecord")]
        [ApiActionPermission]
        public IActionResult ReviewLeaveRecord([FromBody] LeaveRecordReviewDto dto)
        {
            return Json(_service.Review(dto));
        }

        [HttpPost, Route("GetLeaveRecordList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetLeaveRecordList([FromBody] PageInput<SearchLeaveRecordDto> dto)
        {
            return Json(await _service.GetLeaveRecordList(dto));
        }


        [HttpGet, Route("GetLeaveRecordStatus")]
        [ApiActionPermission]
        public IActionResult GetLeaveRecordStatus()
        {
            return Json(_service.GetLeaveRecordStatus());
        }

        [HttpGet, Route("GetLeaveRecordType")]
        [ApiActionPermission]
        public async Task<IActionResult> GetLeaveRecordType()
        {
            return Json(await _service.GetLeaveRecordType());
        }
    }
}
