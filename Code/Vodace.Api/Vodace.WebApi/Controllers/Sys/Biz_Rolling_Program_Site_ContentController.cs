
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/RollingProgramSiteContent")]
    [ApiVersion("1.0")]
    public partial class Biz_Rolling_Program_Site_ContentController : ApiBaseController<IBiz_Rolling_Program_Site_ContentService>
    {
        public Biz_Rolling_Program_Site_ContentController(IBiz_Rolling_Program_Site_ContentService service)
        : base(service)
        {
        }
    }
}

