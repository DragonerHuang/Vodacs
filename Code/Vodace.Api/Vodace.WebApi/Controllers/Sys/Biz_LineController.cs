
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/Biz_Line")]
    [PermissionTable(Name = "Biz_Line")]
    public partial class Biz_LineController : ApiBaseController<IBiz_LineService>
    {
        public Biz_LineController(IBiz_LineService service)
        : base(service)
        {
        }
    }
}

