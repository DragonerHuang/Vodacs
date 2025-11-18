
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
    public partial class Sys_Construction_Content_InitController
    {
        private readonly ISys_Construction_Content_InitService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Construction_Content_InitController(
            ISys_Construction_Content_InitService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost, Route("AddContentInit")]
        [ApiActionPermission]
        public IActionResult AddContentInit([FromBody] ConstructionContentInitDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelContentInit")]
        [ApiActionPermission]
        public IActionResult DelContentInit(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditContentInit")]
        [ApiActionPermission]
        public IActionResult EditContentInit([FromBody] ConstructionContentInitEditDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPost, Route("GetContentInitList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContentInitList([FromBody] PageInput<ConstructionContentInitEditDto> dto)
        {
            return Json(await _service.GetContentInitList(dto));
        }

        [HttpGet, Route("GetContentIntiWorkTypeList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContentIntiWorkTypeList()
        {
            return Json(await _service.GetContentIntiWorkTypeList());
        }

        /// <summary>
        /// 导入工程初始化文件
        /// </summary>
        /// <param name="file">导入文件</param>
        /// <returns></returns>
        [HttpPost, Route("ImportData")]
        [ApiActionPermission]
        public IActionResult ImportData(IFormFile file)
        {
            return Json(_service.ImportData(file));
        }
    }
}
