
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Task_Group_RelationshipRepository : RepositoryBase<Biz_Task_Group_Relationship> , IBiz_Task_Group_RelationshipRepository
    {
    public Biz_Task_Group_RelationshipRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Task_Group_RelationshipRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Task_Group_RelationshipRepository>(); } }
    }
}
