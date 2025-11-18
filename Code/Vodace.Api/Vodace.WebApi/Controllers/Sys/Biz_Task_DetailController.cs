
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    //[Route("api/Biz_Task_Detail")]
    //[PermissionTable(Name = "Biz_Task_Detail")]
    [Route("api/v{version:apiVersion}/TaskDetail")]
    [ApiVersion("1.0")]
    public partial class Biz_Task_DetailController : ApiBaseController<IBiz_Task_DetailService>
    {
        public Biz_Task_DetailController(IBiz_Task_DetailService service)
        : base(service)
        {
        }
    }
}

