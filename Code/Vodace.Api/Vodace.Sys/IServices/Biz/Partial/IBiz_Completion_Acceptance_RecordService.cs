
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Completion_Acceptance_RecordService
    {
        Task<WebResponseContent> GetFileList(Guid accId);
        Task<WebResponseContent> GetFileListByVersion(Guid accId, int version);
        Task<WebResponseContent> UploadFile(UploadAcceptanceRecordDto recordDto, IFormFile file = null);
        Task<WebResponseContent> UploadImg(UploadAcceptanceRecordExDto exDto, IFormFile file = null);
        Task<WebResponseContent> DeleteFile(Guid id);
        Task<WebResponseContent> UpdateCheckList(Guid id, string jsonData);
        Task<WebResponseContent> AuditData(Guid id, int status);
        Task<WebResponseContent> DownloadSysFile(Guid guidFileId);
        Task<WebResponseContent> SubmitFile(Guid subId);
    }
 }
