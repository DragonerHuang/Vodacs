
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Project_FilesService : ServiceBase<Biz_Project_Files, IBiz_Project_FilesRepository>
    , IBiz_Project_FilesService, IDependency
    {
    public static IBiz_Project_FilesService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Project_FilesService>(); } }
    }
 }
