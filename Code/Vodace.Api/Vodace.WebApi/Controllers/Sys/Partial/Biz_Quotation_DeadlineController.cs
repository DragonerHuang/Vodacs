
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
    public partial class Biz_Quotation_DeadlineController
    {
        private readonly IBiz_Quotation_DeadlineService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_DeadlineController(
            IBiz_Quotation_DeadlineService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dtoQuery"></param>
        /// <returns></returns>
        [HttpPost, Route("GetDeadLineListPage"), ActionPermission]
        public async Task<IActionResult> GetDeadLineListPageAsync([FromBody] PageInput<QnDeadlineSearchDto> dtoQuery)
        {
            return Json(await _service.GetDeadLineListPageAsync(dtoQuery));
        }

        /// <summary>
        /// 查询列表（不分页）
        /// </summary>
        /// <param name="dtoQuery"></param>
        /// <returns></returns>
        [HttpPost, Route("GetDeadLineList"), ActionPermission]
        public async Task<IActionResult> GetDeadLineListAsync([FromBody] QnDeadlineSearchDto dtoQuery)
        {
            return Json(await _service.GetDeadLineListAsync(dtoQuery));
        }

        /// <summary>
        /// 编辑期限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("EditDealLine"), ActionPermission]
        public async Task<IActionResult> EditDealLineAsync([FromBody] EditQnDeadlineDto input)
        {
            return Json(await _service.EditDealLineAsync(input));
        }

    }
}
