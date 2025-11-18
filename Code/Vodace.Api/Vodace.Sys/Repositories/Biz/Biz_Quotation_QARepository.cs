
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Quotation_QARepository : RepositoryBase<Biz_Quotation_QA> , IBiz_Quotation_QARepository
    {
    public Biz_Quotation_QARepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Quotation_QARepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Quotation_QARepository>(); } }
    }
}
