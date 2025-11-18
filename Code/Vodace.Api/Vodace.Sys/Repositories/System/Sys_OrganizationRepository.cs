
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_OrganizationRepository : RepositoryBase<Sys_Organization> , ISys_OrganizationRepository
    {
    public Sys_OrganizationRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_OrganizationRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_OrganizationRepository>(); } }
    }
}
