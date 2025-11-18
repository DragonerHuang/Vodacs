
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
    public partial interface ISys_Work_TypeService
    {
        Task<WebResponseContent> GetDataTree();

        WebResponseContent Add(WorkTypeDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(WorkTypeDto dto);
        Task<WebResponseContent> GetWorkTypeList(PageInput<WorkTypeDto> dto);
        Task<WebResponseContent> GetWorkTypeAllName();
    }
 }
