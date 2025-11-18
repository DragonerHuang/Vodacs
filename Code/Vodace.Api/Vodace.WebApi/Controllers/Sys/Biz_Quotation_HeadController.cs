
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/QuotationHead")]
    [ApiVersion("1.0")]
    public partial class Biz_Quotation_HeadController : ApiBaseController<IBiz_Quotation_HeadService>
    {
        public Biz_Quotation_HeadController(IBiz_Quotation_HeadService service)
        : base(service)
        {
        }
    }
}

