
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Contract_DetailsService
    {
        #region 合同资料

        /// <summary>
        /// 获取合同资料
        /// </summary>
        /// <param name="guidQnId">报价id</param>
        /// <returns></returns>
        Task<WebResponseContent> GetContractData(Guid guidQnId);

        /// <summary>
        /// 保存合同资料
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> SaveContractData(ContractDetailDataDto dtoInput);

        #endregion

        #region 标书资料

        /// <summary>
        /// 投标补充-附件文档上传
        /// </summary>
        /// <param name="lstFiles">文件信息</param>
        /// <returns></returns>
        Task<WebResponseContent> UploadAddendumFiles(List<IFormFile> lstFiles);

        /// <summary>
        /// 获取标书资料
        /// </summary>
        /// <param name="guidQnId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetTenderData(Guid guidQnId);

        /// <summary>
        /// 保存标书资料
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> SaveTenderData(TenderDataDto dtoInput);

        #endregion


    }
 }
