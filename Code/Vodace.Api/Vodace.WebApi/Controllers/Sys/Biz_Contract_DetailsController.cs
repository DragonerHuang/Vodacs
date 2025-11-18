
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/ContractDetails")]
    [PermissionTable(Name = "ContractDetails")]
    [ApiVersion("1.0")]
    public partial class Biz_Contract_DetailsController : ApiBaseController<IBiz_Contract_DetailsService>
    {
        public Biz_Contract_DetailsController(IBiz_Contract_DetailsService service)
        : base(service)
        {
        }
    }
}

