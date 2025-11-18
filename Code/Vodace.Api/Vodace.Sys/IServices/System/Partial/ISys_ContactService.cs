
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_ContactService
    {
        Task<WebResponseContent> GetListByPage(PageInput<ContactQuery> query);
        WebResponseContent Add(ContactDetailDto dtoContactDetail);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(ContactEditlDto dtoContactDetail);
        Task<PageGridData<ContactListDto>> GetContactList(PageInput<SearchContactDto> dtoContactSearchInput);
        Task<WebResponseContent> GetContactAllName();
        Task<WebResponseContent> GetContactById(Guid guid);


        /// <summary>
        /// 获取联系人下拉列表
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> GetContactWithCardDataAsync(PageInput<SearchContactWithCardDto> searchInput);
    }
 }
