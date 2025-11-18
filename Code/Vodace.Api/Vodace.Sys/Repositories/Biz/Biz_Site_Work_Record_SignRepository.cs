
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Site_Work_Record_SignRepository : RepositoryBase<Biz_Site_Work_Record_Sign> , IBiz_Site_Work_Record_SignRepository
    {
    public Biz_Site_Work_Record_SignRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Site_Work_Record_SignRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Site_Work_Record_SignRepository>(); } }
    }
}
