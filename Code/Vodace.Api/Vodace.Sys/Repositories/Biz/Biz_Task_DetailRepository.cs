using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Repositories.Biz
{
    public partial class Biz_Task_DetailRepository : RepositoryBase<Biz_Task_Detail> , IBiz_Task_DetailRepository
    {
    public Biz_Task_DetailRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Task_DetailRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Task_DetailRepository>(); } }
    }
}
