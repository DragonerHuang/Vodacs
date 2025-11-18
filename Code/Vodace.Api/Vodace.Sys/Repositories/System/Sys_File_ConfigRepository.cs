
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_File_ConfigRepository : RepositoryBase<Sys_File_Config> , ISys_File_ConfigRepository
    {
    public Sys_File_ConfigRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_File_ConfigRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_File_ConfigRepository>(); } }
    }
}
