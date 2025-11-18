
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_File_RecordsService : ServiceBase<Sys_File_Records, ISys_File_RecordsRepository>
    , ISys_File_RecordsService, IDependency
    {
    public static ISys_File_RecordsService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_File_RecordsService>(); } }
    }
 }
