using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_QuotationService : ServiceBase<Biz_Quotation, IBiz_QuotationRepository>
    , IBiz_QuotationService, IDependency
    {
    public static IBiz_QuotationService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_QuotationService>(); } }
    }
 }
