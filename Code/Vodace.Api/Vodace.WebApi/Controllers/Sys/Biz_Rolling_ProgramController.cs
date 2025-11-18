
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/RollingProgram")]
    [ApiVersion("1.0")]
    public partial class Biz_Rolling_ProgramController : ApiBaseController<IBiz_Rolling_ProgramService>
    {
        public Biz_Rolling_ProgramController(IBiz_Rolling_ProgramService service)
        : base(service)
        {
        }
    }
}

