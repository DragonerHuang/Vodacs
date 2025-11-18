
using log4net.Config;
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
    public partial class Sys_Work_TypeController
    {
        private readonly ISys_Work_TypeService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Work_TypeController(
            ISys_Work_TypeService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 获取工种信息
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("GetDataTree")]
        [ApiActionPermission]
        public async Task<IActionResult> GetDataTree() 
        {
            return Json(await _service.GetDataTree());
        }

        [HttpPost, Route("AddWorkType")]
        [ApiActionPermission]
        public IActionResult AddWorkType([FromBody] WorkTypeDto dto)
        {
            return Json(_service.Add(dto));
        }

        [HttpDelete, Route("DelWorkType")]
        [ApiActionPermission]
        public IActionResult DelWorkType(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditWorkType")]
        [ApiActionPermission]
        public IActionResult EditWorkType([FromBody] WorkTypeDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPost, Route("GetWorkTypeList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetWorkTypeList([FromBody] PageInput<WorkTypeDto> dto)
        {
            return Json(await _service.GetWorkTypeList(dto));
        }

        [HttpGet, Route("GetWorkTypeAllName")]
        [ApiActionPermission]
        public async Task<IActionResult> GetWorkTypeAllName()
        {
            return Json(await _service.GetWorkTypeAllName());
        }
    }
}
