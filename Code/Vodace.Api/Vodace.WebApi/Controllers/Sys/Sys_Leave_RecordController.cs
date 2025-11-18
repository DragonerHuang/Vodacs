
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/LeaveRecord")]
    [ApiVersion("1.0")]
    public partial class Sys_Leave_RecordController : ApiBaseController<ISys_Leave_RecordService>
    {
        public Sys_Leave_RecordController(ISys_Leave_RecordService service)
        : base(service)
        {
        }
    }
}

