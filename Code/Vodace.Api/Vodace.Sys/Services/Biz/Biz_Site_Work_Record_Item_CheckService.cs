
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Site_Work_Record_Item_CheckService : ServiceBase<Biz_Site_Work_Record_Item_Check, IBiz_Site_Work_Record_Item_CheckRepository>
    , IBiz_Site_Work_Record_Item_CheckService, IDependency
    {
    public static IBiz_Site_Work_Record_Item_CheckService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Site_Work_Record_Item_CheckService>(); } }
    }
 }
