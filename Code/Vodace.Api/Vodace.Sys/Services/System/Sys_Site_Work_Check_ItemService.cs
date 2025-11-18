
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Site_Work_Check_ItemService : ServiceBase<Sys_Site_Work_Check_Item, ISys_Site_Work_Check_ItemRepository>
    , ISys_Site_Work_Check_ItemService, IDependency
    {
    public static ISys_Site_Work_Check_ItemService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Site_Work_Check_ItemService>(); } }
    }
 }
