
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Rolling_Program_Site_ContentRepository : RepositoryBase<Biz_Rolling_Program_Site_Content> , IBiz_Rolling_Program_Site_ContentRepository
    {
    public Biz_Rolling_Program_Site_ContentRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Rolling_Program_Site_ContentRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Rolling_Program_Site_ContentRepository>(); } }
    }
}
