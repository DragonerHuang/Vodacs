
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Contact_RelationshipRepository : RepositoryBase<Biz_Contact_Relationship> , IBiz_Contact_RelationshipRepository
    {
    public Biz_Contact_RelationshipRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Contact_RelationshipRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Contact_RelationshipRepository>(); } }
    }
}
