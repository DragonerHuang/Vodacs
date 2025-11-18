
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Site_Work_RecordRepository : RepositoryBase<Biz_Site_Work_Record> , IBiz_Site_Work_RecordRepository
    {
    public Biz_Site_Work_RecordRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Site_Work_RecordRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Site_Work_RecordRepository>(); } }
    }
}
