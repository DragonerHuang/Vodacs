
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/ContractContact")]
    [ApiVersion("1.0")]
    public partial class Biz_Contract_ContactController : ApiBaseController<IBiz_Contract_ContactService>
    {
        public Biz_Contract_ContactController(IBiz_Contract_ContactService service)
        : base(service)
        {
        }
    }
}

