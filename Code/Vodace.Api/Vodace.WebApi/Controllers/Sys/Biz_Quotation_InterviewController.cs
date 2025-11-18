
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/QuotationInterview")]
    [PermissionTable(Name = "Biz_Quotation_Interview")]
    [ApiVersion("1.0")]
    public partial class Biz_Quotation_InterviewController : ApiBaseController<IBiz_Quotation_InterviewService>
    {
        public Biz_Quotation_InterviewController(IBiz_Quotation_InterviewService service)
        : base(service)
        {
        }
    }
}

