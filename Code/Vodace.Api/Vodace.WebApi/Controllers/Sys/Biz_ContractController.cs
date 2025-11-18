
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/Contract")]
    [ApiVersion("1.0")]
    public partial class Biz_ContractController : ApiBaseController<IBiz_ContractService>
    {
        public Biz_ContractController(IBiz_ContractService service)
        : base(service)
        {
        }
    }
}

