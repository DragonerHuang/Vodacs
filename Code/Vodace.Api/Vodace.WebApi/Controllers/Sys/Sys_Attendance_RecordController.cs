
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/AttendanceRecord")]
    [PermissionTable(Name = "Sys_Attendance_Record")]
    [ApiVersion("1.0")]
    public partial class Sys_Attendance_RecordController : ApiBaseController<ISys_Attendance_RecordService>
    {
        public Sys_Attendance_RecordController(ISys_Attendance_RecordService service)
        : base(service)
        {
        }
    }
}

