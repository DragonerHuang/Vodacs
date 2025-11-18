
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_TaskService : ServiceBase<Biz_Task, IBiz_TaskRepository>
    , IBiz_TaskService, IDependency
    {
    public static IBiz_TaskService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_TaskService>(); } }
    }
 }
