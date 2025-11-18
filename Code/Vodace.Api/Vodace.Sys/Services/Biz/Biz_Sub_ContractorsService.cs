using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_Sub_ContractorsService : ServiceBase<Biz_Sub_Contractors, IBiz_Sub_ContractorsRepository>
    , IBiz_Sub_ContractorsService, IDependency
    {
    public static IBiz_Sub_ContractorsService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Sub_ContractorsService>(); } }
    }
 }
