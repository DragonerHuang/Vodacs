
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Quotation_RecordService
    {
        WebResponseContent Add(QuotationRecordAddDto dtoQuotationRecord, IFormFile file = null);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(QuotationRecordEditDto dtoQuotationRecord, IFormFile file = null);
        Task<WebResponseContent> GetQuotationRecordList(PageInput<SearchQuotationRecordDto> dtoQnSearchInput);
        Task<WebResponseContent> GetQuotationRecordById(Guid guid);
    }
 }
