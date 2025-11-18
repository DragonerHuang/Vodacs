
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_OrganizationService
    {
        WebResponseContent Add(OrganizationDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(OrganizationEditDto dto);
        WebResponseContent Enable(OrganizationEnableDto dto);
        Task<WebResponseContent> GetOrganizationList(PageInput<OrganizationEditDto> dto);
    }
}
