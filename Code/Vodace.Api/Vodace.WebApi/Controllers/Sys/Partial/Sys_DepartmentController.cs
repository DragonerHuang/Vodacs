/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Sys_Department",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Filters;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_DepartmentController
    {

        private readonly ISys_DepartmentService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_DepartmentController(
            ISys_DepartmentService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost, Route("AddDepartment")]
        [ApiActionPermission]
        public IActionResult AddDepartment([FromBody] DepartmentDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelDepartment")]
        [ApiActionPermission]
        public IActionResult DelDepartment(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditDepartment")]
        [ApiActionPermission]
        public IActionResult EditDepartment([FromBody] DepartmentEditDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPut, Route("EnableDepartment")]
        [ApiActionPermission]
        public IActionResult EnableDepartment([FromBody] DepartmentEnableDto dto)
        {
            return Json(_service.Enable(dto));
        }

        [HttpPost, Route("GetDepartmentList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetDepartmentList([FromBody] PageInput<DepartmentEditDto> dto)
        {
            return Json(await _service.GetDepartmentList(dto));
        }
    }
}

