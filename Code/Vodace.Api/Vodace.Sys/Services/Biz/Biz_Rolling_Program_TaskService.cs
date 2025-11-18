
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Rolling_Program_TaskService : ServiceBase<Biz_Rolling_Program_Task, IBiz_Rolling_Program_TaskRepository>
    , IBiz_Rolling_Program_TaskService, IDependency
    {
    public static IBiz_Rolling_Program_TaskService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Rolling_Program_TaskService>(); } }
    }
 }
