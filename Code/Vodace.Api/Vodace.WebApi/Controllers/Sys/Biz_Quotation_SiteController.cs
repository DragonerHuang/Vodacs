
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/QuotationSite")]
    [PermissionTable(Name = "Biz_Quotation_Site")]
    [ApiVersion("1.0")]
    public partial class Biz_Quotation_SiteController : ApiBaseController<IBiz_Quotation_SiteService>
    {
        public Biz_Quotation_SiteController(IBiz_Quotation_SiteService service)
        : base(service)
        {
        }
    }
}

