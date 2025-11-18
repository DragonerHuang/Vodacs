using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_Confirmed_OrderService : ServiceBase<Biz_Confirmed_Order, IBiz_Confirmed_OrderRepository>
    , IBiz_Confirmed_OrderService, IDependency
    {
    public static IBiz_Confirmed_OrderService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Confirmed_OrderService>(); } }
    }
 }
