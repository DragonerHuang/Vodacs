
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Contact_RelationshipService : ServiceBase<Biz_Contact_Relationship, IBiz_Contact_RelationshipRepository>
    , IBiz_Contact_RelationshipService, IDependency
    {
    public static IBiz_Contact_RelationshipService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Contact_RelationshipService>(); } }
    }
 }
