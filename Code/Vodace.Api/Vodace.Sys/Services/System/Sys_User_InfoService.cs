
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_User_InfoService : ServiceBase<Sys_User_Info, ISys_User_InfoRepository>
    , ISys_User_InfoService, IDependency
    {
    public static ISys_User_InfoService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_User_InfoService>(); } }
    }
 }
