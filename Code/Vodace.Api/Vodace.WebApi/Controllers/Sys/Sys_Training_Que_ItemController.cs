
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/Sys_Training_Que_Item")]
    [PermissionTable(Name = "Sys_Training_Que_Item")]
    public partial class Sys_Training_Que_ItemController : ApiBaseController<ISys_Training_Que_ItemService>
    {
        public Sys_Training_Que_ItemController(ISys_Training_Que_ItemService service)
        : base(service)
        {
        }
    }
}

