using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Repositories.Biz
{
    public partial class Biz_Confirmed_OrderRepository : RepositoryBase<Biz_Confirmed_Order> , IBiz_Confirmed_OrderRepository
    {
    public Biz_Confirmed_OrderRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Confirmed_OrderRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Confirmed_OrderRepository>(); } }
    }
}
