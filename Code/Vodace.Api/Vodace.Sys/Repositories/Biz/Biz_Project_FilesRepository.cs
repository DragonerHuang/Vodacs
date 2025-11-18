
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Biz_Project_FilesRepository : RepositoryBase<Biz_Project_Files> , IBiz_Project_FilesRepository
    {
    public Biz_Project_FilesRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static IBiz_Project_FilesRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBiz_Project_FilesRepository>(); } }
    }
}
