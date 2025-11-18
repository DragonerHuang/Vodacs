
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
    public partial class Sys_Leave_BalanceController
    {
        private readonly ISys_Leave_BalanceService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Leave_BalanceController(
            ISys_Leave_BalanceService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost, Route("AddLeaveBalance")]
        [ApiActionPermission]
        public IActionResult AddLeaveBalance([FromBody] LeaveBalanceDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelLeaveBalance")]
        [ApiActionPermission]
        public IActionResult DelLeaveBalance(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditLeaveBalance")]
        [ApiActionPermission]
        public IActionResult EditLeaveBalance([FromBody] LeaveBalanceEditDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPost, Route("GetLeaveBalanceList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetLeaveBalanceList([FromBody] PageInput<LeaveBalanceEditDto> dto)
        {
            return Json(await _service.GetLeaveBalanceList(dto));
        }
    }
}
