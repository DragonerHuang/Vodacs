using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Configuration;
using Vodace.Core.Enums;
using Vodace.Core.Localization;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Entity.DomainModels.Biz.partial;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices.Biz.Partial;

namespace Vodace.Sys.Services.Biz.Partial
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IBiz_Project_FilesRepository _project_FilesRepository;

        private readonly ILocalizationService _localizationService;

        public FileStorageService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, IBiz_Project_FilesRepository project_FilesRepository, ILocalizationService localizationService)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            _project_FilesRepository = project_FilesRepository;
            _localizationService = localizationService;
        }

        public async Task<string> GetFilePhysicalPathAsync(Guid fileId)
        {
            var fileInfo = await GetFileInfoAsync(fileId);
            if (fileInfo == null)
            {
                Log4NetHelper.Info($"文件信息不存在: {fileId}");
                return null;
            }
            var basePath = _configuration["FileSettings:FolderPath"] ?? "Uploads";
            var storagePath = fileInfo.StoragePath;
            if (Path.DirectorySeparatorChar != '\\')
            {
                storagePath = storagePath.Replace('\\', Path.DirectorySeparatorChar);
            }
            var fullPath = Path.Combine(basePath, storagePath);
            return fullPath;
        }
        public async Task<FileInfoEx> GetFileInfoAsync(Guid fileId)
        {
            try
            {
                var data = _project_FilesRepository.Find(d => d.id == fileId && d.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                return await Task.FromResult(new FileInfoEx
                {
                    FileId = data.id,
                    OriginalName = data.file_name,
                    StoragePath = data.file_path,
                    MimeType = CommonHelper.GetMimeType(data.file_name),
                    Size = 1024000,
                    UploaderId = "1"
                });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Info($"获取文件信息失败: {fileId}");
                return null;
            }
        }

        /// <summary>
        /// 预览缩略图图片（文件流）
        /// </summary>
        /// <param name="id">图片id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> ViewThumbnailFileByteByIdAsync(Guid id)
        {
            try
            {
                var photoData = await _project_FilesRepository.FindFirstAsync(p => p.id == id);

                var strSaveFolder = AppSetting.FileSaveSettings.FolderPath;

                var relPathFixed = photoData.file_thumbnail_path?.TrimStart('\\', '/');
                var strFilePath = Path.Combine(strSaveFolder, relPathFixed);
                if (!File.Exists(strFilePath))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_null"));
                }

                var bytsFile = File.ReadAllBytes(strFilePath);                // 读取文件内容
                var strContentType = CommonHelper.GetContentType(strFilePath);// 获取文件的MIME类型

                return WebResponseContent.Instance.OK(
                    _localizationService.GetString("operation_success"),
                    new FileDownLoadDto
                    {
                        cntent_type = strContentType,
                        file_bytes = bytsFile,
                        file_name = photoData.file_thumbnail_name
                    });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}
