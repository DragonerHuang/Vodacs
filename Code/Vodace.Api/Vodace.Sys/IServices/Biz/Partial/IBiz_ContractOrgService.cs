
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
    public partial interface IBiz_ContractOrgService
    {
        WebResponseContent Add(ContractOrgDto dto);
        WebResponseContent AddBatch(List<ContractOrgDto> dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(ContractOrgDto dto);
        Task<WebResponseContent> GetContractOrgList(PageInput<ContractOrgSearchDto> dto);
    }
 }
