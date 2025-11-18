
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Attendance_RecordRepository : RepositoryBase<Sys_Attendance_Record> , ISys_Attendance_RecordRepository
    {
    public Sys_Attendance_RecordRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Attendance_RecordRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Attendance_RecordRepository>(); } }
    }
}
