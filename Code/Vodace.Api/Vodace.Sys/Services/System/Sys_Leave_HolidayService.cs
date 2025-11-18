
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Leave_HolidayService : ServiceBase<Sys_Leave_Holiday, ISys_Leave_HolidayRepository>
    , ISys_Leave_HolidayService, IDependency
    {
    public static ISys_Leave_HolidayService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Leave_HolidayService>(); } }
    }
 }
