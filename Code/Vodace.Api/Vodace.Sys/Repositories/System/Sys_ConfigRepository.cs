
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_ConfigRepository : RepositoryBase<Sys_Config> , ISys_ConfigRepository
    {
    public Sys_ConfigRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_ConfigRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_ConfigRepository>(); } }
    }
}
