
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
    public partial interface IBiz_Quotation_InterviewService
    {
        /// <summary>
        /// 查询招标面试列表（分页）
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetQnInterviewPageAsync(PageInput<QnInterviewSearchDto> pageInput);

        /// <summary>
        /// 查询招标面试列表
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetQnInterviewAsync(QnInterviewSearchDto pageInput);

        /// <summary>
        /// 上传完成文件，放置在临时目录
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        Task<WebResponseContent> UpLoadFinishFile(List<IFormFile> lstFiles);

        /// <summary>
        /// 创建招标面试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> AddQnInterviewAsync(AddQnInterview input);

        /// <summary>
        /// 编辑招标面试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditQnInterviewAsync(EditQnInterview input);

        /// <summary>
        /// 删除招标面试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> DeleteQnInterviewAsync(Guid id);

        /// <summary>
        /// 修改回复日期
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditReplyDateAsync(QnInterviewSearchDto input);
    }
 }
