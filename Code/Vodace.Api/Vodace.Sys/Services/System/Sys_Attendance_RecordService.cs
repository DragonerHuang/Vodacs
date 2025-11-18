
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Attendance_RecordService : ServiceBase<Sys_Attendance_Record, ISys_Attendance_RecordRepository>
    , ISys_Attendance_RecordService, IDependency
    {
    public static ISys_Attendance_RecordService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Attendance_RecordService>(); } }
    }
 }
