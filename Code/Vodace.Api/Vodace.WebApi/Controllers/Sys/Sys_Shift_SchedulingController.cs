
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/Sys_Shift_Scheduling")]
    [PermissionTable(Name = "Sys_Shift_Scheduling")]
    public partial class Sys_Shift_SchedulingController : ApiBaseController<ISys_Shift_SchedulingService>
    {
        public Sys_Shift_SchedulingController(ISys_Shift_SchedulingService service)
        : base(service)
        {
        }
    }
}

