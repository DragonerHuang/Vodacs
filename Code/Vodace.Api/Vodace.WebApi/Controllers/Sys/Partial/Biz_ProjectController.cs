
using Microsoft.AspNetCore.Authorization;
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
    public partial class Biz_ProjectController
    {
        private readonly IBiz_ProjectService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_ProjectController(
            IBiz_ProjectService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求参数说明：<br/>
        /// id: 项目ID（GUID）(可为null)<br />
        /// company_id: 所属公司id(GUID)(可为null)<br />
        /// master_id: 所属上级项目ID(GUID)(可为null)<br />
        /// original_id: 上级新增项目ID（vo/wo）(GUID)(可为null)<br />
        /// pro_type_id: 项目状态代码(int)<br />
        /// customer_id: 客户id(GUID)(可为null)<br />
        /// project_no: 项目编号(必填项)<br />
        /// name_sho: 项目简称<br />
        /// name_eng: 项目英文名(必填项)<br />
        /// name_cht: 项目中文名(必填项)<br />
        /// name_ali: 项目别名<br />
        /// exp_start_date: 期望开始时间(DateTime)(可为null)<br />
        /// act_start_date: 实际开始时间(DateTime)(可为null)<br />
        /// exp_end_date: 期望结束时间(DateTime)(可为null)<br />
        /// act_end_date: 实际结束时间(DateTime)(可为null)<br />
        /// delete_status: 是否删除（0：正常；1：删除；2：数据库删除)<br/>
        /// create_date: 创建时间(DateTime)(可为null)<br />
        /// remark: 备注<br/>
        /// </remarks>
        [HttpPost, Route("AddProject")]
        [ApiActionPermission]
        public IActionResult AddProject([FromBody] ProjectDto projectDto)
        {
            return Json(Service.Add(projectDto));
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>实际为软删除</remarks>
        [HttpDelete, Route("DelProject")]
        [ApiActionPermission]
        public IActionResult DelProject(Guid guid)
        {
            return Json(Service.Del(guid));
        }

        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求参数说明：<br/>
        /// id: 项目ID（GUID）(可为null)<br />
        /// company_id: 所属公司id(GUID)(可为null)<br />
        /// master_id: 所属上级项目ID(GUID)(可为null)<br />
        /// original_id: 上级新增项目ID（vo/wo）(GUID)(可为null)<br />
        /// pro_type_id: 项目状态代码(int)<br />
        /// customer_id: 客户id(GUID)(可为null)<br />
        /// project_no: 项目编号(必填项)<br />
        /// name_sho: 项目简称<br />
        /// name_eng: 项目英文名(必填项)<br />
        /// name_cht: 项目中文名(必填项)<br />
        /// name_ali: 项目别名<br />
        /// exp_start_date: 期望开始时间(DateTime)(可为null)<br />
        /// act_start_date: 实际开始时间(DateTime)(可为null)<br />
        /// exp_end_date: 期望结束时间(DateTime)(可为null)<br />
        /// act_end_date: 实际结束时间(DateTime)(可为null)<br />
        /// delete_status: 是否删除（0：正常；1：删除；2：数据库删除)<br/>
        /// create_date: 创建时间(DateTime)(可为null)<br />
        /// remark: 备注<br/>
        /// </remarks>
        [HttpPut, Route("EditProject")]
        [ApiActionPermission]
        public IActionResult EditProject([FromBody] ProjectDto projectDto)
        {
            return Json(Service.Edit(projectDto));
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        /// <remarks>
        /// 返回结果字段及说明：<br/>
        /// id：项目主键<br/>
        /// project_no：项目编号<br/>
        /// name_eng：项目英文名<br/>
        /// name_cht：项目中文名<br/>
        /// exp_start_date：项目期望开始时间<br/>
        /// exp_end_date：项目期望结束时间<br/>
        /// strSiteName：统合施工地点集，多个之间以逗号之间区别<br/>
        /// strContractName：项目合约名集，多个之间以逗号之间区别<br/>
        /// intContractProgress：合约进度<br/>
        /// children.id：项目关联的合约id<br/>
        /// children.contract_no：项目关联的合约编号<br/>
        /// children.name_eng：合约英文名<br/>
        /// children.name_cht：合约中文名<br/>
        /// children.issue_date：合约发行日期<br/>
        /// children.end_date：合约截止日期<br/>
        /// children.sites.name_eng：合约施工地点英文名<br/>
        /// children.sites.name_cht：合约施工地点中文名
        /// </remarks>
        [HttpPost, Route("GetProjectList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetProjectList([FromBody] PageInput<SearchProjectDto> dtoQnSearchInput)
        {
            return Json(await _service.GetProjectList(dtoQnSearchInput));
        }


        /// <summary>
        /// 获取项目合约列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// 返回结果参数说明：<br />
        /// strSiteName：合约施工地点，多个之间以逗号之后区分<br/>
        /// strQuotationNo：报价编码<br/>
        /// strProjectName：项目名称<br/>
        /// strCompanyName：公司名称<br/>
        /// exp_start_date：期望开始时间<br/>
        /// act_start_date：实际开始时间<br/>
        /// exp_end_date：期望结束时间<br/>
        /// act_end_date：实际结束时间<br/>
        /// id：报价编码<br/>
        /// po_id：采购单ID<br/>
        /// project_id：项目ID<br/>
        /// contract_no：合约编码<br/>
        /// name_sho：合约简称<br/>
        /// name_eng：合约英文名<br/>
        /// name_cht：合约中文名<br/>
        /// name_ali：合约别名<br/>
        /// delete_status：是否删除（0：正常；1：删除；2：数据库手删除）<br/>
        /// remark：备注<br/>
        /// create_date：创建时间<br/>
        /// title：合同标题<br/>
        /// category：合同类别<br/>
        /// tender_type：投标类型<br/>
        /// ref_no：合同编号/投标参考编号<br/>
        /// vo_wo_type：vo和wo类型<br/>
        /// master_id：上一层级合约id<br/>
        /// company_id：所属公司id<br/>
        /// </remarks>
        [HttpPost, Route("GetProjectContractList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetProjectContractList([FromBody] PageInput<ContractSearchDto> dtoQnSearchInput)
        {
            return Json(await _service.GetProjectContractList(dtoQnSearchInput));
        }

        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectById")]
        [ApiActionPermission]
        public async Task<IActionResult> GetProjectById(Guid guid)
        {
            return Json(await _service.GetProjectById(guid));
        }

        /// <summary>
        /// 获取项目名称及ID列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 返回结果字段及说明：<br/>
        /// id: 项目ID（GUID）<br />
        /// name_eng: 项目英文名 <br />
        /// name_cht: 项目中文名 <br />
        /// project_no: 项目编码<br />
        /// </remarks>
        [HttpGet, Route("GetProjectAllName")]
        [ApiActionPermission]
        public async Task<IActionResult> GetProjectAllName()
        {
            return Json(await _service.GetProjectAllName());
        }

        /// <summary>
        /// 首页项目统计
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>
        /// 返回结果字段及说明：<br/>
        /// intTotalProjects：项目总数<br/>
        /// intOngoingProjects：进行中项目数<br/>
        /// intCompletedProjects：已完成项目数<br/>
        /// doubleGeneralIncome：总收入<br/>
        /// </remarks>
        [HttpGet, Route("GetProjectStatistics")]
        [ApiActionPermission]
        public async Task<IActionResult> GetProjectStatistics()
        {
            return Json(await _service.GetProjectStatistics());
        }

        /// <summary>
        /// 获取项目下拉列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        [HttpGet, Route("GetProjectDownListAsync")]
        [ApiActionPermission]
        public async Task<IActionResult> GetProjectDownListAsync(string project_name)
        {
            return Json(await _service.GetProjectDownListAsync(project_name));
        }
    }
}
