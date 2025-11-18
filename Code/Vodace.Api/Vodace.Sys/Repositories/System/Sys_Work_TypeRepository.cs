
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Work_TypeRepository : RepositoryBase<Sys_Work_Type> , ISys_Work_TypeRepository
    {
    public Sys_Work_TypeRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Work_TypeRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Work_TypeRepository>(); } }
    }
}
