
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_LineService : ServiceBase<Biz_Line, IBiz_LineRepository>
    , IBiz_LineService, IDependency
    {
    public static IBiz_LineService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_LineService>(); } }
    }
 }
