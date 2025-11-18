
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/VariousWorkOrder")]
    [ApiVersion("1.0")]
    public partial class Biz_Various_Work_OrderController : ApiBaseController<IBiz_Various_Work_OrderService>
    {
        public Biz_Various_Work_OrderController(IBiz_Various_Work_OrderService service)
        : base(service)
        {
        }
    }
}

