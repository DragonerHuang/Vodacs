
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.Services;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_Site_Work_Check_ItemService
    {
        /// <summary>
        /// 分页查询：按 global_code 前缀匹配；默认排序 level→item_code→order_no（升序）
        /// </summary>
        /// <param name="input">查询与分页参数</param>
        /// <returns>分页结果</returns>
        Task<WebResponseContent> GetPageListAsync(PageInput<SiteWorkCheckItemSearchDto> input);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto">新增输入DTO</param>
        /// <returns>操作结果</returns>
        Task<WebResponseContent> AddAsync(SiteWorkCheckItemAddDto dto);

        /// <summary>
        /// 修改：item_code；global_code+type_id 唯一校验；缺省值按新增规则处理
        /// </summary>
        /// <param name="dto">编辑输入DTO</param>
        /// <returns>操作结果</returns>
        Task<WebResponseContent> EditAsync(SiteWorkCheckItemEditDto dto);

        /// <summary>
        /// 删除：逻辑删除，设置 delete_status=Invalid
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作结果</returns>
        Task<WebResponseContent> DelAsync(Guid id);

        /// <summary>
        /// 启用/禁用：更新 enable 字段
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="enable">0 禁用；1 启用</param>
        /// <returns>操作结果</returns>
        Task<WebResponseContent> EnableAsync(Guid id, byte enable);
    }
 }
