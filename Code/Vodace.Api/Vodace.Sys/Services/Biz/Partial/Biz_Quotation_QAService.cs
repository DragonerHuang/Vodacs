
using AutoMapper;
using Dm.util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
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
    public partial class Biz_Quotation_QAService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Quotation_QARepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly ISys_File_RecordsRepository _RecordsRepository;
        private readonly ISys_File_RecordsService _FileRecordsService;
        private readonly IBiz_QuotationService _QuotationService;
        private readonly IBiz_ContractService _ContractService;
        private readonly IBiz_ContractRepository _ContractRepository;
        private readonly IBiz_QuotationRepository _QuotationRepository;
        private readonly string _strUploadPath;         // 配置的上传地址

        private readonly IBiz_Quotation_HeadRepository _quotationHeadRepository;   // 负责人仓储
        private readonly IBiz_Quotation_DeadlineService _quotationDeadlineService; // 期限管理服务

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_QAService(
            IBiz_Quotation_QARepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            ISys_File_RecordsRepository sys_File_RecordsRepository,
            IMapper mapper,
            ISys_File_RecordsService fileRecordsService,
            IBiz_QuotationService quotationService,
            IBiz_ContractService contractService,
            IBiz_ContractRepository contractRepository,
            IBiz_QuotationRepository quotationRepository,
            IBiz_Quotation_HeadRepository quotationHeadRepository,
            IBiz_Quotation_DeadlineService quotationDeadlineService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _RecordsRepository = sys_File_RecordsRepository;
            _FileRecordsService = fileRecordsService;
            _QuotationService = quotationService;
            _strUploadPath = AppSetting.FileSaveSettings.FolderPath;
            _ContractService = contractService;
            _ContractRepository = contractRepository;
            _QuotationRepository = quotationRepository;
            _quotationHeadRepository = quotationHeadRepository;
            _quotationDeadlineService = quotationDeadlineService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public override PageGridData<Biz_Quotation_QA> GetPageData(PageDataOptions options)
        {
            return base.GetPageData(options);
        }

        public PageGridData<QuotationQAListDto> GetListByPage(PageInput<QuotationQA_Query> query)
        {
            PageGridData<QuotationQAListDto> pageGridData = new PageGridData<QuotationQAListDto>();
            var queryPam = query.search;
            var code = queryPam.type == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents;
            var lstFile = _RecordsRepository.Find(d => d.file_code == code);
            var lstData = repository.Find(d =>d.delete_status == 0
            && (queryPam.type == 0 ? true : d.type == (int)queryPam.type)
            && (queryPam.qn_id == Guid.Empty ? true : d.qn_id == queryPam.qn_id)).Select(d => new QuotationQAListDto
            {
                id = d.id,
                qn_id = d.qn_id,
                type = d.type,
                end_date = d.end_date,
                file_name = d.create_name,
                issue_date = d.create_date,
                subject = d.subject,
                submit_date = d.submit_date,
            });
            int skip = (query.page_index - 1) * query.page_rows;
            pageGridData.data = lstData.Skip(skip).Take<QuotationQAListDto>(query.page_rows).ToList();
            pageGridData.status = true;
            pageGridData.total = lstData.ToList().Count;
            return pageGridData;
        }

        public WebResponseContent AddData(QuotationQADto quotationQADto)
        {
            try
            {
                if (quotationQADto == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                var entData = _mapper.Map<Biz_Quotation_QA>(quotationQADto);
                var isExistData = _repository.Exists(d => d.delete_status == 0 && d.subject == quotationQADto.subject);
                if (isExistData) return WebResponseContent.Instance.Error(_localizationService.GetString("qa_subject_exist"));
                entData.id = Guid.NewGuid();
                entData.create_id = UserContext.Current.UserId;
                entData.create_date = DateTime.Now;
                entData.delete_status = (int)SystemDataStatus.Valid;
                entData.create_name = UserContext.Current.UserName;
                repository.Add(entData);

                if (quotationQADto.FileIds.Count() > 0) 
                {
                    var fileIds = quotationQADto.FileIds.ToList();
                    var fileData = _RecordsRepository.Find(d => d.file_code == "qa_pre" || d.file_code == "qa_ten").ToList();
                    foreach (var item in fileIds)
                    {
                        var upd_data = fileData.Where(d => d.id == item).FirstOrDefault();
                        if (upd_data != null)
                        {
                            upd_data.master_id = entData.id;
                            upd_data.upload_status = 1;
                            upd_data.modify_name = UserContext.Current.UserName;
                            upd_data.modify_id = UserContext.Current.UserId;
                            upd_data.modify_date = DateTime.Now;
                            _RecordsRepository.Update(upd_data, false);
                        }
                    }
                    _RecordsRepository.SaveChanges();
                }

                _RecordsRepository.SaveChanges();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent AddDataWithFile(QuotationQAWithFileAddDto quotationQADto, IFormFile file = null) 
        {
            try
            {
                if (quotationQADto == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                var entData = _mapper.Map<Biz_Quotation_QA>(quotationQADto);
                var isExistData = _repository.Exists(d => d.delete_status == 0 && d.subject == quotationQADto.subject && d.type == quotationQADto.type && d.qn_id == quotationQADto.qn_id);
                if (isExistData) return WebResponseContent.Instance.Error(_localizationService.GetString("qa_subject_exist"));

                entData.delete_status = (int)SystemDataStatus.Valid;
                entData.id = Guid.NewGuid();
                entData.create_id = UserContext.Current.UserId;
                entData.create_date = DateTime.Now;
                entData.create_name = UserContext.Current.UserName;
       
                repository.Add(entData);

                if (file != null)
                {
                    //获取报价所在的文件夹
                    if (!entData.qn_id.HasValue)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                    }
                    var getfolderResult = _QuotationService.GetRootFolderName(entData.qn_id.Value);
                    if (!getfolderResult.status)
                    {
                        return getfolderResult;
                    }

                    //读取文件配置地址
                    var strFolderPath = _FileRecordsService.GetFileSaveFolder(
                        quotationQADto.type == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents,
                        getfolderResult.data as string);

                    //保存文件
                    var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, strFolderPath.data as string);
                    var fileInfo = saveFileResult.data as List<FileInfoDto>;
                    if (fileInfo == null)
                    {
                        return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                    }

                    //保存数据库
                    foreach (var item in fileInfo)
                    {
                        var fileData = new Sys_File_Records
                        {
                            id = Guid.NewGuid(),
                            master_id = entData.id,
                            file_name = item.file_name,
                            file_ext = item.file_ext,
                            file_path = item.file_relative_path,
                            file_code = quotationQADto.type == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents,
                            file_size = item.file_size,
                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = DateTime.Now,
                            upload_status = (int)UploadStatus.Finish
                        };
                        _RecordsRepository.Add(fileData);
                    }
                }
                
                repository.SaveChanges();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent EditDataWithFile(QuotationQAWithFileEditDto quotationQADto, IFormFile file = null)
        {
            try
            {
                if (quotationQADto.id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                if (quotationQADto == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                var entData = _mapper.Map<Biz_Quotation_QA>(quotationQADto);
                var isExistData = _repository.Exists(d => d.delete_status == 0 && d.subject == quotationQADto.subject && d.type == quotationQADto.type && d.id != quotationQADto.id && d.qn_id == quotationQADto.qn_id);
                if (isExistData) return WebResponseContent.Instance.Error(_localizationService.GetString("qa_subject_exist"));

                entData.modify_id = UserContext.Current.UserId;
                entData.modify_date = DateTime.Now;
                entData.modify_name = UserContext.Current.UserName;

                repository.Add(entData);

                //保存文件
                if (file != null)
                {
                    //获取报价所在的文件夹
                    if (!entData.qn_id.HasValue)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                    }
                    var getfolderResult = _QuotationService.GetRootFolderName(entData.qn_id.Value);
                    if (!getfolderResult.status)
                    {
                        return getfolderResult;
                    }

                    //读取文件配置地址
                    var strFolderPath = _FileRecordsService.GetFileSaveFolder(
                        quotationQADto.type == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents,
                        getfolderResult.data as string);

                    var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, strFolderPath.data as string);
                    var fileInfo = saveFileResult.data as List<FileInfoDto>;
                    if (fileInfo == null)
                    {
                        return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                    }
                   
                    var oldFile = _RecordsRepository.Find(d => d.master_id == entData.id).ToList();
                    foreach (var item in oldFile)
                    {
                        item.delete_status = (int)SystemDataStatus.Invalid;
                        item.modify_date = DateTime.Now;
                        item.modify_name = UserContext.Current.UserName;
                        item.modify_id = UserContext.Current.UserId;
                    }
                    _RecordsRepository.UpdateRange(oldFile);
                    var fileData = new Sys_File_Records
                    {
                        id = Guid.NewGuid(),
                        master_id = entData.id,
                        file_name = fileInfo[0].file_name,
                        file_ext = fileInfo[0].file_ext,
                        file_path = fileInfo[0].file_relative_path,
                        file_code = quotationQADto.type == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents,
                        file_size = (int)file.Length,
                        delete_status = (int)SystemDataStatus.Valid,
                        create_id = UserContext.Current.UserId,
                        create_name = UserContext.Current.UserName,
                        create_date = DateTime.Now,
                        upload_status = (int)UploadStatus.Finish
                    };

                    _RecordsRepository.Add(fileData);
                }

                repository.SaveChanges();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent EditData(QuotationQADto quotationQADto) 
        {
            try
            {
                if (quotationQADto == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                if (quotationQADto.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("update_id_empty"));
                var entData =repository.Find(d=>d.id == quotationQADto.id).FirstOrDefault();
                if (entData != null) 
                {
                    entData = _mapper.Map<Biz_Quotation_QA>(entData);
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    var res = repository.Update(entData,true);

                    if (quotationQADto.FileIds.Count() > 0)
                    {
                        var fileIds = quotationQADto.FileIds.ToList();
                        var fileData = _RecordsRepository.Find(d => d.file_code == "qa_pre" || d.file_code == "qa_ten" && d.master_id == entData.id).ToList();
                        foreach (var item in fileIds)
                        {
                            var upd_data = fileData.Where(d => d.id == item).FirstOrDefault();
                            if (upd_data != null) 
                            {
                                upd_data.master_id = entData.id;
                                upd_data.upload_status = 1;
                                upd_data.modify_name = UserContext.Current.UserName;
                                upd_data.modify_id = UserContext.Current.UserId;
                                upd_data.modify_date = DateTime.Now;
                                _RecordsRepository.Update(upd_data, false);
                            }
                        }
                        _RecordsRepository.SaveChanges();
                    }
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                    return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed")); 
            }
        }


        public async Task<WebResponseContent> UploadQAFile(IFormFile file, int intSubmitType, Guid guidQaId)
        {
            try
            {
                if (file == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                var strFileType = intSubmitType == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents;

                #region 删除旧文件
                var qaData = repository.Find(d => d.id == guidQaId).FirstOrDefault();
                var oldFile = _RecordsRepository.Find(d => d.file_code == strFileType && d.master_id == guidQaId).FirstOrDefault();
                if (oldFile != null)
                {
                    oldFile.delete_status = (int)SystemDataStatus.Invalid;
                    oldFile.modify_date = DateTime.Now;
                    oldFile.modify_id = UserContext.Current.UserId;
                    oldFile.modify_name = UserContext.Current.UserName;
                    _RecordsRepository.Update(oldFile,true);
                }
                if (qaData != null) 
                {
                    qaData.modify_name = UserContext.Current.UserName;
                    qaData.modify_id = UserContext.Current.UserId;
                    qaData.modify_date = DateTime.Now;
                    _repository.Update(qaData,true);

                    #endregion
                    var getfolderResult = _QuotationService.GetRootFolderName(qaData.qn_id.Value);
                    if (!getfolderResult.status)
                    {
                        return getfolderResult;
                    }
                    var strFolder = getfolderResult.data as string;
                    List<IFormFile> lstFiles = new List<IFormFile>();
                    lstFiles.Add(file);
                    return await _FileRecordsService.CommonUploadAsync(lstFiles, strFileType, strFolder, guidQaId);
                }
                else
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                } 
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> DownloadSysQAFile(Guid guidFileId,int intSubmitType)
        {
            try
            {
                var code = intSubmitType == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents;
                var file = await _RecordsRepository.FindAsyncFirst(p => p.master_id == guidFileId && p.file_code == code && p.delete_status == 0);
                if (file == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_info_null"));
                }

                // 检查文件是否存在
               var strSaveFolder = _strUploadPath;
                var strFilePath = Path.Combine(strSaveFolder, file.file_path);
                if (!File.Exists(strFilePath))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_download_null"));
                }

                var bytsFile = File.ReadAllBytes(strFilePath);   // 读取文件内容
                var strContentType = _FileRecordsService.GetContentType(strFilePath);// 获取文件的MIME类型

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
        public WebResponseContent DelData(Guid id)
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
                    var res = repository.Update(entData, true);
                    if (res > 0)return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
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

        public async Task<WebResponseContent> DownLoadFiles(Guid qnid,int type) 
        {
            var ids= _repository.Find(d => d.delete_status == (int)SystemDataStatus.Valid && d.qn_id == qnid && d.type == type).Select(d=>d.id).ToList();
            var lstData = _RecordsRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid && ids.Contains(d.master_id.Value) && (d.file_code == UploadFileCode.Preliminary_Enquiry_QA_Documents || d.file_code == UploadFileCode.Tender_QA_Documents)).ToList();
            if (lstData.Count == 0)
            {
                return WebResponseContent.Instance.Error(_localizationService.GetString("file_submit_no_found"));
            }
            var data = await _FileRecordsService.ZipFileAsync(lstData);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data);
        }

        /// <summary>
        /// 获取问答列表（分页）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetDataByPage(PageInput<QuotationQA_Query> query)
        {
            try
            {
                var queryPam = query.search;
                var code = queryPam.type == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents;

                // 获取报价 、合同
                var qnData = await _QuotationRepository.FindFirstAsync(p => p.id == queryPam.qn_id);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                var ctrData = await _ContractRepository.FindAsyncFirst(p => p.id == qnData.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                var lstFile = _RecordsRepository.FindAsIQueryable(d => d.file_code == code && d.delete_status == (int)SystemDataStatus.Valid);

                var lstSearch = _repository.WhereIF(true, p => p.qn_id == queryPam.qn_id && p.delete_status == 0)
                                           .WhereIF(queryPam.type != null && queryPam.type != -1, p => p.type == queryPam.type)
                                           .OrderBy(p => p.create_date)
                                           .Select(p => new QuotationQAListDto
                                           {
                                               id = p.id,
                                               subject = p.subject,
                                               type = p.type,
                                               submit_date = p.submit_date,
                                               issue_date = p.issue_date,
                                               end_date = p.end_date,
                                               qn_id = p.qn_id,
                                               //file_name = lstFile.FirstOrDefault(c => c.master_id == p.id).file_name,
                                               remark = p.remark,
                                           });

                //var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
                //(queryPam.qn_id == null || queryPam.qn_id == Guid.Empty ? true : d.qn_id == queryPam.qn_id)
                //&& (queryPam.type == -1 || queryPam.type  == null ? true : d.type == queryPam.type)).Select(d => new QuotationQAListDto
                //{
                //    id = d.id,
                //    subject = d.subject,
                //    type = d.type,
                //    submit_date = d.submit_date,
                //    issue_date = d.issue_date,
                //    end_date = d.end_date,
                //    qn_id = d.qn_id,
                //    file_name = lstFile.FirstOrDefault(c=>c.master_id == d.id).file_name,
                //    remark = d.remark,
                //});
                //var result = await lstData.GetPageResultAsync(query);
                var result = await lstSearch.GetPageResultAsync(query);

                int itemRow = 1;
                foreach (var item in result.data)
                {
                    // 公开招标中的预审问答中第一份是预审资格，其他都是Q&A文件
                    if (code == UploadFileCode.Preliminary_Enquiry_QA_Documents)
                    {
                        if (ctrData.tender_type == "Advertisement")
                        {
                            item.type_name_cht = itemRow == 1 ? "预审资格" : "预审资格问答";
                            item.type_name_eng = itemRow == 1 ? "Pre-qualification" : "Pre_qualififaction Q&A";
                        }
                        else
                        {
                            item.type_name_cht = "预审问答";
                            item.type_name_eng = "Pre-tender Q&A";
                        }
                        itemRow++;
                    }
                    else
                    {
                        item.type_name_cht = "投标问答";
                        item.type_name_eng = "Tender Q&A";
                    }

                }
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
         
        /// <summary>
        /// 添加问答记录
        /// </summary>
        /// <param name="quotationQADto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> AddQADataAsync(QuotationQAWithFileAddDto quotationQADto)
        {
            try
            {
                if (quotationQADto == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                } 
                
                // 添加问答
                var entData = _mapper.Map<Biz_Quotation_QA>(quotationQADto);
                var isExistData = _repository.Exists(d => d.delete_status == 0 && d.subject == quotationQADto.subject);
                if (isExistData)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qa_subject_exist"));
                }

                entData.id = Guid.NewGuid();
                entData.create_id = UserContext.Current.UserId;
                entData.create_date = DateTime.Now;
                entData.delete_status = (int)SystemDataStatus.Valid;
                entData.create_name = UserContext.Current.UserName;
                _repository.Add(entData);


                var qnData = await _QuotationRepository.FindFirstAsync(p => p.id == entData.qn_id);      // 报价
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                var ctrData = await _ContractRepository.FindAsyncFirst(p => p.id == qnData.contract_id); // 合同
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                //await DoDeadlineData(entData.id, entData.end_date, entData.type.Value, qnData, ctrData, false);


                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 修改问答记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditQADataAsync(EditQuotationQADto input)
        {
            try
            {
                if (input == null) return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                if (input.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("update_id_empty"));

                // 修改记录
                var entData = _repository.Find(d => d.id == input.id).FirstOrDefault();
                if (entData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qa_item_null"));
                }
                var isExistData = _repository.Exists(d => d.delete_status == 0 && d.subject == input.subject && d.type == input.type && d.id != input.id && d.qn_id == input.qn_id);
                if (isExistData)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qa_subject_exist"));
                }
                entData.subject = input.subject;
                entData.issue_date = input.issue_date;
                entData.end_date = input.end_date;
                entData.submit_date = input.submit_date;
                entData.remark = input.remark;
                entData.modify_date = DateTime.Now;
                entData.modify_id = UserContext.Current.UserId;
                entData.modify_name = UserContext.Current.UserName;

                var qnData = await _QuotationRepository.FindFirstAsync(p => p.id == entData.qn_id);      // 报价
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                var ctrData = await _ContractRepository.FindAsyncFirst(p => p.id == qnData.contract_id); // 合同
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 处理期限
                //var doDeadlineResult = await DoDeadlineData(entData.id, entData.end_date, entData.type.Value, qnData, ctrData, input.upload_finish_file_info.Count > 0);
                //if (!doDeadlineResult.status)
                //{
                //    return doDeadlineResult;
                //}

                // 处理文件
                var fileQACode = input.type == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents;
                var doQAFileResult = await DoSaveFile(qnData, entData.id, fileQACode, input.upload_qa_file_info, entData.subject);
                if (!doQAFileResult.status)
                {
                    return doQAFileResult;
                }

                var fileFinishCode = input.type == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Finsih_Documents : UploadFileCode.Tender_QA_Finish_Documents;
                var doFinishFileResult = await DoSaveFile(qnData, entData.id, fileFinishCode, input.upload_finish_file_info, entData.subject);
                if (!doFinishFileResult.status)
                {
                    return doFinishFileResult;
                }

                _repository.Update(entData);

                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 根据id获取问答记录
        /// </summary>
        /// <param name="qaId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetQADataByIdAsync(Guid qaId)
        {
            try
            {
                var qaData = _repository.Find(d => d.id == qaId).FirstOrDefault();
                if (qaData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qa_item_null"));
                }

                var entData = _mapper.Map<EditQuotationQADto>(qaData);

                // 获取文件信息
                var lstFiles = await _RecordsRepository
                   .FindAsync(p => p.master_id == qaId &&
                                   p.upload_status == (int)UploadStatus.Finish &&
                                   p.delete_status == (int)SystemDataStatus.Valid);
                var lstQAFiles = new List<ContractQnFileDto>();
                var lstFinishFiles = new List<ContractQnFileDto>();
                foreach (var item in lstFiles)
                {
                    var dtoData = new ContractQnFileDto
                    {
                        file_id = item.id,
                        file_name = item.file_name,
                        file_remark = item.remark,
                        file_size = item.file_size.HasValue ? item.file_size.Value : 0
                    };

                    if (item.file_code == UploadFileCode.Preliminary_Enquiry_QA_Documents || item.file_code == UploadFileCode.Tender_QA_Documents)
                    {
                        entData.upload_qa_file_info.add(dtoData);
                    }
                    else
                    {
                        entData.upload_finish_file_info.add(dtoData);
                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), entData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 问答文件上传
        /// </summary>
        /// <param name="lstFiles">文件列表</param>
        /// <param name="qnId">报价id</param>
        /// <param name="QAType">问答类型 0：预审问答；1：投标问答；</param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadQAFilesAsync(List<IFormFile> lstFiles, Guid qnId, int QAType)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }

                var fileType = QAType == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Documents : UploadFileCode.Tender_QA_Documents;

                return await _FileRecordsService.CommonUploadToTempAsync(lstFiles, fileType);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 问答完成文件上传
        /// </summary>
        /// <param name="lstFiles">文件列表</param>
        /// <param name="qnId">报价id</param>
        /// <param name="qaType">问答类型 0：预审问答；1：投标问答；</param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadQAFinishFilesAsync(List<IFormFile> lstFiles, Guid qnId, int qaType)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }

                var fileType = qaType == 0 ? UploadFileCode.Preliminary_Enquiry_QA_Finsih_Documents : UploadFileCode.Tender_QA_Finish_Documents;

                return await _FileRecordsService.CommonUploadToTempAsync(lstFiles, fileType);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据id下载问答文件
        /// </summary>
        /// <param name="qaId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DownLoadQAFilesByIdAsync(Guid qaId)
        {
            try
            {
                return await DownLoadQAFiles(new List<Guid?> { qaId });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 下载全部问答
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="qaType"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DownLoadAllQAFilesAsync(Guid qnId, int qaType)
        {
            try
            {
                var ids = await _repository
                    .FindAsIQueryable(d => d.qn_id == qnId && d.type == qaType && d.delete_status == (int)SystemDataStatus.Valid)
                    .Select(p => p.id)
                    .ToListAsync();

                var qnIds = new List<Guid?>();
                foreach (var item in ids)
                {
                    qnIds.add(item);
                }

                return await DownLoadQAFiles(qnIds, true);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除Q&A
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteQAAsync(Guid id)
        {
            try
            {
                var qaData = _repository.Find(d => d.id == id).FirstOrDefault();
                if (qaData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qa_item_null"));
                }

                // 删除期限记录
                var qnData = await _QuotationRepository.FindFirstAsync(p => p.id == qaData.qn_id);      // 报价
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                var ctrData = await _ContractRepository.FindAsyncFirst(p => p.id == qnData.contract_id); // 合同
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 公开招标中的预审问答中第一份是预审资格，其他都是Q&A文件（预审资格不给删除）
                if (ctrData.tender_type == "Advertisement" && qaData.type == 0)
                {
                    var firstQAData = await _repository
                        .FindAsIQueryable(p => p.qn_id == qaData.qn_id && p.type == qaData.type && p.delete_status == (int)SystemDataStatus.Valid)
                        .OrderBy(p => p.create_date)
                        .FirstOrDefaultAsync();
                   
                    if (firstQAData.id == qaData.id)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("qa_item_null"));
                    }
                }

                // 删除期限
                //var deadType = qaData.type == 0 ? UpcomingEventsEnum.QnPQQA : UpcomingEventsEnum.QnTenderQA;
                //var delResult = await _quotationDeadlineService.DeleteAsync(qaData.qn_id.Value, qaData.id, deadType);
                //if (!delResult.status) 
                //{
                //    return delResult;
                //}

                // 删除文件
                var lstFiles = await _RecordsRepository.FindAsync(p => p.master_id == qaData.id && p.delete_status == (int)SystemDataStatus.Valid);
                _FileRecordsService.DeleteUploadFiles(lstFiles);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 下载问答文件
        /// </summary>
        /// <param name="qaId"></param>
        /// <param name="isWithParentLevel"></param>
        /// <returns></returns>
        private async Task<WebResponseContent> DownLoadQAFiles(List<Guid?> qaId, bool isWithParentLevel = false)
        {
            var lstFiles = await _RecordsRepository
                  .FindAsync(p => qaId.Contains(p.master_id) &&
                                  p.upload_status == (int)UploadStatus.Finish &&
                                  p.delete_status == (int)SystemDataStatus.Valid);

            if (lstFiles.Count == 0)
            {
                return WebResponseContent.Instance.Error(_localizationService.GetString("file_submit_no_found"));
            }
            var data = await _FileRecordsService.ZipFileAsync(lstFiles, isWithParentLevel);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data);
        }


        /// <summary>
        /// 处理期限管理
        /// </summary>
        /// <param name="qaId">Q&Aid</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="qaType">问答类型 0：预审问答；1：投标问答</param>
        /// <param name="isFinish">是否完成</param>
        /// <returns></returns>
        private async Task<WebResponseContent> DoDeadlineData(Guid qaId, DateTime? endTime, int qaType, Biz_Quotation qnData, Biz_Contract ctrData, bool isFinish)
        {
            try
            {
                // 新增或者修改期限管理内容
                var headlerData = await _quotationHeadRepository
                     .FindAsIQueryable(p => p.qn_id == qnData.id &&
                                            p.handler_type == "Tender Document" &&
                                            p.delete_status == (int)SystemDataStatus.Valid)
                     .FirstOrDefaultAsync();

                var deadType = qaType == 0 ? UpcomingEventsEnum.QnPQQA : UpcomingEventsEnum.QnTenderQA;

                // 公开招标中的预审问答中第一份是预审资格，其他都是Q&A文件
                if (ctrData.tender_type == "Advertisement" && qaType == 0)
                {
                    var firstQAData = await _repository
                        .FindAsIQueryable(p => p.qn_id == qnData.id && p.type == qaType && p.delete_status == (int)SystemDataStatus.Valid)
                        .OrderBy(p => p.create_date)
                        .FirstOrDefaultAsync();
                    if (firstQAData == null)
                    {
                        deadType = UpcomingEventsEnum.QnPQ;
                    }
                    else if (firstQAData != null && firstQAData.id == qaId)
                    {
                        deadType = UpcomingEventsEnum.QnPQ;
                    }
                }

                return await _quotationDeadlineService.EditByQAChangeAsync(qnData.id, deadType, qaId, endTime, isFinish);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 保存文件（将文件移动到正式目录）
        /// </summary>
        /// <param name="qnData"></param>
        /// <param name="qnId"></param>
        /// <param name="qaId"></param>
        /// <param name="fileCode"></param>
        /// <param name="fileInfos"></param>
        /// <returns></returns>
        private async Task<WebResponseContent> DoSaveFile(
            Biz_Quotation qnData, 
            Guid qaId, 
            string fileCode, 
            List<ContractQnFileDto> fileInfos,
            string paSubject)
        {
            try
            {
                //if (fileInfos.Count == 0)
                //{
                //    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                //}

                // 正式文件夹
                var getFileFolderResult = _QuotationService.GetFormalFolder(qnData, fileCode);
                if (!getFileFolderResult.status)
                {
                    return getFileFolderResult;
                }
                var fileFolder = getFileFolderResult.data as string;

                // Q&A要带上主题文件夹
                fileFolder = Path.Combine(fileFolder, paSubject.ToValidFolderName());

                // 获取上传的文件记录
                var uploadIds = fileInfos.Where(p => p.file_id.HasValue).Select(p => p.file_id.Value).ToList();
                var dicUpload = fileInfos.Where(p => p.file_id.HasValue).ToDictionary(p => p.file_id.Value);
                var lstNowRecords = await _RecordsRepository.FindAsync(p => uploadIds.Contains(p.id));
                var dicNowRecords = lstNowRecords.ToDictionary(p => p.id);

                // 获取之前存储的文件记录(用来删除的)
                var lstBeforeRecords = await _RecordsRepository.FindAsync(p => p.delete_status == (int)SystemDataStatus.Valid &&
                                                                               p.file_code == fileCode &&
                                                                               p.master_id == qaId &&
                                                                               !uploadIds.Contains(p.id));

                // 处理文件信息
                var lstEditRecords = new List<Sys_File_Records>();
                foreach (var item in lstNowRecords)
                {
                    var boolGet = dicUpload.TryGetValue(item.id, out var dtoFile);
                    if (!boolGet)
                    {
                        continue;
                    }
                    // 没masterid的是新增的
                    if (!item.master_id.HasValue)
                    {
                        if (item.upload_status == (int)UploadStatus.Upload)
                        {
                            item.upload_status = (int)UploadStatus.Finish;
                            // 这个需要移动文件
                            item.file_path = _FileRecordsService.MoveFile(fileFolder, item);
                        }
                        item.master_id = qaId;
                        item.modify_id = UserContext.Current.UserInfo.User_Id;
                        item.modify_name = UserContext.Current.UserInfo.UserName;
                        item.modify_date = DateTime.Now;
                        item.remark = dtoFile.file_remark;
                        lstEditRecords.add(item);
                        continue;
                    }

                    if (item.remark != dtoFile.file_remark)
                    {
                        item.modify_id = UserContext.Current.UserInfo.User_Id;
                        item.modify_name = UserContext.Current.UserInfo.UserName;
                        item.modify_date = DateTime.Now;
                        item.remark = dtoFile.file_remark;
                        lstEditRecords.add(item);
                        continue;
                    }

                }

                // 删除 之前存储的文件记录
                foreach (var brforeItem in lstBeforeRecords)
                {
                    // 根据文件记录id没获取到信息，则是删除的
                    brforeItem.delete_status = (int)SystemDataStatus.Invalid;

                    brforeItem.modify_id = UserContext.Current.UserInfo.User_Id;
                    brforeItem.modify_name = UserContext.Current.UserInfo.UserName;
                    brforeItem.modify_date = DateTime.Now;
                    lstEditRecords.Add(brforeItem);
                }

                if (lstEditRecords.Count > 0)
                {
                    _RecordsRepository.UpdateRange(lstEditRecords);
                }
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

    }
}
