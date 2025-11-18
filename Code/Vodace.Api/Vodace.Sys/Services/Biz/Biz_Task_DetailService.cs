using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_Task_DetailService : ServiceBase<Biz_Task_Detail, IBiz_Task_DetailRepository>
    , IBiz_Task_DetailService, IDependency
    {
    public static IBiz_Task_DetailService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Task_DetailService>(); } }
    }
 }
