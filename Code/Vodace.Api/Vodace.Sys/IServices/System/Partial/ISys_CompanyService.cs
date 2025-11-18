
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_CompanyService
    {
        Task<WebResponseContent> GetCompanyList();
        Task<WebResponseContent> Add(CompanyDto company);
        Task<WebResponseContent> Update(CompanyDto company);
        Task<WebResponseContent> Audit(CompanyAuditDto company);
        Task<WebResponseContent> DelData(Guid id);
        Task<WebResponseContent> GetListByPage(PageInput<CompanyQuery> query);
    }
 }
