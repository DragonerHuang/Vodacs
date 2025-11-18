
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Submission_FilesService
    {
        Task<WebResponseContent> GetListByPage(PageInput<SubmissionFilesQuery> query);
        Task<WebResponseContent> GetListByPageByUser(PageInput<SubmissionFilesByUserQuery> query);
        Task<WebResponseContent> GetDataById(Guid id);
        WebResponseContent CheckFileNo(Guid contractId, string fileno, Guid? id);
        Task<WebResponseContent> AddData(SubmissionFilesAddDto addDto);
        Task<WebResponseContent> EditData(SubmissionFilesEditDto editDto);
        Task<WebResponseContent> AuditByBatch(SubmissionAuditDto dto);
        Task<WebResponseContent> AddDataByBatch(Guid contractId, int userId);
        Task<WebResponseContent> SubmitAudit(Guid fileId);


        Task<WebResponseContent> GetFileList(Guid subId);
        Task<WebResponseContent> GetFileListByVersion(Guid subId, int ver);
        Task<WebResponseContent> UploadFile(UploadSubmissionFilesRecordDto recordDto, IFormFile file = null);
        Task<WebResponseContent> UploadFileByApproved(UploadByApprovedDto recordDto, IFormFile file = null);
        Task<WebResponseContent> DeleteFile(Guid id);
        Task<WebResponseContent> UpdateCheckList(Guid id, string jsonData);
        Task<WebResponseContent> AuditData(Guid id, int status);
        Task<WebResponseContent> DownloadSysFile(Guid guidFileId);
        Task<WebResponseContent> SubmitFile(Guid fileId);
        Task<WebResponseContent> UpdateSubmitStatus(Guid id, int status);


        #region 需求变更后 -2025-10-12
        Task<WebResponseContent> GetFileListNew(Guid subId);
        Task<WebResponseContent> UploadFileNew(UploadSubmissionFilesRecordNewDto recordDto, List<IFormFile> file = null);
        Task<WebResponseContent> UploadFileCover(UploadSubmissionFilesCoverDto recordDto, IFormFile file = null);
        Task<WebResponseContent> RenameFile(SubmissionRenameFileDto fileDto);
        Task<WebResponseContent> CopyFile(SubmissionFilesCopyDto copyDto);
        Task<WebResponseContent> MoveFile(SubmissionFilesCopyDto copyDto);
        Task<WebResponseContent> CheckFileNoNew(SubmissionFilesCheckDto dto);
        #endregion
    }
}