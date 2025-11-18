
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Quotation_SiteRepository : RepositoryBase<Biz_Quotation_Site> , IBiz_Quotation_SiteRepository
    {
    public Biz_Quotation_SiteRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Quotation_SiteRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Quotation_SiteRepository>(); } }
    }
}
