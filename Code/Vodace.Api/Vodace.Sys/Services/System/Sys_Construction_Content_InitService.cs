
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Construction_Content_InitService : ServiceBase<Sys_Construction_Content_Init, ISys_Construction_Content_InitRepository>
    , ISys_Construction_Content_InitService, IDependency
    {
    public static ISys_Construction_Content_InitService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Construction_Content_InitService>(); } }
    }
 }
