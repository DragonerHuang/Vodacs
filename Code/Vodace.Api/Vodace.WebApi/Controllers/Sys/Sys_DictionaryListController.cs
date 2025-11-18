
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [PermissionTable(Name = "Sys_DictionaryList")]
    [Route("api/v{version:apiVersion}/DictionaryList")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Sys_DictionaryListController : ApiBaseController<ISys_DictionaryListService>
    {
        public Sys_DictionaryListController(ISys_DictionaryListService service)
        : base(service)
        {
        }
    }
}

