
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
using Vodace.Sys.Services;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_Site_Work_Check_ItemController
    {
        private readonly ISys_Site_Work_Check_ItemService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Site_Work_Check_ItemController(
            ISys_Site_Work_Check_ItemService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetPageList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetPageListAsync([FromBody] PageInput<SiteWorkCheckItemSearchDto> search)
        {
            var result = await _service.GetPageListAsync(search);
            return Json(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("AddCheckItem")]
        [ApiActionPermission]
        public async Task<IActionResult> AddAsync([FromBody] SiteWorkCheckItemAddDto input)
        {
            var result = await _service.AddAsync(input);
            return Json(result);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("EditCheckItem")]
        [ApiActionPermission]
        public async Task<IActionResult> EditAsync([FromBody] SiteWorkCheckItemEditDto input)
        {
            var result = await _service.EditAsync(input);
            return Json(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteCheckItem")]
        [ApiActionPermission]
        public async Task<IActionResult> DelAsync(Guid id)
        {
            var result = await _service.DelAsync(id);
            return Json(result);
        }

        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="enable">0 禁用；1 启用</param>
        /// <returns></returns>
        [HttpPost, Route("EnableCheckItem")]
        [ApiActionPermission]
        public async Task<IActionResult> EnableAsync(Guid id, byte enable)
        {
            var result = await _service.EnableAsync(id, enable);
            return Json(result);
        }
    }
}
