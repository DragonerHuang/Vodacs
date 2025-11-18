
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_File_ConfigService : ServiceBase<Sys_File_Config, ISys_File_ConfigRepository>
    , ISys_File_ConfigService, IDependency
    {
    public static ISys_File_ConfigService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_File_ConfigService>(); } }
    }
 }
