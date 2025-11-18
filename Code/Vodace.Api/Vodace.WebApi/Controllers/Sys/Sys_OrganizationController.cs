
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/Organization")]
    [ApiVersion("1.0")]
    public partial class Sys_OrganizationController : ApiBaseController<ISys_OrganizationService>
    {
        public Sys_OrganizationController(ISys_OrganizationService service)
        : base(service)
        {
        }
    }
}

