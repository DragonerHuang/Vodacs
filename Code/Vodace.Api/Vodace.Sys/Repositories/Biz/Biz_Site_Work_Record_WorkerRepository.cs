
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Site_Work_Record_WorkerRepository : RepositoryBase<Biz_Site_Work_Record_Worker> , IBiz_Site_Work_Record_WorkerRepository
    {
    public Biz_Site_Work_Record_WorkerRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Site_Work_Record_WorkerRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Site_Work_Record_WorkerRepository>(); } }
    }
}
