
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_LineRepository : RepositoryBase<Biz_Line> , IBiz_LineRepository
    {
    public Biz_LineRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_LineRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_LineRepository>(); } }
    }
}
