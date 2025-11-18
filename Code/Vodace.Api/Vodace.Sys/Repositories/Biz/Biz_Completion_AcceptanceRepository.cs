
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Completion_AcceptanceRepository : RepositoryBase<Biz_Completion_Acceptance> , IBiz_Completion_AcceptanceRepository
    {
    public Biz_Completion_AcceptanceRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Completion_AcceptanceRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Completion_AcceptanceRepository>(); } }
    }
}
