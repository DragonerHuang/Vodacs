
using Microsoft.AspNetCore.Authorization;
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
    public partial class Sys_OrganizationController
    {
        private readonly ISys_OrganizationService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_OrganizationController(
            ISys_OrganizationService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost, Route("AddOrganization")]
        [ApiActionPermission]
        public IActionResult AddOrganization([FromBody] OrganizationDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelOrganization")]
        [ApiActionPermission]
        public IActionResult DelOrganization(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditOrganization")]
        [ApiActionPermission]
        public IActionResult EditOrganization([FromBody] OrganizationEditDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPut, Route("EnableOrganization")]
        [ApiActionPermission]
        public IActionResult EnableOrganization([FromBody] OrganizationEnableDto dto)
        {
            return Json(_service.Enable(dto));
        }

        [HttpPost, Route("GetOrganizationList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetOrganizationList([FromBody] PageInput<OrganizationEditDto> dto)
        {
            return Json(await _service.GetOrganizationList(dto));
        }
    }
}
