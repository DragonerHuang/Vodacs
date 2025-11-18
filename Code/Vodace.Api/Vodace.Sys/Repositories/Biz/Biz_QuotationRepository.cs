using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Repositories.Biz
{
    public partial class Biz_QuotationRepository : RepositoryBase<Biz_Quotation> , IBiz_QuotationRepository
    {
    public Biz_QuotationRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_QuotationRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_QuotationRepository>(); } }
    }
}
