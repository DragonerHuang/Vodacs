
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Training_Que_ItemRepository : RepositoryBase<Sys_Training_Que_Item> , ISys_Training_Que_ItemRepository
    {
    public Sys_Training_Que_ItemRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Training_Que_ItemRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Training_Que_ItemRepository>(); } }
    }
}
