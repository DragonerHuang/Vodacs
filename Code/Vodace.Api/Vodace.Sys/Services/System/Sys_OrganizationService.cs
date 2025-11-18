
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_OrganizationService : ServiceBase<Sys_Organization, ISys_OrganizationRepository>
    , ISys_OrganizationService, IDependency
    {
    public static ISys_OrganizationService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_OrganizationService>(); } }
    }
 }
