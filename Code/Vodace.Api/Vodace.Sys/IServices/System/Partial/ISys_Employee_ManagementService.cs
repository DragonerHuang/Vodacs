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
    public partial interface ISys_Employee_ManagementService
    {
        Task<WebResponseContent> GetListByPageAsync(PageInput<SysEmpMentQueryDto> query);

        Task<WebResponseContent> CreateOrUpdateAsync(SysEmpMentCreateOrUpdateDto dtoContactDetail);

        Task<WebResponseContent> DelAsync(List<Guid> guid);

        Task<WebResponseContent> GetForEditAsync(Guid guid);
    }
}