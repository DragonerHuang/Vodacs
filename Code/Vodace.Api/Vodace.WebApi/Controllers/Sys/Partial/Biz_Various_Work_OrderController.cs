
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Various_Work_OrderController
    {
        private readonly IBiz_Various_Work_OrderService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Various_Work_OrderController(
            IBiz_Various_Work_OrderService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 新增VO/WO
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        [HttpPost, Route("AddDetail")]
        [ApiActionPermission]
        public IActionResult AddDetail([FromBody] Various_Work_OrderDto dtoVarious_Work_Order)
        {
            return Json(Service.AddDetail(dtoVarious_Work_Order));
        }

        /// <summary>
        /// 新增VO/WO
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        [HttpPost, Route("AddVariousWork")]
        [ApiActionPermission]
        public IActionResult AddVariousWork([FromBody] Biz_Various_Work_Order biz_Various_Work_Order)
        {
            return Json(Service.Add(biz_Various_Work_Order));
        }

        /// <summary>
        /// 删除VO/WO
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>实际为软删除</remarks>
        [HttpDelete, Route("DelVariousWork")]
        [ApiActionPermission]
        public IActionResult DelVariousWork(Guid guid)
        {
            return Json(Service.Del(guid));
        }

        /// <summary>
        /// 修改VO/WO
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        [HttpPut, Route("EditVariousWork")]
        [ApiActionPermission]
        public IActionResult EditVariousWork([FromBody] Biz_Various_Work_Order biz_Various_Work_Order)
        {
            return Json(Service.Edit(biz_Various_Work_Order));
        }
    }
}
