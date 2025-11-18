
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Message_NotificationService : ServiceBase<Sys_Message_Notification, ISys_Message_NotificationRepository>
    , ISys_Message_NotificationService, IDependency
    {
    public static ISys_Message_NotificationService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Message_NotificationService>(); } }
    }
 }
