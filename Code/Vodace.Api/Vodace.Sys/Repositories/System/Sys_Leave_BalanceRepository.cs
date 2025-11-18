
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Leave_BalanceRepository : RepositoryBase<Sys_Leave_Balance> , ISys_Leave_BalanceRepository
    {
    public Sys_Leave_BalanceRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Leave_BalanceRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Leave_BalanceRepository>(); } }
    }
}
