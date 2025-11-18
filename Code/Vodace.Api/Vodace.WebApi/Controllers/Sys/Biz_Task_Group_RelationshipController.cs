
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    //[Route("api/Biz_Task_Group_Relationship")]
    //[PermissionTable(Name = "Biz_Task_Group_Relationship")]
    [Route("api/v{version:apiVersion}/TaskGroupRelationship")]
    [ApiVersion("1.0")]
    public partial class Biz_Task_Group_RelationshipController : ApiBaseController<IBiz_Task_Group_RelationshipService>
    {
        public Biz_Task_Group_RelationshipController(IBiz_Task_Group_RelationshipService service)
        : base(service)
        {
        }
    }
}

