
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Rolling_ProgramService
    {
        WebResponseContent Add(RollingProgramDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(RollingProgramEditDto dto);
        Task<WebResponseContent> GetRollingProgramList(PageInput<RollingProgramEditDto> dto);
    }
 }
