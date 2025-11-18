
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Quotation_SiteService
    {
        /// <summary>
        /// 查询现场考察列表（分页）
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetQnSitePageAsync(PageInput<QnSiteSearchDto> pageInput);

        /// <summary>
        /// 查询现场考察列表
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetQnSiteAsync(QnSiteSearchDto pageInput);

        /// <summary>
        /// 上传完成文件
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        Task<WebResponseContent> UpLoadFinishFile(List<IFormFile> lstFiles);

        /// <summary>
        /// 创建现场考察
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> AddQnSiteAsync(AddQnSiteDto input);

        /// <summary>
        /// 编辑现场考察
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditQnSiteAsync(EditQnSiteDto input);

        /// <summary>
        /// 删除现场考察
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> DeleteQnSiteAsync(Guid id);

        /// <summary>
        /// 修改回复日期
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditReplyDateAsync(QnSiteSearchDto input);
    }
 }
