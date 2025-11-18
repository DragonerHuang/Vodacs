
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Site_Work_Check_ItemRepository : RepositoryBase<Sys_Site_Work_Check_Item> , ISys_Site_Work_Check_ItemRepository
    {
    public Sys_Site_Work_Check_ItemRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Site_Work_Check_ItemRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Site_Work_Check_ItemRepository>(); } }
    }
}
