
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
    public partial interface IBiz_Project_FilesService
    {
        /// <summary>
        /// 根据合同id获取文件放置文件夹
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="fileCode"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetFileFolderByContactIdAsync(Guid contractId, string fileCode = "");

        /// <summary>
        /// 删除文件记录（移动文件到临时目录）
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        WebResponseContent DeleteFile(List<Biz_Project_Files> lstFiles);

        /// <summary>
        /// 判断文件是否为图片文件
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        bool CheckFileIsPhoto(List<IFormFile> lstFiles);

        #region 预览

        /// <summary>
        /// 预览文件（文件流）
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns></returns>
        Task<WebResponseContent> ViewFileByteByIdAsync(Guid id);

        /// <summary>
        /// 预览文件（url）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> ViewPhotoUrlByIdAsync(Guid id);

        #endregion

        #region 上传

        /// <summary>
        /// 上传文件到临时目录
        /// </summary>
        /// <param name="lstFflFiles">文件集合</param>
        /// <param name="fileTypeCode">文件类型（0：主文件，1：编辑文件，2：客户评语，3：参考文件，4：内部验收凭证，5：客户验收凭证，6：开工前，7：施工中，8：完工后）</param>
        /// <param name="isSaveChange">是否触发SaveChange，默认不触发</param>
        /// <returns></returns>
        Task<WebResponseContent> UploadFileToTempAsync(List<IFormFile> lstFflFiles, int fileTypeCode, bool isSaveChange = false);

        /// <summary>
        /// 完成总体操作后，将临时目录移动到正式目录中
        /// </summary>
        /// <param name="fileInfos">文件信息</param>
        /// <param name="fileTypeCode">文件类型代码（用于查找放置的文件夹）</param>
        /// <param name="contractId">合同id</param>
        /// <param name="relationId">关联id</param>
        /// <param name="isSaveChange">是否触发SaveChange，默认不触发</param>
        /// <returns></returns>
        Task<WebResponseContent> MoveFileToFolderAsync(List<UFileInfoDto> fileInfos,
            string fileTypeCode,
            Guid contractId,
            Guid relationId,
            bool isSaveChange = false);

        #endregion

    }
}
