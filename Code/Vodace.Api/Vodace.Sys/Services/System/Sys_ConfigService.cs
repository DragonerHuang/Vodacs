
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_ConfigService : ServiceBase<Sys_Config, ISys_ConfigRepository>
    , ISys_ConfigService, IDependency
    {
    public static ISys_ConfigService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_ConfigService>(); } }
    }
 }
