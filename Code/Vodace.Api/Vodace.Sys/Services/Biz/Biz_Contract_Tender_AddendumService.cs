
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Contract_Tender_AddendumService : ServiceBase<Biz_Contract_Tender_Addendum, IBiz_Contract_Tender_AddendumRepository>
    , IBiz_Contract_Tender_AddendumService, IDependency
    {
    public static IBiz_Contract_Tender_AddendumService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Contract_Tender_AddendumService>(); } }
    }
 }
