
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    public partial class Biz_Deadline_Management_FileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Deadline_Management_FileRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_CompanyRepository _companyRepository;
        private readonly IBiz_ContractRepository _contractRepository;
        private readonly ISys_File_ConfigService _fileConfigService;
        private readonly ISys_File_RecordsService _FileRecordsService;
        private readonly IBiz_Deadline_ManagementRepository _Deadline_ManagementRepository;
        private readonly IBiz_Project_FilesRepository _projectFilesRepository;
        private readonly IBiz_Project_FilesService _projectFilesService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_Deadline_Management_FileService(
            IBiz_Deadline_Management_FileRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            ILocalizationService localizationService,
            IMapper mapper,
            ISys_CompanyRepository companyRepository,
            IBiz_ContractRepository contractRepository,
            ISys_File_ConfigService fileConfigService,
            ISys_File_RecordsService fileRecordsService,
            IBiz_Deadline_ManagementRepository deadline_ManagementRepository,
            IBiz_Project_FilesRepository projectFilesRepository,
            IBiz_Project_FilesService projectFilesService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _contractRepository = contractRepository;
            _fileConfigService = fileConfigService;
            _FileRecordsService = fileRecordsService;
            _Deadline_ManagementRepository = deadline_ManagementRepository;
            _projectFilesRepository = projectFilesRepository;
            _projectFilesService = projectFilesService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public async Task<WebResponseContent> DownloadSysFile(Guid guidFileId)
        {
            try
            {
                var file = await _projectFilesRepository.FindAsyncFirst(p => p.id == guidFileId && p.delete_status == 0);
                if (file == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_info_null"));
                }

                // 检查文件是否存在
                var strFilePath = Path.Combine(AppSetting.FileSaveSettings.FolderPath, file.file_path);
                if (!File.Exists(strFilePath))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_null"));
                }
                var bytsFile = File.ReadAllBytes(strFilePath);   // 读取文件内容
                var strContentType = CommonHelper.GetContentType(strFilePath);// 获取文件的MIME类型

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

        public async Task<WebResponseContent> UploadFileOld(UploadDto recordDto, IFormFile file = null)
        {
            try
            {
                string strContractFolder = GetProFolder(recordDto.deadline_id);
                if (file != null)
                {
                    var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, strContractFolder as string);
                    var fileInfo = saveFileResult.data as List<FileInfoDto>;
                    if (fileInfo == null)
                    {
                        return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                    }
                    //保存数据库
                    List<Biz_Deadline_Management_File> list = new List<Biz_Deadline_Management_File>();
                    foreach (var item in fileInfo)
                    {
                        Biz_Deadline_Management_File files_Record = new Biz_Deadline_Management_File
                        {
                            id = Guid.NewGuid(),
                            file_name = item.file_name,
                            file_path = item.file_relative_path,
                            file_size = item.file_size,
                            file_type = (int)recordDto.file_type,
                            deadline_id = recordDto.deadline_id,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = DateTime.Now
                        };
                        list.Add(files_Record);
                    }
                    await _repository.AddRangeAsync(list);
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("file_submit_no_found"));

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> UploadFile(UploadDto recordDto, IFormFile file = null)
        {
            try
            {
                var contract = _Deadline_ManagementRepository.Find(d => d.id == recordDto.deadline_id).FirstOrDefault();
                var contract_id = contract.contract_id;
                string strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, null).Result.data.ToString(); //GetProFolder(recordDto.subId);//Path.Combine(AppSetting.FileSaveSettings.FolderPath, $"{companyData.company_no}\\{strProFolder}\\{contractData.contract_no}");
                if (file != null)
                {                
                    var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, strContractFolder as string);
                    var fileInfo = saveFileResult.data as List<FileInfoDto>;
                    if (fileInfo == null)
                    {
                        return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                    }
                    //保存数据库
                    #region 新库
                    List<Biz_Project_Files> files = new List<Biz_Project_Files>();
                    foreach (var item in fileInfo)
                    {
                        Biz_Project_Files _Files = new Biz_Project_Files
                        {
                            id = Guid.NewGuid(),
                            relation_id = recordDto.deadline_id,
                            file_type = recordDto.file_type,
                            file_name = item.file_name,
                            file_ext = item.file_ext,
                            file_path = item.file_relative_path,
                            file_size = (int)file.Length,
                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = DateTime.Now,
                        };
                        files.Add(_Files);

                    }
                    await _projectFilesRepository.AddRangeAsync(files);
                    await _projectFilesRepository.SaveChangesAsync();
                    #endregion
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("file_submit_no_found"));

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> DeleteFile(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("delete_id_empty"));
                var entData = repository.Find(d => d.id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.delete_status = (int)SystemDataStatus.Invalid;
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    repository.Update(entData);

                    #region 删除文件
                    _FileRecordsService.MoveFileToTemporary(entData.file_path, entData.file_name, null);
                    #endregion
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                else
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        private string GetProFolder(Guid? deadline_id)
        {
            // 公司信息
            var companyData = _companyRepository.FindFirst(p => p.id == UserContext.Current.UserInfo.Company_Id);
            if (companyData == null)
            {
                return "";
            }
            // 组合目录
            var contract = _Deadline_ManagementRepository.Find(d => d.id == deadline_id).FirstOrDefault();
            var contract_id = contract.contract_id;
            var contractData = _contractRepository.FindFirst(d => d.id == contract_id);

            var getProFolderResult = _fileConfigService.GetMainProFolderName(contractData.create_date.Value);
            if (!getProFolderResult.status)
            {
                return getProFolderResult.data as string;
            }
            var strProFolder = getProFolderResult.data as string;
            return Path.Combine($"{companyData.company_no}\\{strProFolder}\\{contractData.contract_no}");
        }
    }
}
