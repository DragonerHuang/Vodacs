
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Leave_TypeRepository : RepositoryBase<Sys_Leave_Type> , ISys_Leave_TypeRepository
    {
    public Sys_Leave_TypeRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Leave_TypeRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Leave_TypeRepository>(); } }
    }
}
