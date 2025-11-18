
using AutoMapper;
using Castle.Core.Resource;
using Dm;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.Common;
using Vodace.Core.Configuration;
using Vodace.Core.DBManager;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
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
    public partial class Biz_QuotationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_QuotationRepository _repository;                          //访问数据库
        private readonly IBiz_SiteRepository _siteRepository;                           //访问现场位置数据库
        private readonly IBiz_Site_RelationshipRepository _Site_RelationshipRepository; //访问现场位置关系数据库
        private readonly ISys_DictionaryRepository _sysDictionaryRepository;            //访问数据字典数据库
        private readonly ISys_DictionaryListRepository _sysDicListRepository;           //访问数据字典子表
        private readonly IBiz_ContractRepository _contractRepository;                   //访问合约仓储
        private readonly IBiz_ProjectRepository _projectRepository;                     //访问项目仓储
        private readonly ISys_File_RecordsRepository _FileRecordsRepository;            //访问文件记录仓储
        private readonly IBiz_Contract_DetailsRepository _contractDetailsRepository;    //访问文件详情仓储
        private readonly IBiz_Confirmed_OrderRepository _confirmedOrderRepository;      //访问确认报价仓储
        private readonly ISys_ConfigRepository _configRepository;                       //访问系统配置仓储
        private readonly ISys_CompanyRepository _companyRepository;                     //访问公司仓储
        private readonly IBiz_Contract_ContactRepository _contractContactRepository;    //访问合同联系人仓储

        private readonly ISys_DictionaryListService _sysDicListService;                 //数字字典详情服务
        private readonly Biz_Upcoming_EventsService _EventsService;                     //事件服务
        private readonly ISys_ConfigService _configService;                             //配置服务
        private readonly IBiz_Quotation_DeadlineService _quotationDeadlineService;      //期限管理服务
        private readonly IBiz_Quotation_HeadService _quotationHeadService;              //负责人服务
        private readonly ISys_File_ConfigService _fileConfigService;                    //文件配置服务
        private readonly IBiz_ContractOrgRepository _contractOrgRepository;

        private readonly IMapper _mapper;

        private readonly ISys_File_RecordsService _fileRecordsService;  //文件上传记录服务
        private readonly ILocalizationService _localizationService;     //国际化


        [ActivatorUtilitiesConstructor]
        public Biz_QuotationService(
            IBiz_QuotationRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            ISys_File_RecordsService fileRecordsService,
            IBiz_SiteRepository siteRepository,
            IBiz_Site_RelationshipRepository site_RelationshipRepository,
            ISys_DictionaryRepository sysDictionaryRepository,
            ISys_DictionaryListRepository sysDicListRepository,
            IBiz_ContractRepository contractRepository,
            ISys_DictionaryListService sysDicListService,
            IMapper mapper,
            Biz_Upcoming_EventsService eventsService,
            IBiz_ProjectRepository projectRepository,
            ISys_File_RecordsRepository fileRecordsRepository,
            IBiz_Contract_DetailsRepository contractDetailsRepository,
            IBiz_Confirmed_OrderRepository confirmedOrderRepository,
            ISys_ConfigRepository configRepository,
            ISys_CompanyRepository companyRepository,
            ISys_ConfigService configService,
            IBiz_Quotation_DeadlineService quotationDeadlineService,
            IBiz_Quotation_HeadService quotationHeadService,
            IBiz_Contract_ContactRepository contractContactRepository,
            ISys_File_ConfigService fileConfigService,
            IBiz_ContractOrgRepository contractOrgRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _fileRecordsService = fileRecordsService;
            _siteRepository = siteRepository;
            _Site_RelationshipRepository = site_RelationshipRepository;
            _sysDictionaryRepository = sysDictionaryRepository;
            _sysDicListRepository = sysDicListRepository;
            _contractRepository = contractRepository;
            _sysDicListService = sysDicListService;
            _mapper = mapper;
            _EventsService = eventsService;
            _projectRepository = projectRepository;
            _FileRecordsRepository = fileRecordsRepository;
            _contractDetailsRepository = contractDetailsRepository;
            _confirmedOrderRepository = confirmedOrderRepository;
            _configRepository = configRepository;
            _companyRepository = companyRepository;
            _configService = configService;
            _quotationDeadlineService = quotationDeadlineService;
            _quotationHeadService = quotationHeadService;
            _contractContactRepository = contractContactRepository;
            _fileConfigService = fileConfigService;
            _contractOrgRepository = contractOrgRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        #region 新增

        /// <summary>
        /// 合同1信息文件上传
        /// </summary>
        /// <param name="lstFiles">文件信息</param>
        /// <param name="intStatus">上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）</param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadContractFiles(
            List<IFormFile> lstFiles,
            int intStatus = 0)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                //return await _fileRecordsService.UploadFilesAsync(lstFiles, "ctr_file", intStatus);
                //return await _fileRecordsService.CommonUploadAsync(lstFiles, UploadFileCode.CTR);
                //return await _fileRecordsService.CommonUploadToTempAsync(lstFiles, UploadFileCode.CTR);
                return await _fileRecordsService.CommonUploadToTempAsync(lstFiles, UploadFileCode.CTR);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 新增报价
        /// </summary>
        /// <param name="dtoAddQnRequest"></param>
        /// <returns></returns>
        public WebResponseContent AddQn(AddQnRequestDto dtoAddQnRequest)
        {
            try
            {
                if (dtoAddQnRequest == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                }
                #region 项目

                // 获取项目id
                var dtoProQn = dtoAddQnRequest.dto_qn_tender;
                var resultProData = GetProject(dtoProQn, dtoAddQnRequest.project_source);
                if (!resultProData.status)
                {
                    return resultProData;
                }
                var proData = resultProData.data as Biz_Project;

                #endregion

                #region 报价、合约
                var dtoCtrQn = dtoAddQnRequest.dto_qn_contract; // 合约
                var dtoCtaQn = dtoAddQnRequest.dto_qn_contacts;  // 联系人

                var qnId = Guid.NewGuid();

                var resultCtr = AddContract(dtoCtrQn, dtoCtaQn, dtoProQn, proData, dtoAddQnRequest.customer_type, dtoAddQnRequest.project_source, qnId);
                if (!resultCtr.status)
                {
                    return resultCtr;
                }
                var ctrData = resultCtr.data as Biz_Contract;

                var qnData = new Biz_Quotation
                {
                    id = qnId,
                    status_code = string.IsNullOrEmpty(dtoCtrQn.tender_type) ? dtoAddQnRequest.project_source: dtoCtrQn.tender_type,
                    qn_no = DataSequence.GetSequence(VodacsCommonParams.QN),
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,
                    delete_status = (int)SystemDataStatus.Valid,
                    contract_id = ctrData.id,
                    is_interest = 1,
                };
                _repository.Add(qnData);

                #region 创建报价合同文件目录，转移临时文件

                var moveResult = FileMoveToFormal(qnData, ctrData, dtoCtrQn.upload_file_info);
                if (!moveResult.status)
                {
                    return moveResult;
                }

                #endregion

                //_fileRecordsService.FinishFilesUploadNoSaveChange(dtoCtrQn.upload_file_info, ctrData.id);

                #region 创建相关负责人记录

                _quotationHeadService.AddHeadersByAddQn(qnData.id);

                #endregion

                _repository.SaveChanges();

                #endregion

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取项目
        /// </summary>
        /// <param name="projectQnDto">项目信息</param>
        /// <param name="contractType"></param>
        /// <returns></returns>
        private WebResponseContent GetProject(TenderQnDto projectQnDto, string contractType)
        {
            try
            {
                // 如果存在项目的话，就获取项目
                if (contractType == "Advertisement" || contractType == "Preliminary Enquiry Invitation (PEI)")
                {
                    var addProResult = AddProject(projectQnDto.pro_name);
                    if (!addProResult.status)
                    {
                        return addProResult;
                    }
                    return WebResponseContent.Instance.OK("ok", addProResult.data);
                }
                else 
                {
                    if (projectQnDto.pro_id.HasValue)
                    {
                        var proObj = _projectRepository.FindFirst(p => p.id == projectQnDto.pro_id);
                        if (proObj == null)
                        {
                            return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_project_null"));
                        }
                        return WebResponseContent.Instance.OK("ok", proObj);
                    }
                }


                return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_project_null"));
                //return WebResponseContent.Instance.Error("ok", null);

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(ErrorInfoHelper.GetErrorInfo(this, ex.Message), ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 创建合约（没有savechange）
        /// </summary>
        /// <param name="contractQnDto">合约信息</param>
        /// <param name="contactQnDtos">联系人信息</param>
        /// <param name="proQnDto">项目信息</param>
        /// <param name="proData">项目信息</param>
        /// <param name="customerType">客户类型</param>
        /// <param name="contractType">合同类型（项目来源）</param>
        /// <param name="qnId">报价id</param>
        /// <returns></returns>
        private WebResponseContent AddContract(
            ContractQnDto contractQnDto,
            List<ContactQnDto> contactQnDtos,
            TenderQnDto proQnDto,
            Biz_Project proData,
            string customerType,
            string contractType,
            Guid qnId)
        {
            try
            {

                // 判断是否有重复合同信息（根据合同编号）
                if (!string.IsNullOrEmpty(contractQnDto.contract_no)) 
                {
                    var isRepeat = CheckRepeatContract(contractQnDto.contract_no);
                    if (isRepeat)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("contract_no_repeat"));
                    }
                }
                
                // 创建合同
                var ctrData = new Biz_Contract
                {
                    id = Guid.NewGuid(),
                    project_id = proData != null ? proData.id : null,

                    contract_no = contractQnDto.contract_no,
                    name_cht = contractQnDto.contract_name,
                    name_eng = contractQnDto.contract_name,
                    ref_no = contractQnDto.contract_ref_no,
                    title = contractQnDto.contract_title,
                    category = contractQnDto.contract_category,
                    tender_type = string.IsNullOrEmpty(contractQnDto.tender_type) ? contractType :  contractQnDto.tender_type,

                    company_id = UserContext.Current.UserInfo.Company_Id,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,
                };

                if (contractQnDto.master_id.HasValue)
                {
                    if (ctrData.id == contractQnDto.master_id)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("qn_master_id_same"));
                    }

                    ctrData.master_id = contractQnDto.master_id;
                    ctrData.vo_wo_type = contractQnDto.vo_wo_type;
                }
                else
                {
                    ctrData.master_id = null;
                    ctrData.vo_wo_type = "";
                }

                // 创建合同细项
                var ctrDetail = new Biz_Contract_Details
                {
                    id = Guid.NewGuid(),
                    contract_id = ctrData.id,

                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,

                    issue_date = proQnDto.tender_issue_date,
                    end_date = proQnDto.tender_end_date,
                    title = proQnDto.tender_title,
                    pei_subject = proQnDto.tender_subject,
                    pei_tender_ref = proQnDto.tender_ref,
                    pei_info = proQnDto.tender_info,

                    remark = contractQnDto.contract_description,
                    trade = contractQnDto.contract_trade,
                    antic_pql_sub_close_date = contractQnDto.closing_date,
                    antic_pql_sub_date = contractQnDto.anticipated_date,
                    antic_inv_tndr_date = contractQnDto.ant_send_inv_tender_date,
                    antic_cntr_awd_date = contractQnDto.ant_contract_award_date,
                    range_cost = contractQnDto.cost_range,

                    //contact_name = contactQnDto.contact_name,
                    //contact_title = contactQnDto.contact_tilte,
                    //contact_email = contactQnDto.contact_mail,
                    //contact_fax = contactQnDto.contact_faxno,
                    //contact_tel = contactQnDto.contact_phone,

                    customer_type = customerType,

                    //tender_end_date = proQnDto.tender_end_date
                };


                #region 保存合同联系人

                var lstAddContact = new List<Biz_Contract_Contact>();
                foreach (var contactQnDto in contactQnDtos)
                {
                    lstAddContact.Add(new Biz_Contract_Contact
                    {
                        id = Guid.NewGuid(),
                        contract_id = ctrData.id,
                        contact_name = contactQnDto.contact_name,
                        contact_tel = contactQnDto.contact_phone,
                        contact_fax = contactQnDto.contact_faxno,
                        contact_email = contactQnDto.contact_mail,
                        mail_to = contactQnDto.contact_mail_to,
                        contact_title = contactQnDto.contact_tilte,
                        remark = "",

                        delete_status = (int)SystemDataStatus.Valid,
                        create_id = UserContext.Current.UserInfo.User_Id,
                        create_name = UserContext.Current.UserInfo.UserName,
                        create_date = DateTime.Now,
                    });
                }

                _contractContactRepository.AddRange(lstAddContact);
               

                #endregion

                #region 创建相关的期限记录

                //_quotationDeadlineService.AddDeadlineByQnAdd(qnId, ctrData.tender_type, ctrDetail.end_date, ctrDetail.id);

                #endregion

                _contractRepository.Add(ctrData);
                _contractDetailsRepository.Add(ctrDetail);

                return WebResponseContent.Instance.OK("ok", ctrData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }


      

        #endregion

        #region 修改

        /// <summary>
        /// 报价修改
        /// </summary>
        /// <param name="editQnRequestDto"></param>
        /// <returns></returns>
        public WebResponseContent EditQn(EditQnRequestDto editQnRequestDto)
        {
            try
            {
                if (editQnRequestDto == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                }

                var qnData = _repository.FindFirst(p => p.id == editQnRequestDto.qn_id);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }

                var ctrData = _contractRepository.FindFirst(p => p.id == qnData.contract_id); // 合约信息
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 修改合约信息
                if (!string.IsNullOrEmpty(editQnRequestDto.contract_no))
                {
                    var isRepeat = CheckRepeatContract(editQnRequestDto.contract_no, false, ctrData.id); // 判断是否有重复合同信息（根据合同编号）
                    if (isRepeat)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("contract_no_repeat"));
                    }
                }
             
                ctrData.name_eng = editQnRequestDto.contract_name;
                ctrData.name_cht = editQnRequestDto.contract_name;
                if (editQnRequestDto.master_id.HasValue)
                {
                    if (ctrData.id == editQnRequestDto.master_id)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("qn_master_id_same"));
                    }

                    ctrData.master_id = editQnRequestDto.master_id;
                    ctrData.vo_wo_type = editQnRequestDto.vo_wo_type;
                }
                else
                {
                    ctrData.master_id = null;
                    ctrData.vo_wo_type = "";
                }
                ctrData.contract_no = editQnRequestDto.contract_no;

                // 获取配置项
                var configData = _configRepository.FindFirst(p => p.config_type == 3 && p.config_key == "currency");
                if (configData == null) 
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("config_null"));
                }

                // 修改报价
                qnData.status_code = editQnRequestDto.qn_stuts_code; // 合约状态
                qnData.customer_id = editQnRequestDto.customer_id;   // 合约客户
                qnData.qn_year = editQnRequestDto.year;              // 合约年份
                qnData.qn_amt = editQnRequestDto.qn_amt;             // 报价金额
                qnData.confirm_amt = editQnRequestDto.confirm_amt;   // 确认金额
                qnData.confirm_currency = configData.config_value;   // 货币单位
                qnData.qn_currency = configData.config_value;        // 货币单位

                #region 旧的代码（负责人和截止时间）

                //// 招标前 发送事件
                //if (editQnRequestDto.pq_handler.contact_id.HasValue && editQnRequestDto.pq_handler.end_date.HasValue)
                //{
                //    if (qnData.pq_handler_id != editQnRequestDto.pq_handler.contact_id ||
                //        qnData.pq_handler_closing_date != editQnRequestDto.pq_handler.end_date)
                //    {
                //        AddEvent(UpcomingEventsEnum.QnPQ,
                //            qnData.id,
                //            qnData.qn_no,
                //            editQnRequestDto.pq_handler.end_date,
                //            editQnRequestDto.contract_name,
                //            "招标前",
                //            editQnRequestDto.pq_handler.contact_id,
                //            ctrData.contract_no,
                //            "Pre-Tender"
                //            );
                //    }
                //}
                //qnData.pq_handler_id = editQnRequestDto.pq_handler.contact_id;
                //qnData.pq_handler_closing_date = editQnRequestDto.pq_handler.end_date;

                //// 现场考察 发送事件
                //if (editQnRequestDto.pe_handler.contact_id.HasValue && editQnRequestDto.pe_handler.end_date.HasValue)
                //{
                //    if (qnData.pe_handler_id != editQnRequestDto.pe_handler.contact_id || qnData.pe_handler_closing_date != editQnRequestDto.pe_handler.end_date)
                //    {
                //        string strTitle = $"（现场考察）{editQnRequestDto.contract_name}";
                //        AddEvent(UpcomingEventsEnum.QnPE,
                //            qnData.id,
                //            qnData.qn_no,
                //            editQnRequestDto.pe_handler.end_date,
                //            editQnRequestDto.contract_name,
                //            "现场考察",
                //            editQnRequestDto.pe_handler.contact_id,
                //            ctrData.contract_no,
                //            "On-Site Inspection");
                //    }
                //}
                //qnData.pe_handler_id = editQnRequestDto.pe_handler.contact_id;
                //qnData.pe_handler_closing_date = editQnRequestDto.pe_handler.end_date;

                //// 招标 发送事件
                //if (editQnRequestDto.tender_handler.contact_id.HasValue && editQnRequestDto.tender_handler.end_date.HasValue)
                //{
                //    if (qnData.tender_handler_id != editQnRequestDto.tender_handler.contact_id || qnData.tender_handler_closing_date != editQnRequestDto.tender_handler.end_date)
                //    {
                //        AddEvent(UpcomingEventsEnum.QnTender,
                //            qnData.id,
                //            qnData.qn_no,
                //            editQnRequestDto.tender_handler.end_date,
                //            editQnRequestDto.contract_name,
                //            "招标",
                //            editQnRequestDto.tender_handler.contact_id,
                //            ctrData.contract_no,
                //            "Tender");
                //    }
                //}
                //qnData.tender_handler_id = editQnRequestDto.tender_handler.contact_id;
                //qnData.tender_handler_closing_date = editQnRequestDto.tender_handler.end_date;

                #endregion


                qnData.modify_id = UserContext.Current.UserInfo.User_Id;
                qnData.modify_name = UserContext.Current.UserInfo.UserName;
                qnData.modify_date = DateTime.Now;

                // 招标截止时间与标书资料的截止时间是一样的，所以这里变那边也需要跟着变
                //var ctrDetails = _contractDetailsRepository.FindFirst(p => p.contract_id == ctrData.id && p.delete_status == (int)SystemDataStatus.Valid);
                //if (ctrDetails != null)
                //{
                //    ctrDetails.tender_end_date = editQnRequestDto.tender_handler.end_date;
                //    _contractDetailsRepository.Update(ctrDetails);
                //}

                // 修改现场位置id
                SaveSitesNoSaveChange(ctrData.id, editQnRequestDto.site_ids);

                //_contractDetailsRepository.Update(ctrDetailData);
                _contractRepository.Update(ctrData);
                _repository.Update(qnData);
                _repository.SaveChanges();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 删除

        /// <summary>
        /// 报价删除
        /// </summary>
        /// <param name="qnId">报价id</param>
        /// <returns></returns>
        public WebResponseContent DelQn(Guid qnId)
        {
            try
            {
                var delData = _repository.FindFirst(p => p.id == qnId);
                if (delData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                delData.delete_status = (int)SystemDataStatus.Invalid;
                _repository.Update(delData);

                //DelEvent(qnId, UpcomingEventsEnum.QnPQ);
                //DelEvent(qnId, UpcomingEventsEnum.QnPE);
                //DelEvent(qnId, UpcomingEventsEnum.QnTender);

                _repository.SaveChanges();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 获取报价分页集合
        /// </summary>
        /// <param name="dtoQnSearchInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetQnPageListAsync(
            PageInput<QnSearchDto> dtoQnSearchInput)
        {
            try
            {
                var dtoQnSearch = dtoQnSearchInput.search;

                Expression<Func<Biz_Quotation, Biz_Contract, IEnumerable<Biz_Site>, Biz_Sub_Contractors, QnDataDto>> select =
                    (qn, contract, sites, cus) => new QnDataDto
                    {
                        qn_id = qn.id,
                        qn_no = qn.qn_no,
                        contract_id = contract.id,
                        qn_status_code = qn.status_code,
                        contract_no = contract.contract_no,
                        contract_name = contract.name_eng,
                        confirm_amt = qn.confirm_amt,
                        qn_amt = qn.qn_amt,
                        pq_closing_date = qn.pq_handler_closing_date,
                        tender_closing_date = qn.tender_handler_closing_date,
                        pe_closing_date = qn.pe_handler_closing_date,
                        create_date = qn.create_date,
                        create_id = qn.create_id,
                        create_name = qn.create_name,
                        lst_dto_sites = sites.Select(s => new SiteDataDto
                        {
                            id = s.id,
                            name_chs = s.name_chs,
                            name_cht = s.name_cht,
                            name_eng = s.name_eng,
                            name_sho = s.name_sho,
                            create_date = s.create_date,
                            create_id = s.create_id,
                            create_name = s.create_name
                        }).ToList(),
                        customer_id = cus.id,
                        customer_name = cus.name_sho,
                        contract_type = contract.tender_type,
                        qn_year = qn.qn_year,
                        qn_currency = qn.qn_currency,
                        confirm_currency = qn.confirm_currency
                    };
                select = select.BuildExtendSelectExpre();

                // 获取当前用户公司ID
                var currentCompanyId = UserContext.Current.UserInfo.Company_Id;

                // 获取报价、合同、客户、项目
                var quotations = DBServerProvider.DbContext.Set<Biz_Quotation>()
                    .AsExpandable().AsNoTracking()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid);

                var contracts = DBServerProvider.DbContext.Set<Biz_Contract>()
                    .AsNoTracking()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid);

                if (!UserContext.Current.IsSuperAdmin)
                {
                    if (currentCompanyId.HasValue)
                    {
                        contracts = contracts.Where(p => p.company_id == currentCompanyId);
                    }
                    else
                    {
                        contracts = contracts.Where(p => p.company_id == Guid.Empty);
                    }
                }
               

               

                var customers = DBServerProvider.DbContext.Set<Biz_Sub_Contractors>()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid)
                    .AsNoTracking();

                // 获取站点关系和站点
                var siteRelationships = DBServerProvider.DbContext.Set<Biz_Site_Relationship>()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid && p.relation_type == 0) // relation_type = 0 表示报价和站点的关系
                    .AsNoTracking();

                var sites = DBServerProvider.DbContext.Set<Biz_Site>()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid)
                    .AsNoTracking();

                // 构建主查询：关联报价、合约和客户，合约表已经应用了公司过滤
                var query = from q in quotations
                            join c in contracts on q.contract_id equals c.id into qc
                            from contract in qc.DefaultIfEmpty()
                            // 只保留存在的合约记录（确保公司过滤生效）
                            where contract != null
                            join cs in customers on q.customer_id equals cs.id into qcs
                            from customer in qcs.DefaultIfEmpty()
                            select new { q, contract, customer };

                // 如果有站点ID集合筛选条件
                if (dtoQnSearch.lst_site_ids != null && dtoQnSearch.lst_site_ids.Count > 0)
                {
                    // 使用方法语法进行过滤，避免查询表达式语法错误
                    query = query.Where(qc => siteRelationships
                        .Any(sr => sr.relation_id == qc.contract.id && dtoQnSearch.lst_site_ids.Contains(sr.site_id.Value)));
                }

                // 继续构建查询，为每个报价添加关联的所有站点
                var finalQuery = from qc in query
                                     // 子查询获取当前报价关联的所有站点
                                 let qnSites = (from sr in siteRelationships
                                                join s in sites on sr.site_id equals s.id
                                                where sr.relation_id == qc.contract.id
                                                select s).AsEnumerable()
                                 // 应用选择表达式
                                 select @select.Invoke(qc.q, qc.contract, qnSites, qc.customer);

                // 应用其他可能的查询条件
                if (!string.IsNullOrEmpty(dtoQnSearch.number))
                {
                    finalQuery = finalQuery.Where(dto => dto.qn_no.Contains(dtoQnSearch.number));
                }

                if (!string.IsNullOrEmpty(dtoQnSearch.contract_name))
                {
                    finalQuery = finalQuery.Where(dto => dto.contract_name.Contains(dtoQnSearch.contract_name));
                }

                if (!string.IsNullOrEmpty(dtoQnSearch.contract_no))
                {
                    finalQuery = finalQuery.Where(dto => dto.contract_no.Contains(dtoQnSearch.contract_no));
                }

                if (!string.IsNullOrEmpty(dtoQnSearch.customer_name))
                {
                    finalQuery = finalQuery.Where(dto => dto.customer_name.Contains(dtoQnSearch.customer_name));
                }

                if (!string.IsNullOrEmpty(dtoQnSearch.project_form))
                {
                    finalQuery = finalQuery.Where(dto => dto.contract_type == dtoQnSearch.project_form);
                }

                if (!string.IsNullOrEmpty(dtoQnSearch.status_code))
                {
                    finalQuery = finalQuery.Where(dto => dto.qn_status_code == dtoQnSearch.status_code);
                }

                if (dtoQnSearch.year.HasValue)
                {
                    finalQuery = finalQuery.Where(dto => dto.qn_year == dtoQnSearch.year);
                }

                // 执行分页查询
                if (string.IsNullOrEmpty(dtoQnSearchInput.sort_field))
                {
                    dtoQnSearchInput.sort_field = "create_date";
                    dtoQnSearchInput.sort_type = "desc";
                }
                var result = await finalQuery.GetPageResultAsync(dtoQnSearchInput);
                result.data = await SetProperty(result.data);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);

                // 赋予其他信息
                async Task<List<QnDataDto>> SetProperty(List<QnDataDto> lstDtoQnData)
                {
                    // 赋予状态名称
                    var getDicResult = await _sysDicListService.GetDictionaryByCode("qn_process_status");
                    if (!getDicResult.status)
                    {
                        return lstDtoQnData;
                    }
                    var lstDic = getDicResult.data as List<ContractStatusDto>;
                    if (lstDic == null)
                    {
                        return lstDtoQnData;
                    }
                    var dicContractStatus = lstDic.ToDictionary(p => p.code, v=> v.value);
                    foreach (var item in lstDtoQnData)
                    {
                        if (string.IsNullOrEmpty(item.qn_status_code) || !dicContractStatus.ContainsKey(item.qn_status_code))
                        {
                            continue;
                        }

                        item.qn_status_value = dicContractStatus[item.qn_status_code];
                    }

                    return lstDtoQnData;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取报价详情
        /// </summary>
        /// <param name="queryQn"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetQnByIdAsync(Guid guidQnId)
        {
            try
            {
                // 获取报价
                var qnData = await _repository.FindAsyncFirst(p => p.id == guidQnId);
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
                // 获取合约详细信息
                var ctrDetailData = await _contractDetailsRepository.FindAsyncFirst(p => p.contract_id == qnData.contract_id);
                if (ctrDetailData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 获取现场地点
                var lstSites = await _Site_RelationshipRepository
                        .FindAsync(p => p.relation_type == 0 && p.relation_id == ctrData.id && p.delete_status == (int)SystemDataStatus.Valid);
                var lstSiteIds = lstSites.Select(p => p.site_id).ToList();
                //var lstSiteDatas = await _siteRepository.FindAsync(p => lstSiteIds.Contains(p.id));

                // 获取已确认订单
                var coData = await _confirmedOrderRepository.FindAsyncFirst(p => p.qn_id == qnData.id && p.delete_status == (int)SystemDataStatus.Valid);

                var dtoQnDetail = new QnDetailDataDto
                {
                    qn_id = qnData.id,
                    pn_no = qnData.qn_no,
                    contract_id = ctrData.id,
                    co_no = coData != null ? coData.co_no : string.Empty,


                    contract_name = ctrData.name_eng,
                    contract_no = ctrData.contract_no,
                    customer_id = qnData.customer_id,
                    customer_type = ctrDetailData.customer_type,
                    qn_stuts_code = qnData.status_code,
                    site_ids = lstSiteIds,
                    year = qnData.qn_year,
                    master_id = ctrData.master_id,
                    vo_wo_type = ctrData.vo_wo_type,
                    qn_amt = qnData.qn_amt,
                    confirm_amt = qnData.confirm_amt,

                    pq_handler = new QnHeaderDto
                    {
                        contact_id = qnData.pq_handler_id,
                        end_date = qnData.pq_handler_closing_date
                    },
                    pe_handler = new QnHeaderDto
                    {
                        contact_id = qnData.pe_handler_id,
                        end_date = qnData.pe_handler_closing_date
                    },
                    tender_handler = new QnHeaderDto
                    {
                        contact_id = qnData.tender_handler_id,
                        end_date = qnData.tender_handler_closing_date
                    },

                    create_id = qnData.create_id,
                    create_date = qnData.create_date,
                    create_name = qnData.create_name,
                    modify_id = qnData.modify_id,
                    modify_date = qnData.modify_date,
                    modify_name = qnData.modify_name,

                    qn_currency = qnData.qn_currency,
                    confirm_currency = qnData.confirm_currency,
                    //contract_ref_no = ctrData.ref_no,
                    //contract_status_code = ctrData.tender_type
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), dtoQnDetail);

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 统计报价金额
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> CountQnAmtAsync()
        {
            try
            {
                Expression<Func<Biz_Contract, Biz_Quotation, QnSumAmtDto>> select = (contract, qn) => new QnSumAmtDto
                {
                    total_qn_amt = qn.qn_amt.HasValue? qn.qn_amt.Value : 0,
                    total_confirm_amt = qn.confirm_amt.HasValue? qn.confirm_amt.Value : 0
                };
                select = select.BuildExtendSelectExpre();

                // 获取当前用户公司ID
                var currentCompanyId = UserContext.Current.UserInfo.Company_Id;

                var contracts = DBServerProvider.DbContext.Set<Biz_Contract>()
                    .AsExpandable().AsNoTracking()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid && 
                                p.vo_wo_type != VodacsCommonParams.WO && 
                                p.company_id == UserContext.Current.UserInfo.Company_Id);

                if (currentCompanyId.HasValue)
                {
                    contracts = contracts.Where(p => p.company_id == currentCompanyId);
                }
                else
                {
                    if (!UserContext.Current.IsSuperAdmin)
                    {
                        contracts = contracts.Where(p => p.company_id == Guid.Empty);
                    }
                }

                var quotations = DBServerProvider.DbContext.Set<Biz_Quotation>()
                    .AsNoTracking()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid);

                var query = from c in contracts
                            join q in quotations on c.id equals q.contract_id into cq
                            from q in cq.DefaultIfEmpty()
                            // 应用选择表达式
                            select @select.Invoke(c, q);

                var queryResult = await query.ToListAsync();

                // 计算总和
                var statistics = new QnSumAmtDto
                {
                    total_qn_amt = queryResult.Sum(p => p.total_qn_amt),
                    total_confirm_amt = queryResult.Sum(p => p.total_confirm_amt)
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), statistics);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 是否有重复合同（根据合同编码）
        /// </summary>
        /// <param name="strContractNo">合同编码</param>
        /// <param name="isAdd">是否是新增</param>
        /// <param name="guidContractId">如果是修改，则要带上修改那个合同id</param>
        /// <returns></returns>
        public bool CheckRepeatContract(
            string strContractNo, 
            bool isAdd = true,
            Guid? guidContractId = null)
        {
            if (string.IsNullOrEmpty(strContractNo))
            {
                return false;
            }

            // 获取当前用户公司ID
            var currentCompanyId = UserContext.Current.UserInfo.Company_Id;

            var query = _contractRepository
                .WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid && p.company_id == currentCompanyId && p.contract_no == strContractNo);

            // 如果是新增模式，检查是否存在任何匹配记录
            if (isAdd)
            {
                return query.Any();
            }
            else
            {
                // 如果是编辑模式，排除当前正在编辑的合同ID
                if (guidContractId.HasValue)
                {
                    query = query.Where(p => p.id != guidContractId.Value);
                }

                // 检查是否存在除当前合同外的其他合同具有相同编码
                return query.Any();
            }
        }

        /// <summary>
        /// 获取报价下拉列表
        /// </summary>
        /// /// <param name="boolIsMaster">是否选则父项</param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetQnDropList(bool boolIsMaster = true, Guid? qnId = null)
        {
            try
            {
                // 获取当前用户公司ID
                var currentCompanyId = UserContext.Current.UserInfo.Company_Id;

                var qnData = await _repository.FindAsyncFirst(p => p.id == qnId);

                var lstContract = boolIsMaster ?
                    await _contractRepository
                    .WhereIF(!UserContext.Current.IsSuperAdmin, p => p.company_id == currentCompanyId)
                    .WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid)
                    .WhereIF(qnData != null, p => p.id != qnData.contract_id)
                    .Where(p => !p.master_id.HasValue) // 已经是vo/wo的就不能再选择了
                    .ToDictionaryAsync(p => p.id)
                    :
                    await _contractRepository
                    .WhereIF(!UserContext.Current.IsSuperAdmin, p => p.company_id == currentCompanyId)
                    .WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid)
                    .WhereIF(qnData != null, p => p.id != qnData.contract_id)
                    .Where(p => !p.master_id.HasValue) // 已经是vo/wo的就不能再选择了
                    .ToDictionaryAsync(p => p.id);

                var lstContractIds = lstContract.Keys.ToList();

                var lstQn = await _repository.FindAsync(p => p.contract_id.HasValue && lstContractIds.Contains(p.contract_id.Value) && p.delete_status == 0);

                var lstDropData = new List<ContractDropListDto>();
                foreach (var item in lstQn)
                {
                    if (!item.contract_id.HasValue)
                    {
                        continue;
                    }
                    bool isOk = lstContract.TryGetValue(item.contract_id.Value, out var contractData);
                    if (isOk)
                    {
                        lstDropData.Add(new ContractDropListDto
                        {
                            id = contractData.id,
                            qn_id = item.id,
                            qn_no = item.qn_no,
                            contract_no = contractData.contract_no,
                            contract_name = (string.IsNullOrEmpty(contractData.name_eng) ? contractData.name_cht : contractData.name_eng),
                            confirm_amt = item.confirm_amt,
                            qn_amt = item.qn_amt
                        });
                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstDropData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> GetQnDropList()
        {
            try
            {
                // 获取当前用户公司ID
                var currentCompanyId = UserContext.Current.UserInfo.Company_Id;
                var qnData = await _repository.FindAsyncFirst(p => p.delete_status == (int)SystemDataStatus.Valid);
                var lstContract = 
                    await _contractRepository
                    .WhereIF(!UserContext.Current.IsSuperAdmin, p => p.company_id == currentCompanyId)
                    .WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid)
                    .WhereIF(qnData != null, p => p.id != qnData.contract_id)
                    .ToDictionaryAsync(p => p.id);

                var lstContractIds = lstContract.Keys.ToList();

                var lstQn = await _repository.FindAsync(p => p.contract_id.HasValue && lstContractIds.Contains(p.contract_id.Value) && p.delete_status == 0);
                var lstConractOrg = _contractOrgRepository.Find(d=>d.delete_status ==(int)SystemDataStatus.Valid).OrderByDescending(d=>d.create_date).ToList();
                var lstDropData = new List<ContractDropListDto>();
                foreach (var item in lstQn)
                {
                    if (!item.contract_id.HasValue)
                    {
                        continue;
                    }
                    bool isOk = lstContract.TryGetValue(item.contract_id.Value, out var contractData);
                    if (isOk)
                    {
                        lstDropData.Add(new ContractDropListDto
                        {
                            id = contractData.id,
                            qn_id = item.id,
                            qn_no = item.qn_no,
                            contract_no = contractData.contract_no,
                            contract_name = (string.IsNullOrEmpty(contractData.name_eng) ? contractData.name_cht : contractData.name_eng),
                            confirm_amt = item.confirm_amt,
                            qn_amt = item.qn_amt,
                            submit_file_code = lstConractOrg.Where(d => d.contract_id == contractData.id).FirstOrDefault() ==null?"": lstConractOrg.Where(d=>d.contract_id == contractData.id).FirstOrDefault().submit_file_code
                        });
                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstDropData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取报价状态下拉列表
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetStatuCodeByQnId(Guid qnId)
        {
            try
            {
                // 获取报价
                var qnData = await _repository.FindAsyncFirst(p => p.id == qnId);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }

                var getDicResult = await _sysDicListService.GetDictionaryByCode("qn_process_status");
                if (!getDicResult.status)
                {
                    return getDicResult;
                }

                var lstDic = getDicResult.data as List<ContractStatusDto>;

               
                var qnStatus = lstDic.FirstOrDefault(p => p.code == qnData.status_code);
                if (qnStatus == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("dic_null"));
                }

                // 获取确认订单的位置
                var dtoCOStatuc = lstDic.FirstOrDefault(p => p.code == "Confirmed Order");
                if (dtoCOStatuc == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("dic_null"));
                }

                // 如果qn的状态不是确认订单后面的状态，则返回全部
                if (!dtoCOStatuc.index.HasValue || !qnStatus.index.HasValue)
                {
                    return getDicResult;
                }
                var lstData = lstDic.ToList();
                if (dtoCOStatuc.index > qnStatus.index)
                {
                    // 如果是确认订单后面的状态，则返回确认订单后面的状态
                    lstData = lstData.Where(p => p.index < dtoCOStatuc.index).ToList();
                    //return getDicResult;
                }
                else
                {
                    // 如果是确认订单后面的状态，则返回确认订单后面的状态
                    lstData = lstData.Where(p => p.index >= dtoCOStatuc.index).ToList();
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 提交文件

        /// <summary>
        /// 提交文件-上传文件（财务类文件上传、技术类文件上传）
        /// </summary>
        /// <param name="lstFiles">文件</param>
        /// <param name="intSubmitType">文件类型（0：财务类文件、1：技术类文件）</param>
        /// <param name="guidQnId">所属qn的id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadSubmitFile(List<IFormFile> lstFiles, int intSubmitType, Guid guidQnId)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                var strFileType = intSubmitType == 0 ? UploadFileCode.Financial_Documents : UploadFileCode.Technical_Documents;

                var getfolderResult = GetRootFolderName(guidQnId);
                if (!getfolderResult.status)
                {
                    return getfolderResult;
                }
                var strFolder = getfolderResult.data as string;
                return await _fileRecordsService.CommonUploadAsync(lstFiles, strFileType, strFolder, guidQnId);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }


        /// <summary>
        /// 下载提交的文件
        /// </summary>
        /// <param name="guidQnid"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DownSubmitFile(Guid guidQnid)
        {
            try
            {
                var lstFileRecords = await _FileRecordsRepository
                    .FindAsync(p => p.master_id == guidQnid &&
                                    p.delete_status == (int)SystemDataStatus.Valid &&
                                    (p.file_code == UploadFileCode.Financial_Documents || p.file_code == UploadFileCode.Technical_Documents));
                if (lstFileRecords.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_submit_no_found"));
                }
                var data = await _fileRecordsService.ZipFileAsync(lstFileRecords);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 提交文件（目前只改状态）
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SubmitQnFiles(Guid qnId)
        {
            try
            {
                // 获取报价
                var qnData = await _repository.FindFirstAsync(p => p.id == qnId);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                if (!IsCanSumitFileStatus(qnData.status_code))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_unconfirm_file"));
                }

                qnData.status_code = "Submission QN";
                qnData.modify_id = UserContext.Current.UserInfo.User_Id;
                qnData.modify_name = UserContext.Current.UserInfo.UserName;
                qnData.modify_date = DateTime.Now;
                _repository.Update(qnData);
                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取提交文件（财务类文件上传、技术类文件上传）列表
        /// </summary>
        /// <param name="guidQnId"></param>
        /// <param name="intSubmitType"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSumitFilesAync(Guid guidQnId, int intSubmitType)
        {
            try
            {
                var strFileType = intSubmitType == 0 ? UploadFileCode.Financial_Documents : UploadFileCode.Technical_Documents;

                var lstFileRecords = await _FileRecordsRepository
                    .FindAsIQueryable(p => p.master_id == guidQnId && p.file_code == strFileType && p.delete_status == (int)SystemDataStatus.Valid)
                    .OrderBy(p => p.id)
                    .Select(p => new QnSubmitFileDto
                    {
                        file_id = p.id,
                        file_name = p.file_name,
                        file_path = p.file_path,
                        file_remark = p.remark,
                        file_size = p.file_size.HasValue ? p.file_size.Value : 0,
                        upload_id = p.create_id,
                        upload_name = p.create_name,
                        upload_time = p.create_date
                    })
                    .ToListAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstFileRecords);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除提交文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteSumitFileAsync(Guid id)
        {
            try
            {
                return await _fileRecordsService.DeleteUploadFiles(new List<Guid> { id });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 提交完成文件
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <param name="qnId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SumbitFinishFilesAsync(List<IFormFile> lstFiles, Guid qnId)
        {
            try
            {
                // 处理期限
                //await _quotationDeadlineService.EditBySumbitFileAsync(qnId, UpcomingEventsEnum.QnTender);

                // 保存文件
                var strFileType = UploadFileCode.Submit_Finish_Documents;
                var getfolderResult = GetRootFolderName(qnId);
                if (!getfolderResult.status)
                {
                    return getfolderResult;
                }
                var strFolder = getfolderResult.data as string;
                return await _fileRecordsService.CommonUploadAsync(lstFiles, strFileType, strFolder, qnId);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 是否可以提交文件
        /// </summary>
        /// <param name="strQnStatus"></param>
        /// <returns></returns>
        private bool IsCanSumitFileStatus(string strQnStatus)
        {
            var lstExcludeStatus = new List<string>
            {
                //"Submission QN",                       //提交报价
                "Confirmed Order",                     //确认订单
                "Site Work",                           //进行工程
                "Completed",                           //完成工程
                "Defect Liability Period (DLP)",       //保修期
                "Closed",                              //关闭
            };

            return !lstExcludeStatus.Contains(strQnStatus);
        }

        #endregion

        #region 现场位置

        /// <summary>
        /// 保存现场位置
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="lstSiteIds"></param>
        public void SaveSitesNoSaveChange(Guid contractId, List<Guid?> lstSiteIds)
        {
            // 修改现场位置id
            var oldRelSites = _Site_RelationshipRepository.WhereIF(true, p => p.relation_id == contractId && p.delete_status == (int)SystemDataStatus.Valid).ToList();
            var addRelSites = new List<Biz_Site_Relationship>();
            var editRelSites = new List<Biz_Site_Relationship>();

            foreach (var oldRelSite in oldRelSites)
            {
                if (oldRelSite.site_id.HasValue && lstSiteIds.Contains(oldRelSite.site_id.Value))
                {
                    continue;
                }
                // 如果不存在的话则改成删除状态
                oldRelSite.delete_status = (int)SystemDataStatus.Invalid;
                oldRelSite.modify_id = UserContext.Current.UserInfo.User_Id;
                oldRelSite.modify_name = UserContext.Current.UserInfo.UserName;
                oldRelSite.modify_date = DateTime.Now;
                editRelSites.Add(oldRelSite);
            }
            foreach (var siteId in lstSiteIds)
            {
                if (oldRelSites.Any(p => p.site_id == siteId))
                {
                    // 如果跟数据存储一样，则过掉
                    continue;
                }
                addRelSites.Add(new Biz_Site_Relationship
                {
                    id = Guid.NewGuid(),
                    site_id = siteId,
                    relation_id = contractId,
                    relation_type = 0,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,
                    delete_status = (int)SystemDataStatus.Valid
                });
            }
            _Site_RelationshipRepository.UpdateRange(editRelSites);
            _Site_RelationshipRepository.AddRange(addRelSites);
        }

        #endregion

        #region 文件夹

        /// <summary>
        /// 报价上传文件从临时文件夹转移到正式文件夹
        /// </summary>
        /// <param name="qnData"></param>
        /// <param name="ctrData"></param>
        /// <param name="lstFileInfo"></param>
        /// <returns></returns>
        public WebResponseContent FileMoveToFormal(Biz_Quotation qnData, Biz_Contract ctrData, List<ContractQnFileDto> lstFileInfo)
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
                var getProFolderResult = _fileConfigService.GetMainProFolderName(qnData.create_date.Value);
                if (!getProFolderResult.status)
                {
                    return getProFolderResult;
                }
                var strProFolder = getProFolderResult.data as string;
                string strContractFolder = Path.Combine(AppSetting.FileSaveSettings.FolderPath, $"{companyData.company_no}\\{strProFolder}\\{qnData.qn_no}");

                // 创建合同文件夹目录
                var foldResult = CreateContractFolder(strContractFolder);
                if (!foldResult.status)
                {
                    return foldResult;
                }
                // 将临时文件夹的数据移动到正式的文件夹
                var moveResult = _fileRecordsService.FinishFilesUploadNoSaveChange(lstFileInfo, ctrData.id, true, strContractFolder, UploadFileCode.CTR);
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

        /// <summary>
        /// 根据qn的id获取文件存储文件名
        /// </summary>
        /// <param name="qnNo"></param>
        /// <param name="isNeedCompany"></param>
        /// <returns></returns>
        public WebResponseContent GetRootFolderName(Guid qnNo, bool isNeedCompany = false)
        {
            try
            {
                var qnData = _repository.FindFirst(p => p.id == qnNo);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }

                return DoProFolderName(qnData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据qn的id获取文件存储文件名
        /// </summary>
        /// <param name="qnNo"></param>
        /// <param name="isNeedCompany"></param>
        /// <returns></returns>
        public WebResponseContent GetRootFolderName(Biz_Quotation qn, bool isNeedCompany = false)
        {
            return DoProFolderName(qn, isNeedCompany);
        }

        /// <summary>
        /// 获取文件正式存放位置（相对）
        /// </summary>
        /// <param name="qnData"></param>
        /// <returns></returns>
        public WebResponseContent GetFormalFolder(Biz_Quotation qnData, string fileType)
        {
            // 合同的存放路径
            var getProFolderResult = GetRootFolderName(qnData, false);
            if (!getProFolderResult.status)
            {
                return getProFolderResult;
            }
            var strProFolder = getProFolderResult.data as string;
            // 文件的存放路径
            var getFileFolderResult = _fileRecordsService.GetFileSaveFolder(fileType, strProFolder);
            if (!getFileFolderResult.status)
            {
                return getFileFolderResult;
            }

            return getFileFolderResult;
        }

        /// <summary>
        /// 创建合同文件夹目录
        /// </summary>
        /// <param name="strContractPath"></param>
        /// <returns></returns>
        private WebResponseContent CreateContractFolder(string strContractPath)
        {
            try
            {
                var config = _configRepository.FindFirst(p => p.config_key == "ContractFolderPath" && p.config_type == 1 && p.delete_status == (int)SystemDataStatus.Valid);
                if (config == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_config_null"));
                }

                var lstFolder = JsonConvert.DeserializeObject<List<string>>(config.config_value.Replace("\\", "\\\\"));
                foreach (var item in lstFolder)
                {
                    var strFolder = Path.Combine(strContractPath, item);
                    if (!Directory.Exists(strFolder))
                    {
                        Directory.CreateDirectory(strFolder);
                    }
                }

                return WebResponseContent.Instance.OK("ok");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 处理文件夹名
        /// </summary>
        /// <param name="qnData"></param>
        /// <param name="isNeedCompany"></param>
        /// <returns></returns>
        private WebResponseContent DoProFolderName(Biz_Quotation qnData, bool isNeedCompany = false)
        {
            try
            {
                var getProFolderResult = _fileConfigService.GetMainProFolderName(qnData.create_date.Value);
                if (!getProFolderResult.status)
                {
                    return getProFolderResult;
                }
                var strProFolder = getProFolderResult.data as string;
                string strContractFolder = $"{strProFolder}\\{qnData.qn_no}";

                if (isNeedCompany)
                {
                    // 公司信息
                    var companyData = _companyRepository.FindFirst(p => p.id == UserContext.Current.UserInfo.Company_Id);
                    if (companyData == null)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("company_info_noexist"));
                    }
                    strContractFolder = $"{companyData.company_no}\\{strContractFolder}";
                }

                return WebResponseContent.Instance.OK("ok", strContractFolder);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 新增项目

        /// <summary>
        /// 创建项目
        /// </summary>
        /// <param name="strProName"></param>
        /// <returns></returns>
        public WebResponseContent AddProject(string strProName)
        {
            try
            {
                var isExit = _projectRepository
                    .WhereIF(!UserContext.Current.IsSuperAdmin, p => p.company_id == UserContext.Current.UserInfo.Company_Id)
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid && p.name_eng.Contains(strProName))
                    .Any();
                if (isExit)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("pro_name_exist"));
                }

                var proData = new Biz_Project
                {
                    id = Guid.NewGuid(),
                    company_id = UserContext.Current.UserInfo.Company_Id,
                    project_no = DataSequence.GetSequence(VodacsCommonParams.PR),
                    name_eng = strProName,
                    name_cht = strProName,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,
                };

                _projectRepository.Add(proData);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), proData);
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
