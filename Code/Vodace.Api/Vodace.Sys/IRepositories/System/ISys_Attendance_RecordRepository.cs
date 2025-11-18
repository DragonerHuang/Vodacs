
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
    public partial interface ISys_Attendance_RecordRepository : IDependency,IRepository<Sys_Attendance_Record>
    {
    }
}
