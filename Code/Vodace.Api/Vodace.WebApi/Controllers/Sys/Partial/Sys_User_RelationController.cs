
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
    public partial class Sys_User_RelationController
    {
        private readonly ISys_User_RelationService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_User_RelationController(
            ISys_User_RelationService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost, Route("AddUserRelation")]
        [ApiActionPermission]
        public IActionResult AddUserRelation([FromBody] SysUserRelationDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelUserRelation")]
        [ApiActionPermission]
        public IActionResult DelUserRelation(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditUserRelation")]
        [ApiActionPermission]
        public IActionResult EditUserRelation([FromBody] SysUserRelationDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPost, Route("GetSysUserRelationList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetSysUserRelationList([FromBody] PageInput<SysUserRelationSearchDto> dto)
        {
            return Json(await _service.GetSysUserRelationList(dto));
        }
    }
}
