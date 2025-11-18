
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Biz_Contract_ContactService : ServiceBase<Biz_Contract_Contact, IBiz_Contract_ContactRepository>
    , IBiz_Contract_ContactService, IDependency
    {
    public static IBiz_Contract_ContactService Instance
    {
      get { return AutofacContainerModule.GetService<IBiz_Contract_ContactService>(); } }
    }
 }
