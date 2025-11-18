
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_ContactService : ServiceBase<Sys_Contact, ISys_ContactRepository>
    , ISys_ContactService, IDependency
    {
    public static ISys_ContactService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_ContactService>(); } }
    }
 }
