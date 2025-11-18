
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    //[Route("api/Biz_Site")]
    //[PermissionTable(Name = "Biz_Site")]

    [Route("api/v{version:apiVersion}/Site")]
    [ApiVersion("1.0")]
    public partial class Biz_SiteController : ApiBaseController<IBiz_SiteService>
    {
        public Biz_SiteController(IBiz_SiteService service)
        : base(service)
        {
        }
    }
}

