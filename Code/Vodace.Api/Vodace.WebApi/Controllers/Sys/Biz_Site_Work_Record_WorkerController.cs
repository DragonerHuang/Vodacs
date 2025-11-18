
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/SiteWorkRecordWorker")]
    [PermissionTable(Name = "Biz_Site_Work_Record")]
    [ApiVersion("1.0")]
    public partial class Biz_Site_Work_Record_WorkerController : ApiBaseController<IBiz_Site_Work_Record_WorkerService>
    {
        public Biz_Site_Work_Record_WorkerController(IBiz_Site_Work_Record_WorkerService service)
        : base(service)
        {
        }
    }
}

