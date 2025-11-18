
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_CompanyRepository : RepositoryBase<Sys_Company> , ISys_CompanyRepository
    {
    public Sys_CompanyRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_CompanyRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_CompanyRepository>(); } }
    }
}
