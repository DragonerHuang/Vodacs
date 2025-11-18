
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Task_GroupRepository : RepositoryBase<Biz_Task_Group> , IBiz_Task_GroupRepository
    {
    public Biz_Task_GroupRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Task_GroupRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Task_GroupRepository>(); } }
    }
}
