
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Contact_RelationshipService
    {
        WebResponseContent Add(ContactRelationshipAddDto dto);
        WebResponseContent Del(Guid guid);
        Task<WebResponseContent> GetContactRelationshipList(PageInput<ContactRelationshipDto> searchDto);
        Task<WebResponseContent> GetContactRelationshipOtherList(ContactInfoDto dto);
        Task<WebResponseContent> GetContactInfoByContactId(ContactInfoDto dto);
        WebResponseContent GetContactRelationUserList(ContactRelationSearchAllDto dto);
    }
 }
