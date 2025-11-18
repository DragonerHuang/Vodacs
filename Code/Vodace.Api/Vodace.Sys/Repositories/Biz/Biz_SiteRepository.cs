
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_SiteRepository : RepositoryBase<Biz_Site> , IBiz_SiteRepository
    {
    public Biz_SiteRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_SiteRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_SiteRepository>(); } }
    }
}
