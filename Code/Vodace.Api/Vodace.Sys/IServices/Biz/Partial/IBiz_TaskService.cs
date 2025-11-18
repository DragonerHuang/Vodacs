
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_TaskService
    {
       /// <summary>
        /// 添加Task
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        public WebResponseContent AddTask(TaskDto dtoTask);
        /// <summary>
        /// 修改 Task
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        public WebResponseContent EditTask(TaskDto dtoTask);
        /// <summary>
        /// 删除 Task
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public WebResponseContent DelTask(Guid guid);

        /// <summary>
        /// 获取Task信息列表
        /// </summary>
        /// <param name="pdo"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskList(PageDataOptions pdo);
        /// <summary>
        ///  根据Id 获取Task信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskById(Guid guid);
    }
 }
