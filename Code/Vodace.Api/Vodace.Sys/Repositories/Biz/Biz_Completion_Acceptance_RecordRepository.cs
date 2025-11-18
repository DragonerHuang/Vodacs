
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Completion_Acceptance_RecordRepository : RepositoryBase<Biz_Completion_Acceptance_Record> , IBiz_Completion_Acceptance_RecordRepository
    {
    public Biz_Completion_Acceptance_RecordRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Completion_Acceptance_RecordRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Completion_Acceptance_RecordRepository>(); } }
    }
}
