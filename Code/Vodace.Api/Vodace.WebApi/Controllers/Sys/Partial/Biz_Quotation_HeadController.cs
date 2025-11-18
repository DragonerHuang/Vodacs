
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
    public partial class Biz_Quotation_HeadController
    {
        private readonly IBiz_Quotation_HeadService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_HeadController(
            IBiz_Quotation_HeadService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 负责人列表中人员下拉列表
        /// </summary>
        /// <param name="qn_id">报价id</param>
        /// <returns></returns>
        [HttpGet, Route("GetContactDownList"), ActionPermission]
        public async Task<IActionResult> GetContactDownListAsync(Guid qn_id)
        {
            return Json(await _service.GetContactDownListAsync(qn_id));
        }

        /// <summary>
        /// 获取负责人列表
        /// </summary>
        /// <param name="qn_id">报价的id</param>
        /// <returns></returns>
        [HttpGet, Route("GetHeadList"), ActionPermission]
        public async Task<IActionResult> GetHeadListAsync(Guid qn_id)
        {
            return Json(await _service.GetHeadListAsync(qn_id));
        }

        /// <summary>
        /// 编辑负责人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("EditHead"), ActionPermission]
        public async Task<IActionResult> EditHeadAsync([FromBody]QnHeadInputDto input)
        {
            return Json(await _service.EditHeadAsync(input));
        }
    }
}
