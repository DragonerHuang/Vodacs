
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Attendance_LocationService : ServiceBase<Sys_Attendance_Location, ISys_Attendance_LocationRepository>
    , ISys_Attendance_LocationService, IDependency
    {
    public static ISys_Attendance_LocationService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Attendance_LocationService>(); } }
    }
 }
