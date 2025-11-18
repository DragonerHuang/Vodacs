
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Worker_RegisterRepository : RepositoryBase<Sys_Worker_Register> , ISys_Worker_RegisterRepository
    {
    public Sys_Worker_RegisterRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Worker_RegisterRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Worker_RegisterRepository>(); } }
    }
}
