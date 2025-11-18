
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Upcoming_EventsService : ServiceBase<Biz_Upcoming_Events, IBiz_Upcoming_EventsRepository>
    , IBiz_Upcoming_EventsService, IDependency
    {
    public static IBiz_Upcoming_EventsService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Upcoming_EventsService>(); } }
    }
 }
