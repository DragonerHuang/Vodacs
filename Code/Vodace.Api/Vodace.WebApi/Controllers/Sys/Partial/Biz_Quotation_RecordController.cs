
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
    public partial class Biz_Quotation_RecordController
    {
        private readonly IBiz_Quotation_RecordService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_RecordController(
            IBiz_Quotation_RecordService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 新增报价单记录
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求接口参数说明：<br/>
        /// version：版本号(必填项)<br/>
        /// amount：报价金额(HK$)<br/>
        /// file_name：文档名<br/>
        /// qn_id: 报价ID（guid）(必填项)<br/>
        /// </remarks>
        [HttpPost, Route("AddQuotationRecord")]
        [ApiActionPermission]
        public IActionResult AddQuotationRecord([FromForm] QuotationRecordAddDto dtoQuotationRecord, IFormFile file = null)
        {
            return Json(Service.Add(dtoQuotationRecord, file));
        }

        /// <summary>
        /// 删除报价单记录
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>实际为软删除</remarks>
        [HttpDelete, Route("DelQuotationRecord")]
        [ApiActionPermission]
        public IActionResult DelQuotationRecord(Guid guid)
        {
            return Json(Service.Del(guid));
        }

        /// <summary>
        /// 修改报价单记录
        /// </summary>
        /// <param name="dtoQuotationRecord"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求接口参数说明：<br/>
        /// id：报价单记录ID(必填项)<br/>
        /// version：版本号(必填项)<br/>
        /// amount：报价金额(HK$)<br/>
        /// file_name：文档名<br/>
        /// qn_id: 报价ID（guid）(必填项)<br/>
        /// delete_status： 是否删除（0：正常；1：删除；2：数据库手删除）后端接口处理<br/>
        /// create_date： 创建时间 后端接口处理<br/>
        /// </remarks>
        [HttpPut, Route("EditQuotationRecord")]
        [ApiActionPermission]
        public IActionResult EditQuotationRecord([FromForm] QuotationRecordEditDto dtoQuotationRecord, IFormFile file = null)
        {
            return Json(Service.Edit(dtoQuotationRecord, file));
        }

        /// <summary>
        /// 获取报价单记录
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求接口参数说明：<br/>
        /// id：报价单记录ID(必填项)<br/>
        /// version：版本号(必填项)<br/>
        /// amount：报价金额(HK$)<br/>
        /// file_name：文档名<br/>
        /// qn_id: 报价ID（guid）(必填项)<br/>
        /// delete_status： 是否删除（0：正常；1：删除；2：数据库手删除）后端接口处理<br/>
        /// create_date： 创建时间 后端接口处理<br/>
        /// qn_no：报价编码<br/>
        /// status_code：合约状态<br/>
        /// </remarks>
        [HttpPost, Route("GetQuotationRecordList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetQuotationRecordList([FromBody] PageInput<SearchQuotationRecordDto> dtoSearchInput)
        {
            return Json(await _service.GetQuotationRecordList(dtoSearchInput));
        }

        /// <summary>
        /// 根据ID获取报价单详情
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        /// <remarks>
        /// 请求接口参数说明：<br/>
        /// id：报价单记录ID(必填项)<br/>
        /// version：版本号(必填项)<br/>
        /// amount：报价金额(HK$)<br/>
        /// file_name：文档名<br/>
        /// qn_id: 报价ID（guid）(必填项)<br/>
        /// delete_status： 是否删除（0：正常；1：删除；2：数据库手删除）后端接口处理<br/>
        /// create_date： 创建时间 后端接口处理<br/>
        /// qn_no：报价编码<br/>
        /// status_code：合约状态<br/>
        /// </remarks>
        [HttpGet, Route("GetQuotationRecordById")]
        [ApiActionPermission]
        public async Task<IActionResult> GetQuotationRecordById(Guid guid)
        {
            return Json(await _service.GetQuotationRecordById(guid));
        }
    }
}
