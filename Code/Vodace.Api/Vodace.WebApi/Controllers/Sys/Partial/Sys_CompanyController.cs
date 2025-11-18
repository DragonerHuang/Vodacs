
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Enums;
using Vodace.Core.Filters;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_CompanyController
    {
        private readonly ISys_CompanyService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_CompanyController(
            ISys_CompanyService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Route("GetCompanyList"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetCompanyList()
        {
            return Json(await _service.GetCompanyList());
        }
        /// <summary>
        /// 注册公司
        /// </summary>
        /// <param name="company">
        /// company_name:公司名称-中文名<br/>
        /// company_name_eng：公司名称-英文名<br/>
        /// license_no：执照编号<br/>
        /// contact：联系人<br/>
        /// contact_phone：联系人电话号<br/>
        /// contact_email：联系人邮箱<br/>
        /// </param>
        /// <returns></returns>
        [HttpPost, Route("RegisterCompany"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Add([FromBody] CompanyDto company)
        {
            return Json( await _service.Add(company));
        }
        /// <summary>
        /// 编辑公司
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost, Route("UpdateCompany"), ApiActionPermission]
        public async Task<IActionResult> Update([FromBody] CompanyDto company)
        {
            return Json(await _service.Update(company));
        }
        /// <summary>
        /// 审核公司信息
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost, Route("AuditCompany"), ApiActionPermission]
        public async Task<IActionResult> Audit([FromBody] CompanyAuditDto company)
        {
            return Json(await _service.Audit(company));
        }
        /// <summary>
        /// 获取公司列表-分页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost,Route("GetListByPage"),ActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody]PageInput<CompanyQuery> query)
        {
            return Json(await _service.GetListByPage(query));
        }
        /// <summary>
        /// 删除公司
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelData")]
        [ApiActionPermission(ActionPermissionOptions.Delete)]
        public async Task<IActionResult> DelData(Guid id) 
        {
            return Json(await  _service.DelData(id));
        }
    }
}
