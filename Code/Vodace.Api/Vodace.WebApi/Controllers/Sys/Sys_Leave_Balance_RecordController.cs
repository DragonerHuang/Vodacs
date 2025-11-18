
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/Sys_Leave_Balance_Record")]
    [PermissionTable(Name = "Sys_Leave_Balance_Record")]
    public partial class Sys_Leave_Balance_RecordController : ApiBaseController<ISys_Leave_Balance_RecordService>
    {
        public Sys_Leave_Balance_RecordController(ISys_Leave_Balance_RecordService service)
        : base(service)
        {
        }
    }
}

