
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_Leave_BalanceService
    {
        WebResponseContent Add(LeaveBalanceDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(LeaveBalanceEditDto dto);
        Task<WebResponseContent> GetLeaveBalanceList(PageInput<LeaveBalanceEditDto> dto);
    }
 }
