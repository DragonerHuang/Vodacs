
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Deadline_ManagementController
    {
        private readonly IBiz_Deadline_ManagementService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Deadline_ManagementController(
            IBiz_Deadline_ManagementService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost, Route("GetListByPage"),ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody]PageInput<SubmissionFilesQuery> query) 
        {
            return Json(await _service.GetListByPage(query));
        }
        [HttpGet, Route("GetDataById"), ApiActionPermission]
        public async Task<IActionResult> GetDataById(Guid id)
        {
            return Json(await _service.GetDataById(id));
        }
        [HttpPost, Route("AddData"), ApiActionPermission]
        public async Task<IActionResult> AddData([FromBody] DeadlineManagementAddDto addDto )
        {
            return Json(await _service.AddData(addDto));
        }
        [HttpPut, Route("EditData"), ApiActionPermission]
        public async Task<IActionResult> EditData([FromBody] DeadlineManagementEditDto editDto)
        {
            return Json(await _service.EditData(editDto));
        }
        [HttpDelete, Route("DeleteData"), ApiActionPermission]
        public async Task<IActionResult> DeleteData(Guid id)
        {
            return Json(await _service.DeleteData(id));
        }
        [HttpPut, Route("AuditByBatch"), ApiActionPermission]
        public async Task<IActionResult> AuditData([FromBody] SubmissionAuditDto dto)
        {
            return Json(await _service.AuditByBatch(dto));
        }


        [HttpPost, Route("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm]int type, List<IFormFile> file)
        {
            var result = await _service.UploadFiles(type,file);
            return Json(result);
        }

        [HttpGet, Route("DownloadSysFile")]
        public async Task<IActionResult> DownloadSysFile(Guid fileId)
        {
            var result = await _service.DownloadSysFile(fileId);
            return Json(result);
        }
        [HttpDelete, Route("DeleteFile")]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            var result = await _service.DeleteFile(fileId);
            return Json(result);
        }
    }
}
