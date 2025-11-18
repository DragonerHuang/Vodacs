
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_InterviewService : ServiceBase<Biz_Quotation_Interview, IBiz_Quotation_InterviewRepository>
    , IBiz_Quotation_InterviewService, IDependency
    {
    public static IBiz_Quotation_InterviewService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Quotation_InterviewService>(); } }
    }
 }
