
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_SiteService : ServiceBase<Biz_Quotation_Site, IBiz_Quotation_SiteRepository>
    , IBiz_Quotation_SiteService, IDependency
    {
    public static IBiz_Quotation_SiteService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Quotation_SiteService>(); } }
    }
 }
