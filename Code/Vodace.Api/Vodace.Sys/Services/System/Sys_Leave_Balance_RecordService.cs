
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Leave_Balance_RecordService : ServiceBase<Sys_Leave_Balance_Record, ISys_Leave_Balance_RecordRepository>
    , ISys_Leave_Balance_RecordService, IDependency
    {
    public static ISys_Leave_Balance_RecordService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Leave_Balance_RecordService>(); } }
    }
 }
