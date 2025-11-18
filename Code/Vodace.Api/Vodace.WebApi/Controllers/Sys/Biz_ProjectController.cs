
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/Project")]
    [ApiVersion("1.0")]
    public partial class Biz_ProjectController : ApiBaseController<IBiz_ProjectService>
    {
        public Biz_ProjectController(IBiz_ProjectService service)
        : base(service)
        {
        }
    }
}

