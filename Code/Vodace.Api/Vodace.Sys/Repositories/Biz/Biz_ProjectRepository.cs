
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_ProjectRepository : RepositoryBase<Biz_Project> , IBiz_ProjectRepository
    {
    public Biz_ProjectRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_ProjectRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_ProjectRepository>(); } }
    }
}
