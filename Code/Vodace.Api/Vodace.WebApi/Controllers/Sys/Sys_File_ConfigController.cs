
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/FileConfig")]
    [ApiVersion("1.0")]
    public partial class Sys_File_ConfigController : ApiBaseController<ISys_File_ConfigService>
    {
        public Sys_File_ConfigController(ISys_File_ConfigService service)
        : base(service)
        {
        }
    }
}

