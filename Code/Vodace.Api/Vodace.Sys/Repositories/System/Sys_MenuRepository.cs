
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_MenuRepository : RepositoryBase<Sys_Menu> , ISys_MenuRepository
    {
    public Sys_MenuRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_MenuRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_MenuRepository>(); } }
    }
}
