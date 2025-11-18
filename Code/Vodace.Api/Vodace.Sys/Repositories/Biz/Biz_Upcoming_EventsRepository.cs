
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Upcoming_EventsRepository : RepositoryBase<Biz_Upcoming_Events> , IBiz_Upcoming_EventsRepository
    {
    public Biz_Upcoming_EventsRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Upcoming_EventsRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Upcoming_EventsRepository>(); } }
    }
}
