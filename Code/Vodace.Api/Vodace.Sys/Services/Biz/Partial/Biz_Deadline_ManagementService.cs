
using AutoMapper;
using Dm.util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.Formula.PTG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
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
    public partial class Biz_Deadline_ManagementService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Deadline_ManagementRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_User_NewRepository _user_NewRepository;
        private readonly IBiz_Upcoming_EventsRepository _upcomingEventsRepository;  
        private readonly IMapper _mapper;
        private readonly ISys_Message_NotificationService _messageService;
        private readonly ISys_CompanyRepository _companyRepository;
        private readonly IBiz_ContractRepository _contractRepository;
        private readonly ISys_File_ConfigService _fileConfigService;
        private readonly ISys_File_RecordsService _FileRecordsService;
        private readonly IBiz_Project_FilesRepository _projectFilesRepository;
        private readonly IBiz_Project_FilesService _projectFilesService;


        [ActivatorUtilitiesConstructor]
        public Biz_Deadline_ManagementService(
            IBiz_Deadline_ManagementRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            ILocalizationService localizationService,
            ISys_User_NewRepository user_NewRepository,
            IMapper mapper,
            ISys_Message_NotificationService messageService,
            ISys_CompanyRepository companyRepository,
            ISys_File_ConfigService fileConfigService,
            ISys_File_RecordsService fileRecordsService,
            IBiz_Upcoming_EventsRepository upcomingEventsRepository,
            IBiz_ContractRepository contractRepository,
            IBiz_Project_FilesRepository projectFilesRepository,
            IBiz_Project_FilesService projectFilesService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _user_NewRepository = user_NewRepository;
            _mapper = mapper;
            _messageService = messageService;
            _companyRepository = companyRepository;
            _fileConfigService = fileConfigService;
            _FileRecordsService = fileRecordsService;
            _upcomingEventsRepository = upcomingEventsRepository;
            _contractRepository = contractRepository;
            _projectFilesRepository = projectFilesRepository;
            _projectFilesService = projectFilesService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public async Task<WebResponseContent> GetListByPage(PageInput<SubmissionFilesQuery> query)
        {
            var queryPam = query.search;
            var lstRecord = _projectFilesRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
           //(d.director_user_id == UserContext.Current.UserId || d.approved_id == UserContext.Current.UserId) &&
           (queryPam.contract_id == Guid.Empty || queryPam.contract_id == null ? true : d.contract_id == queryPam.contract_id)).Select(d => new DeadlineManagementListDto
           {
               id = d.id,
               deadline_type = d.deadline_type,
               approved_id = d.approved_id,
               approved_name = lstUser.Where(x => x.user_id == d.approved_id).FirstOrDefault().user_true_name,
               director_user_id = d.director_user_id,
               director_user_name = lstUser.Where(x => x.user_id == d.director_user_id).FirstOrDefault().user_true_name,
               describe = d.describe,
               contract_id = d.contract_id,
               subject = d.subject,
               deadline_date = d.deadline_date,
               img_count = lstRecord.Where(x => x.relation_id == d.id && x.file_type == (int)SubmissionFileEnum.ImageFile).Count(),
               doc_count = lstRecord.Where(x => x.relation_id == d.id && x.file_type == (int)SubmissionFileEnum.DocFile).Count(),
           });
            query.sort_field = "create_date";
            query.sort_type = "desc";
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        public async Task<WebResponseContent> GetDataById(Guid id)
        {
            var lstRecord = _projectFilesRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var data = await _repository.FindAsIQueryable(d => d.id == id).Select(d => new DeadlineManagementModelDto
            {
                id = d.id,
                contract_id = d.contract_id,
                deadline_type = d.deadline_type,
                describe = d.describe,
                subject = d.subject,
                approved_id = d.approved_id,
                remark = d.remark,
                actual_date = d.actual_date,
                deadline_date = d.deadline_date,
                estimated_date = d.estimated_date,
                customer_contact = d.customer_contact,
                director_user_id = d.director_user_id,
                doc_files = lstRecord.Where(x => x.relation_id == d.id && x.file_type == (int)SubmissionFileEnum.DocFile).Select(x => new FileModel
                {
                    id = x.id,
                    file_name = x.file_name,
                    file_size = x.file_size,
                    remark = x.remark
                }).ToList(),
                img_files = lstRecord.Where(x => x.relation_id == d.id && x.file_type == (int)SubmissionFileEnum.ImageFile).Select(x => x.id).ToList()
            }).ToListAsync();
            if (data == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data);
        }

        public async Task<WebResponseContent> AddData(DeadlineManagementAddDto addDto)
        {
            try
            {
                if(UserContext.Current.UserInfo.Company_Id ==null || UserContext.Current.UserInfo.Company_Id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("company_null")); 
                if (addDto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                var checkSubject = _repository.Exists(d => d.contract_id == addDto.contract_id && d.delete_status == (int)SystemDataStatus.Valid && d.deadline_type == addDto.deadline_type);
                if (checkSubject) return WebResponseContent.Instance.Error(_localizationService.GetString("deadline_type_exist！"));
                if (addDto.contract_id == Guid.Empty || addDto.contract_id == null) return WebResponseContent.Instance.Error(_localizationService.GetString("contract_id_null"));
                Biz_Deadline_Management biz_Submission_Files = _mapper.Map<Biz_Deadline_Management>(addDto);
                biz_Submission_Files.id = Guid.NewGuid();
                biz_Submission_Files.delete_status = (int)SystemDataStatus.Valid;
                biz_Submission_Files.create_id = UserContext.Current.UserId;
                biz_Submission_Files.create_name = UserContext.Current.UserName;
                biz_Submission_Files.create_date = DateTime.Now;
                await _repository.AddAsync(biz_Submission_Files);

                #region 上传文件
                var contract_id = addDto.contract_id;
                //string strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, UploadFileCode.Deadline_Management).Result.data.ToString();
                if (addDto.docFile != null) 
                {
                    //var lstFileIds = addDto.docFile.Select(x => x.id).ToList();
                    //var lstFile = await _projectFilesRepository.FindAsync(p => lstFileIds.Contains(p.id) && p.upload_status == (int)UploadStatus.Finish);
                    //if (lstFile.Count == 0) 
                    //{
                        var res = await FileMoveToFormal(biz_Submission_Files, addDto.docFile, UploadFileCode.Deadline_Management_doc);
                        if (!res.status) return res;
                    //}
                  
                }
                if (addDto.imgFile != null)
                {
                    //var lstFileIds = addDto.imgFile.Select(x => x.id).ToList();
                    //var lstFile = await _projectFilesRepository.FindAsync(p => lstFileIds.Contains(p.id) && p.upload_status == (int)UploadStatus.Finish);
                    //if (lstFile.Count == 0) 
                    //{
                        var res = await FileMoveToFormal(biz_Submission_Files, addDto.imgFile, UploadFileCode.Deadline_Management_img);
                        if (!res.status) return res;
                    //}   
                }
                #endregion

                #region 审核人不为空时 添加消息提醒
                if (addDto.approved_id != null && addDto.approved_id != 0)
                {
                    Log4NetHelper.Info($"期限管理有审核人：{addDto.approved_id}，需推送消息");
                    var userData = _user_NewRepository.FindAsIQueryable(d => d.user_id == addDto.approved_id).FirstOrDefault();
                    if (!string.IsNullOrEmpty(userData.user_name))
                    {
                        Log4NetHelper.Info($"期限管理推送消息给：{userData.user_name}");
                        AddMessgae(userData.user_name, biz_Submission_Files.id);
                    }
                    var contractData = _contractRepository.FindAsIQueryable(d => d.id == addDto.contract_id).FirstOrDefault();
                    var newContactId = _user_NewRepository.FindAsIQueryable(d => d.user_id == addDto.approved_id).FirstOrDefault().contact_id;
                    Upcoming_Events_RemarkDto remarkDto = new Upcoming_Events_RemarkDto
                    {
                        qn_no = contractData.contract_no,
                        qn_type = (int)addDto.deadline_type,
                        closing_date = addDto.deadline_date.ToString("yyyy-MM-dd")
                    };
                    Biz_Upcoming_Events events = new Biz_Upcoming_Events
                    {
                        id = Guid.NewGuid(),
                        relation_id = biz_Submission_Files.id,
                        recipient_user_id = newContactId,
                        event_no = contractData.contract_no,
                        event_name = addDto.subject,
                        closing_date = Convert.ToDateTime(addDto.deadline_date),
                        days_left_to_close = CommonHelper.DiffDays(Convert.ToDateTime(addDto.deadline_date)),
                        event_type = (int)addDto.deadline_type,
                        remark =  JsonSerializer.Serialize(remarkDto),
                        delete_status = (int)SystemDataStatus.Valid,
                        create_id = UserContext.Current.UserId,
                        create_name = UserContext.Current.UserName,
                        create_date = DateTime.Now
                    };
                  await _upcomingEventsRepository.AddAsync(events);
                }
                await _repository.SaveChangesAsync();
                #endregion
                string _msg = $"{_localizationService.GetString("save")}{_localizationService.GetString("success")}";
                return WebResponseContent.Instance.OK(_msg);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> EditData(DeadlineManagementEditDto editDto)
        {
            try
            {
                if (editDto == null || editDto.id == Guid.Empty) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                var checkFileNo = _repository.Exists(d => d.contract_id == editDto.contract_id && d.delete_status == (int)SystemDataStatus.Valid && d.deadline_type == editDto.deadline_type && d.id != editDto.id);
                if (checkFileNo) return WebResponseContent.Instance.Error(_localizationService.GetString("deadline_type_exist！"));
                var oldData = _repository.FindFirst(d => d.id == editDto.id);
                #region 更新
                if (oldData.approved_id != editDto.approved_id) 
                {
                    var contractData = _contractRepository.FindAsIQueryable(d => d.id == editDto.contract_id).FirstOrDefault();
                    var userData = _user_NewRepository.FindAsIQueryable(d => d.user_id == oldData.approved_id).FirstOrDefault();
                    var newContactId = _user_NewRepository.FindAsIQueryable(d => d.user_id == editDto.approved_id).FirstOrDefault().contact_id;
                    var lstEvent = await _upcomingEventsRepository.FindAsync(p => editDto.id == p.relation_id && p.recipient_user_id == userData.contact_id && p.delete_status == (int)SystemDataStatus.Valid);
                    foreach (var item in lstEvent)
                    {
                        item.recipient_user_id = newContactId;
                        item.days_left_to_close = CommonHelper.DiffDays(Convert.ToDateTime( editDto.deadline_date));
                        item.event_name = editDto.subject;
                        item.remark = JsonSerializer.Serialize(new Upcoming_Events_RemarkDto
                        {
                            qn_no = contractData.contract_no,
                            qn_type = (int)editDto.deadline_type,
                            closing_date = editDto.deadline_date.ToString("yyyy-MM-dd")
                        });
                        item.modify_id = UserContext.Current.UserInfo.User_Id;
                        item.modify_name = UserContext.Current.UserInfo.UserName;
                        item.modify_date = DateTime.Now;
                    }
                    _upcomingEventsRepository.UpdateRange(lstEvent);
                }
                #endregion
                if (oldData != null) 
                {
                    oldData.deadline_type = editDto.deadline_type;
                    oldData.subject = editDto.subject;
                    oldData.estimated_date = editDto.estimated_date;
                    oldData.deadline_date = editDto.deadline_date;
                    oldData.actual_date = editDto.actual_date;
                    oldData.customer_contact = editDto.customer_contact;
                    oldData.director_user_id = editDto.director_user_id;
                    oldData.approved_id = editDto.approved_id;
                    oldData.describe = editDto.describe;
                    oldData.status = editDto.status;
                    //oldData.remark = editDto.remark;
                    oldData.modify_date = DateTime.Now;
                    oldData.modify_id = UserContext.Current.UserId;
                    oldData.modify_name = UserContext.Current.UserName;

                    _repository.Update(oldData);
                    await _repository.SaveChangesAsync();
                }
                # region 上传文件
                if (editDto.docFile != null)
                {
                    var lstFileIds = editDto.docFile.Select(x => x.id).ToList();
                    var lstFile = await _projectFilesRepository.FindAsync(p => lstFileIds.Contains(p.id) && p.upload_status == (int)UploadStatus.Finish);
                    if (lstFile.Count == 0)
                    {
                        var res = await FileMoveToFormal(oldData, editDto.docFile, UploadFileCode.Deadline_Management_doc);
                        if (!res.status) return res;
                    }

                }
                if (editDto.imgFile != null)
                {
                    var lstFileIds = editDto.imgFile.Select(x => x.id).ToList();
                    var lstFile = await _projectFilesRepository.FindAsync(p => lstFileIds.Contains(p.id) && p.upload_status == (int)UploadStatus.Finish);
                    if (lstFile.Count == 0)
                    {
                        var res = await FileMoveToFormal(oldData, editDto.imgFile, UploadFileCode.Deadline_Management_img);
                        if (!res.status) return res;
                    }
                }
                #endregion
                string _msg = $"{_localizationService.GetString("save")}{_localizationService.GetString("success")}";
                return WebResponseContent.Instance.OK(_msg);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> DeleteData(Guid id)
        {
            try
            {
                var data = _repository.FindFirst(p => p.id == id);
                if (data == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                }
                data.delete_status = (int)SystemDataStatus.Invalid;
                data.modify_id = UserContext.Current.UserId;
                data.modify_name = UserContext.Current.UserName;
                data.modify_date = DateTime.Now;

                _repository.Update(data);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
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
                    d.status = (int)InnerStatusEnum.UnderReview;//审批通过
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


        private void AddMessgae(string userName, Guid relationId)
        {
            MessageNotificationAddDto notificationAddDto = new MessageNotificationAddDto
            {
                msg_title = "【期限管理】有需要待您审核的数据",
                msg_content = "",
                receive_user = userName,
                relation_id = relationId
            };
            var res = _messageService.AddMessage(notificationAddDto);
        }

        #region 编辑页
        public async Task<WebResponseContent> DownloadSysFile(Guid fileId)
        {
            try
            {
                var file = await _projectFilesRepository.FindAsyncFirst(p => p.id == fileId && p.delete_status == 0);
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

        public async Task<WebResponseContent> UploadFile(UploadDto recordDto, IFormFile file = null)
        {
            try
            {
                var contract = _repository.Find(d => d.id == recordDto.deadline_id).FirstOrDefault();
                if(contract == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                var contract_id = contract.contract_id;
                string strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, UploadFileCode.Deadline_Management).Result.data.ToString(); //GetProFolder(recordDto.subId);//Path.Combine(AppSetting.FileSaveSettings.FolderPath, $"{companyData.company_no}\\{strProFolder}\\{contractData.contract_no}");
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

        public async Task<WebResponseContent> DeleteFile(Guid fileId)
        {
            try
            {
                if (fileId == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("delete_id_empty"));
                var entData = _projectFilesRepository.Find(d => d.id == fileId).FirstOrDefault();
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
                    return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 上传文件到临时目录
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <param name="intStatus"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadFiles(int type,List<IFormFile> lstFiles)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                return await _projectFilesService.UploadFileToTempAsync(lstFiles, type ==0?(int)SubmissionFileEnum.DocFile: (int)SubmissionFileEnum.ImageFile, true);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public async Task<WebResponseContent> UploadImg(List<IFormFile> lstFiles)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                return await _projectFilesService.UploadFileToTempAsync(lstFiles, (int)SubmissionFileEnum.ImageFile, true);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 文件移动到正式目录
        /// </summary>
        /// <param name="addDto"></param>
        /// <param name="lstFileInfo"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> FileMoveToFormal(Biz_Deadline_Management addDto,  List<UFileInfoDto> lstFileInfo, string fileTypeCode)
        {
            try
            {
                // 公司信息
                var companyData = _companyRepository.FindFirst(p => p.id == UserContext.Current.UserInfo.Company_Id);
                if (companyData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("company_info_noexist"));
                }
                // 组合目录
                var getProFolderResult = _fileConfigService.GetMainProFolderName(DateTime.Now);
                if (!getProFolderResult.status)
                {
                    return getProFolderResult;
                }
                string strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)addDto.contract_id, fileTypeCode).Result.data.ToString();
                if (!Directory.Exists(Path.Combine(AppSetting.FileSaveSettings.FolderPath, strContractFolder)))
                {
                    Directory.CreateDirectory(Path.Combine(AppSetting.FileSaveSettings.FolderPath, strContractFolder));
                }
               
                // 将临时文件夹的数据移动到正式的文件夹
                var moveResult = await _projectFilesService.MoveFileToFolderAsync(lstFileInfo, fileTypeCode, (Guid)addDto.contract_id, addDto.id, true);
                if (!moveResult.status)
                {
                    return moveResult;
                }
                return WebResponseContent.Instance.OK("ok");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        #endregion
    }
}
