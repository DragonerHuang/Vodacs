
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Site_Work_Record_WorkerService : ServiceBase<Biz_Site_Work_Record_Worker, IBiz_Site_Work_Record_WorkerRepository>
    , IBiz_Site_Work_Record_WorkerService, IDependency
    {
    public static IBiz_Site_Work_Record_WorkerService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Site_Work_Record_WorkerService>(); } }
    }
 }
