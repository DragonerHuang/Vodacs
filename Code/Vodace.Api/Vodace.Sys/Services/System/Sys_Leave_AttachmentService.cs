
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_Leave_AttachmentService : ServiceBase<Sys_Leave_Attachment, ISys_Leave_AttachmentRepository>
    , ISys_Leave_AttachmentService, IDependency
    {
    public static ISys_Leave_AttachmentService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_Leave_AttachmentService>(); } }
    }
 }
