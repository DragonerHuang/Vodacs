
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Task_DetailService
    {
        /// <summary>
        /// 添加TaskDetail
        /// </summary>
        /// <param name="mTaskDetailDto"></param>
        /// <returns></returns>
        public WebResponseContent AddTaskDetail(TaskDetailDto dtoTaskDetail);
        /// <summary>
        /// 修改 TaskDetail
        /// </summary>
        /// <param name="mTaskDetailDto"></param>
        /// <returns></returns>
        public WebResponseContent EditTaskDetail(TaskDetailDto dtoTaskDetail);
        /// <summary>
        /// 删除 TaskDetail
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public WebResponseContent DelTaskDetail(Guid guid);

        /// <summary>
        /// 获取TaskDetail信息列表
        /// </summary>
        /// <param name="pdo"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskDetailList(PageDataOptions pdo);
        /// <summary>
        ///  根据Id 获取TaskDetail信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskDetailById(Guid guid);
    }
 }
