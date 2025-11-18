
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_QAService : ServiceBase<Biz_Quotation_QA, IBiz_Quotation_QARepository>
    , IBiz_Quotation_QAService, IDependency
    {
    public static IBiz_Quotation_QAService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Quotation_QAService>(); } }
    }
 }
