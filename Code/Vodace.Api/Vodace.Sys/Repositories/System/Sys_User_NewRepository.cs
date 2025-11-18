
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_User_newRepository : RepositoryBase<Sys_User_New> , ISys_User_NewRepository
    {
    public Sys_User_newRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_User_NewRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_User_NewRepository>(); } }
    }
}
