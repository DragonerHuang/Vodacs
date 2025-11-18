
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/Biz_Contract_Tender_Addendum")]
    [PermissionTable(Name = "Biz_Contract_Tender_Addendum")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public partial class Biz_Contract_Tender_AddendumController : ApiBaseController<IBiz_Contract_Tender_AddendumService>
    {
        public Biz_Contract_Tender_AddendumController(IBiz_Contract_Tender_AddendumService service)
        : base(service)
        {
        }
    }
}

