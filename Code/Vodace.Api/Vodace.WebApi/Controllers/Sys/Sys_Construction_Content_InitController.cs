
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/ConstructionContentInit")]
    [ApiVersion("1.0")]
    public partial class Sys_Construction_Content_InitController : ApiBaseController<ISys_Construction_Content_InitService>
    {
        public Sys_Construction_Content_InitController(ISys_Construction_Content_InitService service)
        : base(service)
        {
        }
    }
}

