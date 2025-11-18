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
    public partial interface ISys_Leave_HolidayService
    {
        Task<WebResponseContent> GetListByPage(PageInput<LeaveHolidayQuery> query);

        Task<WebResponseContent> GetList();

        Task<WebResponseContent> AddData(LeaveHolidayAddDto dto);

        Task<WebResponseContent> EditData(LeaveHolidayEditDto dto);

        Task<WebResponseContent> DelData(List<Guid> id);
        
        /// <summary>
        /// json导入更新
        /// </summary>
        /// <param name="importDto"></param>
        /// <returns></returns>
        Task<WebResponseContent> ImportDataAsync(LeaveHolidayImportDto importDto);
    }
}