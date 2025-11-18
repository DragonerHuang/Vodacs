
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Core.Localization;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Completion_AcceptanceController
    {
        private readonly IBiz_Completion_AcceptanceService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly ISys_User_NewRepository _user_NewRepository;
        private readonly ISys_Message_NotificationService _messageService;
        private readonly IBiz_Completion_Acceptance_RecordRepository _Acceptance_RecordRepository;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_Completion_AcceptanceController(
            IBiz_Completion_AcceptanceService service,
            IHttpContextAccessor httpContextAccessor
,
            ILocalizationService localizationService,
            ISys_User_NewRepository user_NewRepository,
            ISys_Message_NotificationService messageService,
            IBiz_Completion_Acceptance_RecordRepository acceptance_RecordRepository,
            IMapper mapper)
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _localizationService = localizationService;
            _user_NewRepository = user_NewRepository;
            _messageService = messageService;
            _Acceptance_RecordRepository = acceptance_RecordRepository;
            _mapper = mapper;
        }
        [HttpPost, Route("GetListByPage"), ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody] PageInput<SubmissionFilesQuery> query)
        {
            return Json(await _service.GetListByPage(query));
        }
        [HttpPost, Route("GetListByPageByUser"), ApiActionPermission]
        public async Task<IActionResult> GetListByPageByUser([FromBody] PageInput query)
        {
            return Json(await _service.GetListByPageByUser(query));
        }
        [HttpGet, Route("GetDataById"), ApiActionPermission]
        public async Task<IActionResult> GetDataById(Guid id)
        {
            return Json(await _service.GetDataById(id));
        }
        [HttpGet, Route("CheckFileNo"), ApiActionPermission]
        public IActionResult CheckFileNo(Guid contractId, string fileno)
        {
            return Json(_service.CheckFileNo(contractId, fileno));
        }

        [HttpPost, Route("AddData"), ApiActionPermission]
        public async Task<IActionResult> AddData([FromBody] CompletionAcceptanceAddDto addDto)
        {
            return Json(await _service.AddData(addDto));
        }

        [HttpPut, Route("EditData"), ApiActionPermission]
        public async Task<IActionResult> EditData([FromBody] CompletionAcceptanceEditDto editDto)
        {
            return Json(await _service.EditData(editDto));
        }
        [HttpPut, Route("AuditByBatch"), ApiActionPermission]
        public async Task<IActionResult> AuditByBatch([FromBody] SubmissionAuditDto dto)
        {
            return Json(await _service.AuditByBatch(dto));
        }
        [HttpPut, Route("SubmitAudit"), ApiActionPermission]
        public async Task<IActionResult> SubmitAudit(Guid fileId)
        {
            return Json(await _service.SubmitAudit(fileId));
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
        public async Task<IActionResult> SubmitFile(Guid fileId)
        {
            var result = await _service.SubmitFile(fileId);
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
        [HttpPost, Route("UploadFileByApproved"), ApiActionPermission]
        public async Task<IActionResult> UploadFileByApproved(UploadByApprovedDto recordDto, IFormFile file = null)
        {
            var result = await _service.UploadFileByApproved(recordDto, file);
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

        [HttpPut, Route("UpdateSubmitStatus"), ApiActionPermission]
        public async Task<IActionResult> UpdateSubmitStatus(Guid id, int status)
        {
            var result = await _service.UpdateSubmitStatus(id, status);
            return Json(result);
        }

    }
}
