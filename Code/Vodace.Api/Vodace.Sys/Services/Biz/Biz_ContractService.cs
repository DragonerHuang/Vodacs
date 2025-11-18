using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_ContractService : ServiceBase<Biz_Contract, IBiz_ContractRepository>
    , IBiz_ContractService, IDependency
    {
    public static IBiz_ContractService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_ContractService>(); } }
    }
 }
