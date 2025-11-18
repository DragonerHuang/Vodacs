
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
    public partial class Biz_Rolling_ProgramController
    {
        private readonly IBiz_Rolling_ProgramService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Rolling_ProgramController(
            IBiz_Rolling_ProgramService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("AddRollingProgram")]
        [ApiActionPermission]
        public IActionResult AddRollingProgram([FromBody] RollingProgramDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelRollingProgram")]
        [ApiActionPermission]
        public IActionResult DelRollingProgram(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditRollingProgram")]
        [ApiActionPermission]
        public IActionResult EditRollingProgram([FromBody] RollingProgramEditDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPost, Route("GetRollingProgramList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetRollingProgramList([FromBody] PageInput<RollingProgramEditDto> dto)
        {
            return Json(await _service.GetRollingProgramList(dto));
        }
    }
}
