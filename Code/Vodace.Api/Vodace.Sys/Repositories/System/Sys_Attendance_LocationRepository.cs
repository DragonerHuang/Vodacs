
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Attendance_LocationRepository : RepositoryBase<Sys_Attendance_Location> , ISys_Attendance_LocationRepository
    {
    public Sys_Attendance_LocationRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Attendance_LocationRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Attendance_LocationRepository>(); } }
    }
}
