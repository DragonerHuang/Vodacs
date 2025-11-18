
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Contract_ContactRepository : RepositoryBase<Biz_Contract_Contact> , IBiz_Contract_ContactRepository
    {
    public Biz_Contract_ContactRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Contract_ContactRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Contract_ContactRepository>(); } }
    }
}
