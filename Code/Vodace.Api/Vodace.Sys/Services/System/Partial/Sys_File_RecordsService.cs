

using Dm;
using Dm.util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.Configuration;
using Vodace.Core.Enums;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Core.Utilities.PDFHelper;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;

namespace Vodace.Sys.Services
{
    public partial class Sys_File_RecordsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_File_RecordsRepository _repository;                       // 访问数据库

        private readonly ISys_DictionaryListRepository _ISys_DictionaryListRepository;  // 数据字典(子)
        private readonly ISys_DictionaryRepository _ISys_DictionaryRepository;          // 数据字典（主）
        private readonly IConfiguration _configuration;                                 // 系统配置
        private readonly ILocalizationService _localizationService;                     // 国际化
        private readonly ISys_File_ConfigRepository _configFileRepository;              // 访问文件配置仓储
        private readonly ISys_CompanyRepository _companyRepository;                     // 访问公司仓储

        private readonly string _strUploadPath;         // 配置的上传地址
        private readonly string _strTemporaryFolder;    // 配置的临时文件夹

        [ActivatorUtilitiesConstructor]
        public Sys_File_RecordsService(
            ISys_File_RecordsRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment env,
            ISys_DictionaryListRepository sys_DictionaryListRepository,
            IConfiguration configuration,
            ISys_DictionaryRepository iSys_DictionaryRepository,
            ILocalizationService localizationService,
            ISys_File_ConfigRepository configFileRepository,
            ISys_CompanyRepository companyRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);

