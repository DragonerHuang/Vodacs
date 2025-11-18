
using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_Leave_AttachmentRepository : RepositoryBase<Sys_Leave_Attachment> , ISys_Leave_AttachmentRepository
    {
    public Sys_Leave_AttachmentRepository(VOLContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_Leave_AttachmentRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_Leave_AttachmentRepository>(); } }
    }
}
