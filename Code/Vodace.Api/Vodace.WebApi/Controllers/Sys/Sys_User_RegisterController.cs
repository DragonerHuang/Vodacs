
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/UseRegister")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Sys_User_RegisterController : ApiBaseController<ISys_User_RegisterService>
    {
        public Sys_User_RegisterController(ISys_User_RegisterService service)
        : base(service)
        {
        }
    }
}

