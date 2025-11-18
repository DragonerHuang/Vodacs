
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_User_NewService : ServiceBase<Sys_User_New, ISys_User_NewRepository>
    , ISys_User_NewService, IDependency
    {
    public static ISys_User_NewService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_User_NewService>(); } }
    }
 }
