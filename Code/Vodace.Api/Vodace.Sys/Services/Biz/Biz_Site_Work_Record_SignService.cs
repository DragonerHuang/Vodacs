
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Site_Work_Record_SignService : ServiceBase<Biz_Site_Work_Record_Sign, IBiz_Site_Work_Record_SignRepository>
    , IBiz_Site_Work_Record_SignService, IDependency
    {
    public static IBiz_Site_Work_Record_SignService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Site_Work_Record_SignService>(); } }
    }
 }
