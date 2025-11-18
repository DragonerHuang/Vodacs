using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Vodace.Core.Controllers.Basic;
using Vodace.Sys.IServices.Biz.Partial;

namespace Vodace.Api.Controllers.Sys.Partial
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/v{version:apiVersion}/ConfirmedOrder")]
    [ApiVersion("1.0")]
    public class DebugController : VolController
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<DebugController> _logger;

        public DebugController(IFileStorageService fileStorageService, ILogger<DebugController> logger)
        {
            _fileStorageService = fileStorageService;
            _logger = logger;
        }
        [HttpGet("test-file-direct/{fileId}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        public async Task<IActionResult> TestFileDirect(Guid fileId)
        {
            try
            {
                // 1. 直接获取文件路径
                var physicalPath = await _fileStorageService.GetFilePhysicalPathAsync(fileId);
                _logger.LogInformation($"测试直接访问 - 文件物理路径: {physicalPath}");

                // 2. 检查文件是否存在
                if (!System.IO.File.Exists(physicalPath))
                {
                    return NotFound(new { error = "文件不存在", path = physicalPath });
                }

                // 3. 尝试返回文件
                var fileInfo = await _fileStorageService.GetFileInfoAsync(fileId);
                var contentType = GetSafeContentType(fileInfo?.MimeType);

                _logger.LogInformation($"测试直接访问 - 成功，Content-Type: {contentType}");
                return File(System.IO.File.OpenRead(physicalPath), contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"测试直接访问异常: {ex.Message}");
                return StatusCode(500, new { error = "文件访问失败", details = ex.ToString() });
            }
        }

        [HttpGet("test-file-access/{fileId}")]
        public async Task<IActionResult> TestFileAccess(Guid fileId)
        {
            try
            {
                var physicalPath = await _fileStorageService.GetFilePhysicalPathAsync(fileId);
                var exists = await _fileStorageService.FileExistsAsync(fileId);
                var fileInfo = await _fileStorageService.GetFileInfoAsync(fileId);

                return Ok(new
                {
                    fileId,
                    physicalPath,
                    fileExists = exists,
                    fileInfo = new
                    {
                        fileInfo?.OriginalName,
                        fileInfo?.MimeType,
                        fileInfo?.StoragePath
                    },
                    contentType = GetSafeContentType(fileInfo?.MimeType)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private string GetSafeContentType(string mimeType)
        {
            if (string.IsNullOrEmpty(mimeType))
                return "application/octet-stream";

            return mimeType.Contains("/") ? mimeType : "application/octet-stream";
        }
    }
}
