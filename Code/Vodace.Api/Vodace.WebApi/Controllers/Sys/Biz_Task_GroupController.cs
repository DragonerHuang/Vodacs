
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    //[Route("api/Biz_Task_Group")]
    //[PermissionTable(Name = "Biz_Task_Group")]
    [Route("api/v{version:apiVersion}/TaskGroup")]
    [ApiVersion("1.0")]
    public partial class Biz_Task_GroupController : ApiBaseController<IBiz_Task_GroupService>
    {
        public Biz_Task_GroupController(IBiz_Task_GroupService service)
        : base(service)
        {
        }
    }
}

