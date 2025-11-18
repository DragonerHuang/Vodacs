
using AutoMapper;
using Castle.Core.Internal;
using Dm;
using Dm.util;
using LinqKit;
using MathNet.Numerics.Providers.SparseSolver;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Http.Generated;
using Microsoft.VisualBasic.FileIO;
using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.Formula.PTG;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Vodace.Sys.Services
{
    public partial class Biz_Submission_FilesService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Submission_FilesRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_User_NewRepository _user_NewRepository;
        private readonly ISys_Message_NotificationService _messageService;
        private readonly IBiz_ContractRepository _ContractRepository;
        private readonly IMapper _mapper;
        private readonly IBiz_Project_FilesRepository _projectFilesRepository;
        private readonly IBiz_Project_FilesService _projectFilesService;
        private readonly ISys_File_RecordsService _FileRecordsService;
        private readonly ISys_File_ConfigService _fileConfigService;

        [ActivatorUtilitiesConstructor]
        public Biz_Submission_FilesService(
            IBiz_Submission_FilesRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            ILocalizationService localizationService,
            IMapper mapper,
            ISys_ContactRepository contactRepository,
            ISys_Message_NotificationService messageService,
            ISys_User_NewRepository user_NewRepository,
            IBiz_ContractRepository contractRepository,
            IBiz_Project_FilesRepository projectFilesRepository,
            IBiz_Project_FilesService projectFilesService,
            ISys_File_RecordsService fileRecordsService,
            ISys_File_ConfigService fileConfigService = null)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _messageService = messageService;
            _user_NewRepository = user_NewRepository;
            _ContractRepository = contractRepository;
            _projectFilesRepository = projectFilesRepository;
            _projectFilesService = projectFilesService;
            _FileRecordsService = fileRecordsService;
            _fileConfigService = fileConfigService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public async Task<WebResponseContent> GetListByPage(PageInput<SubmissionFilesQuery> query)
        {
            var queryPam = query.search;
            var fileData = _projectFilesRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
           (queryPam.contract_id == Guid.Empty || queryPam.contract_id == null ? true : d.contract_id == queryPam.contract_id) &&
           (string.IsNullOrEmpty(queryPam.file_no) || queryPam.file_no == null ? true : d.file_no.Contains(queryPam.file_no)) &&
           (string.IsNullOrEmpty(queryPam.describe) || queryPam.describe == null ? true : d.describe.Contains(queryPam.describe))
           ).Select(d => new SubmissionFilesListDto
           {
               id = d.id,
               file_no = d.file_no,
               version = d.version,
               main_type = d.main_type,
               version_str = CommonHelper.GetSubmissionVersionStr(d.version),
               inner_status = d.inner_status,
               actual_upload_date = d.actual_upload_date ?? null,
               approved_date = d.approved_date,
               approved_id = d.approved_id,
               approved_name = lstUser.Where(x => x.user_id == d.approved_id).FirstOrDefault().user_name_eng,
               producer_id = d.producer_id,
               producer_name = lstUser.Where(x => x.user_id == d.producer_id).FirstOrDefault().user_name_eng,
               describe = d.describe,
               contract_id = d.contract_id,
               expected_upload_date = d.expected_upload_date,
               submit_status = d.submit_status,
               create_date = d.create_date,
               file_id = fileData.Where(x => x.relation_id == d.id).OrderByDescending(d => d.version).FirstOrDefault().id,
           });
            query.sort_field = string.IsNullOrEmpty(query.sort_field) ? "create_date": query.sort_field;
            query.sort_type = string.IsNullOrEmpty(query.sort_type)? "desc" : query.sort_type;
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        public async Task<WebResponseContent> GetListByPageByUser(PageInput<SubmissionFilesByUserQuery> query)
        {
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstFile = _projectFilesRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid && d.file_type == (int)SubmissionFileEnum.MasterFile);
            var lstContact = _ContractRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
            (string.IsNullOrEmpty(query.search.file_no) ? true : query.search.file_no == d.file_no) &&
            (string.IsNullOrEmpty(query.search.describe) ? true : d.describe == query.search.describe)&&
           (d.approved_id == UserContext.Current.UserId && d.producer_id == UserContext.Current.UserId)).Select(d => new SubmissionFilesListExDto
           {
               id = d.id,
               file_no = d.file_no,
               version = d.version,
               contract_no = lstContact.Where(x => x.id == d.contract_id).FirstOrDefault().contract_no,
               version_str = CommonHelper.GetSubmissionVersionStr(d.version),
               file_id = lstFile.Where(x => x.relation_id == d.id && x.version == d.version).FirstOrDefault()==null?null: lstFile.Where(x => x.relation_id == d.id && x.version == d.version).FirstOrDefault().id,
               inner_status = d.inner_status,
               actual_upload_date = d.actual_upload_date ?? null,
               approved_date = d.approved_date,
               approved_id = d.approved_id,
               approved_name = lstUser.Where(x => x.user_id == d.approved_id).FirstOrDefault().user_name_eng,
               producer_id = d.producer_id,
               producer_name = lstUser.Where(x => x.user_id == d.producer_id).FirstOrDefault().user_name_eng,
               describe = d.describe,
               contract_id = d.contract_id,
               expected_upload_date = d.expected_upload_date,
               submit_status = d.submit_status,
                create_date = d.create_date
           });
            query.sort_field = "create_date";
            query.sort_type = "desc";
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        public async Task<WebResponseContent> GetDataById(Guid id)
        {
            var data = await _repository.FindAsIQueryable(d => d.id == id).Select(d => new 
            { 
                d.id,
                d.file_no,
                d.version,
                d.contract_id,
                d.describe,
                d.producer_id,
                d.approved_id,
                d.expected_upload_date,
                d.submit_status,
                d.inner_status,
                d.create_date,
                d.actual_upload_date,
                d.approved_date,
                d.brand,
                d.remark,
                IsApproved = d.approved_id == UserContext.Current.UserId
            }).FirstOrDefaultAsync();
            if (data == null) return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data);
        }
        public WebResponseContent CheckFileNo(Guid contractId, string fileno,Guid? id)
        {
            var checkFileNo = _repository.Exists(d => d.contract_id == contractId && d.delete_status == (int)SystemDataStatus.Valid && d.file_no == fileno &&(id==null?true :d.id !=id));
            if (checkFileNo) return WebResponseContent.Instance.Error(_localizationService.GetString("file_no_exist！"));
            return WebResponseContent.Instance.OK("Ok");
        }
        public async Task<WebResponseContent> AddData(SubmissionFilesAddDto addDto)
        {
            try
            {
                if (addDto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                var checkFileNo = _repository.Exists(d => d.contract_id == addDto.contract_id && d.delete_status == (int)SystemDataStatus.Valid && d.file_no == addDto.file_no);
                if (checkFileNo) return WebResponseContent.Instance.Error(_localizationService.GetString("file_no_exist！"));
                if(addDto.contract_id == Guid.Empty || addDto.contract_id == null) return WebResponseContent.Instance.Error(_localizationService.GetString("contract_id_null"));
                Biz_Submission_Files biz_Submission_Files = _mapper.Map<Biz_Submission_Files>(addDto);
                biz_Submission_Files.id = Guid.NewGuid();
                //biz_Submission_Files.inner_status = addDto.approved_id == addDto.producer_id ? (int)InnerStatusEnum.UnderReview : (int)InnerStatusEnum.Editing;
                biz_Submission_Files.submit_status = (int)SubmissionFileStatusEnum.ToDo;
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

        public async Task<WebResponseContent> AddDataByBatch(Guid contractId,int userId)
        {
            try
            {
                if(contractId==Guid.Empty) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("id_null")}");
                List<Biz_Submission_Files> list = new List<Biz_Submission_Files>();
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/001",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "Organization Chart and Contact",
                    create_date =DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/002",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "Key Persons' CV",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/003",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "EC Insurance",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/004",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "Waste disposal Ticket",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/005",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "LD202",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/006",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "ClC Form 1",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/007",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "Master Program",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/008",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "Submission Schedule",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/009",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "SMP (Safety Plan)",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/010",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "EMP (Environment Plan)",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/011",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "QMP (Quality Plan)",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/012",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "FSP (Fire Safety Plan)",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/013",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "PCFB- Foam 1B",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/014",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "PSST((Project specific safety Training Plan)",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/015",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "Monthly report",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                list.Add(new Biz_Submission_Files()
                {
                    id = Guid.NewGuid(),
                    contract_id = contractId,
                    file_no = "GEN or GI/OTH/016",
                    version = 0,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    approved_id = userId,
                    describe = "Nomination of xxx",
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                });
                await _repository.AddRangeAsync(list);
                await _repository.SaveChangesAsync();
                string _msg = $"{_localizationService.GetString("save")}{_localizationService.GetString("success")}";
                return WebResponseContent.Instance.OK(_msg);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Biz_Submission_FilesService.AddDataByBatch 批量新增内容保存失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> EditData(SubmissionFilesEditDto editDto)
        {
            try
            {
                if (editDto == null || editDto.id == Guid.Empty) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                var checkFileNo = _repository.Exists(d => d.contract_id == editDto.contract_id && d.delete_status == (int)SystemDataStatus.Valid && d.file_no == editDto.file_no && d.id != editDto.id);
                if (checkFileNo) return WebResponseContent.Instance.Error(_localizationService.GetString("file_no_exist！"));
                var oldData = _repository.FindFirst(d => d.id == editDto.id);
                if (oldData == null) return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                oldData.file_no = editDto.file_no;
                oldData.remark = editDto.remark;
                oldData.describe = editDto.describe;
                oldData.approved_id = editDto.approved_id;
                oldData.producer_id = editDto.producer_id;
                oldData.actual_upload_date = editDto.actual_upload_date;
                oldData.expected_upload_date = editDto.expected_upload_date;
                oldData.inner_status = editDto.producer_id == editDto.approved_id?(int)InnerStatusEnum.UnderReview : editDto.inner_status;
                oldData.brand = editDto.brand;
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
                        Log4NetHelper.Info($"提交文件有审核人：{data.approved_id}，需推送消息");
                        var userData = _user_NewRepository.FindAsIQueryable(d => d.user_id == data.approved_id).FirstOrDefault();
                        if (!string.IsNullOrEmpty(userData.user_name))
                        {
                            Log4NetHelper.Info($"提交文件推送消息给：{userData.user_name}");
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

        public void AddMessgae(string userName,Guid relationId) 
        {
            MessageNotificationAddDto notificationAddDto  = new MessageNotificationAddDto
            {
                msg_title = "【提交管理】有需要待您审核的数据",
                msg_content = "",
                receive_user = userName,
                relation_id = relationId
            };
           var res =  _messageService.AddMessage(notificationAddDto);
        }

        #region 编辑页面
        public async Task<WebResponseContent> GetFileList(Guid subId)
        {
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var list = await _projectFilesRepository.FindAsIQueryable(d => d.relation_id == subId
            && d.delete_status == (int)SystemDataStatus.Valid
            && d.file_type == (int)SubmissionFileEnum.MasterFile).Select(d => new SubmissionFilesRecordListDto
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
        public async Task<WebResponseContent> UploadFile(UploadSubmissionFilesRecordDto recordDto, IFormFile file = null)
        {
            try
            {
                var contract = _repository.Find(d => d.id == recordDto.subId).FirstOrDefault();
                if (contract == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                var contract_id = contract.contract_id;
                string strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, UploadFileCode.Submission_Files).Result.data.ToString(); //GetProFolder(recordDto.subId);//Path.Combine(AppSetting.FileSaveSettings.FolderPath, $"{companyData.company_no}\\{strProFolder}\\{contractData.contract_no}");
               
                if (file != null)
                {
                    var oldFile = _projectFilesRepository.Find(d => d.relation_id == recordDto.subId
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
                    var subData = _repository.Find(d=>d.id == recordDto.subId).FirstOrDefault();
                    if (subData != null) 
                    {
                        subData.inner_status = subData.producer_id == subData.approved_id ? (int)InnerStatusEnum.UnderReview : (int)InnerStatusEnum.Editing;
                        _repository.Update(subData);
                    }
                    #region 新库
                    List<Biz_Project_Files> files = new List<Biz_Project_Files>();
                    foreach (var item in fileInfo)
                    {
                        Biz_Project_Files _Files = new Biz_Project_Files
                        {
                            id = Guid.NewGuid(),
                            relation_id = recordDto.subId,
                            version = recordDto.version,
                            file_type = recordDto.file_type,
                            file_name = item.file_name,
                            file_ext = item.file_ext,
                            file_path = item.file_relative_path,
                            file_size = (int)file.Length,
                            check_list = recordDto.check_list,
                            inner_status = subData.producer_id == subData.approved_id ? (int)InnerStatusEnum.UnderReview : (int)InnerStatusEnum.Editing,
                            submit_status = (int)SubmissionFileStatusEnum.ToDo,
                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = DateTime.Now,
                        };
                        files.Add(_Files);
                    }
                    var submit_date = _repository.Find(d=>d.id ==recordDto.subId).FirstOrDefault();
                    if (submit_date == null)
                    {
                        subData.version = recordDto.version;
                        subData.modify_id = UserContext.Current.UserId;
                        subData.modify_name = UserContext.Current.UserName;
                        subData.modify_date = DateTime.Now;
                        _repository.Update(subData);
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
                string strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, UploadFileCode.Submission_Files).Result.data.ToString(); //GetProFolder(recordDto.subId);//Path.Combine(AppSetting.FileSaveSettings.FolderPath, $"{companyData.company_no}\\{strProFolder}\\{contractData.contract_no}");

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
                    var submission = _repository.Find(d => d.id == recordDto.subId).FirstOrDefault();
                    if (submission != null) 
                    {
                        submission.inner_status = (int)InnerStatusEnum.Submitted;
                        submission.approved_id = UserContext.Current.UserId;
                        submission.approved_date = DateTime.Now;

                        _repository.Update(submission);
                    }

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
                    return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
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
                var entData = _projectFilesRepository.Find(d => d.id == id && d.file_type == (int)SubmissionFileEnum.MasterFile).FirstOrDefault();
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
                var entData = _projectFilesRepository.Find(d => d.id == fileId && d.file_type == (int)SubmissionFileEnum.MasterFile).FirstOrDefault();
                if (entData != null)
                {
                    if(entData.inner_status != (int)InnerStatusEnum.Submitted) return WebResponseContent.Instance.OK(_localizationService.GetString("no_audit"));
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

                    #region 发送邮件
                    var to_email_userId = _repository.Find(d=>d.id == entData.relation_id).FirstOrDefault().to_email_user;
                    Sys_User_New user_New = null;
                    List<string> cc_email_userIds_array = new List<string>();
                    if (to_email_userId != null && to_email_userId != 0) 
                    {
                        user_New = _user_NewRepository.Find(d => d.user_id == to_email_userId).FirstOrDefault();
                    }
                    var submissionData = _repository.Find(d => d.id == entData.relation_id).FirstOrDefault();
                    if (!string.IsNullOrEmpty(submissionData.cc_email_users)) 
                    {
                         cc_email_userIds_array = _user_NewRepository.Find(d => submissionData.cc_email_users.contains(d.user_id.ToString())).Select(d=>d.email).ToList();
                    }
                    if (!string.IsNullOrEmpty(user_New.email)) 
                    {
                        //附件
                        var path = Path.Combine(AppSetting.FileSaveSettings.FolderPath, entData.file_path);
                        List<string> attachmentFilePaths = new List<string>();
                        attachmentFilePaths.add(path);
                        var contractData = _ContractRepository.Find(d=>d.id == submissionData.contract_id).FirstOrDefault();
                        var project_info = contractData.contract_no +"_"+ contractData.name_eng;
                        var subject = $"The [{entData.file_name}] for the [{project_info}].";
                        var approvedData = _user_NewRepository.Find(d => d.user_id == submissionData.approved_id).FirstOrDefault();
                        var senderContact = approvedData.user_name_eng + "_" + approvedData.phone_no;
                        var emailContent = MailHelperOutLook.GetMailTemplate(user_New.user_name_eng, entData.file_name, project_info, senderContact);
                        await MailHelperOutLook.SendMailOutLookEx(subject, emailContent, user_New.email, cc_email_userIds_array, attachmentFilePaths);
                    }
                    
                    #endregion
                    var submission = _repository.Find(d => d.id == entData.relation_id).FirstOrDefault();
                    if (submission != null)
                    {
                        submission.modify_date = DateTime.Now;
                        submission.modify_id = UserContext.Current.UserId;
                        submission.modify_name = UserContext.Current.UserName;
                        submission.submit_status = (int)SubmissionFileStatusEnum.Submitted;
                        submission.submit_date = DateTime.Now;
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
        public async Task<WebResponseContent> UpdateSubmitStatus(Guid id,int status)
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

                    var submission = _repository.Find(d => d.id == entData.relation_id).FirstOrDefault();
                    if (submission != null) 
                    {
                        submission.submit_status = status;
                        submission.submit_date = DateTime.Now;
                        submission.modify_date = DateTime.Now;
                        submission.modify_id = UserContext.Current.UserId;
                        submission.modify_name = UserContext.Current.UserName;

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
        #endregion

        #region 需求变更后
        public async Task<WebResponseContent> GetFileListNew(Guid subId)
        {
            var lstUser = _user_NewRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var result = await _projectFilesRepository.FindAsIQueryable(d => d.relation_id == subId)
            .GroupBy(x => x.version)
            .Select(g => new
            {
                Id = g.Where(x=> x.version == g.Key && x.file_type == (int)SubmissionFileEnum.MasterFile).FirstOrDefault() ==null? Guid.Empty: g.Where(x=> x.version == g.Key && x.file_type == (int)SubmissionFileEnum.MasterFile).FirstOrDefault().id,
                inner_status = g.Where(x => x.file_type == (int)SubmissionFileEnum.MasterFile).FirstOrDefault() == null ? (int)InnerStatusEnum.Editing : g.Where(x => x.file_type == (int)SubmissionFileEnum.MasterFile).FirstOrDefault().inner_status,
                submit_status = g.Where(x => x.file_type == (int)SubmissionFileEnum.MasterFile).FirstOrDefault() == null ? (int)SubmissionFileStatusEnum.ToDo : g.Where(x => x.file_type == (int)SubmissionFileEnum.MasterFile).FirstOrDefault().submit_status,
                Version = g.Key,
                VersionStr = CommonHelper.GetSubmissionVersionStr(g.Key),
                ApprovedDate = g.Max(x => x.approved_date),
                SubmitDate = g.Max(x => x.submit_date),
                CheckList = g.Where(x => x.file_type == (int)SubmissionFileEnum.MasterFile).FirstOrDefault()==null?"": g.Where(x=>x.file_type ==(int)SubmissionFileEnum.MasterFile).FirstOrDefault().check_list,
                children = g.Select(x=> new SubmissionFilesRecordListDto
                {
                    id = x.id,
                    submission_id = x.relation_id,
                    version = x.version,
                    version_str = CommonHelper.GetSubmissionVersionStr(x.version),
                    inner_status = x.inner_status,
                    upload_user_id = x.create_id,
                    upload_user_name = lstUser.Where(x => x.user_id == (int)x.create_id).FirstOrDefault().user_true_name,
                    file_version = x.file_version,
                    modify_date = x.modify_date,
                    submit_date = x.submit_date,
                    submit_status = x.submit_status,
                    file_type = x.file_type,
                    file_name = x.file_name,
                    //file_path = x.file_path,
                    check_list = x.check_list,
                    create_date = x.create_date
                }).ToList()
            }).OrderBy(d => d.Version).ToListAsync();
            return WebResponseContent.Instance.OK("Ok", result);
        }
        public async Task<WebResponseContent> UploadFileNew(UploadSubmissionFilesRecordNewDto recordDto, List<IFormFile> file = null)
        {
            try
            {
                var contract = _repository.Find(d => d.id == recordDto.subId).FirstOrDefault();
                if (contract == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                var contract_id = contract.contract_id;
                //var fileTypeFloder =  GetFloderByType(recordDto.file_type, recordDto.version,(Guid)contract_id);
                var fileTypeFloder = GetFloderByTypeNew(recordDto, (Guid)contract_id);
                if (fileTypeFloder == null) return WebResponseContent.Instance.Error(_localizationService.GetString("directory_creation_failed"));
                if (file.Count > 0)
                {
                    var saveFileResult = _FileRecordsService.SaveFileByPath(file, fileTypeFloder as string);
                    var fileInfo = saveFileResult.data as List<FileInfoDto>;
                    if (fileInfo == null || fileInfo.Count == 0)
                    {
                        return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                    }
                    #region 新库
                    List<Biz_Project_Files> files = new List<Biz_Project_Files>();
                    foreach (var item in fileInfo)
                    {
                        Biz_Project_Files _Files = new Biz_Project_Files
                        {
                            id = Guid.NewGuid(),
                            relation_id = recordDto.subId,
                            version = 1,
                            file_type = recordDto.file_type,
                            file_name = item.file_name,
                            file_ext = item.file_ext,
                            file_path = item.file_relative_path,
                            file_size = item.file_size,
                            check_list = recordDto.check_list,
                            inner_status = (int)InnerStatusEnum.Editing,
                            submit_status = (int)SubmissionFileStatusEnum.ToDo,
                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = DateTime.Now,
                        };
                        files.Add(_Files);
                    }
                    var submit_date = _repository.Find(d => d.id == recordDto.subId).FirstOrDefault();
                    if (submit_date != null)
                    {
                        submit_date.version = recordDto.version;
                        submit_date.modify_id = UserContext.Current.UserId;
                        submit_date.modify_name = UserContext.Current.UserName;
                        submit_date.modify_date = DateTime.Now;
                        _repository.Update(submit_date);
                    }
                    await _projectFilesRepository.AddRangeAsync(files);
                    await _projectFilesRepository.SaveChangesAsync();
                    #endregion
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
        public async Task<WebResponseContent> UploadFileCover(UploadSubmissionFilesCoverDto recordDto, IFormFile file = null)
        {
            try
            {
                var contract = _repository.Find(d => d.id == recordDto.subId).FirstOrDefault();
                if (contract == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                var contract_id = contract.contract_id;
                if (file != null)
                {
                    var oldFile = _projectFilesRepository.Find(d => d.id == recordDto.file_Id && d.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                    if (oldFile != null)
                    {
                        #region 移除旧文件
                        _FileRecordsService.MoveFileToTemporary(oldFile.file_path, oldFile.file_name, null);
                        #endregion
                    }
                    else { return WebResponseContent.Instance.Error(_localizationService.GetString("record_null")); }
                    //var fileTypeFloder =  GetFloderByType(recordDto.file_type, oldFile.version,(Guid)contract_id);
               
                    UploadSubmissionFilesRecordNewDto Dto = new UploadSubmissionFilesRecordNewDto
                    {
                        file_type = (int)oldFile.file_type,
                        version = oldFile.version,
                        subId = recordDto.subId,
                        check_list = oldFile.check_list
                    };
                    var fileTypeFloder = GetFloderByTypeNew(Dto, (Guid)contract_id);
                    if (fileTypeFloder == null) return WebResponseContent.Instance.Error(_localizationService.GetString("directory_creation_failed"));
                    var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, fileTypeFloder as string);
                    var fileInfo = saveFileResult.data as List<FileInfoDto>;
                    if (fileInfo == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                    //保存数据库
                    var single_file = fileInfo.FirstOrDefault();
                    if (single_file != null)
                    {
                        oldFile.file_name = single_file.file_name;
                        oldFile.file_ext = single_file.file_ext;
                        oldFile.file_path = single_file.file_relative_path;
                        oldFile.file_size = single_file.file_size;
                        oldFile.file_version = oldFile.file_version + 1;
                        oldFile.modify_id = UserContext.Current.UserId;
                        oldFile.modify_name = UserContext.Current.UserName;
                        oldFile.modify_date = DateTime.Now;
                        _projectFilesRepository.Update(oldFile);
                    }
                    await _projectFilesRepository.SaveChangesAsync();
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
        public async Task<WebResponseContent> RenameFile(SubmissionRenameFileDto fileDto)
        {
            try
            {
                if (fileDto == null || fileDto.file_Id == Guid.Empty) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                var fileData = _projectFilesRepository.Find(d => d.id == fileDto.file_Id).FirstOrDefault();
                if(fileData ==null) return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                //获取文件完整路径
                var contract = _repository.Find(d => d.id == fileDto.subId).FirstOrDefault();
                if (contract == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                var contract_id = contract.contract_id;
                //var fileTypeFloder = GetFloderByType((int)fileData.file_type, fileData.version,(Guid)contract_id);
                UploadSubmissionFilesRecordNewDto newDto = new UploadSubmissionFilesRecordNewDto
                {
                    subId = fileDto.subId,
                    file_type = (int)fileData.file_type,
                    version = fileData.version,
                    check_list = fileData.check_list,
                };
                var fileTypeFloder = GetFloderByTypeNew(newDto, (Guid)contract_id);
                var fileRelPathNew = fileTypeFloder + $"\\{fileDto.file_name}.{fileData.file_ext}";
                var fileAbsPathNew = Path.Combine(AppSetting.FileSaveSettings.FolderPath, fileRelPathNew);
                var fullPathOld = Path.Combine(AppSetting.FileSaveSettings.FolderPath, fileData.file_path);

               var res = FileHelper.RenameFile(fullPathOld, fileAbsPathNew);
                if (res) 
                {
                    fileData.file_name = $"{fileDto.file_name}.{fileData.file_ext}";
                    fileData.file_path = fileRelPathNew;
                    fileData.modify_date = DateTime.Now;
                    fileData.modify_id = UserContext.Current.UserId;
                    fileData.modify_name = UserContext.Current.UserName;
                    _projectFilesRepository.Update(fileData);
                    await _projectFilesRepository.SaveChangesAsync();
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
        public async Task<WebResponseContent> CopyFile(SubmissionFilesCopyDto copyDto)
        {
            try
            {
                if (copyDto == null || copyDto.file_Id == Guid.Empty) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                var fileData = _projectFilesRepository.Find(d => d.id == copyDto.file_Id).FirstOrDefault();
                if (fileData == null) return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                var contract = _repository.Find(d => d.id == copyDto.subId).FirstOrDefault();
                if (contract == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                var contract_id = contract.contract_id;
                #region 1.复制文件
                UploadSubmissionFilesBaseDto baseDto = new UploadSubmissionFilesBaseDto
                {
                    subId = copyDto.subId,
                    file_type = copyDto.target_type,
                    version = copyDto.target_version
                };
                var fileTypeFloder = GetFloderByTypeNew(baseDto, (Guid)contract_id);
                if (fileTypeFloder == null) return WebResponseContent.Instance.Error(_localizationService.GetString("directory_creation_failed"));
                
                var newPath = fileTypeFloder+ $"\\{fileData.file_name}";
                var newPathFull = Path.Combine(AppSetting.FileSaveSettings.FolderPath, newPath);
                var oldPath = Path.Combine(AppSetting.FileSaveSettings.FolderPath, fileData.file_path);
                var copy_res = await FileHelper.CopyFile(oldPath, newPathFull);
                if(!copy_res) return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                #endregion

                #region 2.保存数据库 
                Biz_Project_Files project_Files = new Biz_Project_Files 
                {
                    id = Guid.NewGuid(),
                    version = copyDto.target_version,
                    file_type =copyDto.target_type,
                    file_name = fileData.file_name,
                    file_path = newPath,//fileData.file_path,
                    file_size = fileData.file_size,
                    file_ext = fileData.file_ext,
                    relation_id = fileData.relation_id,
                    file_version = fileData.file_version,
                    check_list = fileData.check_list,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now,
                };
                await _projectFilesRepository.AddAsync(project_Files);
                await _projectFilesRepository.SaveChangesAsync();
                #endregion
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }  
        }
        public async Task<WebResponseContent> MoveFile(SubmissionFilesCopyDto copyDto)
        {
            try
            {
                if (copyDto == null || copyDto.file_Id == Guid.Empty) return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                var fileData = _projectFilesRepository.Find(d => d.id == copyDto.file_Id).FirstOrDefault();
                if (fileData == null) return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                var contract = _repository.Find(d => d.id == copyDto.subId).FirstOrDefault();
                if (contract == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                var contract_id = contract.contract_id;
                #region 1.移动文件
                UploadSubmissionFilesBaseDto baseDto = new UploadSubmissionFilesBaseDto
                {
                    subId = copyDto.subId,
                    file_type = copyDto.target_type,
                    version = copyDto.target_version
                };
                var fileTypeFloder = GetFloderByTypeNew(baseDto, (Guid)contract_id);
                if (fileTypeFloder == null) return WebResponseContent.Instance.Error(_localizationService.GetString("directory_creation_failed"));

                var newPath = fileTypeFloder + $"\\{fileData.file_name}";
                var newPathFull = Path.Combine(AppSetting.FileSaveSettings.FolderPath, newPath);
                var oldPath = Path.Combine(AppSetting.FileSaveSettings.FolderPath, fileData.file_path);
                var move_res = FileHelper.MoveFile(oldPath, newPathFull);
                if (!move_res) return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                #endregion

                #region 2.保存数据库 
                Biz_Project_Files project_Files = new Biz_Project_Files
                {
                    id = Guid.NewGuid(),
                    version = copyDto.target_version,
                    file_type = copyDto.target_type,
                    file_name = fileData.file_name,
                    file_path = newPath,//fileData.file_path,
                    file_size = fileData.file_size,
                    file_ext = fileData.file_ext,
                    relation_id = fileData.relation_id,
                    file_version = fileData.file_version,
                    check_list = fileData.check_list,
                    inner_status = (int)InnerStatusEnum.Editing,
                    submit_status = (int)SubmissionFileStatusEnum.ToDo,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now,
                };
                await _projectFilesRepository.AddAsync(project_Files);

                //移动文件后删除记录
                fileData.delete_status = (int)SystemDataStatus.Invalid;
                fileData.modify_date = DateTime.Now;
                fileData.modify_id = UserContext.Current.UserId;
                fileData.modify_name = UserContext.Current.UserName;
                _projectFilesRepository.Update(fileData);

                await _projectFilesRepository.SaveChangesAsync();
                #endregion
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> CheckFileNoNew(SubmissionFilesCheckDto dto)
        {
            var checkFileNo = await _repository.ExistsAsync(d => d.contract_id == dto.contract_id && d.delete_status == (int)SystemDataStatus.Valid && d.file_no == dto.fileNo && d.main_type == dto.mainType && (string.IsNullOrEmpty(dto.childType)?true:  d.child_type == dto.childType));
            if (checkFileNo) return WebResponseContent.Instance.Error(_localizationService.GetString("file_no_exist！"));
            else return WebResponseContent.Instance.Error(_localizationService.GetString("non_existent！"));
        }
        #endregion

        #region 私有方法
        private string GetFloderByType(int file_type,int version, Guid contract_id)
        {
            try
            {
                var fileTypeFloder = "";
                var versionStr = CommonHelper.GetSubmissionVersionStr(version);
                if (file_type == (int)SubmissionFileEnum.MasterFile) fileTypeFloder = "Submit";
                else if (file_type == (int)SubmissionFileEnum.EditFile) fileTypeFloder = "Edit";
                else if (file_type == (int)SubmissionFileEnum.ReferenceFile) fileTypeFloder = "Information";
                else fileTypeFloder = "Comments";

                #region 数据库创建配置
                //var new_file_no = file_no.replace("/", "_");
                var file_code = UploadFileCode.Submission_Files+"_" + versionStr + "_"+ fileTypeFloder;
                var floder_path = $@"\Submission_Files\{versionStr}\{fileTypeFloder}";
                var add_res = _fileConfigService.AddCofig(file_code, floder_path);
                #endregion
                if (add_res) 
                {
                    var strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, file_code).Result.data.ToString();
                    if (!Directory.Exists(Path.Combine(AppSetting.FileSaveSettings.FolderPath, strContractFolder)))
                    {
                        Directory.CreateDirectory(Path.Combine(AppSetting.FileSaveSettings.FolderPath, strContractFolder));
                    }
                    return strContractFolder;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return null;
            }
        }

        private string GetFloderByTypeNew(UploadSubmissionFilesBaseDto recordDto, Guid contract_id)
        {
            try
            {
                var fileTypeFloder = "";
                if (recordDto.file_type == (int)SubmissionFileEnum.MasterFile) fileTypeFloder = "Submit";
                else if (recordDto.file_type == (int)SubmissionFileEnum.EditFile) fileTypeFloder = "Edit";
                else if (recordDto.file_type == (int)SubmissionFileEnum.ReferenceFile) fileTypeFloder = "Information";
                else fileTypeFloder = "Comments";

                var subData = _repository.Find(d => d.id == recordDto.subId).FirstOrDefault();
                var main_type = subData.main_type;
                var chiild_type = subData.child_type;
                var versionStr = CommonHelper.GetSubmissionVersionIntStr(recordDto.version);

                var file_code = "";
                var floder_path = "";
                var new_file_no = subData.file_no.replace("/", "_");
                if (main_type.contains("General"))
                {
                    file_code = UploadFileCode.Submission_Files + "_" + main_type + "_" + new_file_no + "_" + versionStr + "_" + fileTypeFloder;
                    floder_path = $@"\Submission\{main_type}\{new_file_no}_{versionStr}\{fileTypeFloder}\";
                }
                else
                {
                    file_code = UploadFileCode.Submission_Files + "_" + main_type + "_" + chiild_type + "_" + new_file_no + "_" + versionStr+"_"+ fileTypeFloder;
                    floder_path = $@"\Submission\{main_type}\{chiild_type}\{new_file_no}_{versionStr}\{fileTypeFloder}\";
                }

                #region 数据库创建配置
                var add_res = _fileConfigService.AddCofig(file_code, floder_path);
                #endregion
                if (add_res)
                {
                    var strContractFolder = _projectFilesService.GetFileFolderByContactIdAsync((Guid)contract_id, file_code).Result.data.ToString();
                    if (!Directory.Exists(Path.Combine(AppSetting.FileSaveSettings.FolderPath, strContractFolder)))
                    {
                        Directory.CreateDirectory(Path.Combine(AppSetting.FileSaveSettings.FolderPath, strContractFolder));
                    }
                    return strContractFolder;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return null;
            }
        }
        #endregion
    }
}
