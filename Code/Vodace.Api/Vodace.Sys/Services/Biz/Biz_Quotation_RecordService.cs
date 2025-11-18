
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_RecordService : ServiceBase<Biz_Quotation_Record, IBiz_Quotation_RecordRepository>
    , IBiz_Quotation_RecordService, IDependency
    {
    public static IBiz_Quotation_RecordService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Quotation_RecordService>(); } }
    }
 }
