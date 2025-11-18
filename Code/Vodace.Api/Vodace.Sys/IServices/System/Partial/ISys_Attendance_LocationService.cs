
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_Attendance_LocationService
    {

        /// <summary>
        /// 获取数据列表（分页）
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetDataListAsync(PageInput<LocationSearchDto> dtoSearchInput);

        /// <summary>
        /// 获取数据列表（不分页）
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetDataListAsync(LocationSearchDto dtoSearchInput);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> AddData(LocationInputDto dtoInput);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="dtoEdit"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditData(LocationEditDto dtoEdit);

        /// <summary>
        /// 删除数据（软删）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> DeleteData(Guid id);
    }
 }
