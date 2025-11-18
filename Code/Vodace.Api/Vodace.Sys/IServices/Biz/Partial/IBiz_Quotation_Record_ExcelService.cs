
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using System;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Quotation_Record_ExcelService
    {
        WebResponseContent Add(QuotationRecordExcelDto dtoQuotationRecordExcel);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(QuotationRecordExcelEditDto dtoQuotationRecordExcel);
        Task<WebResponseContent> GetQuotationRecordExcelList(PageInput<SearchQuotationRecordExcelInputDto> dtoSearchInput);
    }
 }
