
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Leave_HolidayRepository : RepositoryBase<Sys_Leave_Holiday> , ISys_Leave_HolidayRepository
    {
    public Sys_Leave_HolidayRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Leave_HolidayRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Leave_HolidayRepository>(); } }
    }
}
