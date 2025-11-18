
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/Biz_Task")]
    [PermissionTable(Name = "Biz_Task")]
    public partial class Biz_TaskController : ApiBaseController<IBiz_TaskService>
    {
        public Biz_TaskController(IBiz_TaskService service)
        : base(service)
        {
        }
    }
}

