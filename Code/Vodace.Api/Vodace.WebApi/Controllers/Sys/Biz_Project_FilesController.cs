
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [PermissionTable(Name = "Biz_Project_Files")]
    [Route("api/v{version:apiVersion}/ProjectFiles")]
    [ApiVersion("1.0")]
    public partial class Biz_Project_FilesController : ApiBaseController<IBiz_Project_FilesService>
    {
        public Biz_Project_FilesController(IBiz_Project_FilesService service)
        : base(service)
        {
        }
    }
}

