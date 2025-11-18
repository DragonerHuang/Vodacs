
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
    public partial interface IBiz_QuotationService
    {
        #region 报价

        /// <summary>
        /// 合同1信息文件上传
        /// </summary>
        /// <param name="lstFiles">文件列表</param>
        /// <param name="intStatus">上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）</param>
        /// <returns></returns>
        Task<WebResponseContent> UploadContractFiles(
            List<IFormFile> lstFiles,
            int intStatus = 0);

        /// <summary>
        /// 创建报价
        /// </summary>
        /// <param name="dtoAddQnRequest"></param>
        /// <returns></returns>
        WebResponseContent AddQn(AddQnRequestDto dtoAddQnRequest);


        /// <summary>
        /// 报价修改
        /// </summary>
        /// <param name="editQnRequestDto"></param>
        /// <returns></returns>
        WebResponseContent EditQn(EditQnRequestDto editQnRequestDto);

        /// <summary>
        /// 报价删除
        /// </summary>
        /// <param name="qnId">报价id</param>
        /// <returns></returns>
        WebResponseContent DelQn(Guid qnId);

        /// <summary>
        /// 获取报价分页集合
        /// </summary>
        /// <param name="dtoQnSearchInput">查询条件</param>
        /// <returns></returns>
        Task<WebResponseContent> GetQnPageListAsync(PageInput<QnSearchDto> dtoQnSearchInput);

        /// <summary>
        /// 统计报价金额
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> CountQnAmtAsync();

        /// <summary>
        /// 获取报价详情
        /// </summary>
        /// <param name="queryQn"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetQnByIdAsync(Guid guidQnId);

        /// <summary>
        /// 获取报价下拉列表
        /// </summary>
        /// <param name="boolIsMaster">是否选则父项</param>
        /// <param name="qnId">报价id</param>
        /// <returns></returns>
        Task<WebResponseContent> GetQnDropList(bool boolIsMaster = true, Guid? qnId = null);
        Task<WebResponseContent> GetQnDropList();

        /// <summary>
        /// 获取报价状态下拉列表
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetStatuCodeByQnId(Guid qnId);

        /// <summary>
        /// 是否有重复合同（根据合同编码）
        /// </summary>
        /// <param name="strContractNo">合同编码</param>
        /// <param name="isAdd">是否是新增</param>
        /// <param name="guidContractId">如果是修改，则要带上修改那个合同id</param>
        /// <returns></returns>
        bool CheckRepeatContract(string strContractNo, bool isAdd = true, Guid? guidContractId = null);

        #endregion

        #region 提交文件

        /// <summary>
        /// 提交文件（目前只改状态）
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        Task<WebResponseContent> SubmitQnFiles(Guid qnId);

        /// <summary>
        /// 提交文件-上传文件（财务类文件上传、技术类文件上传）
        /// </summary>
        /// <param name="lstFiles">文件</param>
        /// <param name="intSubmitType">文件类型（0：财务类文件、1：技术类文件）</param>
        /// <param name="guidQnId">所属qn的id</param>
        /// <returns></returns>
        Task<WebResponseContent> UploadSubmitFile(List<IFormFile> lstFiles, int intSubmitType, Guid guidQnId);

        /// <summary>
        /// 下载提交的文件
        /// </summary>
        /// <param name="guidQnid"></param>
        /// <returns></returns>
        Task<WebResponseContent> DownSubmitFile(Guid guidQnid);

        /// <summary>
        /// 获取提交文件（财务类文件上传、技术类文件上传）列表
        /// </summary>
        /// <param name="guidQnId">所属qn的id</param>
        /// <param name="intSubmitType">文件类型（0：财务类文件、1：技术类文件）</param>
        /// <returns></returns>
        Task<WebResponseContent> GetSumitFilesAync(Guid guidQnId, int intSubmitType);

        /// <summary>
        /// 删除提交文件
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns></returns>
        Task<WebResponseContent> DeleteSumitFileAsync(Guid id);

        /// <summary>
        /// 提交完成文件
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <param name="qnId"></param>
        /// <returns></returns>
        Task<WebResponseContent> SumbitFinishFilesAsync(List<IFormFile> lstFiles, Guid qn_id);

        #endregion

        #region 合同文件夹

        /// <summary>
        /// 根据qn的id获取文件存储文件名
        /// </summary>
        /// <param name="qnNo"></param>
        /// <param name="isNeedCompany"></param>
        /// <returns></returns>
        WebResponseContent GetRootFolderName(Guid qnNo, bool isNeedCompany = false);

        /// <summary>
        /// 根据qn的id获取文件存储文件名
        /// </summary>
        /// <param name="qnNo"></param>
        /// <param name="isNeedCompany"></param>
        /// <returns></returns>
        WebResponseContent GetRootFolderName(Biz_Quotation qn, bool isNeedCompany = false);

        /// <summary>
        /// 获取文件正式存放位置（相对）
        /// </summary>
        /// <param name="qnData"></param>
        /// <returns></returns>
        WebResponseContent GetFormalFolder(Biz_Quotation qnData, string fileType);

        #endregion

        #region 现场地点关系

        /// <summary>
        /// 保存现场位置
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="lstSiteIds"></param>
        void SaveSitesNoSaveChange(Guid contractId, List<Guid?> lstSiteIds);

        #endregion
    }
}
