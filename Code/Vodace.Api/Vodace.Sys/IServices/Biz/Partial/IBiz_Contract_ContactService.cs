
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Contract_ContactService
    {
        /// <summary>
        /// 保存合同联系人
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="lstContact"></param>
        /// <returns></returns>
        Task<WebResponseContent> SaveContactsAsync(Guid contractId, List<ContactQnDto> lstContact);

        /// <summary>
        /// 获取合同的联系人
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        Task<List<ContactQnDto>> GetContactsAsync(Guid contractId);
    }
 }
