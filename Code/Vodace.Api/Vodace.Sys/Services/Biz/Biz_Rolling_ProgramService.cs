
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Rolling_ProgramService : ServiceBase<Biz_Rolling_Program, IBiz_Rolling_ProgramRepository>
    , IBiz_Rolling_ProgramService, IDependency
    {
    public static IBiz_Rolling_ProgramService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Rolling_ProgramService>(); } }
    }
 }
