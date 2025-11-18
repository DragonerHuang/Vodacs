
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
    public partial class Biz_Rolling_Program_TaskController
    {
        private readonly IBiz_Rolling_Program_TaskService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Rolling_Program_TaskController(
            IBiz_Rolling_Program_TaskService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("AddRollingProgramTask")]
        [ApiActionPermission]
        public IActionResult AddRollingProgramTask([FromBody] RollingProgramTaskDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelRollingProgramTask")]
        [ApiActionPermission]
        public IActionResult DelRollingProgramTask(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditRollingProgramTask")]
        [ApiActionPermission]
        public IActionResult EditRollingProgramTask([FromBody] RollingProgramTaskDto dto)
        {
            return Json(_service.Edit(dto));
        }
    }
}
