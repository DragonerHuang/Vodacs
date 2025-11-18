
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Completion_AcceptanceService : ServiceBase<Biz_Completion_Acceptance, IBiz_Completion_AcceptanceRepository>
    , IBiz_Completion_AcceptanceService, IDependency
    {
    public static IBiz_Completion_AcceptanceService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Completion_AcceptanceService>(); } }
    }
 }
