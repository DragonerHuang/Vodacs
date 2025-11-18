
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Extensions;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_File_RecordsController
    {
        private readonly ISys_File_RecordsService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_File_RecordsController(
            ISys_File_RecordsService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 工人注册上传文件接口
        /// </summary>
        /// <param name="litFiles"></param>
        /// <param name="strFileType"></param>
        /// <returns></returns>
        [HttpPost, Route("WorkRegisterUpload"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> WorkRegisterUploadAsync(
            List<IFormFile> litFiles,
            [FromForm]string strFileType)
        {
            var result = await _service.WorkRegisterUploadAsync(litFiles, strFileType, 0);
            return Json(result);
        }

        /// <summary>
        /// 上传文件（多个）
        /// </summary>
        /// <param name="litFiles"></param>
        /// <param name="strFileType"></param>
        /// <returns></returns>
        [HttpPost, Route("UploadFilesAsync")]
        [ApiActionPermission]
        public async Task<IActionResult> UploadFilesAsync(
            List<IFormFile> litFiles,
            [FromForm] string strFileType)
        {
            var result = await _service.UploadFilesAsync(litFiles, strFileType, 0);
            return Json(result);
        }

        /// <summary>
        /// 下载文件（通用）
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet, Route("DownFile")]
        [ApiActionPermission]
        public async Task<IActionResult> DownSysFile(Guid guidId)
        {
            var result = await _service.DownloadSysFile(guidId);
            if (!result.status)
            {
                return Json(result);
            }
            var file = result.data as FileDownLoadDto;

            return File(file.file_bytes, file.cntent_type, file.file_name);
        }

        /// <summary>
        /// 预览文件（通用）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("PreviewFile")]
        [ApiActionPermission]
        public async Task<IActionResult> PreviewFile(Guid id)
        {
            var result = await _service.DownloadSysFile(id);
            if (!result.status)
            {
                return Json(result);
            }
            var file = result.data as FileDownLoadDto;

            return File(file.file_bytes, file.cntent_type);
        }


        /// <summary>
        /// 文件转换
        /// </summary>
        /// <param name="guidId"></param>
        /// <returns></returns>
        [HttpGet, Route("PDFFileChange")]
        [ApiActionPermission]
        public async Task<IActionResult> PDFFileChange(Guid guidId)
        {
            var result = await _service.FileConvertPDFById(guidId);

            return Json(result);
        }

        /// <summary>
        /// 文件转换
        /// </summary>
        /// <param name="guidId"></param>
        /// <returns></returns>
        [HttpGet, Route("PDFFileChangeByPath")]
        [ApiActionPermission]
        public IActionResult PDFFileChangeByPath()
        {
            var result = _service.FileConvertPDFByPath("");

            return Json(result);
        }
    }
}
