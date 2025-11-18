using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Repositories.Biz
{
    public partial class Biz_Various_Work_OrderRepository : RepositoryBase<Biz_Various_Work_Order> , IBiz_Various_Work_OrderRepository
    {
    public Biz_Various_Work_OrderRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Various_Work_OrderRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Various_Work_OrderRepository>(); } }
    }
}
