
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Enums;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_ContactController
    {
        private readonly ISys_ContactService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_ContactController(
            ISys_ContactService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 获取联系人列表-分页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost, Route("GetListByPage")]
        [ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody] PageInput<ContactQuery> query) 
        {
            return Json(await _service.GetListByPage(query));
        }

        /// <summary>
        /// 新增联系人
        /// </summary>
        /// <param name="sys_Contact"></param>
        /// <returns></returns>
        [HttpPost, Route("AddContact")]
        [ApiActionPermission]
        public IActionResult AddContact([FromBody] ContactDetailDto dtoContactDetail)
        {
            return Json(Service.Add(dtoContactDetail));
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>实际为软删除</remarks>
        [HttpDelete, Route("DelContact")]
        [ApiActionPermission]
        public IActionResult DelContact(Guid guid)
        {
            return Json( Service.Del(guid));
        }

        /// <summary>
        /// 修改联系人
        /// </summary>
        /// <param name="sys_Contact"></param>
        /// <returns></returns>
        [HttpPut, Route("EditContact")]
        [ApiActionPermission]
        public IActionResult EditContact([FromBody] ContactEditlDto dtoContactDetail)
        {
            return Json( Service.Edit(dtoContactDetail));
        }

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>
        /// strCompanyName：公司名称 <br/>
        /// name_eng：联络人名称（英文） <br/>
        /// name_cht：联络人名称（中文） <br/>
        /// strOrganizationName：职位/组织 名称 <br/>
        /// strDepartmentName：部门名称 <br/>
        /// tel：电话 <br/>
        /// fax：传真 <br/>
        /// email：邮箱 <br/>
        /// title：角色 （头衔、职称）<br/>
        /// strDistrictNameEng：行政区域名称（英文） <br/>
        /// strDistrictNameCht：行政区域名称（中文） <br/>
        /// </remarks>
        [HttpPost, Route("GetContactList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactList([FromBody] PageInput<SearchContactDto> dtoContactSearchInput)
        {
            return Json(await _service.GetContactList(dtoContactSearchInput));
        }
        /// <summary>
        /// 获取联系人名称及ID列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 返回结果字段及说明：<br/>
        /// id: 项目ID（GUID）<br />
        /// name_eng: 项目英文名 <br />
        /// name_cht: 项目中文名 <br />
        /// </remarks>
        [HttpGet, Route("GetContactAllName")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactAllName()
        {
            return Json(await _service.GetContactAllName());
        }

        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetContactById")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactById(Guid guid)
        {
            return Json(await _service.GetContactById(guid));
        }

        /// <summary>
        /// 获取联系人下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetContactWithCardData")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactWithCardDataAsync([FromBody] PageInput<SearchContactWithCardDto> search)
        {
            return Json(await _service.GetContactWithCardDataAsync(search));
        }
    }
}
