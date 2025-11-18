
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Site_RelationshipService : ServiceBase<Biz_Site_Relationship, IBiz_Site_RelationshipRepository>
    , IBiz_Site_RelationshipService, IDependency
    {
    public static IBiz_Site_RelationshipService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Site_RelationshipService>(); } }
    }
 }
