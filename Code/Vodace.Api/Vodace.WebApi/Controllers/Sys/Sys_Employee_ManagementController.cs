
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/EmployeeManagement")]
    [ApiVersion("1.0")]
    public partial class Sys_Employee_ManagementController : ApiBaseController<ISys_Employee_ManagementService>
    {
        public Sys_Employee_ManagementController(ISys_Employee_ManagementService service)
        : base(service)
        {
        }
    }
}

