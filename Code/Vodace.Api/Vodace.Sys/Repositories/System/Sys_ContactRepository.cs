
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_ContactRepository : RepositoryBase<Sys_Contact> , ISys_ContactRepository
    {
    public Sys_ContactRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_ContactRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_ContactRepository>(); } }
    }
}
