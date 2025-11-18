using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Employee_ManagementService : ServiceBase<Sys_Employee_Management, ISys_Employee_ManagementRepository>
    , ISys_Employee_ManagementService, IDependency
    {
        public static ISys_Employee_ManagementService Instance
        {
            get { return AutofacContainerModule.GetService<ISys_Employee_ManagementService>(); }
        }
    }
}