
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/Contact")]
    [ApiVersion("1.0")]
    public partial class Sys_ContactController : ApiBaseController<ISys_ContactService>
    {
        public Sys_ContactController(ISys_ContactService service)
        : base(service)
        {
        }
    }
}

