
using Dm.util;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.DataContracts;
using System.Threading.Tasks;
using Vodace.Core.Common;
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
    public partial class Biz_Confirmed_OrderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Confirmed_OrderRepository _repository;                 // 访问数据库
        private readonly ILocalizationService _localizationService;                  // 国际化服务
        private readonly IBiz_QuotationRepository _quotationRepository;              // qn仓储
        private readonly IBiz_Contract_DetailsRepository _contractDetailsRepository; // 合同详情仓储
        private readonly IBiz_ContractRepository _contractRepository;                // 合同仓储
        private readonly ISys_File_RecordsRepository _recordsRepository;             // 文件记录仓储
        private readonly IBiz_Site_RelationshipRepository _siteRelRepository;        // 访问现场位置关系数据库
        private readonly ISys_File_RecordsRepository _fileRecordsRepository;         // 文件记录仓储
        private readonly ISys_ConfigRepository  _configRepository;                   // 配置仓储

        private readonly IBiz_QuotationService _quotationService;                    // qn服务
        private readonly ISys_DictionaryListService _dictionaryListService;          // 数字字典服务

        private readonly ISys_File_RecordsService _fileRecordsService;  // 文件记录服务

        [ActivatorUtilitiesConstructor]
        public Biz_Confirmed_OrderService(
            IBiz_Confirmed_OrderRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IBiz_Contract_DetailsRepository contractDetailsRepository,
            ISys_File_RecordsService fileRecordsService,
            IBiz_ContractRepository contractRepository,
            IBiz_QuotationRepository quotationRepository,
            ISys_File_RecordsRepository recordsRepository,
            IBiz_Site_RelationshipRepository siteRelRepository,
            ISys_File_RecordsRepository fileRecordsRepository,
            IBiz_QuotationService quotationService,
            ISys_DictionaryListService dictionaryListService,
            ISys_ConfigRepository configRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _contractDetailsRepository = contractDetailsRepository;
            _fileRecordsService = fileRecordsService;
            _contractRepository = contractRepository;
            _quotationRepository = quotationRepository;
            _recordsRepository = recordsRepository;
            _siteRelRepository = siteRelRepository;
            _fileRecordsRepository = fileRecordsRepository;
            _quotationService = quotationService;
            _dictionaryListService = dictionaryListService;
            _configRepository = configRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// CO分页查询
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCoPageList(PageInput<CoSearchDto> dtoSearchInput)
        {
            try
            {
                var dtoSearch = dtoSearchInput.search;

                Expression<Func<Biz_Confirmed_Order, Biz_Quotation, Biz_Contract, Biz_Sub_Contractors, IEnumerable<Biz_Site>, Sys_Contact, CoDataDto>> select =
                    (co, qn, contract, cus, sites, contact) => new CoDataDto
                    {
                        co_id = co.id,
                        co_no = co.co_no,
                        qn_no = qn.qn_no,
                        customer_id = cus.id,
                        customer_name = cus.name_sho,
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
                        contract_id = contract.id,
                        contract_no = contract.contract_no,
                        contract_ref_no = contract.ref_no,
                        contract_name = contract.name_eng,
                        confirm_date = co.confirm_date,
                        confirm_amount = co.confirm_amt,
                        create_date = co.create_date,
                        create_id= co.create_id,
                        create_name = co.create_name,
                        status_code = qn.status_code,
                        qn_year = qn.qn_year,
                        contract_type = contract.tender_type,
                        head_id = co.head_id,
                        head_name = contact.name_sho,
                        commen_date = co.commen_date,
                        complet_act_date = co.complet_act_date,
                        complet_exp_date = co.complet_exp_date,
                        confirm_currency = qn.confirm_currency
                    };
                select = select.BuildExtendSelectExpre();

                // 获取当前用户公司ID
                var currentCompanyId = UserContext.Current.UserInfo.Company_Id;

                // 获取确认报价、报价、合同、项目、客户
                var confirmedOrders = DBServerProvider.DbContext.Set<Biz_Confirmed_Order>()
                    .AsExpandable().AsNoTracking()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid); // 0表示正常状态

                var quotations = DBServerProvider.DbContext.Set<Biz_Quotation>()
                    .AsNoTracking()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid);

                var contracts = DBServerProvider.DbContext.Set<Biz_Contract>()
                    .AsNoTracking()
                    .Where(p => p.delete_status == 0);

                var contact = DBServerProvider.DbContext.Set<Sys_Contact>()
                    .AsNoTracking()
                    .Where(p => p.delete_status == 0);


                //var projects = DBServerProvider.DbContext.Set<Biz_Project>()
                //    .AsNoTracking()
                //    .Where(p => p.delete_status == (int)SystemDataStatus.Valid);
                //.Where(p => p.delete_status == (int)SystemDataStatus.Valid && p.company_id == UserContext.Current.UserInfo.Company_Id);

                if (!UserContext.Current.IsSuperAdmin)
                {
                    if (currentCompanyId.HasValue)
                    {
                        confirmedOrders = confirmedOrders.Where(p => p.company_id == currentCompanyId);
                    }
                    else
                    {
                        confirmedOrders = confirmedOrders.Where(p => p.company_id == Guid.Empty);
                    }
                }

               

                var customers = DBServerProvider.DbContext.Set<Biz_Sub_Contractors>()
                    .AsNoTracking()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid);

                // 获取站点关系和站点
                var siteRelationships = DBServerProvider.DbContext.Set<Biz_Site_Relationship>()
                    .AsNoTracking()
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid && p.relation_type == 0); // relation_type = 0 表示合同和站点的关系

                var sites = DBServerProvider.DbContext.Set<Biz_Site>()
                    .AsNoTracking()
                    .Where(p => p.delete_status == 0);

                // 构建主查询：确认报价左连接报价、合同、项目、客户、负责人
                var query = from co in confirmedOrders
                            join qn in quotations on co.qn_id equals qn.id into co_qn
                            from quotation in co_qn.DefaultIfEmpty()
                            join c in contracts on quotation.contract_id equals c.id into qn_contract
                            from contract in qn_contract.DefaultIfEmpty()
                            join cs in customers on co.customer_id equals cs.id into co_customer
                            from customer in co_customer.DefaultIfEmpty()
                            join hd in contact  on co.head_id equals hd.id into co_header
                            from header in co_header.DefaultIfEmpty()
                            select new { co, quotation, contract, customer, header };

                // 如果有站点ID集合筛选条件
                if (dtoSearch != null && dtoSearch.lst_site_ids != null && dtoSearch.lst_site_ids.Count > 0)
                {
                    // 使用方法语法进行过滤，避免查询表达式语法错误
                    query = query.Where(item => siteRelationships
                        .Any(sr => sr.relation_id == item.contract.id && dtoSearch.lst_site_ids.Contains(sr.site_id.Value)));
                }

                // 继续构建查询，为每个确认报价添加关联的所有站点
                var finalQuery = from item in query
                                 // 子查询获取当前合同关联的所有站点
                                 let contractSites = (from sr in siteRelationships
                                                      join s in sites on sr.site_id equals s.id
                                                      where sr.relation_id == item.contract.id
                                                      select s).AsEnumerable()
                                 // 应用选择表达式
                                 select @select.Invoke(item.co, item.quotation, item.contract, item.customer, contractSites, item.header);

                // 应用其他可能的查询条件
                if (dtoSearch != null)
                {
                    if (!string.IsNullOrEmpty(dtoSearch.qn_no))
                    {
                        finalQuery = finalQuery.Where(dto => dto.qn_no.Contains(dtoSearch.qn_no));
                    }

                    if (!string.IsNullOrEmpty(dtoSearch.contract_name))
                    {
                        finalQuery = finalQuery.Where(dto => dto.contract_name.Contains(dtoSearch.contract_name));
                    }

                    if (!string.IsNullOrEmpty(dtoSearch.contract_no))
                    {
                        finalQuery = finalQuery.Where(dto => dto.contract_no.Contains(dtoSearch.contract_no));
                    }

                    if (!string.IsNullOrEmpty(dtoSearch.customer_name))
                    {
                        finalQuery = finalQuery.Where(dto => dto.customer_name.Contains(dtoSearch.customer_name));
                    }

                    if (!string.IsNullOrEmpty(dtoSearch.co_no))
                    {
                        finalQuery = finalQuery.Where(dto => dto.co_no.Contains(dtoSearch.co_no));
                    }

                    if (!string.IsNullOrEmpty(dtoSearch.status_code))
                    {
                        finalQuery = finalQuery.Where(dto => dto.status_code == dtoSearch.status_code);
                    }

                    if (dtoSearch.year.HasValue)
                    {
                        finalQuery = finalQuery.Where(dto => dto.qn_year == dtoSearch.year);
                    }

                    if (!string.IsNullOrEmpty(dtoSearch.contract_type))
                    {
                        finalQuery = finalQuery.Where(dto => dto.contract_type == dtoSearch.contract_type);
                    }
                }

                // 执行分页查询
                if (string.IsNullOrEmpty(dtoSearchInput.sort_field))
                {
                    dtoSearchInput.sort_field = "create_date";
                    dtoSearchInput.sort_type = "desc";
                }
                var result = await finalQuery.GetPageResultAsync(dtoSearchInput);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 确认报价
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ConfirmOrder(ConfirmDto dtoInput)
        {
            try
            {
                // 获取报价、合约
                var qnData = await _quotationRepository.FindAsyncFirst(p => p.id == dtoInput.qn_id);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                
                var ctrData = await _contractRepository.FindAsyncFirst(p => p.id == qnData.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 确认是否能确定报价
                if (qnData.status_code != "Submission QN")
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("co_confirm_status_error"));
                }

                qnData.status_code = "Confirmed Order";
                qnData.modify_date = DateTime.Now;
                qnData.modify_id = UserContext.Current.UserInfo.User_Id;
                qnData.modify_name = UserContext.Current.UserInfo.UserName;

                var coData = new Biz_Confirmed_Order
                {
                    id = Guid.NewGuid(),
                    co_no = DataSequence.GetSequence(VodacsCommonParams.CO),
                    project_id = ctrData.project_id,
                    company_id = ctrData.company_id,
                    qn_id = qnData.id,
                    customer_id = qnData.customer_id,
                    //confirm_date = qnData.confirm_amt_date,
                    confirm_amt = qnData.confirm_amt,
                    confirm_currency = qnData.confirm_currency,
                    //commen_date = ctrData.,
                    //complet_date = ctrData.end_date,
                    enter_id = UserContext.Current.UserInfo.User_Id,
                    delete_status = 0,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,

                    contract_id = qnData.contract_id,
                    po_no = dtoInput.po_no,
                    confirm_date = dtoInput.confirm_date,
                    head_id = dtoInput.head_id,
                    commen_date = dtoInput.commen_date,
                    complet_exp_date = dtoInput.complet_exp_date,
                    complet_act_date = dtoInput.complet_act_date,
                };

               
                #region 文件移动到正式目录

                if (dtoInput.upload_file_info.Count > 0)
                {
                    var getRootFolderResult = _quotationService.GetFormalFolder(qnData, UploadFileCode.Qn_Co);
                    if (!getRootFolderResult.status)
                    {
                        return getRootFolderResult;
                    }
                    var rootFolder = getRootFolderResult.data as string;
                    _fileRecordsService.FinishFilesUploadNoSaveChange(dtoInput.upload_file_info, coData.id, true, rootFolder, UploadFileCode.Qn_Co);
                }

                #endregion

                _quotationRepository.Update(qnData);
                _repository.Add(coData);
                _repository.SaveChanges();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 取消确认报价
        /// </summary>
        /// <param name="guidId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> UnConfirmOrder(Guid guidId)
        {
            try
            {
                // 获取报价
                var qnData = await _quotationRepository.FindAsyncFirst(p => p.id == guidId);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }

                if (qnData.status_code != "Confirmed Order")
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("co_unconfirm_status_error"));
                }

                // 获取对应的确定报价
                var coData = await _repository.FindAsyncFirst(p => p.qn_id == guidId && p.delete_status == 0);
                if (coData == null) 
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }

                qnData.status_code = "Submission QN";
                qnData.modify_date = DateTime.Now;
                qnData.modify_id = UserContext.Current.UserInfo.User_Id;
                qnData.modify_name = UserContext.Current.UserInfo.UserName;

                coData.delete_status = (int)SystemDataStatus.Invalid;
                coData.modify_date = DateTime.Now;
                coData.modify_id = UserContext.Current.UserInfo.User_Id;
                coData.modify_name = UserContext.Current.UserInfo.UserName;

                var lstRecords = await _recordsRepository
                    .FindAsync(p => p.file_code == UploadFileCode.Qn_Co && p.delete_status == (int)SystemDataStatus.Valid && p.master_id == coData.id);
                foreach (var item in lstRecords)
                {
                    item.delete_status = (int)SystemDataStatus.Invalid;
                    item.modify_date = DateTime.Now;
                    item.modify_id = UserContext.Current.UserInfo.User_Id;
                    item.modify_name = UserContext.Current.UserInfo.UserName;
                }
                _recordsRepository.UpdateRange(lstRecords);

                _quotationRepository.Update(qnData);
                _repository.Update(coData);
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
        /// 确认订单文件上传
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadCoFile(List<IFormFile> lstFiles)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                //return await _fileRecordsService.CommonUploadAsync(lstFiles, UploadFileCode.Qn_Co);
                return await _fileRecordsService.CommonUploadToTempAsync(lstFiles, UploadFileCode.Qn_Co);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据id获取co详细信息
        /// </summary>
        /// <param name="guidCOId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCoByIdAsync(Guid guidCOId)
        {
            try
            {
                // 获取CO
                var coData = await _repository.FindFirstAsync(p => p.id == guidCOId);
                if (coData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("co_null"));
                }

                // 获取报价、合约
                var qnData = await _quotationRepository.FindAsyncFirst(p => p.id == coData.qn_id);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                var ctrData = await _contractRepository.FindAsyncFirst(p => p.id == coData.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 获取现场地点
                var lstSites = await _siteRelRepository
                        .FindAsync(p => p.relation_type == 0 && p.relation_id == ctrData.id && p.delete_status == (int)SystemDataStatus.Valid);
                var lstSiteIds = lstSites.Select(p => p.site_id).ToList();

                // 获取上传相关的合同资料
                var lstCtrFiles = await _fileRecordsRepository
                    .FindAsync(p => p.master_id == coData.id &&
                                    p.file_code == UploadFileCode.Qn_Co &&
                                    p.upload_status == (int)UploadStatus.Finish &&
                                    p.delete_status == (int)SystemDataStatus.Valid);

                var dtoCOData = new CoDetailsDto
                {
                    create_date = coData.create_date,
                    create_id = coData.create_id,
                    create_name = coData.create_name,
                    modify_date = coData.modify_date,
                    modify_id = coData.modify_id,
                    modify_name = coData.modify_name,

                    co_id = coData.id,
                    co_no = coData.co_no,
                    qn_id = qnData.id,
                    qn_no = qnData.qn_no,
                    contract_id = ctrData.id,
                    contract_name = ctrData.name_eng,
                    contract_no = ctrData.contract_no,
                    customer_id = qnData.customer_id,
                    qn_stuts_code = qnData.status_code,
                    contract_ref_no = ctrData.ref_no,
                    po_no = coData.po_no,
                    year = qnData.qn_year,
                    confirm_date = coData.confirm_date,
                    confirm_amt = coData.confirm_amt,
                    commen_date = coData.commen_date,
                    complet_exp_date = coData.complet_exp_date,
                    complet_act_date = coData.complet_act_date,
                    head_id = coData.head_id,
                    confirm_currency = qnData.confirm_currency,
                    site_ids = lstSiteIds
                };

                foreach (var item in lstCtrFiles)
                {
                    dtoCOData.upload_file_info.Add(new ContractQnFileDto
                    {
                        file_id = item.id,
                        file_name = item.file_name,
                        file_remark = item.remark,
                        file_size = item.file_size.HasValue ? item.file_size.Value : 0
                    });
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), dtoCOData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改co信息
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditCoAsync(CoInputDto dtoInput)
        {
            try
            {
                var coData = await _repository.FindFirstAsync(p => p.id == dtoInput.co_id);
                if (coData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("co_null"));
                }
                var qnData = await _quotationRepository.FindFirstAsync(p => p.id == coData.qn_id);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                var ctrData = await _contractRepository.FindFirstAsync(p => p.id == coData.contract_id); // 合约信息
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 获取配置项
                var configData = _configRepository.FindFirst(p => p.config_type == 3 && p.config_key == "currency");
                if (configData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("config_null"));
                }

                // 判断选择的状态是否有问题（确认订单后只能往后取数据）
                var checkRsult = await CheckCoStatus(dtoInput.qn_stuts_code);
                if (!checkRsult.status)
                {
                    return checkRsult;
                }
                var boolCheck = checkRsult.data as bool?;
                if (!boolCheck.HasValue || !boolCheck.Value)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 确认订单
                coData.po_no = dtoInput.po_no;
                coData.head_id = dtoInput.head_id;
                coData.confirm_date = dtoInput.confirm_date;
                coData.commen_date = dtoInput.commen_date;
                coData.complet_exp_date = dtoInput.complet_exp_date;
                coData.complet_act_date = dtoInput.complet_act_date;
                coData.confirm_amt = dtoInput.confirm_amt;

                coData.modify_id = UserContext.Current.UserInfo.User_Id;
                coData.modify_name = UserContext.Current.UserInfo.UserName;
                coData.modify_date = DateTime.Now;

                // 合同
                var boolCheckRepeat = _quotationService.CheckRepeatContract(dtoInput.contract_no, false, ctrData.id);//判断编码是否已存在
                if (boolCheckRepeat)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_no_repeat"));
                }

                ctrData.name_cht = dtoInput.contract_name;
                ctrData.name_eng = dtoInput.contract_name;
                ctrData.contract_no = dtoInput.contract_no;
                ctrData.ref_no = dtoInput.contract_ref_no;

                ctrData.modify_id = UserContext.Current.UserInfo.User_Id;
                ctrData.modify_name = UserContext.Current.UserInfo.UserName;
                ctrData.modify_date = DateTime.Now;

                // 报价
                qnData.customer_id = dtoInput.customer_id;
                qnData.status_code = dtoInput.qn_stuts_code;
                qnData.qn_year = dtoInput.year;
                qnData.confirm_amt = dtoInput.confirm_amt;
                qnData.confirm_currency = configData.config_value;

                qnData.modify_id = UserContext.Current.UserInfo.User_Id;
                qnData.modify_name = UserContext.Current.UserInfo.UserName;
                qnData.modify_date = DateTime.Now;

                // 修改现场位置id
                _quotationService.SaveSitesNoSaveChange(ctrData.id, dtoInput.site_ids);

                _repository.Update(coData);
                _quotationRepository.Update(qnData);
                _contractRepository.Update(ctrData);

                #region 正式文件夹的存放路径

                var getFileFolderResult = _quotationService.GetFormalFolder(qnData, UploadFileCode.Qn_Co);
                if (!getFileFolderResult.status)
                {
                    return getFileFolderResult;
                }
                var fileFolder = getFileFolderResult.data as string;

                #endregion

                // 获取上传的文件记录
                var uploadIds = dtoInput.upload_file_info.Where(p => p.file_id.HasValue).Select(p => p.file_id.Value).ToList();
                var dicUpload = dtoInput.upload_file_info.Where(p => p.file_id.HasValue).ToDictionary(p => p.file_id.Value);
                var lstNowRecords = await _fileRecordsRepository.FindAsync(p => uploadIds.Contains(p.id));
                var dicNowRecords = lstNowRecords.ToDictionary(p => p.id);
                // 获取之前存储的文件记录(用来删除的)
                var lstBeforeRecords = await _fileRecordsRepository.FindAsync(p => p.delete_status == (int)SystemDataStatus.Valid &&
                                                                                   p.file_code == UploadFileCode.Qn_Co &&
                                                                                   p.master_id == coData.id &&
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
                            item.file_path = _fileRecordsService.MoveFile(fileFolder, item);
                        }
                        item.master_id = coData.id;
                        //item.upload_status = (int)UploadStatus.Finish;
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

        /// <summary>
        /// 统计确认订单的确认金额
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> CountCOAmt()
        {
            try
            {
                var contracts = DBServerProvider.DbContext.Set<Biz_Contract>()
                .AsNoTracking()
                .Where(p => p.delete_status == (int)SystemDataStatus.Valid);

                var coRepository = _repository.WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid);

                var currentCompanyId = UserContext.Current.UserInfo.Company_Id;
                if (currentCompanyId.HasValue)
                {
                    coRepository = coRepository.Where(p => p.company_id == currentCompanyId);
                }
                else
                {
                    if (!UserContext.Current.IsSuperAdmin)
                    {
                        coRepository = coRepository.Where(p => p.company_id == Guid.Empty);
                    }
                }

                var countAmt = await coRepository.SumAsync(p => p.confirm_amt);

                // 计算总和
                var statistics = new QnSumAmtDto
                {
                    total_qn_amt = 0,
                    total_confirm_amt = countAmt.HasValue? countAmt.Value : 0,
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
        /// 获取确认订单下的状态
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetStatus()
        {
            try
            {
                var getDicResult = await _dictionaryListService.GetDictionaryByCode("qn_process_status");
                if (!getDicResult.status)
                {
                    return getDicResult;
                }
                var lstDic = getDicResult.data as List<ContractStatusDto>;

                // 获取确认订单的位置
                var dtoCOStatuc = lstDic.FirstOrDefault(p => p.code == "Confirmed Order");
                if (dtoCOStatuc == null)
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), false);
                }

                if (dtoCOStatuc == null || !dtoCOStatuc.index.HasValue)
                {
                    return getDicResult;
                }

                var lstData = lstDic.Where(p => p.index >= dtoCOStatuc.index).ToList();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 判断当前状态是否可以选择
        /// </summary>
        /// <param name="strCurrentStatus"></param>
        /// <returns></returns>
        private async Task<WebResponseContent> CheckCoStatus(string strCurrentStatus)
        {
            var getDicResult = await _dictionaryListService.GetDictionaryByCode("qn_process_status");
            if (!getDicResult.status)
            {
                return getDicResult;
            }
            var lstDic = getDicResult.data as List<ContractStatusDto>;

            // 获取确认订单的位置
            var dtoCOStatuc = lstDic.FirstOrDefault(p => p.code == "Confirmed Order");
            if (dtoCOStatuc == null)
            {
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), false);
            }

            // 获取当前选择的状态
            var dtoCurrent = lstDic.FirstOrDefault(p => p.code == strCurrentStatus);
            if (dtoCurrent == null)
            {
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), false);
            }

            if (dtoCurrent.index < dtoCOStatuc.index)
            {
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), false);
            }

            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), true);
        }
    }
}