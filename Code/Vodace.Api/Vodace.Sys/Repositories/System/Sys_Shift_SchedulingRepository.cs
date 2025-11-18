
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Shift_SchedulingRepository : RepositoryBase<Sys_Shift_Scheduling> , ISys_Shift_SchedulingRepository
    {
    public Sys_Shift_SchedulingRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Shift_SchedulingRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Shift_SchedulingRepository>(); } }
    }
}
