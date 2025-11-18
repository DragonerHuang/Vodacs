
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/AttendanceLocation")]
    [PermissionTable(Name = "Sys_Attendance_Location")]
    [ApiVersion("1.0")]
    public partial class Sys_Attendance_LocationController : ApiBaseController<ISys_Attendance_LocationService>
    {
        public Sys_Attendance_LocationController(ISys_Attendance_LocationService service)
        : base(service)
        {
        }
    }
}

