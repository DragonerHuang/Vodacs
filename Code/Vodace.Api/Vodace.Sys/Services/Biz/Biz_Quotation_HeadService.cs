
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_HeadService : ServiceBase<Biz_Quotation_Head, IBiz_Quotation_HeadRepository>
    , IBiz_Quotation_HeadService, IDependency
    {
    public static IBiz_Quotation_HeadService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Quotation_HeadService>(); } }
    }
 }
