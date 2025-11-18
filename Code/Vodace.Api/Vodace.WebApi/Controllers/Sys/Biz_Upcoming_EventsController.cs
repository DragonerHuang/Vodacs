
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/UpcomingEvents")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Biz_Upcoming_EventsController : ApiBaseController<IBiz_Upcoming_EventsService>
    {
        public Biz_Upcoming_EventsController(IBiz_Upcoming_EventsService service)
        : base(service)
        {
        }
    }
}

