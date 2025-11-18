
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_TaskRepository : RepositoryBase<Biz_Task> , IBiz_TaskRepository
    {
    public Biz_TaskRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_TaskRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_TaskRepository>(); } }
    }
}
