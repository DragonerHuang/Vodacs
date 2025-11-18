
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Rolling_Program_TaskRepository : RepositoryBase<Biz_Rolling_Program_Task> , IBiz_Rolling_Program_TaskRepository
    {
    public Biz_Rolling_Program_TaskRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Rolling_Program_TaskRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Rolling_Program_TaskRepository>(); } }
    }
}
