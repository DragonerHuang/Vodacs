
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Leave_RecordRepository : RepositoryBase<Sys_Leave_Record> , ISys_Leave_RecordRepository
    {
    public Sys_Leave_RecordRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Leave_RecordRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Leave_RecordRepository>(); } }
    }
}
