
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_DictionaryListRepository : RepositoryBase<Sys_Dictionary_List> , ISys_DictionaryListRepository
    {
    public Sys_DictionaryListRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_DictionaryListRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_DictionaryListRepository>(); } }
    }
}
