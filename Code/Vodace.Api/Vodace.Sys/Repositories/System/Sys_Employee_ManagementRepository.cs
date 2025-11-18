
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Employee_ManagementRepository : RepositoryBase<Sys_Employee_Management> , ISys_Employee_ManagementRepository
    {
    public Sys_Employee_ManagementRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Employee_ManagementRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Employee_ManagementRepository>(); } }
    }
}
