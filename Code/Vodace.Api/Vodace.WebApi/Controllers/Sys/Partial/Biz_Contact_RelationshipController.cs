
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
    public partial class Biz_Contact_RelationshipController
    {
        private readonly IBiz_Contact_RelationshipService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Contact_RelationshipController(
            IBiz_Contact_RelationshipService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 新增联络人关系
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求参数说明：<br/>
        /// contact_id：联系人ID<br/>
        /// company_id：公司ID<br/>
        /// role_id：角色id<br/>
        /// relation_type：联络类型：0：报价联络人、1：公司联系人、2：组织架构，3：合约组织架构人员<br/>
        /// relation_id：对应的表ID（报价ID、公司联系人ID、组织ID....）<br/>
        /// </remarks>
        [HttpPost, Route("AddContactRelationship")]
        [ApiActionPermission]
        public IActionResult AddContactRelationship([FromBody] ContactRelationshipAddDto dto)
        {
            return Json(_service.Add(dto));
        }

        /// <summary>
        /// 删除联络人关系
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>实际为软删除</remarks>
        [HttpDelete, Route("DelContactRelationship")]
        [ApiActionPermission]
        public IActionResult DelContactRelationship(Guid guid)
        {
            return Json(Service.Del(guid));
        }

        /// <summary>
        /// 联络人关系列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求参数说明：<br/>
        /// company_id：公司ID<br/>
        /// contact_id：联系人ID<br/>
        /// role_id：角色id<br/>
        /// relation_type：联络类型：0：报价联络人、1：公司联系人、2：组织架构，3：合约组织架构人员<br/>
        /// mail_to：发送邮件类型，Gen，Pay ，Safe ，PD， ABWF ，EL， FS（多选，用,号隔开）<br/>
        /// relation_id：对应的表ID（报价ID、公司联系人ID、组织ID....）<br/><br/>
        /// 
        /// 返回参数说明：<br/>
        /// strCompanyName：公司名称 <br/>
        /// name_eng：联络人名称（英文） <br/>
        /// name_cht：联络人名称（中文） <br/>
        /// strOrganizationName：职位/组织 名称 <br/>
        /// strDepartmentName：部门名称 <br/>
        /// tel：电话 <br/>
        /// fax：传真 <br/>
        /// email：邮箱 <br/>
        /// role_name：角色名称 <br/>
        /// relation_type：联络类型：0：报价联络人、1：公司联系人、2：组织架构，3：合约组织架构人员<br/>
        /// title：头衔、职称<br/>
        /// strDistrictNameEng：行政区域名称（英文） <br/>
        /// strDistrictNameCht：行政区域名称（中文） <br/>
        /// </remarks>
        [HttpPost, Route("GetContactRelationshipList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactRelationshipList([FromBody] PageInput<ContactRelationshipDto> searchDto)
        {
            return JsonNormal(await _service.GetContactRelationshipList(searchDto));
        }

        /// <summary>
        /// 同时获取部门、组织、职位、联系人列表
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
        /// 返回参数说明：<br/>
        /// department：部门列表<br/>
        /// organization：组织列表<br/>
        /// role：角色列表<br/>
        /// contact：联系人列表（name_eng：英文名，name_cht：中文名）<br/>
        /// </remarks>
        [HttpPost, Route("GetContactRelationshipOtherList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactRelationshipOtherList([FromBody] ContactInfoDto dto)
        {
            return Json(await _service.GetContactRelationshipOtherList(dto));
        }

        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求参数说明：<br/>
        /// id：联络人Id<br/>
        /// relation_type：联络类型：0：报价联络人、1：公司联系人、2：组织架构，3：合约组织架构人员<br/>
        /// relation_id：对应的表ID（报价ID、公司联系人ID、组织ID....）<br/><br/>
        /// 
        /// 返回结果参数说明：<br/>
        /// id：<br/>
        /// company_id：Sys_Company、所属公司\一般个人<br/>
        /// organization_id：组织<br/>
        /// identification_no：身份证<br/>
        /// name_sho：简称<br/>
        /// name_eng：英文名<br/>
        /// name_cht：中文名<br/>
        /// name_ali：别名<br/>
        /// department_id：所属部门Id<br/>
        /// title：头衔、职称<br/>
        /// address：地址<br/>
        /// district_id：‌行政区id<br/>
        /// email：邮箱<br/>
        /// daily_salary：工人日薪<br/>
        /// delete_status：是否删除（0：正常；1：删除；2：数据库手删除）<br/>
        /// remark：备注<br/>
        /// create_id：<br/>
        /// create_name：<br/>
        /// create_date：<br/>
        /// modify_id：<br/>
        /// modify_name：<br/>
        /// modify_date：<br/>
        /// tel：电话<br/>
        /// fax：传真<br/>
        /// </remarks>
        [HttpPost, Route("GetContactInfoByContactId")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContactInfoByContactId([FromBody] ContactInfoDto dto)
        {
            return Json(await _service.GetContactInfoByContactId(dto));
        }


        [HttpPost, Route("GetContactRelationUserList")]
        [ApiActionPermission]
        public IActionResult GetContactRelationUserList([FromBody] ContactRelationSearchAllDto dto)
        {
            return Json(_service.GetContactRelationUserList(dto));
        }
    }
}
