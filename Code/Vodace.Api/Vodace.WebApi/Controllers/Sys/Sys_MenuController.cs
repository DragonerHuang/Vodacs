
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    [PermissionTable(Name = "Sys_Menu")]
    [Route("api/v{version:apiVersion}/Menu")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Sys_MenuController : ApiBaseController<ISys_MenuService>
    {
        private ISys_MenuService _service { get; set; }
        public Sys_MenuController(ISys_MenuService service) :
            base("System", "System", "Sys_Menu", service)
        {
            _service = service;
        }
    }
}

