using Vodace.Sys.IRepositories;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.EFDbContext;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Repositories
{
    public partial class Sys_MenuRepository
    {
        public override VOLContext DbContext => base.DbContext;
    }
}

