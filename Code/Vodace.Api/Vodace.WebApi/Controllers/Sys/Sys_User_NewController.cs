
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/User")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Sys_User_NewController : ApiBaseController<ISys_User_NewService>
    {
        public Sys_User_NewController(ISys_User_NewService service, Entity.DomainModels.Common.ApiVersionConfig versionConfig = null)
        : base(service)
        {
            _versionConfig = versionConfig;
        }
    }
}

