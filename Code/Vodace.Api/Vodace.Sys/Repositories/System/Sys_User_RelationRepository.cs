
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_User_RelationRepository : RepositoryBase<Sys_User_Relation> , ISys_User_RelationRepository
    {
    public Sys_User_RelationRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_User_RelationRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_User_RelationRepository>(); } }
    }
}
