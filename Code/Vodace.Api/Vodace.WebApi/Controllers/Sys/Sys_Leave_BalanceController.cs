
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/LeaveBalance")]
    [ApiVersion("1.0")]
    public partial class Sys_Leave_BalanceController : ApiBaseController<ISys_Leave_BalanceService>
    {
        public Sys_Leave_BalanceController(ISys_Leave_BalanceService service)
        : base(service)
        {
        }
    }
}

