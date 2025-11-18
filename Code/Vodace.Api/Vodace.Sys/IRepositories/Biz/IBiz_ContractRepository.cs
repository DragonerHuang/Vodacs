
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Entity.DomainModels;
using Vodace.Core.Extensions.AutofacManager;
namespace Vodace.Sys.IRepositories.Biz
{
    public partial interface IBiz_ContractRepository : IDependency,IRepository<Biz_Contract>
    {
    }
}
