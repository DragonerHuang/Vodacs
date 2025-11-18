
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Completion_AcceptanceService
    {
        Task<WebResponseContent> GetListByPage(PageInput<SubmissionFilesQuery> query);
        Task<WebResponseContent> GetListByPageByUser(PageInput query);
        Task<WebResponseContent> GetDataById(Guid id);
        WebResponseContent CheckFileNo(Guid contractId, string fileno);
        Task<WebResponseContent> AddData(CompletionAcceptanceAddDto addDto);
        Task<WebResponseContent> EditData(CompletionAcceptanceEditDto editDto);
        Task<WebResponseContent> AuditByBatch(SubmissionAuditDto dto);
        Task<WebResponseContent> SubmitAudit(Guid fileId);

        Task<WebResponseContent> GetFileList(Guid accId);
        Task<WebResponseContent> GetFileListByVersion(Guid subId, int version);
        Task<WebResponseContent> UploadFile(UploadAcceptanceRecordDto recordDto, IFormFile file = null);
        Task<WebResponseContent> UploadFileByApproved(UploadByApprovedDto recordDto, IFormFile file = null);
        Task<WebResponseContent> UploadImg(UploadAcceptanceRecordExDto exDto, IFormFile file = null);
        Task<WebResponseContent> DeleteFile(Guid id);
        Task<WebResponseContent> UpdateCheckList(Guid id, string jsonData);
        Task<WebResponseContent> AuditData(Guid id, int status);
        Task<WebResponseContent> DownloadSysFile(Guid guidFileId);
        Task<WebResponseContent> SubmitFile(Guid fileId);
        Task<WebResponseContent> UpdateSubmitStatus(Guid id, int status);
    }
 }
