
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_DeadlineService : ServiceBase<Biz_Quotation_Deadline, IBiz_Quotation_DeadlineRepository>
    , IBiz_Quotation_DeadlineService, IDependency
    {
    public static IBiz_Quotation_DeadlineService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Quotation_DeadlineService>(); } }
    }
 }
