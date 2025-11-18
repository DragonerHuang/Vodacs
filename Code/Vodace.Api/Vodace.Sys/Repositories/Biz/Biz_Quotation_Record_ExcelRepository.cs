
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Quotation_Record_ExcelRepository : RepositoryBase<Biz_Quotation_Record_Excel> , IBiz_Quotation_Record_ExcelRepository
    {
    public Biz_Quotation_Record_ExcelRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Quotation_Record_ExcelRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Quotation_Record_ExcelRepository>(); } }
    }
}
