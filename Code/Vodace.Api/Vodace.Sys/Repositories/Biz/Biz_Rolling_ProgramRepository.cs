
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Rolling_ProgramRepository : RepositoryBase<Biz_Rolling_Program> , IBiz_Rolling_ProgramRepository
    {
    public Biz_Rolling_ProgramRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Rolling_ProgramRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Rolling_ProgramRepository>(); } }
    }
}
