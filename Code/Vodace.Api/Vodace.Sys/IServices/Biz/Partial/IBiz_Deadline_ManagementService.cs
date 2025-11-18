
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
    public partial interface IBiz_Deadline_ManagementService
    {
        Task<WebResponseContent> GetListByPage(PageInput<SubmissionFilesQuery> query);
        Task<WebResponseContent> GetDataById(Guid id);
        Task<WebResponseContent> AddData(DeadlineManagementAddDto addDto);
        Task<WebResponseContent> EditData(DeadlineManagementEditDto editDto);
        Task<WebResponseContent> DeleteData(Guid id);
        Task<WebResponseContent> AuditByBatch(SubmissionAuditDto dto);
        //Task<WebResponseContent> UploadFile(UploadDto recordDto, IFormFile file = null);
        Task<WebResponseContent> DownloadSysFile(Guid fileId);
        Task<WebResponseContent> DeleteFile(Guid fileId);
        Task<WebResponseContent> UploadFiles(int type, List<IFormFile> lstFiles);
    }
 }
