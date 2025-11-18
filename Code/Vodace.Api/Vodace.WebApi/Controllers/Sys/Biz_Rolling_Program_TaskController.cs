
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/RollingProgramTask")]
    [ApiVersion("1.0")]
    public partial class Biz_Rolling_Program_TaskController : ApiBaseController<IBiz_Rolling_Program_TaskService>
    {
        public Biz_Rolling_Program_TaskController(IBiz_Rolling_Program_TaskService service)
        : base(service)
        {
        }
    }
}

