
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
    public partial class Biz_Completion_Acceptance_RecordService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Completion_Acceptance_RecordRepository _repository;//访问数据库
        private readonly IBiz_ContractRepository _contractRepository;
        private readonly ILocalizationService _localizationService;
        private readonly ISys_File_ConfigService _fileConfigService;
        private readonly ISys_CompanyRepository _companyRepository;
        private readonly IBiz_Completion_AcceptanceRepository  _acceptanceRepository;
        private readonly ISys_File_RecordsService _FileRecordsService;
        private readonly ISys_Message_NotificationService _messageService;
        private readonly ISys_User_NewRepository _user_NewRepository;

        [ActivatorUtilitiesConstructor]
        public Biz_Completion_Acceptance_RecordService(
            IBiz_Completion_Acceptance_RecordRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            IBiz_ContractRepository contractRepository,
            ILocalizationService localizationService,
            ISys_File_ConfigService fileConfigService,
            ISys_CompanyRepository companyRepository,
            IBiz_Completion_AcceptanceRepository acceptanceRepository,
            ISys_File_RecordsService fileRecordsService,
            ISys_Message_NotificationService messageService,
            ISys_User_NewRepository user_NewRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _contractRepository = contractRepository;
            _localizationService = localizationService;
            _fileConfigService = fileConfigService;
            _companyRepository = companyRepository;
            _acceptanceRepository = acceptanceRepository;
            _FileRecordsService = fileRecordsService;
            _messageService = messageService;
            _user_NewRepository = user_NewRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 获取主文件列表(pdf)
        /// </summary>
        /// <param name="subId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetFileList(Guid accId)
        {
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var list = await _repository.FindAsIQueryable(d => d.acceptance_id == accId
            && d.delete_status == (int)SystemDataStatus.Valid
            && d.file_type == (int)SubmissionFileEnum.MasterFile).Select(d => new CompletionAcceptanceRecordListDto
            {
                id = d.id,
                acceptance_id = d.acceptance_id,
                version = d.version,
                version_str = CommonHelper.GetSubmissionVersionStr(d.version),
                inner_status = d.inner_status,
                upload_user_id = d.upload_user_id,
                upload_user_name = lstUser.Where(x=>x.user_id == (int)d.upload_user_id).FirstOrDefault().user_true_name,
                submit_date = d.submit_date,
                submit_status = d.submit_status,
                file_type = d.file_type,
                file_name = d.file_name,
                check_list = d.check_list,
                create_date = d.create_date
            }).OrderByDescending(d => d.create_date).ToListAsync();
            return WebResponseContent.Instance.OK("Ok", list);
        }
        public async Task<WebResponseContent> GetFileListByVersion(Guid accId, int version)
        {
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var list = await _repository.FindAsIQueryable(d => d.acceptance_id == accId
            && d.delete_status == (int)SystemDataStatus.Valid
            && d.file_type != (int)SubmissionFileEnum.MasterFile
            && d.version == version).Select(d => new CompletionAcceptanceRecordListDto
            {
                id = d.id,
                acceptance_id = d.acceptance_id,
                version = d.version,
                version_str = CommonHelper.GetSubmissionVersionStr(d.version),
                inner_status = d.inner_status,
                upload_user_id = d.upload_user_id,
                upload_user_name = lstUser.Where(x => x.user_id == (int)d.upload_user_id).FirstOrDefault().user_true_name,
                submit_date = d.submit_date,
                submit_status = d.submit_status,
                file_type = d.file_type,
                file_name = d.file_name,
                create_date = d.create_date,
                check_list = d.check_list
            }).OrderBy(d=>d.file_type).ToListAsync();
            return WebResponseContent.Instance.OK("Ok", list);
        }

        public async Task<WebResponseContent> UploadFile(UploadAcceptanceRecordDto recordDto, IFormFile file = null)
        {
            try
            {
                string strContractFolder = GetProFolder(recordDto.acceptance_id);
                if (file != null)
                {
                    var oldFile = _repository.Find(d => d.acceptance_id == recordDto.acceptance_id
                     && d.delete_status == (int)SystemDataStatus.Valid
                     && d.version == recordDto.version
                     && d.file_type == recordDto.file_type
                     && (d.file_type == (int)SubmissionFileEnum.EditFile || d.file_type == (int)SubmissionFileEnum.CustomerReview)).ToList();
                    if (oldFile.Count > 0)
                    {
                        foreach (var item in oldFile)
                        {
                            await DeleteFile(item.id);
                        }
                    }
                    var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, strContractFolder as string);
                    var fileInfo = saveFileResult.data as List<FileInfoDto>;
                    if (fileInfo == null)
                    {
                        return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                    }
                    //保存数据库
                    List<Biz_Completion_Acceptance_Record> list = new List<Biz_Completion_Acceptance_Record>();
                    foreach (var item in fileInfo)
                    {
                        Biz_Completion_Acceptance_Record files_Record = new Biz_Completion_Acceptance_Record
                        {
                            id = Guid.NewGuid(),
                            acceptance_id = recordDto.acceptance_id,
                            version = recordDto.version,
                            file_type = recordDto.file_type,
                            file_name = item.file_name,
                            file_ext = item.file_ext,
                            file_path = item.file_relative_path,
                            file_size = (int)file.Length,
                            check_list = recordDto.check_list,
                            upload_user_id = UserContext.Current.UserId,
                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = DateTime.Now,
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

        public async Task<WebResponseContent> UploadImg(UploadAcceptanceRecordExDto  exDto, IFormFile file = null)
        {
            try
            {
                string strContractFolder = GetProFolder(exDto.acceptance_id);
                var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, strContractFolder as string);
                var fileInfo = saveFileResult.data as List<FileInfoDto>;
                if (fileInfo == null)
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                }
                //保存数据库
                List<Biz_Completion_Acceptance_Record> list = new List<Biz_Completion_Acceptance_Record>();
                foreach (var item in fileInfo)
                {
                    Biz_Completion_Acceptance_Record files_Record = new Biz_Completion_Acceptance_Record
                    {
                        id = Guid.NewGuid(),
                        acceptance_id = exDto.acceptance_id,  
                        file_type = exDto.file_type,
                        file_name = item.file_name,
                        file_ext = item.file_ext,
                        file_path = item.file_relative_path,
                        file_size = (int)file.Length,
                        upload_user_id = UserContext.Current.UserId,
                        delete_status = (int)SystemDataStatus.Valid,
                        create_id = UserContext.Current.UserId,
                        create_name = UserContext.Current.UserName,
                        create_date = DateTime.Now,
                    };
                    list.Add(files_Record);
                }
                await _repository.AddRangeAsync(list);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
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

        public async Task<WebResponseContent> UpdateCheckList(Guid id, string jsonData)
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("update_id_empty"));
                var entData = repository.Find(d => d.id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    entData.check_list = jsonData;
                    repository.Update(entData);
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> AuditData(Guid id, int status)
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                var entData = repository.Find(d => d.id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.inner_status = status;
                    entData.approved_date = DateTime.Now;
                    entData.approved_id = UserContext.Current.UserId;
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    repository.Update(entData);

                    var submission = _acceptanceRepository.Find(d => d.id == entData.acceptance_id).FirstOrDefault();
                    if (submission != null)
                    {
                        submission.modify_date = DateTime.Now;
                        submission.modify_id = UserContext.Current.UserId;
                        submission.modify_name = UserContext.Current.UserName;
                        submission.inner_status = status;
                        submission.approved_date = DateTime.Now;
                        _acceptanceRepository.Update(submission);
                    }
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> DownloadSysFile(Guid guidFileId)
        {
            try
            {
                var file = await repository.FindAsyncFirst(p => p.id == guidFileId && p.delete_status == 0);
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

        public async Task<WebResponseContent> SubmitFile(Guid subId)
        {
            try
            {
                if (subId == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                var entData = repository.Find(d => d.id == subId).FirstOrDefault();
                if (entData != null)
                {
                    entData.submit_status = (int)SubmissionFileStatusEnum.Submitted;
                    entData.submit_date = DateTime.Now;
                    entData.approved_id = UserContext.Current.UserId;
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    repository.Update(entData);

                    var submission = _acceptanceRepository.Find(d => d.id == entData.acceptance_id).FirstOrDefault();
                    if (submission != null)
                    {
                        submission.modify_date = DateTime.Now;
                        submission.modify_id = UserContext.Current.UserId;
                        submission.modify_name = UserContext.Current.UserName;
                        submission.submit_status = (int)SubmissionFileStatusEnum.Submitted;
                        _acceptanceRepository.Update(submission);
                        #region 更新消息通知状态
                        _messageService.UpdateStatusNoSave(submission.id);
                        #endregion
                    }
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        private string GetProFolder(Guid acceptance_id)
        {
            // 公司信息
            var companyData = _companyRepository.FindFirst(p => p.id == UserContext.Current.UserInfo.Company_Id);
            if (companyData == null)
            {
                return "";
            }
            // 组合目录
            var contract = _acceptanceRepository.Find(d => d.id == acceptance_id).FirstOrDefault();
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
