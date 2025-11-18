
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/UserRelation")]
    [ApiVersion("1.0")]
    public partial class Sys_User_RelationController : ApiBaseController<ISys_User_RelationService>
    {
        public Sys_User_RelationController(ISys_User_RelationService service)
        : base(service)
        {
        }
    }
}

