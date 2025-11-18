
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_ContractService
    {
        WebResponseContent Add(ContractDto m_ContractDto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(ContractDto m_ContractDto);
        //Task<WebResponseContent> GetContractList(PageDataOptions loadData);
        Task<WebResponseContent> GetContractList(PageInput<ContractSearchDto> searchDto);
        PageGridData<Biz_Contract> GetListByPage(PageDataOptions pageData);
        Task<WebResponseContent> GetContractById(Guid guid);

        Task<WebResponseContent> GetContractAllName();

        WebResponseContent SetContractMasterId(SetContractMasterDto m_ContractDto);

        Task<WebResponseContent> GetChildContractList(PageInput<ChildContractSearchDto> searchDto);
    }
 }
