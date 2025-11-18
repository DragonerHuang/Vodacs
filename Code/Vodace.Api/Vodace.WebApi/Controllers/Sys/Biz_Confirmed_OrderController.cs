
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/ConfirmedOrder")]
    [PermissionTable(Name = "Biz_Confirmed_Order")]
    [ApiVersion("1.0")]
    public partial class Biz_Confirmed_OrderController : ApiBaseController<IBiz_Confirmed_OrderService>
    {
        public Biz_Confirmed_OrderController(IBiz_Confirmed_OrderService service)
        : base(service)
        {
        }
    }
}

