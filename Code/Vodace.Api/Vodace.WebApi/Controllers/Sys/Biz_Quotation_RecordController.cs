
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/QuotationRecord")]
    [ApiVersion("1.0")]
    public partial class Biz_Quotation_RecordController : ApiBaseController<IBiz_Quotation_RecordService>
    {
        public Biz_Quotation_RecordController(IBiz_Quotation_RecordService service)
        : base(service)
        {
        }
    }
}

