
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Quotation_DeadlineRepository : RepositoryBase<Biz_Quotation_Deadline> , IBiz_Quotation_DeadlineRepository
    {
    public Biz_Quotation_DeadlineRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Quotation_DeadlineRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Quotation_DeadlineRepository>(); } }
    }
}
