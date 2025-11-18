using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Repositories.Biz
{
    public partial class Biz_ContractRepository : RepositoryBase<Biz_Contract> , IBiz_ContractRepository
    {
    public Biz_ContractRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_ContractRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_ContractRepository>(); } }
    }
}
