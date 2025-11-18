
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Contract_Tender_AddendumRepository : RepositoryBase<Biz_Contract_Tender_Addendum> , IBiz_Contract_Tender_AddendumRepository
    {
    public Biz_Contract_Tender_AddendumRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Contract_Tender_AddendumRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Contract_Tender_AddendumRepository>(); } }
    }
}
