
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Enums;
using Vodace.Core.Filters;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_SiteController
    {
        private readonly IBiz_SiteService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_SiteController(
            IBiz_SiteService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 新增 Site
        /// </summary>
        /// <param name="mSiteDto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddSite"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult AddSite([FromBody] SiteDto dtoSite)
        {
            return Json(_service.AddSite(dtoSite));
        }

        /// <summary>
        /// 修改 Site
        /// </summary>
        /// <param name="mSiteDto"></param>
        /// <returns></returns>
        [HttpPost, Route("EditSite"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult EditSite([FromBody] SiteDto dtoSite)
        {
            return Json(_service.EditSite(dtoSite));
        }

        /// <summary>
        /// 删除 Site
        /// </summary>
        /// <param name="mSiteDto"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelSite"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult DelSite(Guid guid)
        {
            return Json(_service.DelSite(guid));
        }

        /// <summary>
        /// 获取Site信息列表
        /// </summary>
        /// <param name="mSiteDto"></param>
        /// <returns></returns>
        [HttpPost, Route("GetSiteList"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSiteList([FromBody] PageDataOptions pdo)
        {
            return Json(await _service.GetSiteList(pdo));
        }

        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet, Route("GetSiteById"), ApiActionPermission(ActionPermissionOptions.Search)]
        [ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSiteById(Guid guid)
        {
            return Json(await _service.GetSiteById(guid));
        }

        /// <summary>
        /// 现场地点下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetSiteDownList")]
        [ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSiteDownList()
        {
            return Json(await _service.GetSiteDownList());
        }
    }
}
