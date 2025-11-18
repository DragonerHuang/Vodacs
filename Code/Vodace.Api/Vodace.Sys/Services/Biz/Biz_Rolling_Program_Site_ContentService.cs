
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Rolling_Program_Site_ContentService : ServiceBase<Biz_Rolling_Program_Site_Content, IBiz_Rolling_Program_Site_ContentRepository>
    , IBiz_Rolling_Program_Site_ContentService, IDependency
    {
    public static IBiz_Rolling_Program_Site_ContentService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Rolling_Program_Site_ContentService>(); } }
    }
 }
