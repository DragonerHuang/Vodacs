using System;
using System.Threading.Tasks;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels.Biz.partial;

namespace Vodace.Sys.IServices.Biz.Partial
{
    public interface IFileStorageService
    {
        //Task<string> GenerateTemporaryUrlAsync(Guid file_Id, TimeSpan expiry, string userId);
        //Task<bool> ValidateTemporaryUrlAsync(string token, Guid file_Id, string userId);
        Task<string> GetFilePhysicalPathAsync(Guid fileId);
        //Task<bool> FileExistsAsync(Guid file_Id);
        Task<FileInfoEx> GetFileInfoAsync(Guid fileId);

        /// <summary>
        /// 预览缩略图图片
        /// </summary>
        /// <param name="id">图片id</param>
        /// <returns></returns>
        Task<WebResponseContent> ViewThumbnailFileByteByIdAsync(Guid id);
    }
}
