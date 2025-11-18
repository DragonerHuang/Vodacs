
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [PermissionTable(Name = "Biz_Quotation_Deadline")]
    [Route("api/v{version:apiVersion}/QuotationDeadline")]
    [ApiVersion("1.0")]
    public partial class Biz_Quotation_DeadlineController : ApiBaseController<IBiz_Quotation_DeadlineService>
    {
        public Biz_Quotation_DeadlineController(IBiz_Quotation_DeadlineService service)
        : base(service)
        {
        }
    }
}

