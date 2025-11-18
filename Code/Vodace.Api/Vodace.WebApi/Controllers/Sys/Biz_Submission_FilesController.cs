
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/SubmissionFiles")]
    [PermissionTable(Name = "Biz_Submission_Files")]
    [ApiVersion("1.0")]
    public partial class Biz_Submission_FilesController : ApiBaseController<IBiz_Submission_FilesService>
    {
        public Biz_Submission_FilesController(IBiz_Submission_FilesService service)
        : base(service)
        {
        }
    }
}

