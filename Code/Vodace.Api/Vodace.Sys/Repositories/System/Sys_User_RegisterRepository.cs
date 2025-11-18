
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_User_RegisterRepository : RepositoryBase<Sys_User_Register> , ISys_User_RegisterRepository
    {
    public Sys_User_RegisterRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_User_RegisterRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_User_RegisterRepository>(); } }
    }
}
