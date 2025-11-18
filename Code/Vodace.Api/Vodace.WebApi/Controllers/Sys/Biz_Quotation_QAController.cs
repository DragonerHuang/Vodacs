
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/QuotationQA")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Biz_Quotation_QAController : ApiBaseController<IBiz_Quotation_QAService>
    {
        public Biz_Quotation_QAController(IBiz_Quotation_QAService service)
        : base(service)
        {
        }
    }
}

