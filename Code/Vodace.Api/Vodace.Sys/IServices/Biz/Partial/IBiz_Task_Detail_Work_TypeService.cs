
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Task_Detail_Work_TypeService
    {
        /// <summary>
        /// 添加 Task与工种的关系
        /// </summary>
        /// <param name="dtoTaskWorkType"></param>
        /// <returns></returns>
        public WebResponseContent AddTaskWorkType(TaskWorkTypeDto dtoTaskWorkType);
        /// <summary>
        /// 修改 Task与工种的关系
        /// </summary>
        /// <param name="dtoTaskWorkType"></param>
        /// <returns></returns>
        public WebResponseContent EditTaskWorkType(TaskWorkTypeDto dtoTaskWorkType);
        /// <summary>
        /// 删除 Task与工种的关系
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public WebResponseContent DelTaskWorkType(Guid guid);

        /// <summary>
        /// 获取Task与工种的关系信息列表
        /// </summary>
        /// <param name="pdo"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskWorkTypeList(PageDataOptions pdo);
        /// <summary>
        ///  根据Id 获取Task与工种的关系
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskWorkTypeById(Guid guid);
    }
 }
