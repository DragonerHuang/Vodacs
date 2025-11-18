
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.DBManager;
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
    public partial class Biz_Contract_DetailsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Contract_DetailsRepository _repository;                        //访问数据库
        private readonly IBiz_QuotationRepository _quotationRepository;                      //访问报价仓储
        private readonly IBiz_ContractRepository _contractRepository;                        //访问合同仓储
        private readonly ISys_File_RecordsRepository _fileRecordsRepository;                 //访问文件记录仓储
        private readonly IBiz_Contract_Tender_AddendumRepository _tender_addendumRepository; //访问标书资料补充仓储
        private readonly ISys_File_ConfigRepository _fileConfigRepository;                   //访问文件配置仓储

        private readonly IBiz_ContractService _contractService;                              //访问合同服务
        private readonly IBiz_QuotationService _quotationService;                            //访问报价服务
        private readonly ISys_File_RecordsService _fileRecordsService;                       //访问文件记录服务
        private readonly IBiz_Quotation_DeadlineService _quotationDeadlineService;           //访问期限管理服务
        private readonly IBiz_Contract_ContactService _contractContactService;               //访问合同联系人服务

        private readonly ILocalizationService _localizationService; //国际化

        [ActivatorUtilitiesConstructor]
        public Biz_Contract_DetailsService(
            IBiz_Contract_DetailsRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IBiz_QuotationRepository quotationRepository,
            IBiz_ContractRepository contractRepository,
            ISys_File_RecordsRepository fileRecordsRepository,
            IBiz_QuotationService quotationService,
            IBiz_Contract_Tender_AddendumRepository tender_addendumRepository,
            ISys_File_RecordsService fileRecordsService,
            IBiz_ContractService contractService,
            IBiz_Quotation_DeadlineService quotationDeadlineService,
            IBiz_Contract_ContactService contractContactService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _quotationRepository = quotationRepository;
            _contractRepository = contractRepository;
            _fileRecordsRepository = fileRecordsRepository;
            _quotationService = quotationService;
            _tender_addendumRepository = tender_addendumRepository;
            _fileRecordsService = fileRecordsService;
            _contractService = contractService;
            _quotationDeadlineService = quotationDeadlineService;
            _contractContactService = contractContactService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        #region 合同资料

        /// <summary>
        /// 获取合同资料
        /// </summary>
        /// <param name="guidQnId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContractData(Guid guidQnId)
        {
            try
            {
                // 获取报价
                var qnData = await _quotationRepository.FindAsyncFirst(p => p.id == guidQnId);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                // 获取合约
                var ctrData = await _contractRepository.FindAsyncFirst(p => p.id == qnData.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }
                // 获取合同详情
                var ctrDetail = await _repository.FindFirstAsync(p => p.contract_id == ctrData.id && p.delete_status == (int)SystemDataStatus.Valid);
                if (ctrDetail == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }
                // 获取上传相关的合同资料
                var lstCtrFiles = await _fileRecordsRepository
                    .FindAsync(p => p.master_id == ctrData.id &&
                                    p.file_code == UploadFileCode.CTR &&
                                    p.upload_status == (int)UploadStatus.Finish &&
                                    p.delete_status == (int)SystemDataStatus.Valid);

                var dtoDetail = new ContractDetailDataDto
                {
                    qn_id = qnData.id,
                    contract_id = ctrData.id,
                    contract_details_id = ctrDetail.id,
                    dto_qn_tender = new TenderQnDto
                    {
                        pro_id = ctrData.project_id,
                        tender_issue_date = ctrDetail.issue_date,
                        tender_end_date = ctrDetail.end_date,
                        tender_title = ctrDetail.title,
                        tender_ref = ctrDetail.pei_tender_ref,
                        tender_subject = ctrDetail.pei_subject,
                        tender_info = ctrDetail.pei_info
                    },
                    dto_qn_contract = new ContractQnDto
                    {
                        contract_name = ctrData.name_eng,
                        contract_no = ctrData.contract_no,
                        contract_ref_no = ctrData.ref_no,
                        contract_title = ctrData.title,
                        contract_category = ctrData.category,
                        contract_description = ctrDetail.remark,
                        contract_trade = ctrDetail.trade,
                        closing_date = ctrDetail.antic_pql_sub_close_date,
                        anticipated_date = ctrDetail.antic_pql_sub_date,
                        ant_send_inv_tender_date = ctrDetail.antic_inv_tndr_date,
                        ant_contract_award_date = ctrDetail.antic_cntr_awd_date,
                        tender_type = ctrData.tender_type,
                        cost_range = ctrDetail.range_cost,
                        upload_file_info = new List<ContractQnFileDto>(),
                        master_id = ctrData.master_id,
                        vo_wo_type = ctrData.vo_wo_type
                    },
                    dto_qn_contact = new ContactQnDto
                    {
                        contact_name = ctrDetail.contact_name,
                        contact_tilte = ctrDetail.contact_title,
                        contact_phone = ctrDetail.contact_tel,
                        contact_mail = ctrDetail.contact_email,
                        contact_faxno = ctrDetail.contact_fax,
                    },
                    is_interest = qnData.is_interest.HasValue ? qnData.is_interest.Value : 1,
                    interest_reason = qnData.interest_reason
                };
                dtoDetail.dto_qn_contacts = await _contractContactService.GetContactsAsync(ctrData.id);


                foreach (var item in lstCtrFiles)
                {
                    dtoDetail.dto_qn_contract.upload_file_info.Add(new ContractQnFileDto
                    {
                        file_id = item.id,
                        file_name = item.file_name,
                        file_remark = item.remark,
                        file_size = item.file_size.HasValue ? item.file_size.Value : 0
                    });
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), dtoDetail);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 保存合同资料
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveContractData(ContractDetailDataDto dtoInput)
        {
            try
            {
                // 获取报价
                var qnData = await _quotationRepository.FindAsyncFirst(p => p.id == dtoInput.qn_id);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                // 获取合约
                var ctrData = await _contractRepository.FindAsyncFirst(p => p.id == dtoInput.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }
                // 获取合同详情
                var ctrDetail = await _repository.FindFirstAsync(p => p.contract_id == ctrData.id && p.delete_status == (int)SystemDataStatus.Valid);
                if (ctrDetail == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                var dtoContractQn = dtoInput.dto_qn_contract;
                var dtoContactQn = dtoInput.dto_qn_contact;
                var dtoTenderQn = dtoInput.dto_qn_tender;

                // 判断合约编码是否有重复
                var boolIsRepeat = _quotationService.CheckRepeatContract(dtoContractQn.contract_no, false, dtoInput.contract_id);
                if (boolIsRepeat)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_no_repeat"));
                }

                // 报价
                qnData.is_interest = dtoInput.is_interest;
                qnData.interest_reason = dtoInput.interest_reason;

                qnData.modify_id = UserContext.Current.UserInfo.User_Id;
                qnData.modify_name = UserContext.Current.UserInfo.UserName;
                qnData.modify_date = DateTime.Now;

                // 合约
                ctrData.contract_no = dtoContractQn.contract_no;
                ctrData.name_cht = dtoContractQn.contract_name;
                ctrData.name_eng = dtoContractQn.contract_name;
                ctrData.ref_no = dtoContractQn.contract_ref_no;
                ctrData.title = dtoContractQn.contract_title;
                ctrData.category = dtoContractQn.contract_category;
                ctrData.tender_type = dtoContractQn.tender_type;
                ctrData.project_id = dtoTenderQn.pro_id;

                if (ctrData.tender_type == "Direct Invitation")
                {
                    if (dtoContractQn.master_id.HasValue)
                    {
                        if (ctrData.id == dtoContractQn.master_id)
                        {
                            return WebResponseContent.Instance.Error(_localizationService.GetString("qn_master_id_same"));
                        }

                        ctrData.master_id = dtoContractQn.master_id;
                        ctrData.vo_wo_type = dtoContractQn.vo_wo_type;
                    }
                    else
                    {
                        ctrData.master_id = null;
                        ctrData.vo_wo_type = "";
                    }
                }
                

                ctrData.modify_id = UserContext.Current.UserInfo.User_Id;
                ctrData.modify_name = UserContext.Current.UserInfo.UserName;
                ctrData.modify_date = DateTime.Now;

                // 合约详情
                ctrDetail.issue_date = dtoTenderQn.tender_issue_date;
                ctrDetail.end_date = dtoTenderQn.tender_end_date;
                ctrDetail.pei_tender_ref = dtoTenderQn.tender_ref;
                ctrDetail.title = dtoTenderQn.tender_title;
                ctrDetail.pei_subject = dtoTenderQn.tender_subject;
                ctrDetail.pei_info = dtoTenderQn.tender_info;

                ctrDetail.remark = dtoContractQn.contract_description;
                ctrDetail.trade = dtoContractQn.contract_trade;
                ctrDetail.antic_pql_sub_close_date = dtoContractQn.closing_date;
                ctrDetail.antic_pql_sub_date = dtoContractQn.anticipated_date;
                ctrDetail.antic_inv_tndr_date = dtoContractQn.ant_send_inv_tender_date;
                ctrDetail.antic_cntr_awd_date = dtoContractQn.ant_contract_award_date;
                ctrDetail.range_cost = dtoContractQn.cost_range;

                ctrDetail.contact_name = dtoContactQn.contact_name;
                ctrDetail.contact_title = dtoContactQn.contact_tilte;
                ctrDetail.contact_tel = dtoContactQn.contact_phone;
                ctrDetail.contact_email = dtoContactQn.contact_mail;
                ctrDetail.contact_fax = dtoContactQn.contact_faxno;

                ctrDetail.modify_id = UserContext.Current.UserInfo.User_Id;
                ctrDetail.modify_name = UserContext.Current.UserInfo.UserName;
                ctrDetail.modify_date = DateTime.Now;

                if (dtoInput.dto_qn_contacts.Count > 0)
                {
                    await _contractContactService.SaveContactsAsync(ctrData.id, dtoInput.dto_qn_contacts);
                }

                //if (ctrData.tender_type == "Advertisement" || ctrData.tender_type == "Preliminary Enquiry Invitation (PEI)")
                //{
                //    // 更改期限内容
                //    var eventEnum = ctrData.tender_type == "Advertisement" ? UpcomingEventsEnum.QnAdvertisement : UpcomingEventsEnum.QnPEI;
                //    await _quotationDeadlineService.EidtByContractDetailsChangeAsync(qnData.id, eventEnum, ctrDetail.id, ctrDetail.end_date, qnData.is_interest == 1);
                //}
                
                #region 正式文件夹的存放路径

                var getFileFolderResult = _quotationService.GetFormalFolder(qnData, UploadFileCode.CTR);
                if (!getFileFolderResult.status)
                {
                    return getFileFolderResult;
                }
                var fileFolder = getFileFolderResult.data as string;

                #endregion

                // 合约文件
                var dicFiles = dtoInput.dto_qn_contract.upload_file_info.ToDictionary(p => p.file_id);
                var lstNowFileRecord = await _fileRecordsRepository.FindAsync(p => dicFiles.Keys.Contains(p.id));

                var lstUpdateFiles = new List<Sys_File_Records>();
                foreach (var item in lstNowFileRecord)
                {
                    var boolOk = dicFiles.TryGetValue(item.id, out var dtoFile);
                    if (!boolOk)
                    {
                        item.delete_status = (int)SystemDataStatus.Invalid;
                        item.modify_id = UserContext.Current.UserInfo.User_Id;
                        item.modify_name = UserContext.Current.UserInfo.UserName;
                        item.modify_date = DateTime.Now;
                        lstUpdateFiles.Add(item);
                        continue;
                    }

                    var boolIsChange = false;
                    if (item.upload_status == (int)UploadStatus.Upload)
                    {
                        // 这个需要移动文件
                        item.file_path = _fileRecordsService.MoveFile(fileFolder, item);
                        item.master_id = ctrData.id;
                        item.upload_status = (int)UploadStatus.Finish;
                        boolIsChange = true;
                    }
                    if (item.remark != dtoFile.file_remark)
                    {
                        item.remark = dtoFile.file_remark;
                        boolIsChange = true;
                    }
                    if (boolIsChange)
                    {
                        item.modify_id = UserContext.Current.UserInfo.User_Id;
                        item.modify_name = UserContext.Current.UserInfo.UserName;
                        item.modify_date = DateTime.Now;
                        lstUpdateFiles.Add(item);
                    }
                }

                var lstBeforeRecords = await _fileRecordsRepository.FindAsync(p => p.delete_status == (int)SystemDataStatus.Valid &&
                                                                                 p.file_code == UploadFileCode.CTR &&
                                                                                 p.master_id == ctrData.id &&
                                                                                 !dicFiles.Keys.Contains(p.id));
                foreach (var item in lstBeforeRecords)
                {
                    // 根据文件记录id没获取到信息，则是删除的
                    item.delete_status = (int)SystemDataStatus.Invalid;

                    item.modify_id = UserContext.Current.UserInfo.User_Id;
                    item.modify_name = UserContext.Current.UserInfo.UserName;
                    item.modify_date = DateTime.Now;
                    lstUpdateFiles.Add(item);
                }


                if (lstUpdateFiles.Count > 0)
                {
                    _fileRecordsRepository.UpdateRange(lstUpdateFiles);
                }

                _quotationRepository.Update(qnData);
                _contractRepository.Update(ctrData);
                _repository.Update(ctrDetail);

                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 标书资料

        /// <summary>
        /// 投标补充-附件文档上传
        /// </summary>
        /// <param name="lstFiles">文件信息</param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadAddendumFiles(List<IFormFile> lstFiles)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                //return await _fileRecordsService.CommonUploadAsync(lstFiles, UploadFileCode.Tender_Add_Addendum);
                return await _fileRecordsService.CommonUploadToTempAsync(lstFiles, UploadFileCode.Tender_Add_Addendum);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取标书资料
        /// </summary>
        /// <param name="guidQnId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetTenderData(Guid guidQnId)
        {
            try
            {
                // 获取报价
                var qnData = await _quotationRepository.FindAsyncFirst(p => p.id == guidQnId);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                // 获取合约
                var ctrData = await _contractRepository.FindAsyncFirst(p => p.id == qnData.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }
                // 获取合同详情
                var ctrDetail = await _repository.FindFirstAsync(p => p.contract_id == ctrData.id && p.delete_status == (int)SystemDataStatus.Valid);
                if (ctrDetail == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }
                // 获取合同补充
                var lstAddendumData = await _tender_addendumRepository.FindAsync(p => p.contract_id == ctrData.id && p.delete_status == (int)SystemDataStatus.Valid);
                var dicAddendumData = lstAddendumData.ToDictionary(p => p.id, p => p);
                var dicFiles = new Dictionary<Guid, List<Sys_File_Records>>();
                if (lstAddendumData.Count > 0)
                {
                    // 获取上传相关的文件
                    var lstFiles = await _fileRecordsRepository
                        .FindAsync(p =>
                                        p.file_code == UploadFileCode.Tender_Add_Addendum &&
                                        p.upload_status == (int)UploadStatus.Finish &&
                                        p.delete_status == (int)SystemDataStatus.Valid &&
                                        p.master_id.HasValue &&
                                        dicAddendumData.Keys.Contains(p.master_id.Value)
                                       );
                    foreach (var item in lstFiles)
                    {
                        if (!item.master_id.HasValue)
                        {
                            continue;
                        }
                        if (dicFiles.Keys.Contains(item.master_id.Value))
                        {
                            dicFiles[item.master_id.Value].Add(item);
                        }
                        else
                        {
                            dicFiles.Add(item.master_id.Value, new List<Sys_File_Records> { item });
                        }
                    }
                }

                var dtoTender = new TenderDataDto
                {
                    qn_id = qnData.id,
                    contract_id = ctrData.id,
                    project_id = ctrData.project_id,
                    contract_details_id = ctrDetail.id,
                    contract_ref_no = ctrData.ref_no,
                    contract_title = ctrData.title,
                    contract_category = ctrData.category,
                    tender_start_date = ctrDetail.tender_start_date,
                    tender_end_date = ctrDetail.tender_end_date,
                    contract_trade = ctrDetail.trade,
                    tender_title = ctrDetail.title,
                    remark = ctrDetail.tender_remark
                };

                foreach (var item in dicAddendumData)
                {
                    var dtoAddendum = new TenderAddendumDto
                    {
                        id = item.Value.id,
                        issue_date = item.Value.issue_date,
                        remark = item.Value.remark
                    };
                    if (dicFiles.ContainsKey(item.Value.id))
                    {
                        var lstFile = dicFiles[item.Value.id];
                        foreach (var fileItem in lstFile)
                        {
                            dtoAddendum.upload_file_info.Add(new ContractQnFileDto
                            {
                                file_id = fileItem.id,
                                file_name = fileItem.file_name,
                                file_size = fileItem.file_size.HasValue ? fileItem.file_size.Value : 0,
                                file_remark = fileItem.remark
                            });
                        }
                    }

                    dtoTender.tender_addendums.Add(dtoAddendum);
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), dtoTender);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 保存标书资料
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveTenderData(TenderDataDto dtoInput)
        {
            try
            {
                // 获取报价
                var qnData = await _quotationRepository.FindAsyncFirst(p => p.id == dtoInput.qn_id);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }

                // 获取合约
                var ctrData = await _contractRepository.FindAsyncFirst(p => p.id == dtoInput.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }
                // 获取合同详情
                var ctrDetail = await _repository.FindFirstAsync(p => p.contract_id == ctrData.id && p.delete_status == (int)SystemDataStatus.Valid);
                if (ctrDetail == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }
                // 获取投标补充
                var lstAddendumData = await _tender_addendumRepository.FindAsync(p => p.contract_id == ctrData.id && p.delete_status == (int)SystemDataStatus.Valid);
                var dicAddendumData = lstAddendumData.ToDictionary(p => p.id, p => p);

                ctrData.project_id = dtoInput.project_id;
                ctrData.ref_no = dtoInput.contract_ref_no;
                ctrData.title = dtoInput.contract_title;
                ctrData.category = dtoInput.contract_category;

                ctrDetail.tender_start_date = dtoInput.tender_start_date;
                ctrDetail.tender_end_date = dtoInput.tender_end_date;
                ctrDetail.trade = dtoInput.contract_trade;
                ctrDetail.title = dtoInput.tender_title;
                ctrDetail.tender_remark = dtoInput.remark;

                //if (ctrData.tender_type == "Preliminary Enquiry Invitation (PEI)")
                //{
                //    ctrDetail.issue_date = ctrDetail.tender_start_date;
                //}

                _contractRepository.Update(ctrData);
                _repository.Update(ctrDetail);

                // 更改期限内容
                //await _quotationDeadlineService.EidtByContractDetailsChangeAsync(qnData.id, UpcomingEventsEnum.QnTender, ctrDetail.id, ctrDetail.tender_end_date);

                #region 正式文件夹的存放路径

                var getFileFolderResult = _quotationService.GetFormalFolder(qnData, UploadFileCode.Tender_Add_Addendum);
                if (!getFileFolderResult.status)
                {
                    return getFileFolderResult;
                }
                var fileFolder = getFileFolderResult.data as string;

                #endregion

                // 获取上传的文件记录
                var lstIds = new List<Guid>();
                foreach (var item in dtoInput.tender_addendums)
                {
                    var uploadIds = item.upload_file_info.Where(p => p.file_id.HasValue).Select(p => p.file_id.Value).ToList();
                    lstIds.AddRange(uploadIds);
                }
                var lstNowRecords = await _fileRecordsRepository.FindAsync(p => lstIds.Contains(p.id));
                var dicNowRecords = lstNowRecords.ToDictionary(p => p.id);
                // 获取之前存储的文件记录(用来删除的)
                var lstAddendumIds = dtoInput.tender_addendums.Where(p => p.id.HasValue).Select(p => p.id).ToList();
                var lstBeforeRecords = await _fileRecordsRepository.FindAsync(p => p.delete_status == (int)SystemDataStatus.Valid &&
                                                                                 p.file_code == UploadFileCode.Tender_Add_Addendum &&
                                                                                 lstAddendumIds.Contains(p.master_id) &&
                                                                                 !lstIds.Contains(p.id));

                // 处理信息
                var lstAddendumUpdate = new List<Biz_Contract_Tender_Addendum>();
                var lstAddendumAdd = new List<Biz_Contract_Tender_Addendum>();
                var lstEditRecords = new List<Sys_File_Records>();
                foreach (var dtoAddendumItem in dtoInput.tender_addendums)
                {
                    var guidId = dtoAddendumItem.id;

                    // 没id的是新增的
                    if (!dtoAddendumItem.id.HasValue)
                    {
                        var addData = new Biz_Contract_Tender_Addendum
                        {
                            id = Guid.NewGuid(),
                            contract_id = ctrData.id,
                            issue_date = dtoAddendumItem.issue_date,
                            remark = dtoAddendumItem.remark,

                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserInfo.User_Id,
                            create_name = UserContext.Current.UserInfo.UserName,
                            create_date = DateTime.Now,
                        };
                        guidId = addData.id;
                        lstAddendumAdd.Add(addData);
                    }
                    // 有id则是编辑
                    else
                    {
                        var isDataOk = dicAddendumData.TryGetValue(guidId.Value, out var addendumData);
                        if (!isDataOk) continue;

                        addendumData.contract_id = ctrData.id;
                        addendumData.issue_date = dtoAddendumItem.issue_date;
                        addendumData.remark = dtoAddendumItem.remark;
                        addendumData.modify_id = UserContext.Current.UserInfo.User_Id;
                        addendumData.modify_name = UserContext.Current.UserInfo.UserName;
                        addendumData.modify_date = DateTime.Now;

                        lstAddendumUpdate.Add(addendumData);
                    }

                    // 处理文件记录
                    foreach (var fileItem in dtoAddendumItem.upload_file_info)
                    {
                        if (!fileItem.file_id.HasValue)
                        {
                            continue;
                        }

                        // 获取现在有的数据
                        var boolIsNowOk = dicNowRecords.TryGetValue(fileItem.file_id.Value, out var fileRecordData);
                        if (boolIsNowOk)
                        {
                            if (fileRecordData.upload_status == (int)UploadStatus.Upload)
                            {
                                fileRecordData.upload_status = (int)UploadStatus.Finish;
                                // 这个需要移动文件
                                fileRecordData.file_path = _fileRecordsService.MoveFile(fileFolder, fileRecordData);
                            }
                            
                            fileRecordData.master_id = guidId;
                            fileRecordData.remark = fileItem.file_remark;

                            fileRecordData.modify_id = UserContext.Current.UserInfo.User_Id;
                            fileRecordData.modify_name = UserContext.Current.UserInfo.UserName;
                            fileRecordData.modify_date = DateTime.Now;
                            lstEditRecords.Add(fileRecordData);
                        }
                    }
                }

                foreach (var brforeItem in lstBeforeRecords)
                {
                    // 根据文件记录id没获取到信息，则是删除的
                    brforeItem.delete_status = (int)SystemDataStatus.Invalid;

                    brforeItem.modify_id = UserContext.Current.UserInfo.User_Id;
                    brforeItem.modify_name = UserContext.Current.UserInfo.UserName;
                    brforeItem.modify_date = DateTime.Now;
                    lstEditRecords.Add(brforeItem);
                }

                _tender_addendumRepository.AddRange(lstAddendumAdd);
                _tender_addendumRepository.UpdateRange(lstAddendumUpdate);
                _fileRecordsRepository.UpdateRange(lstEditRecords);

                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
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
