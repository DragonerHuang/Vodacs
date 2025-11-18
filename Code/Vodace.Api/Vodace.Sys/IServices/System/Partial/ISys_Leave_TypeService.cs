
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_Leave_TypeService
    {
        Task<WebResponseContent> GetListByPage(PageInput<LeaveTypeQuery> query);
        Task<WebResponseContent> GetList();
        Task<WebResponseContent> Add(LeaveTypeAddDto dto);
        Task<WebResponseContent> Update(LeaveTypeEditDto dto);
        Task<WebResponseContent> DelData(Guid id);
    }
 }
