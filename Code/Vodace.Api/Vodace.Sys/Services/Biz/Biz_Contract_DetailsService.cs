
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Contract_DetailsService : ServiceBase<Biz_Contract_Details, IBiz_Contract_DetailsRepository>
    , IBiz_Contract_DetailsService, IDependency
    {
    public static IBiz_Contract_DetailsService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Contract_DetailsService>(); } }
    }
 }
