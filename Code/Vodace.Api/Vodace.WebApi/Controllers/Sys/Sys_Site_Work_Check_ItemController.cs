
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/SiteWorkCheckItem")]
    [PermissionTable(Name = "Sys_Site_Work_Check_Item")]
    [ApiVersion("1.0")]
    public partial class Sys_Site_Work_Check_ItemController : ApiBaseController<ISys_Site_Work_Check_ItemService>
    {
        public Sys_Site_Work_Check_ItemController(ISys_Site_Work_Check_ItemService service)
        : base(service)
        {
        }
    }
}

