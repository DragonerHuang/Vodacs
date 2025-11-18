
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_File_RecordsService
    {
        /// <summary>
        /// 注册工人上传文件
        /// </summary>
        /// <param name="lstFiles">文件集合</param>
        /// <param name="strTypeCode">文件代码（用于区分是什么文件，来源与字典）</param>
        /// <param name="intStatus">上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）</param>
        /// <param name="strSaveFolder">文件存储文件夹，没传则用配置地址</param>
        /// <returns></returns>
        Task<WebResponseContent> WorkRegisterUploadAsync(
            List<IFormFile> lstFiles,
            string strTypeCode,
            int intStatus = 1,
            string strSaveFolder = "");

        #region 文件上传（根据配置文件夹）

        /// <summary>
        /// 通用上传文件（异步）（根据配置文件存）
        /// </summary>
        /// <param name="lstFflFiles">文件集合</param>
        /// <param name="strTypeCode">文件类型代码</param>
        /// <param name="masterId">所属表id</param>
        /// <returns></returns>
        Task<WebResponseContent> CommonUploadAsync(
            List<IFormFile> lstFflFiles,
            string strTypeCode,
            string strIdentifyfolder = "",
            Guid? masterId = null);

        /// <summary>
        /// 通用上传文件（同步）（根据配置文件存）
        /// </summary>
        /// <param name="lstFflFiles">文件集合</param>
        /// <param name="strTypeCode">文件类型代码</param>
        /// <param name="strIdentifyfolder">文件夹标识名（放置的文件名，有些文件是根据报价、项目、合约下的）</param>
        /// <param name="masterId">所属表id</param>
        /// <returns></returns>
        WebResponseContent CommonUpload(
            List<IFormFile> lstFflFiles,
            string strTypeCode,
            string strIdentifyfolder = "",
            Guid? masterId = null);

        /// <summary>
        /// 保存文件（未实现savechange）
        /// </summary>
        /// <param name="lstFflFiles">文件集合</param>
        /// <param name="strTypeCode">文件类型代码</param>
        /// <param name="strIdentifyfolder">文件夹标识名（放置的文件名，有些文件是根据报价、项目、合约下的）</param>
        /// <param name="masterId">所属表id</param>
        /// <returns></returns>
        WebResponseContent SaveFiles(
            List<IFormFile> lstFflFiles,
            string strTypeCode,
            string strIdentifyfolder = "",
            Guid? masterId = null);

        /// <summary>
        /// 获取文件上传配置文件夹（相对位置）
        /// </summary>
        /// <param name="strFileCode">文件代码</param>
        /// <param name="strIdentifyfolder">文件夹标识名（放置的文件名，有些文件是根据报价、项目、合约下的）</param>
        /// <returns></returns>
        WebResponseContent GetFileSaveFolder(string strFileCode, string strIdentifyfolder = "");

        /// <summary>
        /// 保存文件到指定文件夹
        /// </summary>
        /// <param name="fflFiles">文件</param>
        /// <param name="strRelPath">文件存放相对位置</param>
        /// <returns></returns>
        WebResponseContent SaveFileByPath(List<IFormFile> fflFiles, string strRelPath);

        /// <summary>
        /// 记录文件信息（没有执行savechange）
        /// </summary>
        /// <param name="lstFileInfos">文件信息</param>
        /// <param name="guidMasterId">所属表id（可为空）</param>
        /// <param name="strTypeCode">文件类型代码</param>
        /// <returns></returns>
        WebResponseContent RecordFileInfos(List<FileInfoDto> lstFileInfos, Guid? guidMasterId, string strTypeCode);

        /// <summary>
        ///  通用上传文件（异步）（保存到临时目录）
        /// </summary>
        /// <param name="lstFflFiles"></param>
        /// <param name="strTypeCode"></param>
        /// <returns></returns>
        Task<WebResponseContent> CommonUploadToTempAsync(List<IFormFile> lstFflFiles, string strTypeCode);

        #endregion


        #region 文件下载、转换

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="guidFileId">文件id</param>
        /// <param name="strSaveFolder">文件存储文件夹（特定的）</param>
        /// <returns></returns>
        Task<WebResponseContent> DownloadSysFile(
            Guid guidFileId,
            string strSaveFolder = "");

        /// <summary>
        /// office文件转换pdf（根据文件id）
        /// </summary>
        /// <param name="guidFileId">系统文件表id（Sys_File_Records）</param>
        /// <param name="strSaveFolder">文件是存储在特定的文件夹的（默认空的，取配置文件中的地址）</param>
        /// <returns></returns>
        Task<WebResponseContent> FileConvertPDFById(Guid guidFileId, string strSaveFolder = "");

        /// <summary>
        /// 文件转换pdf
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        WebResponseContent FileConvertPDFByPath(string strFilePath);

        /// <summary>
        /// 下载多个文件（返回zip）
        /// </summary>
        /// <param name="guidFileIds"></param>
        /// <returns></returns>
        Task<WebResponseContent> DownloadMultipleFiles(List<Guid> guidFileIds);

        /// <summary>
        /// 压缩文件（内存）
        /// </summary>
        /// <param name="lstFileRecords">文件记录</param>
        /// <param name="isWithParentLevel">是否带上上一层级</param>
        /// <returns></returns>
        Task<FileDownLoadDto> ZipFileAsync(List<Sys_File_Records> lstFileRecords, bool isWithParentLevel = false);

        #endregion

        #region 更改状态

        /// <summary>
        /// 将文件上传状态更改成完成状态
        /// </summary>
        /// <param name="fileIds">文件id集合</param>
        /// <param name="strMasterId">所属id</param>
        /// <returns></returns>
        Task<WebResponseContent> FinishFilesUploadAsync(
            List<Guid> fileIds,
            Guid strMasterId);

        /// <summary>
        /// 更改文件状态及所属id（未实现savechange功能）
        /// </summary>
        /// <param name="litFileIds"></param>
        /// <param name="strMasterId"></param>
        /// <returns></returns>
        WebResponseContent FinishFilesUploadNoSaveChange(
            List<Guid> litFileIds,
            Guid strMasterId);

        /// <summary>
        /// 更改文件状态及所属id（未实现savechange功能）
        /// </summary>
        /// <param name="litFileInfo"></param>
        /// <param name="strMasterId"></param>
        /// <param name="strMoveFolder"></param>
        /// <param name="strFileTypeCode"></param>
        /// <returns></returns>
        WebResponseContent FinishFilesUploadNoSaveChange(
            List<ContractQnFileDto> litFileInfo,
            Guid strMasterId);

        /// <summary>
        /// 移动文件并更改文件状态及所属id（未实现savechange功能）
        /// </summary>
        /// <param name="litFileInfo"></param>
        /// <param name="strMasterId"></param>
        /// <param name="isMoveFile"></param>
        /// <returns></returns>
        WebResponseContent FinishFilesUploadNoSaveChange(
            List<ContractQnFileDto> litFileInfo,
            Guid strMasterId,
            bool isMoveFile = false,
            string strMoveFolder = "",
            string strFileTypeCode = "");

        string GetContentType(string filePath);

        /// <summary>
        /// 删除文件（未执行savechange）
        /// （修改记录状态，移动文件到临时目录）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        WebResponseContent DeleteUploadFiles(List<Sys_File_Records> data);

        /// <summary>
        /// 删除文件（未执行savechange）
        /// （修改记录状态，移动文件到临时目录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> DeleteUploadFiles(List<Guid> id);

        #endregion

        #region 旧的

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fflFile">要上传的文件</param>
        /// <param name="strTypeCode">文件代码（用于区分是什么文件，来源与字典）</param>
        /// <param name="intTypeCodeId">文件代码id（用于区分是什么文件，id来源与字典）</param>
        /// <param name="strSaveFolder">文件存储文件夹</param>
        /// <param name="intStatus">上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）</param>
        /// <returns></returns>
        Task<WebResponseContent> UploadFileAsync(
            IFormFile fflFile,
            string strTypeCode,
            int intStatus = 1,
            string strSaveFolder = "");

        /// <summary>
        /// 上传文件（多个）
        /// </summary>
        /// <param name="lstFflFiles">要上传的文件集合</param>
        /// <param name="strTypeCode">文件代码（用于区分是什么文件，来源与字典）</param>
        /// <param name="strSaveFolder">文件存储文件夹</param>
        /// <param name="intStatus">上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）</param>
        /// <returns></returns>
        Task<WebResponseContent> UploadFilesAsync(
            List<IFormFile> lstFflFiles,
            string strTypeCode,
            int intStatus = 1,
            string strSaveFolder = "");

        #endregion

        #region 移动文件

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="strRelFolder"></param>
        /// <param name="fileRecordData"></param>
        /// <returns></returns>
        string MoveFile(string strRelFolder, Sys_File_Records fileRecordData);

        /// <summary>
        /// 将文件移动到临时目录
        /// </summary>
        /// <param name="filePath">文件相对路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileExt">文件后缀</param>
        /// <returns></returns>
        string MoveFileToTemporary(string filePath, string fileName, string fileExt);

        #endregion
    }
}
