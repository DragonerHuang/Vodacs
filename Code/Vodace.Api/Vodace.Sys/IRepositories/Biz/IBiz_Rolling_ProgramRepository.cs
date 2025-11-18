
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Entity.DomainModels;
using Vodace.Core.Extensions.AutofacManager;
namespace Vodace.Sys.IRepositories
{
    public partial interface IBiz_Rolling_ProgramRepository : IDependency,IRepository<Biz_Rolling_Program>
    {
    }
}
