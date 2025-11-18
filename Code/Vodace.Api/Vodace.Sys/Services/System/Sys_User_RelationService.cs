
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_User_RelationService : ServiceBase<Sys_User_Relation, ISys_User_RelationRepository>
    , ISys_User_RelationService, IDependency
    {
    public static ISys_User_RelationService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_User_RelationService>(); } }
    }
 }
