
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
    public partial interface IBiz_Contract_Tender_AddendumRepository : IDependency,IRepository<Biz_Contract_Tender_Addendum>
    {
    }
}
