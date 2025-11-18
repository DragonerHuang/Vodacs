
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
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Biz_Completion_AcceptanceService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Completion_AcceptanceRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_User_NewRepository _user_NewRepository;
        private readonly ISys_Message_NotificationService _messageService;
        private readonly IBiz_ContractRepository _ContractRepository;
        private readonly IBiz_Completion_Acceptance_RecordRepository _acceptance_RecordRepository;
        private readonly IMapper _mapper;
        private readonly IBiz_Project_FilesRepository _projectFilesRepository;
        private readonly IBiz_Project_FilesService _projectFilesService;
        private readonly ISys_File_RecordsService _FileRecordsService;

        [ActivatorUtilitiesConstructor]
        public Biz_Completion_AcceptanceService(
            IBiz_Completion_AcceptanceRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            ILocalizationService localizationService,
            ISys_User_NewRepository user_NewRepository,
            ISys_Message_NotificationService messageService,
            IBiz_ContractRepository contractRepository,
            IBiz_Completion_Acceptance_RecordRepository acceptance_RecordRepository,
            IMapper mapper,
            IBiz_Project_FilesRepository projectFilesRepository,
            IBiz_Project_FilesService projectFilesService,
            ISys_File_RecordsService fileRecordsService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _user_NewRepository = user_NewRepository;
            _messageService = messageService;
            _ContractRepository = contractRepository;
            _acceptance_RecordRepository = acceptance_RecordRepository;
            _mapper = mapper;
            _projectFilesRepository = projectFilesRepository;
            _projectFilesService = projectFilesService;
            _FileRecordsService = fileRecordsService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public async Task<WebResponseContent> GetListByPage(PageInput<SubmissionFilesQuery> query)
        {
            var queryPam = query.search;
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
           (queryPam.contract_id == Guid.Empty || queryPam.contract_id == null ? true : d.contract_id == queryPam.contract_id)).Select(d => new CompletionAcceptanceListDto
           {
               id = d.id,
               version = d.version,
               version_str = CommonHelper.GetSubmissionVersionStr(d.version),
               inner_status = d.inner_status,
               file_publish_date = d.file_publish_date ?? null,
               approved_date = d.approved_date,
               approved_id = d.approved_id,
               actual_inspection_date = d.actual_inspection_date ?? null,
               acceptance_no = d.acceptance_no,
               remark = d.remark,
               approved_name = lstUser.Where(x => x.user_id == d.approved_id).FirstOrDefault().user_true_name,
               producer_id = d.producer_id,
               producer_name = lstUser.Where(x => x.user_id == d.producer_id).FirstOrDefault().user_true_name,
               describe = d.describe,
               contract_id = d.contract_id,
               submit_status = d.submit_status,
           });
            query.sort_field = "create_date";
            query.sort_type = "desc";
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }
        public async Task<WebResponseContent> GetListByPageByUser(PageInput query)
        {
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstFile = _acceptance_RecordRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid && d.file_type == (int)SubmissionFileEnum.MasterFile);
            var lstContact = _ContractRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
           (d.approved_id == UserContext.Current.UserId && d.producer_id == UserContext.Current.UserId)).Select(d => new CompletionAcceptanceListDto
           {
               id = d.id,
               acceptance_no = d.acceptance_no,
               version = d.version,
               version_str = CommonHelper.GetSubmissionVersionStr(d.version),
               file_id = lstFile.Where(x => x.acceptance_id == d.id && x.version == d.version).FirstOrDefault() == null ? null : lstFile.Where(x => x.acceptance_id == d.id && x.version == d.version).FirstOrDefault().id,
               inner_status = d.inner_status,
               actual_inspection_date = d.actual_inspection_date ?? null,
               approved_date = d.approved_date,
               approved_id = d.approved_id,
               approved_name = lstUser.Where(x => x.user_id == d.approved_id).FirstOrDefault().user_true_name,
               producer_id = d.producer_id,
               producer_name = lstUser.Where(x => x.user_id == d.producer_id).FirstOrDefault().user_true_name,
               describe = d.describe,
               contract_id = d.contract_id,
               submit_status = d.submit_status,
           });
            query.sort_field = "create_date";
            query.sort_type = "desc";
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }
        public async Task<WebResponseContent> GetDataById(Guid id)
        {
            var lstRecord = _acceptance_RecordRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid && (d.file_type == (int)SubmissionFileEnum.InternalAcceptance || d.file_type ==(int)SubmissionFileEnum.CustomerAcceptance));
            var data = await _repository.FindAsIQueryable(d => d.id == id).Select(d => new CompletionAcceptanceModelDto
            {
                id = d.id,
                contract_id = d.contract_id,
                acceptance_no = d.acceptance_no,
                describe = d.describe,
                producer_id = d.producer_id,
                approved_id = d.approved_id,
                file_publish_date = d.file_publish_date ?? null,

                actual_inspection_date = d.actual_inspection_date ?? null,
                test_result = d.test_result,
                approved_date = d.approved_date,
                internal_img_count = lstRecord.Where(x=>x.acceptance_id == d.id && x.file_type == (int)SubmissionFileEnum.InternalAcceptance).Count(),

                inspector = d.inspector,
                engineer_permit_date = d.engineer_permit_date,
                remark = d.remark,
                customer_img_count = lstRecord.Where(x=>x.acceptance_id == d.id && x.file_type == (int)SubmissionFileEnum.CustomerAcceptance).Count()
            }).ToListAsync();
            if (data == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data);
        }
        public WebResponseContent CheckFileNo(Guid contractId, string fileno)
        {
            var checkFileNo = _repository.Exists(d => d.contract_id == contractId && d.delete_status == (int)SystemDataStatus.Valid && d.acceptance_no == fileno);
            if (checkFileNo) return WebResponseContent.Instance.Error(_localizationService.GetString("file_no_exist！"));
            return WebResponseContent.Instance.OK("Ok");
        }
        public async Task<WebResponseContent> AddData(CompletionAcceptanceAddDto addDto)
        {
            try
            {
                if (addDto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                var checkFileNo = _repository.Exists(d => d.contract_id == addDto.contract_id && d.delete_status == (int)SystemDataStatus.Valid && d.acceptance_no == addDto.acceptance_no);
                if (checkFileNo) return WebResponseContent.Instance.Error(_localizationService.GetString("file_no_exist！"));
                if (addDto.contract_id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("contract_id_null"));
                Biz_Completion_Acceptance biz_Submission_Files =_mapper.Map<Biz_Completion_Acceptance>(addDto);
                biz_Submission_Files.id = Guid.NewGuid();
                biz_Submission_Files.delete_status = (int)SystemDataStatus.Valid;
                biz_Submission_Files.create_id = UserContext.Current.UserId;
                biz_Submission_Files.create_name = UserContext.Current.UserName;
                biz_Submission_Files.create_date = DateTime.Now;

               
                await _repository.AddAsync(biz_Submission_Files);
                await _repository.SaveChangesAsync();
                string _msg = $"{_localizationService.GetString("save")}{_localizationService.GetString("success")}";
                return WebResponseContent.Instance.OK(_msg);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Biz_Submission_FilesService.AddData 新增内容保存失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> EditData(CompletionAcceptanceEditDto editDto)
        {
            try
            {
                if (editDto == null || editDto.id == Guid.Empty) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                var checkFileNo = _repository.Exists(d => d.contract_id == editDto.contract_id && d.delete_status == (int)SystemDataStatus.Valid && d.acceptance_no == editDto.acceptance_no && d.id != editDto.id);
                if (checkFileNo) return WebResponseContent.Instance.Error(_localizationService.GetString("file_no_exist！"));
                var oldData = _repository.FindFirst(d => d.id == editDto.id);
                if (oldData == null) return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                oldData.remark = editDto.remark;
                oldData.describe = editDto.describe;
                oldData.approved_id = editDto.approved_id;
                oldData.producer_id = editDto.producer_id;
                oldData.file_publish_date = editDto.file_publish_date;
                oldData.actual_inspection_date = editDto.actual_inspection_date;
                oldData.test_result = editDto.test_result;
                oldData.inspector = editDto.inspector;
                oldData.engineer_permit_date = editDto.engineer_permit_date;
                oldData.remark  = editDto.remark;

                oldData.modify_date = DateTime.Now;
                oldData.modify_id = UserContext.Current.UserId;
                oldData.modify_name = UserContext.Current.UserName;

                _repository.Update(oldData);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Biz_Submission_FilesService.EditData 编辑内容保存失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> AuditByBatch(SubmissionAuditDto dto)
        {
            try
            {
                var lstData = _repository.Find(d => dto.id.Contains(d.id));
                lstData.ForEach(d =>
                {
                    d.inner_status = (int)InnerStatusEnum.Submitted;
                    d.approved_id = UserContext.Current.UserId;
                    d.approved_date = DateTime.Now;
                });
                _repository.UpdateRange(lstData);
                foreach (var item in lstData)
                {
                    _messageService.UpdateStatusNoSave(item.id);
                }
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Biz_Submission_FilesService.AuditByBatch 批量审核失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> SubmitAudit(Guid fileId)
        {
            try
            {
                var file = _projectFilesRepository.FindFirst(d => d.id == fileId);
                if (file == null) return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                file.inner_status = (int)InnerStatusEnum.UnderReview;
                file.modify_date = DateTime.Now;
                file.modify_id = UserContext.Current.UserId;
                file.modify_name = UserContext.Current.UserName;
                _projectFilesRepository.Update(file);

                var data = _repository.FindFirst(d => d.id == file.relation_id);
                if (data != null)
                {
                    data.inner_status = (int)InnerStatusEnum.UnderReview;
                    data.modify_date = DateTime.Now;
                    data.modify_id = UserContext.Current.UserId;
                    data.modify_name = UserContext.Current.UserName;

                    _repository.Update(data);
                    await _repository.SaveChangesAsync();
                    #region 审核人不为空时 添加消息提醒
                    if (data.approved_id != null && data.approved_id != 0)
                    {
                        Log4NetHelper.Info($"完工验收有审核人：{data.approved_id}，需推送消息");
                        var userData = _user_NewRepository.FindAsIQueryable(d => d.user_id == data.approved_id).FirstOrDefault();
                        if (!string.IsNullOrEmpty(userData.user_name))
                        {
                            Log4NetHelper.Info($"完工验收推送消息给：{userData.user_name}");
                            AddMessgae(userData.user_name, data.id);
                        }
                    }
                    #endregion
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Biz_Submission_FilesService.SubmitAudit 提交审核失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        private void AddMessgae(string userName, Guid relationId)
        {
            MessageNotificationAddDto notificationAddDto = new MessageNotificationAddDto
            {
                msg_title = "【完工验收】有需要待您审核的数据",
                msg_content = "",
                receive_user = userName,
                relation_id = relationId
            };
            var res = _messageService.AddMessage(notificationAddDto);
        }

        #region   
        public async Task<WebResponseContent> GetFileList(Guid accId)
        {
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var list = await _projectFilesRepository.FindAsIQueryable(d => d.relation_id == accId
            && d.delete_status == (int)SystemDataStatus.Valid
            && d.file_type == (int)SubmissionFileEnum.MasterFile).Select(d => new CompletionAcceptanceRecordListDto
            {
                id = d.id,
                acceptance_id = d.relation_id,
                version = d.version,
                version_str = CommonHelper.GetSubmissionVersionStr(d.version),
                inner_status = d.inner_status,
                upload_user_id = d.create_id,
                upload_user_name = lstUser.Where(x => x.user_id == (int)d.create_id).FirstOrDefault().user_true_name,
                submit_date = d.submit_date,
                submit_status = d.submit_status,
                file_type = d.file_type,
                file_name = d.file_name,
                check_list = d.check_list,
                create_date = d.create_date
            }).OrderByDescending(d => d.create_date).ToListAsync();
            return WebResponseContent.Instance.OK("Ok", list);
        }
        public async Task<WebResponseContent> GetFileListByVersion(Guid subId, int version)
        {
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var list = await _projectFilesRepository.FindAsIQueryable(d => d.relation_id == subId
            && d.delete_status == (int)SystemDataStatus.Valid
            && d.file_type != (int)SubmissionFileEnum.MasterFile
            && d.version == version).Select(d => new SubmissionFilesRecordListDto
            {
                id = d.id,
                submission_id = d.relation_id,
                version = d.version,
                version_str = CommonHelper.GetSubmissionVersionStr(d.version),
                inner_status = d.inner_status,
                upload_user_id = d.create_id,
                upload_user_name = lstUser.Where(x => x.user_id == (int)d.create_id).FirstOrDefault().user_true_name,
                modify_date = d.modify_date,
                submit_date = d.submit_date,
                submit_status = d.submit_status,
                file_type = d.file_type,
                file_name = d.file_name,
                //file_path = d.file_path,
                check_list = d.check_list,
                create_date = d.create_date
            }).OrderBy(d => d.file_type).ToListAsync();
            return WebResponseContent.Instance.OK("Ok", list);
        }
        public async Task<WebResponseContent> UploadFile(UploadAcceptanceRecordDto recordDto, IFormFile file = null)
        {
            try
            {
                var contract = _repository.Find(d => d.id == recordDto.acceptance_id).FirstOrDefault();
                if (contract == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                var contract_id = contract.contract_id;
                string strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, UploadFileCode.Completion_Acceptance).Result.data.ToString(); //GetProFolder(recordDto.subId);//Path.Combine(AppSetting.FileSaveSettings.FolderPath, $"{companyData.company_no}\\{strProFolder}\\{contractData.contract_no}");

                if (file != null)
                {
                    var oldFile = _projectFilesRepository.Find(d => d.relation_id == recordDto.acceptance_id
                    && d.delete_status == (int)SystemDataStatus.Valid
                    && d.version == recordDto.version
                    && d.file_type == recordDto.file_type
                    && (d.file_type == (int)SubmissionFileEnum.EditFile || d.file_type == (int)SubmissionFileEnum.CustomerReview)).ToList();
                    if (oldFile.Count > 0)
                    {
                        DeleteFileNoSave(oldFile);
                    }
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
                            relation_id = recordDto.acceptance_id,
                            version = recordDto.version,
                            file_type = recordDto.file_type,
                            file_name = item.file_name,
                            file_ext = item.file_ext,
                            file_path = item.file_relative_path,
                            file_size = (int)file.Length,
                            check_list = recordDto.check_list,
                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = DateTime.Now,
                        };
                        files.Add(_Files);
                    }
                    var completion_Acceptance = _repository.Find(d => d.id == recordDto.acceptance_id).FirstOrDefault();
                    if (completion_Acceptance == null)
                    {
                        completion_Acceptance.version = recordDto.version;
                        completion_Acceptance.modify_id = UserContext.Current.UserId;
                        completion_Acceptance.modify_name = UserContext.Current.UserName;
                        completion_Acceptance.modify_date = DateTime.Now;
                        _repository.Update(completion_Acceptance);
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
        public async Task<WebResponseContent> UploadFileByApproved(UploadByApprovedDto recordDto, IFormFile file = null)
        {
            try
            {
                var contract = _repository.Find(d => d.id == recordDto.subId).FirstOrDefault();
                if (contract == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                var contract_id = contract.contract_id;
                string strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, UploadFileCode.Completion_Acceptance).Result.data.ToString(); //GetProFolder(recordDto.subId);//Path.Combine(AppSetting.FileSaveSettings.FolderPath, $"{companyData.company_no}\\{strProFolder}\\{contractData.contract_no}");

                if (file != null)
                {
                    var oldFile = _projectFilesRepository.Find(d => d.relation_id == recordDto.subId
                    && d.delete_status == (int)SystemDataStatus.Valid
                    && d.version == recordDto.version
                    && d.file_type == (int)SubmissionFileEnum.MasterFile).ToList();
                    if (oldFile.Count > 0)
                    {
                        DeleteFileNoSave(oldFile);
                    }
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
                            relation_id = recordDto.subId,
                            version = recordDto.version,
                            file_type = (int)SubmissionFileEnum.MasterFile,
                            file_name = item.file_name,
                            file_ext = item.file_ext,
                            file_path = item.file_relative_path,
                            file_size = (int)file.Length,
                            check_list = recordDto.check_list,
                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = DateTime.Now,
                            inner_status = (int)InnerStatusEnum.Submitted,
                            approved_date = DateTime.Now,
                            approved_id = UserContext.Current.UserId,
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
        public async Task<WebResponseContent> UploadImg(UploadAcceptanceRecordExDto exDto, IFormFile file = null)
        {
            try
            {
                var contract = _repository.Find(d => d.id == exDto.acceptance_id).FirstOrDefault();
                var contract_id = contract.contract_id;
                string strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, UploadFileCode.Completion_Acceptance).Result.data.ToString();
                var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, strContractFolder as string);
                var fileInfo = saveFileResult.data as List<FileInfoDto>;
                if (fileInfo == null)
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                }
                //保存数据库
                List<Biz_Project_Files> list = new List<Biz_Project_Files>();
                foreach (var item in fileInfo)
                {
                    Biz_Project_Files files_Record = new Biz_Project_Files
                    {
                        id = Guid.NewGuid(),
                        relation_id = exDto.acceptance_id,
                        file_type = exDto.file_type,
                        file_name = item.file_name,
                        file_ext = item.file_ext,
                        file_path = item.file_relative_path,
                        file_size = (int)file.Length,
                        delete_status = (int)SystemDataStatus.Valid,
                        create_id = UserContext.Current.UserId,
                        create_name = UserContext.Current.UserName,
                        create_date = DateTime.Now,
                    };
                    list.Add(files_Record);
                }
                await _projectFilesRepository.AddRangeAsync(list);
                await _projectFilesRepository.SaveChangesAsync();
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
                var entData = _projectFilesRepository.Find(d => d.id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.delete_status = (int)SystemDataStatus.Invalid;
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    _projectFilesRepository.Update(entData);

                    #region 删除文件
                    _FileRecordsService.MoveFileToTemporary(entData.file_path, entData.file_name, null);
                    #endregion
                    await _projectFilesRepository.SaveChangesAsync();
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
        private void DeleteFileNoSave(List<Biz_Project_Files> lst)
        {
            try
            {
                foreach (var item in lst)
                {
                    item.delete_status = (int)SystemDataStatus.Invalid;
                    item.modify_date = DateTime.Now;
                    item.modify_id = UserContext.Current.UserId;
                    item.modify_name = UserContext.Current.UserName;
                    #region 删除文件
                    _FileRecordsService.MoveFileToTemporary(item.file_path, item.file_name, null);
                    #endregion
                }
                _projectFilesRepository.UpdateRange(lst);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
            }
        }
        public async Task<WebResponseContent> UpdateCheckList(Guid id, string jsonData)
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("update_id_empty"));
                var entData = _projectFilesRepository.Find(d => d.id == id).FirstOrDefault();
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
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
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
                var entData = _projectFilesRepository.Find(d => d.id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.inner_status = status;
                    entData.approved_date = DateTime.Now;
                    entData.approved_id = UserContext.Current.UserId;
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    repository.Update(entData);

                    var submission = _repository.Find(d => d.id == entData.relation_id).FirstOrDefault();
                    if (submission != null)
                    {
                        submission.modify_date = DateTime.Now;
                        submission.modify_id = UserContext.Current.UserId;
                        submission.modify_name = UserContext.Current.UserName;
                        submission.inner_status = status;
                        submission.approved_date = DateTime.Now;
                        _repository.Update(submission);
                    }
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
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
        public async Task<WebResponseContent> SubmitFile(Guid fileId)
        {
            try
            {
                if (fileId == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                var entData = _projectFilesRepository.Find(d => d.id == fileId).FirstOrDefault();
                if (entData != null)
                {
                    if (entData.inner_status != (int)InnerStatusEnum.Submitted) return WebResponseContent.Instance.OK(_localizationService.GetString("no_audit"));
                    entData.submit_status = (int)SubmissionFileStatusEnum.Submitted;
                    entData.submit_date = DateTime.Now;
                    entData.approved_id = UserContext.Current.UserId;
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    repository.Update(entData);
                    #region 更新消息通知状态
                    _messageService.UpdateStatusNoSave(entData.id);
                    #endregion

                    var submission = _repository.Find(d => d.id == entData.relation_id).FirstOrDefault();
                    if (submission != null)
                    {
                        submission.modify_date = DateTime.Now;
                        submission.modify_id = UserContext.Current.UserId;
                        submission.modify_name = UserContext.Current.UserName;
                        submission.submit_status = (int)SubmissionFileStatusEnum.Submitted;
                        _repository.Update(submission);

                    }
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> UpdateSubmitStatus(Guid id, int status)
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                var entData = _projectFilesRepository.Find(d => d.id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.submit_status = status;
                    entData.submit_date = DateTime.Now;
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    _repository.Update(entData);

                    var completion = _repository.Find(d => d.id == entData.relation_id).FirstOrDefault();
                    if (completion != null)
                    {
                        completion.submit_status = status;
                        completion.modify_date = DateTime.Now;
                        completion.modify_id = UserContext.Current.UserId;
                        completion.modify_name = UserContext.Current.UserName;
                        _repository.Update(completion);
                    }
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        #endregion
    }
}
