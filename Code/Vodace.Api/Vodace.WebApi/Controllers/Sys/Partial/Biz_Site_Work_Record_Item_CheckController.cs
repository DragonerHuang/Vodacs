
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
    public partial class Biz_Site_Work_Record_Item_CheckController
    {
        private readonly IBiz_Site_Work_Record_Item_CheckService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Site_Work_Record_Item_CheckController(
            IBiz_Site_Work_Record_Item_CheckService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 通用分发接口
        /// </summary>
        /// <param name="record_id">工地记录</param>
        /// <param name="type">0:BRF, 1:SCR, 2:CPD, 3:QDC, 4:CP, 5:SIC</param>
        /// <returns></returns>
        [HttpGet, Route("GetWorkerListPage")]
        [ApiActionPermission]
        public async Task<IActionResult> GetWorkerListPageAsync(Guid record_id, int type)
        {
            var res = await _service.GetCheckItemsAsync(record_id, type);
            return Json(res);
        }

        /// <summary>
        /// 单条保存设置值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("SetCheckItemBySingle")]
        [ApiActionPermission]
        public async Task<IActionResult> SetCheckItemBySingleAsync([FromBody] SetItemValueDataDto input)
        {
            var res = await _service.SetCheckItemBySingleAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 保存签名文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("SetItemSignBySingle")]
        [ApiActionPermission]
        public async Task<IActionResult> SetItemSignBySingleAsync([FromForm] SetItemSignDto input, IFormFile file)
        {
            var res = await _service.SetItemSignBySingleAsync(input, file);
            return Json(res);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("SetCheckItemValue")]
        [ApiActionPermission]
        public async Task<IActionResult> SetCheckItemValueAsync([FromForm] SetIemValueDto input, IFormFile formFile)
        {
            var res = await _service.SetCheckItemValueAsync(input);
            return Json(res);
        }

        /// <summary>
        /// 设置SIC清单（选择SIC人员和参考编号）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("SetSICData")]
        [ApiActionPermission]
        public async Task<IActionResult> SetSICDataAsync([FromBody] EditCheckSICdAT input)
        {
            var res = await _service.SetSICDataAsync(input);
            return Json(res);
        }
    }
}
