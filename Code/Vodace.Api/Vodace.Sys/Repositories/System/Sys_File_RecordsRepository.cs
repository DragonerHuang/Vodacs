
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_File_RecordsRepository : RepositoryBase<Sys_File_Records> , ISys_File_RecordsRepository
    {
    public Sys_File_RecordsRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_File_RecordsRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_File_RecordsRepository>(); } }
    }
}
