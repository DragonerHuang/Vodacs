
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [PermissionTable(Name = "Sys_Work_Type")]
    [Route("api/v{version:apiVersion}/WorkType")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Sys_Work_TypeController : ApiBaseController<ISys_Work_TypeService>
    {
        public Sys_Work_TypeController(ISys_Work_TypeService service)
        : base(service)
        {
        }
    }
}

