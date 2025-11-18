
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Task_GroupService : ServiceBase<Biz_Task_Group, IBiz_Task_GroupRepository>
    , IBiz_Task_GroupService, IDependency
    {
    public static IBiz_Task_GroupService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Task_GroupService>(); } }
    }
 }
