
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Rolling_Program_Site_ContentService
    {
        WebResponseContent Add(RollingProgramSiteContentAddDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(RollingProgramSiteContentAddDto dto);
        WebResponseContent GetRollingProgramSiteContentList(PageInput<RollingProgramSiteContentSearchDto> dto);
        Task<WebResponseContent> GetVersion(RollingProgramSiteContentDto dto);
    }
 }
