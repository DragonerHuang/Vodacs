
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Task_Group_RelationshipService
    {
        /// <summary>
        /// 添加TaskGroupRelationship
        /// </summary>
        /// <param name="mTaskGroupRelationshipDto"></param>
        /// <returns></returns>
        public WebResponseContent AddTaskGroupRelationship(TaskGroupRelationshipDto dtoTaskRelationshipGroup);
        /// <summary>
        /// 修改 TaskGroupRelationship
        /// </summary>
        /// <param name="mTaskGroupRelationshipDto"></param>
        /// <returns></returns>
        public WebResponseContent EditTaskGroupRelationship(TaskGroupRelationshipDto dtoTaskRelationshipGroup);
        /// <summary>
        /// 删除 TaskGroupRelationship
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public WebResponseContent DelTaskGroupRelationship(Guid guid);

        /// <summary>
        /// 获取TaskGroupRelationship信息列表
        /// </summary>
        /// <param name="pdo"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskGroupRelationshipList(PageDataOptions pdo);
        /// <summary>
        ///  根据Id 获取TaskGroupRelationship信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetTaskGroupRelationshipById(Guid guid);
    }
 }
