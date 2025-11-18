
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_User_InfoRepository : RepositoryBase<Sys_User_Info> , ISys_User_InfoRepository
    {
    public Sys_User_InfoRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_User_InfoRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_User_InfoRepository>(); } }
    }
}
