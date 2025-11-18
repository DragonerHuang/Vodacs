
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_ContractOrgService : ServiceBase<Biz_ContractOrg, IBiz_ContractOrgRepository>
    , IBiz_ContractOrgService, IDependency
    {
    public static IBiz_ContractOrgService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_ContractOrgService>(); } }
    }
 }
