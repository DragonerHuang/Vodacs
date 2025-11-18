
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_CompanyService : ServiceBase<Sys_Company, ISys_CompanyRepository>
    , ISys_CompanyService, IDependency
    {
    public static ISys_CompanyService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_CompanyService>(); } }
    }
 }
