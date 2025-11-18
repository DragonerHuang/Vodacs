
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Quotation_InterviewRepository : RepositoryBase<Biz_Quotation_Interview> , IBiz_Quotation_InterviewRepository
    {
    public Biz_Quotation_InterviewRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Quotation_InterviewRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Quotation_InterviewRepository>(); } }
    }
}
