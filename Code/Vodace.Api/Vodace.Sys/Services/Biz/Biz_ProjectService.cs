
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_ProjectService : ServiceBase<Biz_Project, IBiz_ProjectRepository>
    , IBiz_ProjectService, IDependency
    {
    public static IBiz_ProjectService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_ProjectService>(); } }
    }
 }
