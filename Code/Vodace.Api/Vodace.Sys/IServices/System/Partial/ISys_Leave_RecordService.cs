
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
    public partial interface ISys_Leave_RecordService
    {
        WebResponseContent Add(LeaveRecordDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(LeaveRecordEditDto dto);
        WebResponseContent Review(LeaveRecordReviewDto dto);
        WebResponseContent GetLeaveRecordStatus();
        Task<WebResponseContent> GetLeaveRecordType();
        Task<WebResponseContent> GetLeaveRecordList(PageInput<SearchLeaveRecordDto> dto);
    }
 }
