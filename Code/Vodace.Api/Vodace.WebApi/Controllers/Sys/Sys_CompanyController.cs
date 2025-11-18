
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [PermissionTable(Name = "Sys_Company")]
    [Route("api/v{version:apiVersion}/Company")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Sys_CompanyController : ApiBaseController<ISys_CompanyService>
    {
        public Sys_CompanyController(ISys_CompanyService service)
        : base(service)
        {
        }
    }
}

