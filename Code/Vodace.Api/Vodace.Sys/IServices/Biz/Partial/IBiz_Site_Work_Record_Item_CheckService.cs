
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Site_Work_Record_Item_CheckService
    {
        /// <summary>
        /// ͨ
        /// 0:BRF, 1:SCR, 2:CPD, 3:QDC, 4:CP, 5:SIC
        /// </summary>
        Task<WebResponseContent> GetCheckItemsAsync(Guid recordId, int checkType);

        /// <summary>
        /// 设置选项值内容（单条，实时）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> SetCheckItemBySingleAsync(SetItemValueDataDto input);

        /// <summary>
        /// 保存签名文件（单条，实时）
        /// </summary>
        /// <param name="signInput"></param>
        /// <param name="signPic"></param>
        /// <returns></returns>
        Task<WebResponseContent> SetItemSignBySingleAsync(SetItemSignDto signInput, IFormFile signPic);

        /// <summary>
        /// 设置选项值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> SetCheckItemValueAsync(SetIemValueDto input);

        /// <summary>
        /// 设置SIC清单（选择SIC人员和参考编号）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> SetSICDataAsync(EditCheckSICdAT input);
    }
 }
