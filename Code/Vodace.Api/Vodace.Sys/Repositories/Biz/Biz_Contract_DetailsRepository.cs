
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Contract_DetailsRepository : RepositoryBase<Biz_Contract_Details> , IBiz_Contract_DetailsRepository
    {
    public Biz_Contract_DetailsRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Contract_DetailsRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Contract_DetailsRepository>(); } }
    }
}
