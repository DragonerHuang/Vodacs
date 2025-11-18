
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_User_RegisterService : ServiceBase<Sys_User_Register, ISys_User_RegisterRepository>
    , ISys_User_RegisterService, IDependency
    {
    public static ISys_User_RegisterService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_User_RegisterService>(); } }
    }
 }
