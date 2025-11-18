
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Enums;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Quotation_DeadlineService
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dtoQuery"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetDeadLineListPageAsync(PageInput<QnDeadlineSearchDto> dtoQuery);

        /// <summary>
        /// 查询列表（不分页）
        /// </summary>
        /// <param name="dtoQuery"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetDeadLineListAsync(QnDeadlineSearchDto dtoQuery);

        /// <summary>
        /// 编辑期限管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditDealLineAsync(EditQnDeadlineDto input);

        /// <summary>
        /// 报价新增时创建截止时间（不执行savechange）
        /// </summary>
        /// <param name="qnId">报价id</param>
        /// <param name="contractType">合同类型（公开招标、邀请招标、直接招标）</param>
        /// <param name="closingTime">截止时间</param>
        /// <param name="relationId">关联id</param>
        /// <returns></returns>
        WebResponseContent AddDeadlineByQnAdd(Guid qnId, string contractType, DateTime? closingTime, Guid? relationId);

        /// <summary>
        /// 因换负责人而改变期限的负责人（不执行savechange）
        /// </summary>
        /// <param name="qnId">报价id</param>
        /// <param name="newContactId">新负责人</param>
        /// <param name="oldContactId">旧负责人</param>
        /// <param name="headlerType">负责人类型</param>
        /// <returns></returns>
        Task<WebResponseContent> EditByHeadlerChangeAsync(Guid qnId, Guid newContactId, Guid? oldContactId, string headlerType);

        /// <summary>
        /// 因合同资料保存而改变期限记录的截止时间和完成时间
        /// </summary>
        /// <param name="qnId">报价id</param>
        /// <param name="eventsEnum">期限类型</param>
        /// <param name="relationId">管理表id</param>
        /// <param name="closingTime">截止时间</param>
        /// <param name="isFinish">是否完成</param>
        /// <returns></returns>
        Task<WebResponseContent> EidtByContractDetailsChangeAsync(Guid qnId, UpcomingEventsEnum eventsEnum, Guid relationId, DateTime? closingTime = null, bool isFinish = false);

        /// <summary>
        /// 因问答保存而改变期限记录的截止时间和完成时间
        /// </summary>
        /// <param name="qnId">报价id</param>
        /// <param name="eventsEnum">期限类型</param>
        /// <param name="relationId">管理表id</param>
        /// <param name="closingTime">截止时间</param>
        /// <param name="isFinish">是否完成</param>
        /// <returns></returns>
        Task<WebResponseContent> EditByQAChangeAsync(Guid qnId, UpcomingEventsEnum eventsEnum, Guid relationId, DateTime? closingTime = null, bool isFinish = false);

        /// <summary>
        /// 因提交完成文件而改变期限记录的完成时间
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="eventsEnum"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditBySumbitFileAsync(Guid qnId, UpcomingEventsEnum eventsEnum);

        /// <summary>
        /// 删除期限管理
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="relationId"></param>
        /// <param name="eventsEnum"></param>
        /// <returns></returns>
        Task<WebResponseContent> DeleteAsync(Guid qnId, Guid relationId, UpcomingEventsEnum eventsEnum);
    }
 }
