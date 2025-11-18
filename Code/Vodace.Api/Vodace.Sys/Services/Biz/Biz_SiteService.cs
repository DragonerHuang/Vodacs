
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_SiteService : ServiceBase<Biz_Site, IBiz_SiteRepository>
    , IBiz_SiteService, IDependency
    {
    public static IBiz_SiteService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_SiteService>(); } }
    }
 }
