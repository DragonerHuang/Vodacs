
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/CompletionAcceptance")]
    [PermissionTable(Name = "Biz_Completion_Acceptance")]
    [ApiVersion("1.0")]
    public partial class Biz_Completion_AcceptanceController : ApiBaseController<IBiz_Completion_AcceptanceService>
    {
        public Biz_Completion_AcceptanceController(IBiz_Completion_AcceptanceService service)
        : base(service)
        {
        }
    }
}

