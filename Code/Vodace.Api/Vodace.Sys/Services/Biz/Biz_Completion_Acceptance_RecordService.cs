
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Completion_Acceptance_RecordService : ServiceBase<Biz_Completion_Acceptance_Record, IBiz_Completion_Acceptance_RecordRepository>
    , IBiz_Completion_Acceptance_RecordService, IDependency
    {
    public static IBiz_Completion_Acceptance_RecordService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Completion_Acceptance_RecordService>(); } }
    }
 }
