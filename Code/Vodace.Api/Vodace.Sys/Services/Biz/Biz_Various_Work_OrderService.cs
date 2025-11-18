using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_Various_Work_OrderService : ServiceBase<Biz_Various_Work_Order, IBiz_Various_Work_OrderRepository>
    , IBiz_Various_Work_OrderService, IDependency
    {
    public static IBiz_Various_Work_OrderService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Various_Work_OrderService>(); } }
    }
 }
