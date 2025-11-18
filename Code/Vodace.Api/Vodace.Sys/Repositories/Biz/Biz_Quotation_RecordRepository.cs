
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Quotation_RecordRepository : RepositoryBase<Biz_Quotation_Record> , IBiz_Quotation_RecordRepository
    {
    public Biz_Quotation_RecordRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Quotation_RecordRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Quotation_RecordRepository>(); } }
    }
}
