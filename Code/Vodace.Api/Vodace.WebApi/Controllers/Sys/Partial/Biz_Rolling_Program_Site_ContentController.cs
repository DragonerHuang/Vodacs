
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
    public partial class Biz_Rolling_Program_Site_ContentController
    {
        private readonly IBiz_Rolling_Program_Site_ContentService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Rolling_Program_Site_ContentController(
            IBiz_Rolling_Program_Site_ContentService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("AddRollingProgramSiteContent")]
        [ApiActionPermission]
        public IActionResult AddRollingProgramSiteContent([FromBody] RollingProgramSiteContentAddDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelRollingProgramSiteContent")]
        [ApiActionPermission]
        public IActionResult DelRollingProgramSiteContent(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditRollingProgramSiteContent")]
        [ApiActionPermission]
        public IActionResult EditRollingProgramSiteContent([FromBody] RollingProgramSiteContentAddDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPost, Route("GetRollingProgramSiteContentList")]
        [ApiActionPermission]
        public IActionResult GetRollingProgramSiteContentList([FromBody] PageInput<RollingProgramSiteContentSearchDto> dto)
        {
            return Json(_service.GetRollingProgramSiteContentList(dto));
        }

        [HttpPost, Route("GetVersion")]
        [ApiActionPermission]
        public async Task<IActionResult> GetVersion([FromBody] RollingProgramSiteContentDto dto)
        {
            return Json(await _service.GetVersion(dto));
        }
    }
}
