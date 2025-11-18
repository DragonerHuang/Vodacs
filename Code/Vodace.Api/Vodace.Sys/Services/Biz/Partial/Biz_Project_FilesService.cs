
using Castle.Core.Internal;
using Dm;
using Dm.util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Configuration;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Biz_Project_FilesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Project_FilesRepository _repository;//访问数据库

        private readonly IBiz_ContractRepository _contractRepository;       // 合同仓储
        private readonly IBiz_QuotationRepository _quotationRepository;     // 报价仓储
        private readonly ISys_CompanyRepository _companyRepository;         // 公司仓储
        private readonly ISys_File_ConfigRepository _configFileRepository;  // 文件配置仓储

        private readonly ILocalizationService _localizationService;     // 国际化
        private readonly ISys_File_ConfigService _fileConfigService;    // 文件配置服务
        private readonly ISys_File_RecordsService _fileRecordsService;  // 文件记录服务

        [ActivatorUtilitiesConstructor]
        public Biz_Project_FilesService(
            IBiz_Project_FilesRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IBiz_ContractRepository contractRepository,
            IBiz_QuotationRepository quotationRepository,
            ISys_CompanyRepository companyRepository,
            ISys_File_ConfigRepository configFileRepository,
            ISys_File_ConfigService fileConfigService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _contractRepository = contractRepository;
            _quotationRepository = quotationRepository;
            _companyRepository = companyRepository;
            _configFileRepository = configFileRepository;
            _fileConfigService = fileConfigService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }


        #region 获取对应的文件夹

        /// <summary>
        /// 根据合同id获取文件放置文件夹
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="fileCode"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetFileFolderByContactIdAsync(Guid contractId, string fileCode = "")
        {
            try
            {
                // 获取合同记录
                var ctrData = await _contractRepository.FindAsIQueryable(p => p.id == contractId).AsNoTracking().FirstOrDefaultAsync();
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }
                // 获取报价
                var qnData = await _quotationRepository.FindAsIQueryable(p => p.contract_id == ctrData.id).AsNoTracking().FirstOrDefaultAsync();
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }

                // 加上项目文件夹
                var getProFolderResult = await _fileConfigService.GetMainProFolderNameAsync(qnData.create_date.Value);
                if (!getProFolderResult.status)
                {
                    return getProFolderResult;
                }
              
                var fileFolder = $"{getProFolderResult.data as string}\\{qnData.qn_no}";

                // 加上公司文件夹
                var companyData = _companyRepository.FindFirst(p => p.id == UserContext.Current.UserInfo.Company_Id);
                if (companyData != null)
                {
                    fileFolder = $"{companyData.company_no}\\{fileFolder}";
                }

                // 根据文件类型，获取配置路径
                if (!string.IsNullOrEmpty(fileCode))
                {
                    var fileConfigData = await _configFileRepository
                        .FindFirstAsync(p => p.file_code == fileCode && p.delete_status == (int)SystemDataStatus.Valid);
                    if (fileConfigData == null)
                    {
                        return WebResponseContent.Instance.OK("ok", fileFolder);
                    }
                    if (!string.IsNullOrEmpty(fileConfigData.folder_path))
                    {
                        fileConfigData.folder_path = fileConfigData.folder_path.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        fileFolder = Path.Combine(fileFolder, fileConfigData.folder_path);
                    }
                }
                if (!Directory.Exists(Path.Combine( AppSetting.FileSaveSettings.FolderPath, fileFolder)))
                {
                    Directory.CreateDirectory(Path.Combine( AppSetting.FileSaveSettings.FolderPath, fileFolder)); 
                }

                return WebResponseContent.Instance.OK("ok", fileFolder);

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 预览

        /// <summary>
        /// 预览文件（文件流）
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> ViewFileByteByIdAsync(Guid id)
        {
            try
            {
                var fileData = await _repository.FindFirstAsync(p => p.id == id);

                var strSaveFolder = AppSetting.FileSaveSettings.FolderPath;
                if (fileData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_null"));
                }

                var relPathFixed = fileData.file_path?.TrimStart('\\', '/');
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
                        file_name = fileData.file_name
                    });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 预览文件（url）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ViewPhotoUrlByIdAsync(Guid id)
        {
            try
            {
                var fileData = await _repository.FindFirstAsync(p => p.id == id);


                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), new FileInfoDto
                {
                    id = fileData.id,
                    file_name = fileData.file_name,
                    file_ext = fileData.file_ext,
                    file_size = fileData.file_size.HasValue ? fileData.file_size.Value : 0,
                    file_relative_path = fileData.file_path.ToUrl()
                });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 上传文件

        /// <summary>
        /// 上传文件到临时目录
        /// </summary>
        /// <param name="lstFflFiles"></param>
        /// <param name="fileType"></param>
        /// <param name="isSaveChange"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadFileToTempAsync(List<IFormFile> lstFflFiles, int fileType, bool isSaveChange = false)
        {
            try
            {
                var strAbsFolder = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // 相对路径
                var companyData = _companyRepository.FindFirst(p => p.id == UserContext.Current.UserInfo.Company_Id);
                if (companyData != null)
                {
                    strAbsFolder = companyData.company_no + $"\\{strAbsFolder}";
                }
                // 保存的路径
                var strSaveFolder = Path.Combine(AppSetting.FileSaveSettings.TemporaryFolder, strAbsFolder);
                if (!Directory.Exists(strSaveFolder))
                {
                    Directory.CreateDirectory(strSaveFolder);
                }

                var lstDtoInfos = new List<FileInfoDto>();
                var lstFileDto = new List<Biz_Project_Files>();
                var intIndex = 0;
                foreach (var fflFile in lstFflFiles)
                {
                    var strExt = Path.GetExtension(fflFile.FileName).TrimStart('.').ToLower(); // 扩展名
                    var strFileName = fflFile.FileName;

                    // 如果文件存在则添加随机数和时间戳
                    if (File.Exists(Path.Combine(strSaveFolder, fflFile.FileName)))
                    {
                        // 存放的物理文件名
                        strFileName = $"{Path.GetFileNameWithoutExtension(fflFile.FileName)}" +
                                      $"_{CommonHelper.GenerateRandomDigitString()}" +
                                      $"_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}" +
                                      $".{strExt}"; 
                    }

                    // 保存数据库中的相对路径
                    var strFileRelPath = Path.Combine(strAbsFolder, strFileName);

                    using (var stream = new FileStream(Path.Combine(strSaveFolder, strFileName), FileMode.Create))
                    {
                       await fflFile.CopyToAsync(stream);
                    }

                    var data = new Biz_Project_Files
                    {
                        id = Guid.NewGuid(),
                        file_type = fileType,
                        file_index = intIndex,
                        file_name = strFileName,
                        file_path = strFileRelPath,
                        file_ext = strExt,
                        file_size = (int)fflFile.Length,
                        delete_status = (int)SystemDataStatus.Valid,
                        upload_status = (int)UploadStatus.Upload,
                        create_date = DateTime.Now,
                        create_id = UserContext.Current.UserInfo.User_Id,
                        create_name = UserContext.Current.UserInfo.UserName
                    };

                    lstFileDto.add(data);

                    lstDtoInfos.add(new FileInfoDto
                    {
                        id = data.id,
                        file_name = strFileName,
                        file_ext = strExt,
                        file_size = (int)fflFile.Length
                    });
                    intIndex++;
                }
               
                _repository.AddRange(lstFileDto);

                if (isSaveChange)
                {
                    await _repository.SaveChangesAsync();
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstFileDto);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 完成总体操作后，将临时目录移动到正式目录中
        /// </summary>
        /// <param name="fileInfos">文件信息</param>
        /// <param name="fileTypeCode">文件类型代码（用于查找放置的文件夹）</param>
        /// <param name="contractId">合同id</param>
        /// <param name="relationId">关联id</param>
        /// <param name="isSaveChange">是否触发SaveChange，默认不触发</param>
        /// <returns></returns>
        public async Task<WebResponseContent> MoveFileToFolderAsync(
            List<UFileInfoDto> fileInfos, 
            string fileTypeCode, 
            Guid contractId, 
            Guid relationId, 
            bool isSaveChange = false)
        {
            try
            {
                // 获取文件记录
                var dicFile = fileInfos.ToDictionary(p => p.id);
                var lstFileIds = dicFile.Keys.ToList();
                var lstFile = await _repository.FindAsync(p => lstFileIds.Contains(p.id) && p.upload_status != (int)UploadStatus.Finish);
         
                var getRelFolderResult = await GetFileFolderByContactIdAsync(contractId, fileTypeCode); // 文件夹相对路径
                if (!getRelFolderResult.status)
                {
                    return getRelFolderResult;
                }
                var strRelFolder = getRelFolderResult.data as string; // 相对路径
                foreach (var item in lstFile)
                {
                    var isOk = dicFile.TryGetValue(item.id, out var fileData);

                    var fileTempPath = Path.Combine(AppSetting.FileSaveSettings.TemporaryFolder, item.file_path); // 临时目录中的文件
                    var fileRelPath = strRelFolder + $"\\{item.file_name}";                                       // 正式目录相对文件位置
                    var fileAbsPath = Path.Combine(AppSetting.FileSaveSettings.FolderPath, fileRelPath);          // 正式目录绝对文件位置

                    if (File.Exists(fileTempPath))
                    {
                        File.Copy(fileTempPath, fileAbsPath, overwrite: true);
                        item.file_path = fileRelPath;
                    }

                    item.relation_id = relationId;
                    item.upload_status = 1;
                    item.modify_id = UserContext.Current.UserInfo.User_Id;
                    item.modify_name = UserContext.Current.UserInfo.UserName;
                    item.modify_date = DateTime.Now;
                    item.remark = fileData.file_remark;
                }

                _repository.UpdateRange(lstFile);

                if (isSaveChange)
                {
                    await _repository.SaveChangesAsync();
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }


        #endregion

        /// <summary>
        /// 删除文件记录（移动文件到临时目录）
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        public WebResponseContent DeleteFile(List<Biz_Project_Files> lstFiles)
        {
            try
            {
                if (lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }

                foreach (var fileRecord in lstFiles)
                {
                    fileRecord.delete_status = (int)SystemDataStatus.Invalid;
                    fileRecord.modify_id = UserContext.Current.UserInfo.User_Id;
                    fileRecord.modify_name = UserContext.Current.UserInfo.UserName;
                    fileRecord.modify_date = DateTime.Now;

                    if (!string.IsNullOrEmpty(fileRecord.file_path))
                    {
                        fileRecord.file_path = _fileRecordsService.MoveFileToTemporary(fileRecord.file_path, fileRecord.file_name, fileRecord.file_ext);
                    }
                    if (!string.IsNullOrEmpty(fileRecord.file_thumbnail_path))
                    {
                        fileRecord.file_thumbnail_path = _fileRecordsService.MoveFileToTemporary(fileRecord.file_thumbnail_path, fileRecord.file_thumbnail_name, fileRecord.file_ext);
                    }
                }

                _repository.UpdateRange(lstFiles);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 判断文件是否为图片文件
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        public bool CheckFileIsPhoto(List<IFormFile> lstFiles)
        {
            var lstFileType = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff", ".svg" };
            var isPic = false;
            foreach (var file in lstFiles)
            {
                if (file == null || file.Length == 0)
                {
                    isPic = false; // 排除空文件
                    break;
                }

                // 1. 校验MIME类型（图片MIME通常以"image/"开头）
                bool isImageMime = file.ContentType?.Trim().StartsWith("image/", StringComparison.OrdinalIgnoreCase) ?? false;

                // 2. 校验文件扩展名
                string extension = Path.GetExtension(file.FileName)?.Trim().ToLower();
                bool isImageExtension = !string.IsNullOrEmpty(extension) && lstFileType.Contains(extension);

                // 两者都满足则认为是图片
                isPic = isImageMime && isImageExtension;
                if (!isPic)
                {
                    break;
                }
            }

            return isPic;
        }
    }
}
