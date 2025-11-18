
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{


    [PermissionTable(Name = "Sys_Leave_Type")]
    [Route("api/v{version:apiVersion}/LeaveType")]
    [ApiVersion("1.0")]
    public partial class Sys_Leave_TypeController : ApiBaseController<ISys_Leave_TypeService>
    {
        public Sys_Leave_TypeController(ISys_Leave_TypeService service)
        : base(service)
        {
        }
    }
}

