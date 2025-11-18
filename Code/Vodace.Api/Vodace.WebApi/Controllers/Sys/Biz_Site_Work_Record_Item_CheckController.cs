
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    
    [Route("api/v{version:apiVersion}/SiteWorkRecordItemCheck")]
    [PermissionTable(Name = "Biz_Site_Work_Record_Item_Check")]
    [ApiVersion("1.0")]
    public partial class Biz_Site_Work_Record_Item_CheckController : ApiBaseController<IBiz_Site_Work_Record_Item_CheckService>
    {
        public Biz_Site_Work_Record_Item_CheckController(IBiz_Site_Work_Record_Item_CheckService service)
        : base(service)
        {
        }
    }
}

