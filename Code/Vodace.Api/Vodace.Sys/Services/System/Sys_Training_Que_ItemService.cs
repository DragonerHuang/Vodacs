
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Training_Que_ItemService : ServiceBase<Sys_Training_Que_Item, ISys_Training_Que_ItemRepository>
    , ISys_Training_Que_ItemService, IDependency
    {
    public static ISys_Training_Que_ItemService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Training_Que_ItemService>(); } }
    }
 }
