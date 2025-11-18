
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Confirmed_OrderController
    {
        private readonly IBiz_Confirmed_OrderService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Confirmed_OrderController(
            IBiz_Confirmed_OrderService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// CO分页查询
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        [HttpPost, Route("GetCoPageList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetCoPageList([FromBody] PageInput<CoSearchDto> dtoSearchInput)
        {
            var result = await _service.GetCoPageList(dtoSearchInput);

            return Json(result);
        }

        /// <summary>
        /// 确认报价
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        [HttpPost, Route("ConfirmOrder")]
        [ApiActionPermission]
        public async Task<IActionResult> ConfirmOrder([FromBody] ConfirmDto dtoInput)
        {
            var result = await _service.ConfirmOrder(dtoInput);

            return Json(result);
        }

        /// <summary>
        /// 取消确认报价
        /// </summary>
        /// <param name="qnId">报价id</param>
        /// <returns></returns>
        [HttpPost, Route("UnConfirmOrder")]
        [ApiActionPermission]
        public async Task<IActionResult> UnConfirmOrder(Guid qnId)
        {
            var result = await _service.UnConfirmOrder(qnId);

            return Json(result);
        }

        /// <summary>
        /// 确认订单文件上传
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        [HttpPost, Route("UploadCOFile")]
        [ApiActionPermission]
        public async Task<IActionResult> UploadCOFile(List<IFormFile> lstFiles)
        {
            var result = await _service.UploadCoFile(lstFiles);

            return Json(result);
        }

        /// <summary>
        /// 根据id获取co详细信息
        /// </summary>
        /// <param name="coId">co的id</param>
        /// <returns></returns>
        [HttpGet, Route("GetCOByIdAsync")]
        [ApiActionPermission]
        public async Task<IActionResult> GetCOByIdAsync(Guid coId)
        {
            var result = await _service.GetCoByIdAsync(coId);

            return Json(result);
        }

        /// <summary>
        /// 修改co信息
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        [HttpPut, Route("EditCoAsync")]
        [ApiActionPermission]
        public async Task<IActionResult> EditCoAsync([FromBody] CoInputDto dtoInput)
        {
            var result = await _service.EditCoAsync(dtoInput);

            return Json(result);
        }

        /// <summary>
        /// 统计确认订单的确认金额
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("CountCOAmt")]
        [ApiActionPermission]
        public async Task<IActionResult> CountCOAmt()
        {
            var result = await _service.CountCOAmt();

            return Json(result);
        }

        /// <summary>
        /// 获取确认订单下的状态
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetStatus")]
        [ApiActionPermission]
        public async Task<IActionResult> GetStatus()
        {
            var result = await _service.GetStatus();

            return Json(result);
        }
    }
}
