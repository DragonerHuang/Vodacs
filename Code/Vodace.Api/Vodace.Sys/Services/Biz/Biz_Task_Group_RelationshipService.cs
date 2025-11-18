
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Task_Group_RelationshipService : ServiceBase<Biz_Task_Group_Relationship, IBiz_Task_Group_RelationshipRepository>
    , IBiz_Task_Group_RelationshipService, IDependency
    {
    public static IBiz_Task_Group_RelationshipService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Task_Group_RelationshipService>(); } }
    }
 }
