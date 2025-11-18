
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/SubContractors")]
    [ApiVersion("1.0")]
    public partial class Biz_Sub_ContractorsController : ApiBaseController<IBiz_Sub_ContractorsService>
    {
        public Biz_Sub_ContractorsController(IBiz_Sub_ContractorsService service)
        : base(service)
        {
        }
    }
}

