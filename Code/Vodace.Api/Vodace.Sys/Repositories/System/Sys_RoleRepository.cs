
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_RoleRepository : RepositoryBase<Sys_Role> , ISys_RoleRepository
    {
    public Sys_RoleRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_RoleRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_RoleRepository>(); } }
    }
}
