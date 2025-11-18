using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Repositories.Biz
{
    public partial class Biz_Sub_ContractorsRepository : RepositoryBase<Biz_Sub_Contractors> , IBiz_Sub_ContractorsRepository
    {
    public Biz_Sub_ContractorsRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Sub_ContractorsRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Sub_ContractorsRepository>(); } }
    }
}
