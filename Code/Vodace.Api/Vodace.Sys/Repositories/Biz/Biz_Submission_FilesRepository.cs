
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Submission_FilesRepository : RepositoryBase<Biz_Submission_Files> , IBiz_Submission_FilesRepository
    {
    public Biz_Submission_FilesRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Submission_FilesRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Submission_FilesRepository>(); } }
    }
}
