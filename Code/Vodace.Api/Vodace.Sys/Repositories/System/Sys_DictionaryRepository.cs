
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_DictionaryRepository : RepositoryBase<Sys_Dictionary> , ISys_DictionaryRepository
    {
    public Sys_DictionaryRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_DictionaryRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_DictionaryRepository>(); } }
    }
}
