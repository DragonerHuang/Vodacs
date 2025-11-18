
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Quotation_HeadRepository : RepositoryBase<Biz_Quotation_Head> , IBiz_Quotation_HeadRepository
    {
    public Biz_Quotation_HeadRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Quotation_HeadRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Quotation_HeadRepository>(); } }
    }
}
