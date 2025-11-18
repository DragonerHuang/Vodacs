
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Worker_RegisterService : ServiceBase<Sys_Worker_Register, ISys_Worker_RegisterRepository>
    , ISys_Worker_RegisterService, IDependency
    {
    public static ISys_Worker_RegisterService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Worker_RegisterService>(); } }
    }
 }
