
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_ContractOrgRepository : RepositoryBase<Biz_ContractOrg> , IBiz_ContractOrgRepository
    {
    public Biz_ContractOrgRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_ContractOrgRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_ContractOrgRepository>(); } }
    }
}
