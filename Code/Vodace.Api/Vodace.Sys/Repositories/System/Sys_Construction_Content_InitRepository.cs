
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Construction_Content_InitRepository : RepositoryBase<Sys_Construction_Content_Init> , ISys_Construction_Content_InitRepository
    {
    public Sys_Construction_Content_InitRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Construction_Content_InitRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Construction_Content_InitRepository>(); } }
    }
}
