
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Rolling_Program_TaskService
    {
        WebResponseContent Add(RollingProgramTaskDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(RollingProgramTaskDto dto);
    }
 }