            // 从配置获取上传路径，默认为文件夹目录
            _configuration = configuration;
            _strUploadPath = AppSetting.FileSaveSettings.FolderPath;
            _strTemporaryFolder = AppSetting.FileSaveSettings.TemporaryFolder;
            //_strUploadPath = _configuration["FileSettings:FolderPath"].ToString();
            //_strTemporaryFolder = _configuration["FileSettings:TemporaryFolder"].ToString();
            _ISys_DictionaryListRepository = sys_DictionaryListRepository;
            _ISys_DictionaryRepository = iSys_DictionaryRepository;
            _localizationService = localizationService;
            _configFileRepository = configFileRepository;
            _companyRepository = companyRepository;
        }


        #region 业务

        /// <summary>
        /// 注册工人上传文件
        /// </summary>
        /// <param name="lstFiles">文件集合</param>
        /// <param name="strTypeCode">文件代码（用于区分是什么文件 wr_stc：建造行业安全训练证明书 wr_wrc：工人注册证）</param>
        /// <param name="intStatus">上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）</param>
        /// <param name="strSaveFolder">文件存储文件夹，没传则用配置地址</param>
        /// <returns></returns>
        public async Task<WebResponseContent> WorkRegisterUploadAsync(
            List<IFormFile> lstFiles,
            string strTypeCode,
            int intStatus = 1,
            string strSaveFolder = "")
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                if (lstFiles.Count > 5)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_length"));
                }

                if (strTypeCode != "wr_stc" && strTypeCode != "wr_wrc") 
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                return await CommonUploadAsync(lstFiles, strTypeCode);

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.WorkRegisterUpload.文件上传异常", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }


        #endregion

        #region 文件上传（根据配置文件夹）

        /// <summary>
        ///  通用上传文件（异步）（保存到临时目录）
        /// </summary>
        /// <param name="lstFflFiles"></param>
        /// <param name="strTypeCode"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> CommonUploadToTempAsync(List<IFormFile> lstFflFiles, string strTypeCode)
        {
            try
            {
                var recordResult = SaveFilesToTemp(lstFflFiles, strTypeCode);
                if (recordResult.status)
                {
                    await _repository.SaveChangesAsync();
                }
                return recordResult;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 通用上传文件（异步）（根据配置文件存）
        /// </summary>
        /// <param name="lstFflFiles">文件集合</param>
        /// <param name="strTypeCode">文件类型代码</param>
        /// <param name="strIdentifyfolder">文件夹标识名（放置的文件名，有些文件是根据报价、项目、合约下的）</param>
        /// <param name="masterId">所属表id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> CommonUploadAsync(
            List<IFormFile> lstFflFiles, 
            string strTypeCode, 
            string strIdentifyfolder = "", 
            Guid? masterId = null)
        {
            try
            {
                var recordResult = SaveFiles(lstFflFiles, strTypeCode, strIdentifyfolder, masterId);
                if (recordResult.status)
                {
                    await _repository.SaveChangesAsync();
                }
                return recordResult;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }


        /// <summary>
        /// 通用上传文件（同步）（根据配置文件存）
        /// </summary>
        /// <param name="lstFflFiles">文件集合</param>
        /// <param name="strTypeCode">文件类型代码</param>
        /// <param name="strIdentifyfolder">文件夹标识名（放置的文件名，有些文件是根据报价、项目、合约下的）</param>
        /// <param name="masterId">所属表id</param>
        /// <returns></returns>
        public WebResponseContent CommonUpload(
            List<IFormFile> lstFflFiles, 
            string strTypeCode,
            string strIdentifyfolder = "",
            Guid? masterId = null)
        {
            try
            {
                var recordResult = SaveFiles(lstFflFiles, strTypeCode, strIdentifyfolder, masterId);
                if (recordResult.status)
                {
                    _repository.SaveChanges();
                }

                return recordResult;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 保存文件（未实现savechange）
        /// </summary>
        /// <param name="lstFflFiles">文件集合</param>
        /// <param name="strTypeCode">文件类型代码</param>
        /// <param name="strIdentifyfolder">文件夹标识名（放置的文件名，有些文件是根据报价、项目、合约下的）</param>
        /// <param name="masterId">所属表id</param>
        /// <returns></returns>
        public WebResponseContent SaveFiles(
            List<IFormFile> lstFflFiles, 
            string strTypeCode, 
            string strIdentifyfolder = "", 
            Guid? masterId = null)
        {
            try
            {
                if (lstFflFiles == null || lstFflFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }

                // 获取文件保存路径（相对位置）
                var getFolderResult = GetFileSaveFolder(strTypeCode, strIdentifyfolder);
                if (!getFolderResult.status)
                {
                    return getFolderResult;
                }

                // 存放文件
                var strSaveFolder = getFolderResult.data as string;

                
                // 存放文件
                var saveFileResult = SaveFileByPath(lstFflFiles, strSaveFolder);
                if (!saveFileResult.status)
                {
                    return saveFileResult;
                }

                // 保存文件数据
                var lstFileInfo = saveFileResult.data as List<FileInfoDto>;
                lstFileInfo.ForEach(p => p.id = Guid.NewGuid());
                var recordResult = RecordFileInfos(lstFileInfo, masterId, strTypeCode);
                if (!recordResult.status)
                {
                    return recordResult;
                }

                // 清空路径
                foreach (var item in lstFileInfo)
                {
                    item.file_relative_path = string.Empty;
                }

                return WebResponseContent.Instance.OK(
                    _localizationService.GetString("operation_success"),
                    lstFileInfo);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 保存文件到临时目录（未实现savechange）
        /// </summary>
        /// <param name="lstFflFiles"></param>
        /// <returns></returns>
        public WebResponseContent SaveFilesToTemp(List<IFormFile> lstFflFiles, string strTypeCode)
        {
            try
            {
                // 相对路径
                var strAbsFolder = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var companyData = _companyRepository.FindFirst(p => p.id == UserContext.Current.UserInfo.Company_Id);
                if (companyData != null)
                {
                    strAbsFolder = companyData.company_no + $"\\{strAbsFolder}";
                }
                // 保存的路径
                var strSaveFolder = Path.Combine(_strTemporaryFolder, strAbsFolder);
                if (!Directory.Exists(strSaveFolder))
                {
                    Directory.CreateDirectory(strSaveFolder);
                }

                var lstDtoInfos = new List<FileInfoDto>();
                foreach (var fflFile in lstFflFiles)
                {
                    var strExt = Path.GetExtension(fflFile.FileName).TrimStart('.').ToLower(); // 扩展名
                    var strFileName = fflFile.FileName;

                    // 如果文件存在则添加随机数和时间戳
                    if (File.Exists(Path.Combine(strSaveFolder, fflFile.FileName)))
                    {
                        strFileName = $"{Path.GetFileNameWithoutExtension(fflFile.FileName)}_{CommonHelper.GenerateRandomDigitString()}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.{strExt}"; // 存放的物理文件名
                    }
                    // 保存数据库中的相对路径
                    var strFileRelPath = Path.Combine(strAbsFolder, strFileName);               

                    using (var stream = new FileStream(Path.Combine(strSaveFolder, strFileName), FileMode.Create))
                    {
                        fflFile.CopyTo(stream);
                    }

                    lstDtoInfos.add(new FileInfoDto
                    {
                        file_name = strFileName,
                        file_ext = strExt,
                        file_size = (int)fflFile.Length,
                        file_relative_path = strFileRelPath
                    });
                }

                // 保存文件数据
                lstDtoInfos.ForEach(p => p.id = Guid.NewGuid());
                var recordResult = RecordFileInfos(lstDtoInfos, null, strTypeCode);
                if (!recordResult.status)
                {
                    return recordResult;
                }

                // 清空路径
                foreach (var item in lstDtoInfos)
                {
                    item.file_relative_path = string.Empty;
                }
                return WebResponseContent.Instance.OK(
                    _localizationService.GetString("operation_success"),
                    lstDtoInfos);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取文件上传配置文件夹（相对位置）
        /// </summary>
        /// <param name="strFileCode">文件代码</param>
        /// <param name="strIdentifyfolder">文件夹标识名（放置的文件名，有些文件是根据报价、项目、合约下的）</param>
        /// <returns></returns>
        public WebResponseContent GetFileSaveFolder(string strFileCode, string strIdentifyfolder = "")
        {
            try
            {
                var fileConfigData = _configFileRepository.FindFirst(p => p.file_code == strFileCode && p.delete_status == (int)SystemDataStatus.Valid);
                if (fileConfigData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_config_null"));
                }
                fileConfigData.folder_path = fileConfigData.folder_path.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

                var companyData = _companyRepository.FindFirst(p => p.id == UserContext.Current.UserInfo.Company_Id);
                if (companyData != null)
                {
                    strIdentifyfolder = string.IsNullOrEmpty(strIdentifyfolder) ?
                        companyData.company_no :
                        Path.Combine(companyData.company_no, strIdentifyfolder);
                }
                
                var strSaveFolder = !string.IsNullOrEmpty(strIdentifyfolder) ?
                    Path.Combine(strIdentifyfolder, fileConfigData.folder_path) :
                    fileConfigData.folder_path;

                return WebResponseContent.Instance.OK("ok", strSaveFolder);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 保存文件到指定文件夹
        /// </summary>
        /// <param name="fflFiles">文件</param>
        /// <param name="strRelPath">文件存放相对位置</param>
        /// <returns></returns>
        public WebResponseContent SaveFileByPath(List<IFormFile> fflFiles, string strRelPath)
        {
            try
            {
                if (string.IsNullOrEmpty(strRelPath))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_path_null"));
                }

                if (!Directory.Exists(Path.Combine(_strUploadPath, strRelPath)))
                {
                    Directory.CreateDirectory(Path.Combine(_strUploadPath, strRelPath));
                }

                // 保存文件
                var lstDtoInfos = new List<FileInfoDto>();
                foreach (var fflFile in fflFiles)
                {
                    var strExt = Path.GetExtension(fflFile.FileName).TrimStart('.').ToLower(); // 扩展名
                    var strFileName = fflFile.FileName;
                    var strAbsFolder = Path.Combine(_strUploadPath, strRelPath);               // 绝对路径文件夹
                    // 如果文件存在则添加随机数和时间戳
                    if (File.Exists(Path.Combine(strAbsFolder, fflFile.FileName)))
                    {
                        strFileName = $"{Path.GetFileNameWithoutExtension(fflFile.FileName)}_{CommonHelper.GenerateRandomDigitString()}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.{strExt}"; // 存放的物理文件名
                    }
                    var strFileRelPath = Path.Combine(strRelPath, strFileName);               // 保存数据库中的相对路径

                    using (var stream = new FileStream(Path.Combine(_strUploadPath, strFileRelPath), FileMode.Create))
                    {
                        fflFile.CopyTo(stream);
                    }

                    lstDtoInfos.add(new FileInfoDto
                    {
                        //file_name = strFileName,
                        file_name = fflFile.FileName,
                        file_ext = strExt,
                        file_size = (int)fflFile.Length,
                        file_relative_path = strFileRelPath
                    });
                }

                return WebResponseContent.Instance.OK("ok", lstDtoInfos);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 记录文件信息（没有执行savechange）
        /// </summary>
        /// <param name="lstFileInfos">文件信息</param>
        /// <param name="guidMasterId">所属表id（可为空）</param>
        /// <param name="strTypeCode">文件类型代码</param>
        /// <returns></returns>
        public WebResponseContent RecordFileInfos(List<FileInfoDto> lstFileInfos, Guid? guidMasterId, string strTypeCode)
        {
            try
            {
                var lstRecordData = new List<Sys_File_Records>();
                foreach (var record in lstFileInfos) 
                {
                    if (!record.id.HasValue)
                    {
                        record.id = Guid.NewGuid();
                    }

                    lstRecordData.add(new Sys_File_Records
                    {
                        id = record.id.Value,
                        file_code = strTypeCode,
                        file_name = record.file_name,
                        file_ext = record.file_ext,
                        file_size = record.file_size,
                        file_path = record.file_relative_path,

                        delete_status = (int)SystemDataStatus.Valid,
                        create_id = UserContext.Current.UserId,
                        create_name = UserContext.Current.UserName,
                        create_date = DateTime.Now,
                        master_id = guidMasterId,
                        upload_status = guidMasterId.HasValue ? (int)UploadStatus.Finish : (int)UploadStatus.Upload
                    });
                    
                }

                _repository.AddRange(lstRecordData);

                return WebResponseContent.Instance.OK("ok");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 文件下载、转换

        /// <summary>
        /// 文件通用下载（单个）
        /// </summary>
        /// <param name="guidFileId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DownloadSysFile(Guid guidFileId, string strSaveFolder = "")
        {
            try
            {
                var file = await repository.FindAsyncFirst(p => p.id == guidFileId && p.delete_status == 0);
                if (file == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_info_null"));
                }

                // 检查文件是否存在
                strSaveFolder = string.IsNullOrEmpty(strSaveFolder) ? _strUploadPath : strSaveFolder;
                var strFilePath = Path.Combine(strSaveFolder, file.file_path);
                if (!File.Exists(strFilePath))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_null"));
                }

                var bytsFile = File.ReadAllBytes(strFilePath);   // 读取文件内容
                var strContentType = GetContentType(strFilePath);// 获取文件的MIME类型

                return WebResponseContent.Instance.OK(
                    _localizationService.GetString("operation_success"),
                    new FileDownLoadDto
                    {
                        cntent_type = strContentType,
                        file_bytes = bytsFile,
                        file_name = file.file_name
                    });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.DownLoadSysFile.文件下载异常", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 下载多个文件（返回zip）
        /// </summary>
        /// <param name="guidFileIds"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DownloadMultipleFiles(List<Guid> guidFileIds)
        {
            try
            {
                var files = await repository.FindAsync(p => guidFileIds.Contains(p.id) && p.delete_status == 0);
                if (files == null || files.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_info_null"));
                }

                var zipData = await ZipFileAsync(files);

                return WebResponseContent.Instance.OK(
                            _localizationService.GetString("operation_success"),
                            zipData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 压缩文件（内存）
        /// </summary>
        /// <param name="lstFileRecords"></param>
        /// <param name="isWithParentLevel"></param>
        /// <returns></returns>
        public async Task<FileDownLoadDto> ZipFileAsync(List<Sys_File_Records> lstFileRecords, bool isWithParentLevel = false)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var item in lstFileRecords)
                    {
                        var filePath = Path.Combine(_strUploadPath, item.file_path);
                        if (!File.Exists(filePath))
                        {
                            continue;
                        }
                        // 获取文件名
                        var fileName = Path.GetFileName(filePath);

                        if (isWithParentLevel)
                        {
                            // 解析路径，获取仅包含上一层级目录和文件名的结构
                            fileName = GetParentLevelPath(item.file_path);

                            // 统一路径分隔符为ZIP标准的正斜杠
                            fileName = fileName.Replace(Path.DirectorySeparatorChar, '/');
                        }

                        // 在ZIP归档中创建条目
                        var entry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                        // 将文件内容写入ZIP条目
                        using (var entryStream = entry.Open())
                        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, true))
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }

                return new FileDownLoadDto
                {
                    cntent_type = "application/zip",
                    file_bytes = memoryStream.ToArray(),
                    file_name = DateTime.Now.ToString("yyyyMMddHHmmssfff")
                };
            }
        }

        /// <summary>
        /// 提取路径中的上一层级目录和文件名（仅保留最后两级）
        /// </summary>
        /// <param name="fullPath">完整路径（相对路径或绝对路径）</param>
        /// <returns>仅包含上一层级目录和文件名的路径</returns>
        private string GetParentLevelPath(string fullPath)
        {
            // 获取文件名（如 "file.txt"）
            string fileName = Path.GetFileName(fullPath);
            if (string.IsNullOrEmpty(fileName))
            {
                return fullPath; // 处理异常情况
            }

            // 获取文件所在的直接目录（上一层级目录，如 "folder2"）
            string fullDirectory = Path.GetDirectoryName(fullPath);
            if (string.IsNullOrEmpty(fullDirectory))
            {
                // 如果没有上层目录，直接返回文件名
                return fileName;
            }

            // 提取直接目录的名称（忽略更上层的路径）
            string parentDirectory = Path.GetFileName(fullDirectory);

            // 组合成 "上层目录/文件名" 的结构
            return Path.Combine(parentDirectory, fileName);
        }


        /// <summary>
        /// office文件转换pdf（根据文件id）
        /// </summary>
        /// <param name="guidFileId">系统文件表id（Sys_File_Records）</param>
        /// <param name="strSaveFolder">文件是存储在特定的文件夹的（默认空的，取配置文件中的地址）</param>
        /// <returns></returns>
        public async Task<WebResponseContent> FileConvertPDFById(Guid guidFileId, string strSaveFolder = "")
        {
            try
            {
                var file = await repository.FindAsyncFirst(p => p.id == guidFileId && p.delete_status == 0);
                if (file == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_info_null"));
                }
                // 检查文件是否存在
                strSaveFolder = string.IsNullOrEmpty(strSaveFolder) ? _strUploadPath : strSaveFolder;
                var strFilePath = Path.Combine(strSaveFolder, file.file_path);
                if (!File.Exists(strFilePath))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_null"));
                }

                // 生成的pdf路径
                var strPdfPath = ChangePdf(strFilePath);
                if (string.IsNullOrEmpty(strPdfPath))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                }

                return WebResponseContent.Instance.OK(
                    _localizationService.GetString("operation_success"),
                    strPdfPath);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.FileConvertPdf.文件转换pdf异常", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 文件转换pdf
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public WebResponseContent FileConvertPDFByPath(string strFilePath)
        {
            try
            {
                if (!File.Exists(strFilePath))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_null"));
                }

                var strPdfPath = ChangePdf(strFilePath);
                if (string.IsNullOrEmpty(strPdfPath))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                }

                return WebResponseContent.Instance.OK(strPdfPath);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.FileConvertPDFByPath.文件转换pdf异常", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 文件上传状态更改

        /// <summary>
        /// 更改文件状态及所属id
        /// </summary>
        /// <param name="litFileIds"></param>
        /// <param name="strMasterId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> FinishFilesUploadAsync(
            List<Guid> litFileIds,
            Guid strMasterId)
        {
            try
            {
                var lstFile = await repository.FindAsync(p => litFileIds.Contains(p.id));
                lstFile.ForEach(file =>
                {
                    file.master_id = strMasterId;
                    file.upload_status = 1;
                    file.modify_id = UserContext.Current.UserInfo.User_Id;
                    file.modify_name = UserContext.Current.UserInfo.UserName;
                    file.modify_date = DateTime.Now;
                });
                repository.UpdateRange(lstFile);
                await repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.FinishFilesUploadAsync.更改文件状态及所属id", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 更改文件状态及所属id（未实现savechange功能）
        /// </summary>
        /// <param name="litFileIds"></param>
        /// <param name="strMasterId"></param>
        /// <returns></returns>
        public WebResponseContent FinishFilesUploadNoSaveChange(
            List<Guid> litFileIds,
            Guid strMasterId)
        {
            try
            {
                var lstFile = repository.Find(p => litFileIds.Contains(p.id));
                lstFile.ForEach(file =>
                {
                    file.master_id = strMasterId;
                    file.upload_status = 1;
                    file.modify_id = UserContext.Current.UserInfo.User_Id;
                    file.modify_name = UserContext.Current.UserInfo.UserName;
                    file.modify_date = DateTime.Now;
                });
                repository.UpdateRange(lstFile);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.FinishFilesUploadAsync.更改文件状态及所属id", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 更改文件状态及所属id（未实现savechange功能）
        /// </summary>
        /// <param name="litFileInfo"></param>
        /// <param name="strMasterId"></param>
        /// <param name="isMoveFile"></param>
        /// <returns></returns>
        public WebResponseContent FinishFilesUploadNoSaveChange(
            List<ContractQnFileDto> litFileInfo,
            Guid strMasterId)
        {
            try
            {
                var dicFile = litFileInfo.ToDictionary(p => p.file_id);
                var lstFileIds = dicFile.Keys.ToList();

                var lstFile = repository.Find(p => lstFileIds.Contains(p.id));

                lstFile.ForEach(file =>
                {
                    var isOk = dicFile.TryGetValue(file.id, out var fileInfo);
                    if (isOk)
                    {
                        file.master_id = strMasterId;
                        file.upload_status = 1;
                        file.modify_id = UserContext.Current.UserInfo.User_Id;
                        file.modify_name = UserContext.Current.UserInfo.UserName;
                        file.modify_date = DateTime.Now;
                        file.remark = fileInfo.file_remark;
                    }
                });
                repository.UpdateRange(lstFile);


                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.FinishFilesUploadAsync.更改文件状态及所属id", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 移动文件并更改文件状态及所属id（未实现savechange功能）
        /// </summary>
        /// <param name="litFileInfo"></param>
        /// <param name="strMasterId"></param>
        /// <param name="isMoveFile"></param>
        /// <returns></returns>
        public WebResponseContent FinishFilesUploadNoSaveChange(
            List<ContractQnFileDto> litFileInfo,
            Guid strMasterId,
            bool isMoveFile = false,
            string strMoveFolder = "",
            string strFileTypeCode = "")
        {
            try
            {
                var dicFile = litFileInfo.Where(p => p.file_id.HasValue).ToDictionary(p => p.file_id);
                var lstFileIds = dicFile.Keys.ToList();

                var lstFile = repository.Find(p => lstFileIds.Contains(p.id));

                var strRelPath = ""; // 保存相对路径的文件夹（如果isMoveFile为true才使用）
                if (isMoveFile && lstFile.Count > 0)
                {
                    var fileConfigData = _configFileRepository.FindFirst(p => p.file_code == strFileTypeCode && p.delete_status == (int)SystemDataStatus.Valid);
                    if (fileConfigData == null)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("file_config_null"));
                    }
                    fileConfigData.folder_path = fileConfigData.folder_path.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    strMoveFolder = Path.Combine(strMoveFolder, fileConfigData.folder_path);
                    if (!Directory.Exists(strMoveFolder))
                    {
                        Directory.CreateDirectory(strMoveFolder);
                    }
                    strRelPath = strMoveFolder;
                }
               
                lstFile.ForEach(file =>
                {
                    var isOk = dicFile.TryGetValue(file.id, out var fileInfo);
                    if (isOk)
                    {
                        if (isMoveFile)
                        {
                            var fileTempPath = Path.Combine(_strTemporaryFolder, file.file_path);
                            strRelPath = strRelPath.Replace(_strUploadPath, string.Empty);
                            strRelPath = Path.Combine(strRelPath, file.file_name);
                            if (File.Exists(fileTempPath))
                            {
                                File.Copy(fileTempPath, Path.Combine(strMoveFolder, file.file_name), overwrite: true);
                                file.file_path = strRelPath;
                            }
                        }
                        file.master_id = strMasterId;
                        file.upload_status = 1;
                        file.modify_id = UserContext.Current.UserInfo.User_Id;
                        file.modify_name = UserContext.Current.UserInfo.UserName;
                        file.modify_date = DateTime.Now;
                        file.remark = fileInfo.file_remark;
                    }
                });
                repository.UpdateRange(lstFile);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.FinishFilesUploadAsync.更改文件状态及所属id", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除文件（未执行savechange）
        /// （修改记录状态，移动文件到临时目录）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public WebResponseContent DeleteUploadFiles(List<Sys_File_Records> data)
        {
            try
            {
                if (data.Count == 0)
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }

                foreach (var fileRecord in data)
                {
                    fileRecord.delete_status = (int)SystemDataStatus.Invalid;
                    fileRecord.modify_id = UserContext.Current.UserInfo.User_Id;
                    fileRecord.modify_name = UserContext.Current.UserInfo.UserName;
                    fileRecord.modify_date = DateTime.Now;

                    fileRecord.file_path = MoveFileToTemporary(fileRecord.file_path, fileRecord.file_name, fileRecord.file_ext);
                }

                _repository.UpdateRange(data);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.DeleteUploadFiles.删除文件", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除文件（未执行savechange）
        /// （修改记录状态，移动文件到临时目录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteUploadFiles(List<Guid> ids)
        {
            try
            {
                var fileRecords = await _repository.FindAsync(p => ids.Contains(p.id));
                if (fileRecords.Count == 0)
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }

                foreach (var fileRecord in fileRecords)
                {
                   fileRecord.delete_status = (int)SystemDataStatus.Invalid;
                   fileRecord.modify_id = UserContext.Current.UserInfo.User_Id;
                   fileRecord.modify_name = UserContext.Current.UserInfo.UserName;
                   fileRecord.modify_date = DateTime.Now;
                   fileRecord.file_path = MoveFileToTemporary(fileRecord.file_path, fileRecord.file_name, fileRecord.file_ext);
                }        

                _repository.Update(fileRecords);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.DeleteUploadFiles.删除文件", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 通用帮助

        /// <summary>
        /// 获取文件的MIME类型
        /// </summary>
        public string GetContentType(string filePath)
        {
            var contentType = "application/octet-stream";
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            // 根据文件扩展名设置MIME类型
            var mimeTypes = new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/msword"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".zip", "application/zip"}
            };

            if (mimeTypes.ContainsKey(extension))
            {
                contentType = mimeTypes[extension];
            }

            return contentType;
        }

        /// <summary>
        /// 转换pdf
        /// </summary>
        /// <param name="strInputPath"></param>
        /// <returns></returns>
        private string ChangePdf(string strInputPath)
        {

            if (string.IsNullOrEmpty(_strTemporaryFolder))
            {
                throw new ArgumentNullException(_localizationService.GetString("setting_find_null"));
            }
            var strPdfFolder = Path.Combine(_strTemporaryFolder, $"{UserContext.Current.UserId}\\{DateTime.Now.ToString("yyyMMddHHmmss")}\\");
            if (!Directory.Exists(strPdfFolder))
            {
                Directory.CreateDirectory(strPdfFolder);
            }
            var strPdfPath = Path.Combine(strPdfFolder, $"{DateTime.Now.ToString("yyyMMddHHmmss")}.pdf");
            var strInputCopyPath = Path.Combine(strPdfFolder, Path.GetFileName(strInputPath));
            try
            {
                File.Copy(strInputPath, strInputCopyPath, true);


                //var bolOk = await _IOfficeConversionService.ConvertToPdfAsync(strInputCopyPath, strPdfPath);

                var bolOk = PDFHelper.ConvertToPdfUsingLibreOffice(strInputCopyPath, strPdfPath);
                return bolOk ? strPdfPath : string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                if (File.Exists(strInputCopyPath) && strInputCopyPath.startsWith(_strTemporaryFolder))
                {
                    File.Delete(strInputCopyPath);
                }
            }
        }

        #endregion

        #region 旧的

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fflFile">要上传的文件</param>
        /// <param name="strTypeCode">文件代码（用于区分是什么文件，来源与字典）</param>
        /// <param name="strSaveFolder">文件存储文件夹</param>
        /// <param name="intStatus">上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）</param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadFileAsync(
            IFormFile fflFile,
            string strTypeCode,
            int intStatus = 1,
            string strSaveFolder = "")
        {
            try
            {
                if (fflFile == null || fflFile.Length == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }

                // 文件存放路径
                strSaveFolder = string.IsNullOrEmpty(strSaveFolder) ? _strUploadPath : strSaveFolder;
                if (string.IsNullOrEmpty(strSaveFolder))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("setting_find_null"));
                }


                // 获取文件名、扩展、相对路径
                var strExt = Path.GetExtension(fflFile.FileName).TrimStart('.').ToLower();
                var strFileName = $"{DateTime.Now.ToString("yyyMMddHHmmss")}.{strExt}"; // 存放的物理文件名
                var strRelFolder = $"{UserContext.Current.UserId}\\{strTypeCode}\\{DateTime.Now.ToString("yyyMMdd")}";
                var strFileRelPath = Path.Combine(strRelFolder, strFileName);  // 保存数据库中的相对路径
                strSaveFolder = Path.Combine(strSaveFolder, strRelFolder);     // 存放的物理路径


                // 存放文件
                if (!Directory.Exists(strSaveFolder))
                {
                    Directory.CreateDirectory(strSaveFolder);
                }
                using (var stream = new FileStream(Path.Combine(strSaveFolder, strFileName), FileMode.Create))
                {
                    await fflFile.CopyToAsync(stream);
                }

                // 保存数据库
                var fileData = new Sys_File_Records
                {
                    id = Guid.NewGuid(),
                    file_name = fflFile.FileName,
                    file_ext = strExt,
                    file_size = (int)fflFile.Length,
                    file_path = strFileRelPath,
                    file_code = strTypeCode,
                    delete_status = 0,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now,
                    upload_status = intStatus
                };
                await _repository.AddAsync(fileData);
                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(
                    _localizationService.GetString("operation_success"),
                    new FileInfoDto
                    {
                        id = fileData.id,
                        file_name = fileData.file_name,
                        file_ext = strExt,
                        file_size = fileData.file_size.HasValue ? fileData.file_size.Value : 0,
                    });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.UploadFileAsync.文件上传异常", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }


        /// <summary>
        /// 上传文件（多个）
        /// </summary>
        /// <param name="lstFflFiles">要上传的文件集合</param>
        /// <param name="strTypeCode">文件代码（用于区分是什么文件）</param>
        /// <param name="strSaveFolder">文件存储文件夹</param>
        /// <param name="intStatus">上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）</param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadFilesAsync(
            List<IFormFile> lstFflFiles,
            string strTypeCode,
            int intStatus = 1,
            string strSaveFolder = "")
        {
            try
            {
                if (lstFflFiles == null || lstFflFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }


                // 文件存放路径
                strSaveFolder = string.IsNullOrEmpty(strSaveFolder) ? _strUploadPath : strSaveFolder;
                if (string.IsNullOrEmpty(strSaveFolder))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("setting_find_null"));
                }

                // 存放路径
                var strRelFolder = $"{UserContext.Current.UserId}\\{strTypeCode}\\{DateTime.Now.ToString("yyyyMMdd")}"; //相对位置文件夹
                strSaveFolder = Path.Combine(strSaveFolder, strRelFolder);// 存放的物理文件夹路径

                var lstFileData = new List<Sys_File_Records>();
                var lstFileDto = new List<FileInfoDto>();
                if (!Directory.Exists(strSaveFolder))
                {
                    Directory.CreateDirectory(strSaveFolder);
                }
                for (int i = 0; i < lstFflFiles.Count; i++)
                {
                    var fflFile = lstFflFiles[i];

                    var strExt = Path.GetExtension(fflFile.FileName).TrimStart('.').ToLower();
                    var strFileName = $"{DateTime.Now.ToString("yyyMMddHHmmss")}.{strExt}"; // 存放的物理文件名
                    var strFileRelPath = Path.Combine(strRelFolder, strFileName);// 保存数据库中的相对路径

                    using (var stream = new FileStream(Path.Combine(strSaveFolder, strFileName), FileMode.Create))
                    {
                        await fflFile.CopyToAsync(stream);
                    }

                    var fileData = new Sys_File_Records
                    {
                        id = Guid.NewGuid(),
                        file_name = fflFile.FileName,
                        file_ext = strExt,
                        file_size = (int)fflFile.Length,
                        file_path = strFileRelPath,
                        file_code = strTypeCode,
                        delete_status = 1,
                        create_id = UserContext.Current.UserId,
                        create_name = UserContext.Current.UserName,
                        create_date = DateTime.Now,
                        upload_status = intStatus
                    };
                    lstFileData.Add(fileData);

                    lstFileDto.Add(new FileInfoDto
                    {
                        id = fileData.id,
                        file_name = fileData.file_name,
                        file_ext = fileData.file_ext,
                        file_size = fileData.file_size.HasValue ? fileData.file_size.Value : 0
                    });
                }

                await _repository.AddRangeAsync(lstFileData);
                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(
                    _localizationService.GetString("operation_success"),
                    lstFileDto);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_File_RecordsService.UploadFilesAsync.文件上传异常", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 文件移动

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="strRelFolder"></param>
        /// <param name="fileRecordData"></param>
        /// <returns></returns>
        public string MoveFile(string strRelFolder, Sys_File_Records fileRecordData)
        {
            var strAbsFolder = Path.Combine(_strUploadPath, strRelFolder);
            if (!Directory.Exists(strAbsFolder))
            {
                Directory.CreateDirectory(strAbsFolder);
            }
            var strTempFilePath = Path.Combine(_strTemporaryFolder, fileRecordData.file_path);
            if (!File.Exists(strTempFilePath)) 
            {
                return fileRecordData.file_path;
            }
            // 判断文件是否存在，存在则需要添加后缀
            var strRelFilePath = Path.Combine(strRelFolder, fileRecordData.file_name); // 相对
            var strAbcFilePath = Path.Combine(_strUploadPath, strRelFilePath);         // 绝对
            if (File.Exists(strAbcFilePath))
            {
                var strFileName = $"{Path.GetFileNameWithoutExtension(fileRecordData.file_name)}_{CommonHelper.GenerateRandomDigitString()}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.{fileRecordData.file_ext}"; // 存放的物理文件名
                strRelFilePath = Path.Combine(strRelFolder, strFileName);      // 相对
                strAbcFilePath = Path.Combine(_strUploadPath, strRelFilePath); // 绝对
            }
            File.Copy(strTempFilePath, strAbcFilePath, true);

            return strRelFilePath;
        }

        /// <summary>
        /// 将文件移动到临时目录
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public string MoveFileToTemporary(string filePath, string fileName, string fileExt)
        {
            // 将物理文件从上传目录移到临时目录（保留相对目录结构）
            var srcAbsPathUpload = Path.Combine(_strUploadPath, filePath);
            var relFolder = Path.GetDirectoryName(filePath) ?? string.Empty;
            var dstAbsFolder = Path.Combine(_strTemporaryFolder, relFolder);
            if (!Directory.Exists(dstAbsFolder))
            {
                Directory.CreateDirectory(dstAbsFolder);
            }

            var dstRelPath = Path.Combine(relFolder, fileName);
            var dstAbsPath = Path.Combine(_strTemporaryFolder, dstRelPath);

            if (File.Exists(srcAbsPathUpload))
            {
                // 如果目标已存在，避免覆盖，按现有风格追加随机+时间戳
                if (File.Exists(dstAbsPath))
                {
                    var uniqueName = $"{Path.GetFileNameWithoutExtension(fileName)}_{CommonHelper.GenerateRandomDigitString()}_{DateTime.Now:yyyyMMddHHmmssfff}.{fileExt}";
                    dstRelPath = Path.Combine(relFolder, uniqueName);
                    dstAbsPath = Path.Combine(_strTemporaryFolder, dstRelPath);
                }
                // 移动至临时目录
                File.Move(srcAbsPathUpload, dstAbsPath);
                // 更新记录中的相对路径为临时目录中的路径，便于后续恢复或查看
                return dstRelPath;
            }

            return string.Empty;
        }

        #endregion
    }
}
