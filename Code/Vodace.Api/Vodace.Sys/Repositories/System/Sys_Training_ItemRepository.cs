
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Training_ItemRepository : RepositoryBase<Sys_Training_Item> , ISys_Training_ItemRepository
    {
    public Sys_Training_ItemRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Training_ItemRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Training_ItemRepository>(); } }
    }
}
