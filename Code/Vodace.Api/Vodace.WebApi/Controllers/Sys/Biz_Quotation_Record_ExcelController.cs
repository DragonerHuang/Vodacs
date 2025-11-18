
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/QuotationRecordExcel")]
    [ApiVersion("1.0")]
    public partial class Biz_Quotation_Record_ExcelController : ApiBaseController<IBiz_Quotation_Record_ExcelService>
    {
        public Biz_Quotation_Record_ExcelController(IBiz_Quotation_Record_ExcelService service)
        : base(service)
        {
        }
    }
}

