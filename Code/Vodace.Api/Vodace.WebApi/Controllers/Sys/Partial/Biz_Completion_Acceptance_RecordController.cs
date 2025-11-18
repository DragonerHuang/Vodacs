
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;
using Vodace.Core.Filters;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Completion_Acceptance_RecordController
    {
        private readonly IBiz_Completion_Acceptance_RecordService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Completion_Acceptance_RecordController(
            IBiz_Completion_Acceptance_RecordService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Route("GetFileList"), ApiActionPermission]
        public async Task<IActionResult> GetFileList(Guid accId)
        {
            var result = await _service.GetFileList(accId);
            return Json(result);
        }
        [HttpGet, Route("GetFileListByVersion"), ApiActionPermission]
        public async Task<IActionResult> GetFileListByVersion(Guid accId, int ver)
        {
            var result = await _service.GetFileListByVersion(accId, ver);
            return Json(result);
        }
        [HttpPost, Route("SubmitFile"), ApiActionPermission]
        public async Task<IActionResult> SubmitFile(Guid subId)
        {
            var result = await _service.SubmitFile(subId);
            return Json(result);
        }
        [HttpPost, Route("UploadFile"), ApiActionPermission]
        public async Task<IActionResult> UploadFile([FromForm] UploadAcceptanceRecordDto recordDto, IFormFile file = null)
        {
            var result = await _service.UploadFile(recordDto, file);
            return Json(result);
        }

        [HttpPost, Route("UploadImg"), ApiActionPermission]
        public async Task<IActionResult> UploadImg([FromForm] UploadAcceptanceRecordExDto exDto, IFormFile file = null)
        {
            var result = await _service.UploadImg(exDto, file);
            return Json(result);
        }

        [HttpDelete, Route("DeleteFile"), ApiActionPermission]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            var result = await _service.DeleteFile(id);
            return Json(result);
        }

        [HttpPut, Route("UpdateCheckList"), ApiActionPermission]
        public async Task<IActionResult> UpdateCheckList(Guid id, string jsonData)
        {
            var result = await _service.UpdateCheckList(id, jsonData);
            return Json(result);
        }

        [HttpPut, Route("AuditData"), ApiActionPermission]
        public async Task<IActionResult> AuditData(Guid id, int status)
        {
            var result = await _service.AuditData(id, status);
            return Json(result);
        }

        [HttpGet, Route("DownloadSysFile"), ApiActionPermission]
        public async Task<IActionResult> DownloadSysFile(Guid guidFileId)
        {
            var result = await _service.DownloadSysFile(guidFileId);
            return Json(result);
        }
    }
}
