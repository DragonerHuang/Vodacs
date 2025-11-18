
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/MessageNotification")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Sys_Message_NotificationController : ApiBaseController<ISys_Message_NotificationService>
    {
        public Sys_Message_NotificationController(ISys_Message_NotificationService service)
        : base(service)
        {
        }
    }
}

