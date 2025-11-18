
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Task_GroupService
    {
        /// <summary>
        /// 添加TaskGroup
        /// </summary>
        /// <param name="mTaskGroupDto"></param>
        /// <returns></returns>
        public WebResponseContent AddTaskGroup(TaskGroupDto dtoTaskGroup);
        /// <summary>
        /// 修改 TaskGroup
        /// </summary>
        /// <param name="mTaskGroupDto"></param>
        /// <returns></returns>
        public WebResponseContent EditTaskGroup(TaskGroupDto dtoTaskGroup);
        /// <summary>
        /// 删除 TaskGroup
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public WebResponseContent DelTaskGroup(Guid guid);

        /// <summary>
        /// 获取TaskGroup信息列表
        /// </summary>
        /// <param name="pdo"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskGroupList(PageDataOptions pdo);
        /// <summary>
        ///  根据Id 获取TaskGroup信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskGroupById(Guid guid);
    }
 }
