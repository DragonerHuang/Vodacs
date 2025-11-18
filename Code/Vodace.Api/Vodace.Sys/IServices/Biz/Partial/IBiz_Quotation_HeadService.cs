
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Quotation_HeadService
    {
        /// <summary>
        /// 负责列表中人员下拉列表
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetContactDownListAsync(Guid qnId);

        /// <summary>
        /// 获取负责人列表
        /// </summary>
        /// <param name="qnId">报价的id</param>
        /// <returns></returns>
        Task<WebResponseContent> GetHeadListAsync(Guid qnId);

        /// <summary>
        /// 当报价创建时创建负责人列表（未实现savechange）
        /// </summary>
        /// <param name="qnId">报价的id</param>
        void AddHeadersByAddQn(Guid qnId);

        /// <summary>
        /// 编辑负责人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditHeadAsync(QnHeadInputDto input);

        /// <summary>
        /// 根据报价id和负责人类型获取对应的负责人
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="headlerType"></param>
        /// <returns></returns>
        Task<Guid?> GetHeadlerId(Guid qnId, string headlerType);
    }
 }
