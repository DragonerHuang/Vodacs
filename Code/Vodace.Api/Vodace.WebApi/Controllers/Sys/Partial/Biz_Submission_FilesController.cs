
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Submission_FilesController
    {
        private readonly IBiz_Submission_FilesService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Submission_FilesController(
            IBiz_Submission_FilesService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost,Route("GetListByPage"),ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody]PageInput<SubmissionFilesQuery> query) 
        {
            return Json(await _service.GetListByPage(query));
        }
        [HttpPost, Route("GetListByPageByUser"),ApiActionPermission]
        public async Task<IActionResult> GetListByPageByUser([FromBody] PageInput<SubmissionFilesByUserQuery> query)
        {
            return Json(await _service.GetListByPageByUser(query));
        }
        [HttpGet,Route("GetDataById"),ApiActionPermission]
        public async Task<IActionResult> GetDataById(Guid id) 
        {
            return Json(await _service.GetDataById(id));
        }
        [HttpGet, Route("CheckFileNo"),ApiActionPermission]
        public IActionResult CheckFileNo(Guid contractId, string fileno,Guid? id)
        {
            return Json(_service.CheckFileNo(contractId, fileno, id));
        }
        [HttpPost,Route("AddData"),ApiActionPermission]   
        public async Task<IActionResult> AddData([FromBody]SubmissionFilesAddDto addDto)
        {
            return Json(await _service.AddData(addDto));
        }
        [HttpPut,Route("EditData"),ApiActionPermission]
        public async Task<IActionResult> EditData([FromBody]SubmissionFilesEditDto editDto)
        {
            return Json(await _service.EditData(editDto));
        }

        [HttpPut, Route("AuditByBatch"), ApiActionPermission]
        public async Task<IActionResult> AuditByBatch([FromBody] SubmissionAuditDto dto) 
        {
            return Json(await _service.AuditByBatch(dto));
        }
        [HttpPost, Route("AddDataByBatch"), ApiActionPermission]
        public async Task<IActionResult> AddDataByBatch(Guid contractId, int userId)
        {
            return Json(await _service.AddDataByBatch(contractId, userId));
        }

        [HttpPut, Route("SubmitAudit"), ApiActionPermission]
        public async Task<IActionResult> SubmitAudit(Guid fileId)
        {
            return Json(await _service.SubmitAudit(fileId));
        }

        [HttpGet, Route("GetFileList"), ApiActionPermission]
        public async Task<IActionResult> GetFileList(Guid subId)
        {
            return Json(await _service.GetFileList(subId));
        }
        [HttpGet, Route("GetFileListByVersion"), ApiActionPermission]
        public async Task<IActionResult> GetFileListByVersion(Guid subId, int ver)
        {
            return Json(await _service.GetFileListByVersion(subId, ver));
        }
        [HttpPost, Route("UploadFile"), ApiActionPermission]
        public async Task<IActionResult> UploadFile([FromForm] UploadSubmissionFilesRecordDto recordDto, IFormFile file = null)
        {
            return Json(await _service.UploadFile(recordDto, file));
        }
        [HttpPost, Route("UploadFileByApproved"), ApiActionPermission]
        public async Task<IActionResult> UploadFileByApproved([FromForm] UploadByApprovedDto recordDto, IFormFile file = null) 
        {
            return Json(await _service.UploadFileByApproved(recordDto, file));
        }
        [HttpDelete, Route("DeleteFile"), ApiActionPermission]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            return Json(await _service.DeleteFile(id));
        }
        [HttpPut, Route("UpdateCheckList"), ApiActionPermission]
        public async Task<IActionResult> UpdateCheckList(Guid id, string jsonData)
        {
            return Json(await _service.UpdateCheckList(id, jsonData));
        }

        [HttpPut, Route("AuditData"), ApiActionPermission]
        public async Task<IActionResult> AuditData(Guid id, int status)
        {
            return Json(await _service.AuditData(id, status));
        }

        [HttpGet, Route("PreviewFile"), ApiActionPermission]
        public async Task<IActionResult> DownloadSysFile(Guid guidFileId)
        {
            var result = await _service.DownloadSysFile(guidFileId);
            if (!result.status)
            {
                return Json(result);
            }
            var file = result.data as FileDownLoadDto;

            return File(file.file_bytes, file.cntent_type);
        }
        [HttpPut, Route("SubmitFile"), ApiActionPermission]
        public async Task<IActionResult> SubmitFile(Guid fileId)
        {
            return Json(await _service.SubmitFile(fileId));
        }
        [HttpPut, Route("UpdateSubmitStatus"), ApiActionPermission]
        public async Task<IActionResult> UpdateSubmitStatus(Guid id, int status) 
        {
            return Json(await _service.UpdateSubmitStatus(id, status));
        }

        #region 需求变更后
        [HttpGet, Route("GetFileListNew"), ApiActionPermission]
        public async Task<IActionResult> GetFileListNew(Guid subId)
        {
            return Json(await _service.GetFileListNew(subId));
        }
        [HttpPut, Route("UploadFileNew"), ApiActionPermission]
        public async Task<IActionResult> UploadFileNew([FromForm] UploadSubmissionFilesRecordNewDto recordDto, List<IFormFile> file = null) 
        {
            return Json(await _service.UploadFileNew(recordDto, file));
        }
        [HttpPut, Route("UploadFileCover"), ApiActionPermission]
        public async Task<IActionResult> UploadFileCover([FromForm] UploadSubmissionFilesCoverDto recordDto, IFormFile file = null)
        {
            return Json(await _service.UploadFileCover(recordDto, file));
        }
        [HttpPut, Route("RenameFile"), ApiActionPermission]
        public async Task<IActionResult> RenameFile([FromBody]SubmissionRenameFileDto fileDto)
        {
            return Json(await _service.RenameFile(fileDto));
        }
        [HttpPut, Route("CopyFile"), ApiActionPermission]
        public async Task<IActionResult> CopyFile([FromBody]SubmissionFilesCopyDto copyDto)
        {
            return Json(await _service.CopyFile(copyDto));
        }
        [HttpPut, Route("MoveFile"), ApiActionPermission]
        public async Task<IActionResult> MoveFile([FromBody]SubmissionFilesCopyDto copyDto)
        {
            return Json(await _service.MoveFile(copyDto));
        }
        [HttpPost, Route("CheckFileNoNew"), ApiActionPermission]
        public async Task<IActionResult> CheckFileNoNew([FromBody] SubmissionFilesCheckDto dto)
        {
            return Json(await _service.CheckFileNoNew(dto));
        }
        #endregion
    }
}
