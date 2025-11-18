
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Submission_FilesService : ServiceBase<Biz_Submission_Files, IBiz_Submission_FilesRepository>
    , IBiz_Submission_FilesService, IDependency
    {
    public static IBiz_Submission_FilesService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Submission_FilesService>(); } }
    }
 }
