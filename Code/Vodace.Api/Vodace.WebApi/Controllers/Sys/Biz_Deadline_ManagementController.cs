
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/DeadlineManagement")]
    [ApiVersion("1.0")]
    public partial class Biz_Deadline_ManagementController : ApiBaseController<IBiz_Deadline_ManagementService>
    {
        public Biz_Deadline_ManagementController(IBiz_Deadline_ManagementService service)
        : base(service)
        {
        }
    }
}

