
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    //[Route("api/Biz_Task_Detail_Work_Type")]
    //[PermissionTable(Name = "Biz_Task_Detail_Work_Type")]
    [Route("api/v{version:apiVersion}/TaskWorkType")]
    [ApiVersion("1.0")]
    public partial class Biz_Task_Detail_Work_TypeController : ApiBaseController<IBiz_Task_Detail_Work_TypeService>
    {
        public Biz_Task_Detail_Work_TypeController(IBiz_Task_Detail_Work_TypeService service)
        : base(service)
        {
        }
    }
}

