
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Site_Work_Record_Item_CheckRepository : RepositoryBase<Biz_Site_Work_Record_Item_Check> , IBiz_Site_Work_Record_Item_CheckRepository
    {
    public Biz_Site_Work_Record_Item_CheckRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Site_Work_Record_Item_CheckRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Site_Work_Record_Item_CheckRepository>(); } }
    }
}
