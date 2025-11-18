
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/Sys_Leave_Attachment")]
    [PermissionTable(Name = "Sys_Leave_Attachment")]
    public partial class Sys_Leave_AttachmentController : ApiBaseController<ISys_Leave_AttachmentService>
    {
        public Sys_Leave_AttachmentController(ISys_Leave_AttachmentService service)
        : base(service)
        {
        }
    }
}

