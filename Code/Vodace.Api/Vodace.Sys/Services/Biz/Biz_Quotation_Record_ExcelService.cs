
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_Record_ExcelService : ServiceBase<Biz_Quotation_Record_Excel, IBiz_Quotation_Record_ExcelRepository>
    , IBiz_Quotation_Record_ExcelService, IDependency
    {
    public static IBiz_Quotation_Record_ExcelService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Quotation_Record_ExcelService>(); } }
    }
 }
