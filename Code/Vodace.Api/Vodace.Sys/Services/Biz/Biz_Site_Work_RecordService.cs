
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Site_Work_RecordService : ServiceBase<Biz_Site_Work_Record, IBiz_Site_Work_RecordRepository>
    , IBiz_Site_Work_RecordService, IDependency
    {
    public static IBiz_Site_Work_RecordService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Site_Work_RecordService>(); } }
    }
 }
