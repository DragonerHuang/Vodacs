
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [PermissionTable(Name = "Sys_Worker_Register")]
    [Route("api/v{version:apiVersion}/WorkerRegister")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Sys_Worker_RegisterController : ApiBaseController<ISys_Worker_RegisterService>
    {
        public Sys_Worker_RegisterController(ISys_Worker_RegisterService service)
        : base(service)
        {
        }
    }
}

