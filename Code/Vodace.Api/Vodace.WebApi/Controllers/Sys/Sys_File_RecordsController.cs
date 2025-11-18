
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [Route("api/v{version:apiVersion}/FileRecords")]
    [PermissionTable(Name = "Sys_File_Records")]
    [ApiVersion("1.0")]
    public partial class Sys_File_RecordsController : ApiBaseController<ISys_File_RecordsService>
    {
        public Sys_File_RecordsController(ISys_File_RecordsService service)
        : base(service)
        {
        }
    }
}

