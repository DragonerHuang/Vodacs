
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/CompletionAcceptanceRecord")]
    [PermissionTable(Name = "Biz_Completion_Acceptance_Record")]
    [ApiVersion("1.0")]
    public partial class Biz_Completion_Acceptance_RecordController : ApiBaseController<IBiz_Completion_Acceptance_RecordService>
    {
        public Biz_Completion_Acceptance_RecordController(IBiz_Completion_Acceptance_RecordService service)
        : base(service)
        {
        }
    }
}

