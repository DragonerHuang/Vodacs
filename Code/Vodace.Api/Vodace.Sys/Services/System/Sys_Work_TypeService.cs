
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Work_TypeService : ServiceBase<Sys_Work_Type, ISys_Work_TypeRepository>
    , ISys_Work_TypeService, IDependency
    {
    public static ISys_Work_TypeService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Work_TypeService>(); } }
    }
 }
