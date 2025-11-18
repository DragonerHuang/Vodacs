
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/Biz_Site_Work_Record_Sign")]
    [PermissionTable(Name = "Biz_Site_Work_Record_Sign")]
    public partial class Biz_Site_Work_Record_SignController : ApiBaseController<IBiz_Site_Work_Record_SignService>
    {
        public Biz_Site_Work_Record_SignController(IBiz_Site_Work_Record_SignService service)
        : base(service)
        {
        }
    }
}

