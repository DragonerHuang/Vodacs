using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_Task_Detail_Work_TypeService : ServiceBase<Biz_Task_Detail_Work_Type, IBiz_Task_Detail_Work_TypeRepository>
    , IBiz_Task_Detail_Work_TypeService, IDependency
    {
    public static IBiz_Task_Detail_Work_TypeService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Task_Detail_Work_TypeService>(); } }
    }
 }
