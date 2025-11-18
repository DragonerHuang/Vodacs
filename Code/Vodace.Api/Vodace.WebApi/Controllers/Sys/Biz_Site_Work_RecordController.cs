
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/SiteWorkRecord")]
    [PermissionTable(Name = "Biz_Site_Work_Record")]
    [ApiVersion("1.0")]
    public partial class Biz_Site_Work_RecordController : ApiBaseController<IBiz_Site_Work_RecordService>
    {
        public Biz_Site_Work_RecordController(IBiz_Site_Work_RecordService service)
        : base(service)
        {
        }
    }
}

