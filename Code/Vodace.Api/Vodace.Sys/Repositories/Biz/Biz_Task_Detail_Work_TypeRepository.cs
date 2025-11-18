using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Repositories.Biz
{
    public partial class Biz_Task_Detail_Work_TypeRepository : RepositoryBase<Biz_Task_Detail_Work_Type> , IBiz_Task_Detail_Work_TypeRepository
    {
    public Biz_Task_Detail_Work_TypeRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Task_Detail_Work_TypeRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Task_Detail_Work_TypeRepository>(); } }
    }
}
