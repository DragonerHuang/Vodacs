
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
    public partial class Biz_Quotation_SiteController
    {
        private readonly IBiz_Quotation_SiteService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_SiteController(
            IBiz_Quotation_SiteService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 查询现场考察列表（分页）
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        [HttpPost, Route("GetQnSitePage"), ActionPermission]
        public async Task<IActionResult> GetQnSitePageAsync([FromBody] PageInput<QnSiteSearchDto> pageInput)
        {
            return Json(await _service.GetQnSitePageAsync(pageInput));
        }

        /// <summary>
        /// 查询现场考察列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetQnSite"), ActionPermission]
        public async Task<IActionResult> GetQnSiteAsync([FromBody] QnSiteSearchDto input)
        {
            return Json(await _service.GetQnSiteAsync(input));
        }

        /// <summary>
        /// 上传完成文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost, Route("UpLoadSiteFinishFile"), ActionPermission]
        public async Task<IActionResult> UpLoadFinishFile(List<IFormFile> files)
        {
            return Json(await _service.UpLoadFinishFile(files));
        }

        /// <summary>
        /// 创建现场考察
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("AddQnSite"), ActionPermission]
        public async Task<IActionResult> AddQnSiteAsync([FromBody] AddQnSiteDto input)
        {
            return Json(await _service.AddQnSiteAsync(input));
        }

        /// <summary>
        /// 编辑现场考察
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("EditQnSite"), ActionPermission]
        public async Task<IActionResult> EditQnSiteAsync([FromBody] EditQnSiteDto input)
        {
            return Json(await _service.EditQnSiteAsync(input));
        }

        /// <summary>
        /// 删除现场考察
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteQnSite"), ActionPermission]
        public async Task<IActionResult> DeleteQnSiteAsync(Guid id)
        {
            return Json(await _service.DeleteQnSiteAsync(id));
        }

        /// <summary>
        /// 编辑回复日期
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Route("EditReplyDate"), ActionPermission]
        public async Task<IActionResult> EditReplyDateAsync([FromBody] QnSiteSearchDto input)
        {
            return Json(await _service.EditReplyDateAsync(input));
        }
    }
}
