
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Site_RelationshipRepository : RepositoryBase<Biz_Site_Relationship> , IBiz_Site_RelationshipRepository
    {
    public Biz_Site_RelationshipRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Site_RelationshipRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Site_RelationshipRepository>(); } }
    }
}
