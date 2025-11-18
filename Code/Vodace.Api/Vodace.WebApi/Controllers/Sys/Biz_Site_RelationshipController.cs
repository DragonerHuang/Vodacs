
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/SiteRelationship")]
    [ApiVersion("1.0")]
    public partial class Biz_Site_RelationshipController : ApiBaseController<IBiz_Site_RelationshipService>
    {
        public Biz_Site_RelationshipController(IBiz_Site_RelationshipService service)
        : base(service)
        {
        }
    }
}

