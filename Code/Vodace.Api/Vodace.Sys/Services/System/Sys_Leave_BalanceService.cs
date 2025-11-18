
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Leave_BalanceService : ServiceBase<Sys_Leave_Balance, ISys_Leave_BalanceRepository>
    , ISys_Leave_BalanceService, IDependency
    {
    public static ISys_Leave_BalanceService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Leave_BalanceService>(); } }
    }
 }
