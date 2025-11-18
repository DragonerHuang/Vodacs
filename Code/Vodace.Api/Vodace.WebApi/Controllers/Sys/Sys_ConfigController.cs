
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/Sys_Config")]
    [ApiVersion("1.0")]
    public partial class Sys_ConfigController : ApiBaseController<ISys_ConfigService>
    {
        public Sys_ConfigController(ISys_ConfigService service)
        : base(service)
        {
        }
    }
}

