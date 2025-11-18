
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;
using Microsoft.AspNetCore.Authorization;
using Vodace.Core.Filters;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Upcoming_EventsController
    {
        private readonly IBiz_Upcoming_EventsService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Upcoming_EventsController(
            IBiz_Upcoming_EventsService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("GetEventList"),ApiActionPermission]
        public IActionResult GetEventList() 
        {
            return Json(_service.GetEventList());
        }
    }
}
