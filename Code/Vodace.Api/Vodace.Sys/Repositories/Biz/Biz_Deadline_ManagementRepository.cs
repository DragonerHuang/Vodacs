
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Deadline_ManagementRepository : RepositoryBase<Biz_Deadline_Management> , IBiz_Deadline_ManagementRepository
    {
    public Biz_Deadline_ManagementRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Deadline_ManagementRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Deadline_ManagementRepository>(); } }
    }
}
