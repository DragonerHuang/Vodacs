
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
    public partial class Biz_ContractController
    {
        private readonly IBiz_ContractService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_ContractController(
            IBiz_ContractService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 新增合约
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
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
        [HttpPost, Route("AddContract")]
        [ApiActionPermission]
        public IActionResult AddContract([FromBody] ContractDto biz_ContractDto)
        {
            return Json(Service.Add(biz_ContractDto));
        }

        /// <summary>
        /// 删除合约
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>实际为软删除</remarks>
        [HttpDelete, Route("DelContract")]
        [ApiActionPermission]
        public IActionResult DelContract(Guid guid)
        {
            return Json(Service.Del(guid));
        }

        /// <summary>
        /// 修改合约
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
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
        [HttpPut, Route("EditContract")]
        [ApiActionPermission]
        public IActionResult EditContract([FromBody] ContractDto biz_ContractDto)
        {
            return Json(Service.Edit(biz_ContractDto));
        }

        /// <summary>
        /// 根据ID获取合约信息
        /// </summary>
        /// <param name="guid"></param>
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
        /// strMasterName：上级合约名称<br/>
        /// strMasterNo：上级合约编码<br/>
        /// project_no：项目编码<br/>
        /// create_id：创建人ID<br/>
        /// create_name：创建人名称<br/>
        /// modify_id：修改人ID<br/>
        /// modify_name：修改人名称<br/>
        /// modify_date：修改时间<br/>
        /// </remarks>
        [HttpGet, Route("GetContractById")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContractById(Guid guid)
        {
            return Json(await _service.GetContractById(guid));
        }

        /// <summary>
        /// 合约列表
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
        [HttpPost, Route("GetContractList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContractList([FromBody] PageInput<ContractSearchDto> searchDto)
        {
            return JsonNormal(await _service.GetContractList(searchDto));
        }

        /// <summary>
        /// 获取合约名称及ID列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 返回结果字段及说明：<br/>
        /// id: 合约ID（GUID）<br />
        /// name_eng: 合约英文名 <br />
        /// name_cht: 合约中文名 <br />
        /// contract_no: 合约编码<br />
        /// </remarks>
        [HttpGet, Route("GetContractAllName")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContractAllName()
        {
            return Json(await _service.GetContractAllName());
        }

        /// <summary>
        /// 设置原合约编码/新增vo/wo
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求参数说明：<br/>
        /// id：合同id (必填)<br/>
        /// master_id：需要更改合同的上级合约id（原合约编码id）<br/>
        /// vo_wo_type：新增的合约类型：VO、WO
        /// </remarks>
        [HttpPut, Route("SetContractMasterId")]
        [ApiActionPermission]
        public IActionResult SetContractMasterId([FromBody] SetContractMasterDto dtoSetContractMaster)
        {
            return Json(Service.SetContractMasterId(dtoSetContractMaster));
        }

        /// <summary>
        /// 子级合约（vo/wo）列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求参数说明：<br/>
        /// contract_id：合约id<br/>
        /// quotation_id：报价id<br/>
        /// qn_no：报价编码<br/>
        /// contract_name：合约名称<br/>
        /// contract_no：合约编码<br/>
        /// title：合约名称<br/>
        /// vo_wo_type：子级合约类型（vo、wo）(必填)<br/>
        /// 
        /// 返回结果参数说明：<br />
        /// contract_id：合约id<br/>
        /// quotation_id：报价id<br/>
        /// qn_no：报价编码<br/>
        /// contract_no：合约编码<br/>
        /// vo_wo_type：vo、wo类型<br/>
        /// name_cht：合约中文名<br/>
        /// name_eng：合约英文名<br/>
        /// confirm_amt：确认报价<br/>
        /// qn_amt：内部报价金额<br/>
        /// </remarks>
        [HttpPost, Route("GetChildContractList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetChildContractList([FromBody] PageInput<ChildContractSearchDto> searchDto)
        {
            return JsonNormal(await _service.GetChildContractList(searchDto));
        }

    }
}
