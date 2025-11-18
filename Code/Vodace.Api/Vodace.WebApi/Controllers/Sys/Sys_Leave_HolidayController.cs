
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [PermissionTable(Name = "Sys_Leave_Holiday")]
    [Route("api/v{version:apiVersion}/LeaveHoliday")]
    [ApiVersion("1.0")]
    public partial class Sys_Leave_HolidayController : ApiBaseController<ISys_Leave_HolidayService>
    {
        public Sys_Leave_HolidayController(ISys_Leave_HolidayService service)
        : base(service)
        {
        }
    }
}

