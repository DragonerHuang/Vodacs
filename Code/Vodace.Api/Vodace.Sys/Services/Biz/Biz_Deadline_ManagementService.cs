
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Deadline_ManagementService : ServiceBase<Biz_Deadline_Management, IBiz_Deadline_ManagementRepository>
    , IBiz_Deadline_ManagementService, IDependency
    {
    public static IBiz_Deadline_ManagementService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Deadline_ManagementService>(); } }
    }
 }
