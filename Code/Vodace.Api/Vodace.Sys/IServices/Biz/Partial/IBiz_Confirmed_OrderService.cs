
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Confirmed_OrderService
    {
        /// <summary>
        /// CO分页查询
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetCoPageList(PageInput<CoSearchDto> dtoSearchInput);

        /// <summary>
        /// 确认报价
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> ConfirmOrder(ConfirmDto dtoInput);

        /// <summary>
        /// 取消确认报价
        /// </summary>
        /// <param name="guidId"></param>
        /// <returns></returns>
        Task<WebResponseContent> UnConfirmOrder(Guid guidId);

        /// <summary>
        /// 确认订单文件上传
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        Task<WebResponseContent> UploadCoFile(List<IFormFile> lstFiles);

        /// <summary>
        /// 根据id获取co详细信息
        /// </summary>
        /// <param name="guidCOId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetCoByIdAsync(Guid guidCOId);

        /// <summary>
        /// 修改co信息
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditCoAsync(CoInputDto dtoInput);

        /// <summary>
        /// 统计确认订单的确认金额
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> CountCOAmt();

        /// <summary>
        /// 获取确认订单下的状态
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> GetStatus();
    }
 }
