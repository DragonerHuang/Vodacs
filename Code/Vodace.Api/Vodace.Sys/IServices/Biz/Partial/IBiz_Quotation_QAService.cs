
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
    public partial interface IBiz_Quotation_QAService
    {
        //PageGridData<QuotationQAListDto> GetListByPageOld(PageInput<QuotationQA_Query> query);
        WebResponseContent AddData(QuotationQADto quotationQADto);
        WebResponseContent AddDataWithFile(QuotationQAWithFileAddDto quotationQADto, IFormFile file = null);
        WebResponseContent EditDataWithFile(QuotationQAWithFileEditDto quotationQADto, IFormFile file = null);
        WebResponseContent EditData(QuotationQADto quotationQADto);
        WebResponseContent DelData(Guid id);
        Task<WebResponseContent> UploadQAFile(IFormFile file, int intSubmitType, Guid guidQaId);
        Task<WebResponseContent> DownloadSysQAFile(Guid guidFileIdint, int SubmitType);
        Task<WebResponseContent> DownLoadFiles(Guid qnid, int type);

        //WebResponseContent GetDataByPage(PageInput<QuotationQA_Query> query);
        Task<WebResponseContent> GetDataByPage(PageInput<QuotationQA_Query> query);

        /// <summary>
        /// 添加问答记录
        /// </summary>
        /// <param name="quotationQADto"></param>
        /// <returns></returns>
        Task<WebResponseContent> AddQADataAsync(QuotationQAWithFileAddDto quotationQADto);

        /// <summary>
        /// 修改问答记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditQADataAsync(EditQuotationQADto input);

        /// <summary>
        /// 根据id获取问答记录
        /// </summary>
        /// <param name="qaId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetQADataByIdAsync(Guid qaId);

        /// <summary>
        /// 问答文件上传
        /// </summary>
        /// <param name="lstFiles">文件列表</param>
        /// <param name="qnId">报价id</param>
        /// <param name="QAType">问答类型 0：预审问答；1：投标问答；</param>
        /// <returns></returns>
        Task<WebResponseContent> UploadQAFilesAsync(List<IFormFile> lstFiles, Guid qnId, int QAType);

        /// <summary>
        /// 问答完成文件上传
        /// </summary>
        /// <param name="lstFiles">文件列表</param>
        /// <param name="qnId">报价id</param>
        /// <param name="qaType">问答类型 0：预审问答；1：投标问答；</param>
        /// <returns></returns>
        Task<WebResponseContent> UploadQAFinishFilesAsync(List<IFormFile> lstFiles, Guid qnId, int qaType);

        /// <summary>
        /// 根据id下载问答文件
        /// </summary>
        /// <param name="qaId"></param>
        /// <returns></returns>
        Task<WebResponseContent> DownLoadQAFilesByIdAsync(Guid qaId);

        /// <summary>
        /// 下载全部问答
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="qaType"></param>
        /// <returns></returns>
        Task<WebResponseContent> DownLoadAllQAFilesAsync(Guid qnId, int qaType);

        /// <summary>
        /// 删除Q&A
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> DeleteQAAsync(Guid id);

    }
 }
