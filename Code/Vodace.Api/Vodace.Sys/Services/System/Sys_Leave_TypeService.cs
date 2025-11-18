
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Leave_TypeService : ServiceBase<Sys_Leave_Type, ISys_Leave_TypeRepository>
    , ISys_Leave_TypeService, IDependency
    {
    public static ISys_Leave_TypeService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Leave_TypeService>(); } }
    }
 }
