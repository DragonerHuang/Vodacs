using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Vodace.Core.Controllers.Basic;
using Vodace.Core.Filters;
using Vodace.Core.ManageUser;
using Vodace.Entity.AttributeManager;
using Vodace.Entity.DomainModels;
using Vodace.Entity.DomainModels.Biz.partial;
using Vodace.Sys.IServices.Biz.Partial;

namespace Vodace.Api.Controllers.Sys.Partial
{
    [Route("api/v{version:apiVersion}/Files")]
    [ApiVersion("1.0")]

    public class FilesController : VolController
    {
        private readonly IFileStorageService _fileStorageService;

        public FilesController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpGet("preview")]
        [AllowAnonymous]
        public async Task<IActionResult> FilePreview(Guid fileId)
        {
            try
            {
                var physicalPath = await _fileStorageService.GetFilePhysicalPathAsync(fileId);
                if (!System.IO.File.Exists(physicalPath))
                {
                    return NotFound(new { error = "文件不存在", path = physicalPath });
                }
                var fileInfo = await _fileStorageService.GetFileInfoAsync(fileId);
                var contentType = GetSafeContentType(fileInfo?.MimeType);
                return File(System.IO.File.OpenRead(physicalPath), contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "文件访问失败", details = ex.ToString() });
            }
        }

        private string GetSafeContentType(string mimeType)
        {
            if (string.IsNullOrEmpty(mimeType))
                return "application/octet-stream";

            return mimeType.Contains("/") ? mimeType : "application/octet-stream";
        }

        /// <summary>
        /// 预览缩略图图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("ViewThumbnailFileByteById")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewThumbnailFileByteByIdAsync(Guid id)
        {
            var result = await _fileStorageService.ViewThumbnailFileByteByIdAsync(id);
            if (!result.status)
            {
                return Json(result);
            }
            var file = result.data as FileDownLoadDto;

            return File(file.file_bytes, file.cntent_type);
        }

    }

    public class GeneratePreviewRequest
    {
        public Guid FileId { get; set; } = Guid.Empty;
    }


    public class BatchUrlRequest
    {
        public List<Guid> FileIds { get; set; } = new List<Guid>();
    }
}
