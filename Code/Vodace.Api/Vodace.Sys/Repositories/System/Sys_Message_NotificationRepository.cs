
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Message_NotificationRepository : RepositoryBase<Sys_Message_Notification> , ISys_Message_NotificationRepository
    {
    public Sys_Message_NotificationRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Message_NotificationRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Message_NotificationRepository>(); } }
    }
}
