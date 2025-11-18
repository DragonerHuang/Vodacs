
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/ContractOrg")]
    [ApiVersion("1.0")]
    public partial class Biz_ContractOrgController : ApiBaseController<IBiz_ContractOrgService>
    {
        public Biz_ContractOrgController(IBiz_ContractOrgService service)
        : base(service)
        {
        }
    }
}

