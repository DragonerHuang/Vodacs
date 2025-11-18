
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
    public partial class Biz_Sub_ContractorsController
    {
        private readonly IBiz_Sub_ContractorsService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Sub_ContractorsController(
            IBiz_Sub_ContractorsService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取合约客户下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetDropList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetDropList()
        {
            var result = await _service.GetContractorsDownList();
            return Json(result);
        }
    }
}
