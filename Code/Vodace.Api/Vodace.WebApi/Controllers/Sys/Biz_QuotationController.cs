
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    
    [Route("api/v{version:apiVersion}/Quotation")]
    [PermissionTable(Name = "Biz_Quotation")]
    [ApiVersion("1.0")]
    public partial class Biz_QuotationController : ApiBaseController<IBiz_QuotationService>
    {
        public Biz_QuotationController(IBiz_QuotationService service)
        : base(service)
        {
        }
    }
}

