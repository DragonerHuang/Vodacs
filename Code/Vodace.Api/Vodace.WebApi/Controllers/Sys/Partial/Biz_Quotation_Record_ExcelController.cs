
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
    public partial class Biz_Quotation_Record_ExcelController
    {
        private readonly IBiz_Quotation_Record_ExcelService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_Record_ExcelController(
            IBiz_Quotation_Record_ExcelService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 新增报价单详情信息
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
        /// quotation_record_id：报价单ID（Biz_Quotation_Record.id）<br/>
        /// sheet_name：Excel Sheet名称<br/>
        /// item_code：项目名称<br/>
        /// item_description：内容<br/>
        /// quantity：数量<br/>
        /// unit：单位<br/>
        /// unit_rage：单价<br/>
        /// amount：银码<br/>
        /// remark：备注<br/>
        /// line_number：行号（系统自动生成）
        /// </remarks>
        [HttpPost, Route("AddQuotationRecordExcel")]
        [ApiActionPermission]
        public IActionResult AddQuotationRecordExcel([FromBody] QuotationRecordExcelDto dtoQuotationRecordExcel)
        {
            return Json(Service.Add(dtoQuotationRecordExcel));
        }

        /// <summary>
        /// 删除报价单详情信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>实际为软删除</remarks>
        [HttpDelete, Route("DelQuotationRecordExcel")]
        [ApiActionPermission]
        public IActionResult DelQuotationRecordExcel(Guid guid)
        {
            return Json(Service.Del(guid));
        }

        /// <summary>
        /// 修改报价单详情信息
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
        /// quotation_record_id：报价单ID（Biz_Quotation_Record.id）<br/>
        /// sheet_name：Excel Sheet名称<br/>
        /// item_code：项目名称<br/>
        /// item_description：内容<br/>
        /// quantity：数量<br/>
        /// unit：单位<br/>
        /// unit_rage：单价<br/>
        /// amount：银码<br/>
        /// remark：备注<br/>
        /// line_number：行号（系统自动生成）<br/>
        /// id：报价单详情Excel ID
        /// </remarks>
        [HttpPut, Route("EditQuotationRecordExcel")]
        [ApiActionPermission]
        public IActionResult EditQuotationRecordExcel([FromBody] QuotationRecordExcelEditDto dtoQuotationRecordExcel)
        {
            return Json(Service.Edit(dtoQuotationRecordExcel));
        }

        /// <summary>
        /// 报价单详情信息
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求参数说明：<br/>
        /// sheet_name：Excel Sheet名称<br/>
        /// item_code：项目名称<br/>
        /// item_description：内容<br/>
        /// qn_no：报价编码<br/>
        /// <br/>
        /// 返回结果参数说明：<br/>
        /// quotation_record_id：报价单ID（Biz_Quotation_Record.id）<br/>
        /// sheet_name：Excel Sheet名称<br/>
        /// item_code：项目名称<br/>
        /// item_description：内容<br/>
        /// quantity：数量<br/>
        /// unit：单位<br/>
        /// unit_rage：单价<br/>
        /// amount：银码<br/>
        /// remark：备注<br/>
        /// line_number：行号（系统自动生成）<br/>
        /// id：报价单详情Excel ID<br/>
        /// version：报价单版本号<br/>
        /// amount：报价金额(HK$)<br/>
        /// file_name：文件名<br/>
        /// qn_no：报价编码<br/>
        /// </remarks>
        [HttpPost, Route("GetQuotationRecordExcelList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetQuotationRecordExcelList([FromBody] PageInput<SearchQuotationRecordExcelInputDto> searchDto)
        {
            return JsonNormal(await _service.GetQuotationRecordExcelList(searchDto));
        }
    }
}
