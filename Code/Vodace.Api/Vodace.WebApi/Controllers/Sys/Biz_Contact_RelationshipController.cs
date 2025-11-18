
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/ContactRelationship")]
    [ApiVersion("1.0")]
    public partial class Biz_Contact_RelationshipController : ApiBaseController<IBiz_Contact_RelationshipService>
    {
        public Biz_Contact_RelationshipController(IBiz_Contact_RelationshipService service)
        : base(service)
        {
        }
    }
}

