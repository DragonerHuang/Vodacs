
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Shift_SchedulingService : ServiceBase<Sys_Shift_Scheduling, ISys_Shift_SchedulingRepository>
    , ISys_Shift_SchedulingService, IDependency
    {
    public static ISys_Shift_SchedulingService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Shift_SchedulingService>(); } }
    }
 }
