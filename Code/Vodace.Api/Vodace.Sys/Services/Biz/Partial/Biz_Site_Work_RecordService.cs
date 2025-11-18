
using Castle.Core.Internal;
using Dm.util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models.Partners.Billing;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Configuration;
using Vodace.Core.DBManager;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.UserManager;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Biz_Site_Work_RecordService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Site_Work_RecordRepository _repository;//访问数据库

        private readonly IBiz_ContractRepository _contractRepository;                   // 合同仓储
        private readonly IBiz_ProjectRepository _projectRepository;                     // 项目仓储
        private readonly IBiz_SiteRepository _siteRepository;                           // 工地仓储
        private readonly IBiz_Site_Work_Record_WorkerRepository _siteWorkerRepository;  // 工人仓储
        private readonly ISys_ContactRepository _contactRepository;                     // 联系人仓储
        private readonly ISys_CompanyRepository _companyRepository;                     // 公司仓储
        private readonly IBiz_Project_FilesRepository _projectfilesRepository;          // 项目文件仓储
        private readonly IBiz_Rolling_ProgramRepository _rollingProgramRepository;      // 滚动计划仓储
        private readonly ISys_Site_Work_Check_ItemRepository _sysCheckItemRepository;   // 选项仓储
        private readonly IBiz_QuotationRepository _quotationRepository;                 // 报价仓储
        private readonly IBiz_Contact_RelationshipRepository _contactRelRepository;     // 联系人仓储
        private readonly ISys_ConfigRepository _configRepository;                       // 配置仓储

        private readonly ILocalizationService _localizationService;                     // 国际化服务
        private readonly IBiz_Project_FilesService _projectfilesService;                // 项目文件服务
        private readonly ISys_User_NewRepository _userRepository;   // 选项仓储

        [ActivatorUtilitiesConstructor]
        public Biz_Site_Work_RecordService(
            IBiz_Site_Work_RecordRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IBiz_ContractRepository contractRepository,
            IBiz_Site_Work_Record_WorkerRepository siteWorkerRepository,
            IBiz_SiteRepository siteRepository,
            IBiz_ProjectRepository projectRepository,
            ISys_ContactRepository contactRepository,
            ISys_CompanyRepository companyRepository,
            IBiz_Project_FilesRepository projectfilesRepository,
            IBiz_Project_FilesService projectfilesService,
            IBiz_Rolling_ProgramRepository rollingProgramRepository,
            ISys_Site_Work_Check_ItemRepository sysCheckItemRepository,
            ISys_User_NewRepository userRepository,
            IBiz_QuotationRepository quotationRepository,
            IBiz_Contact_RelationshipRepository contactRelationship,
            ISys_ConfigRepository configRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _contractRepository = contractRepository;
            _siteWorkerRepository = siteWorkerRepository;
            _siteRepository = siteRepository;
            _projectRepository = projectRepository;
            _contactRepository = contactRepository;
            _companyRepository = companyRepository;
            _projectfilesRepository = projectfilesRepository;
            _projectfilesService = projectfilesService;
            _rollingProgramRepository = rollingProgramRepository;
            _sysCheckItemRepository = sysCheckItemRepository;
            _userRepository = userRepository;
            _quotationRepository = quotationRepository;
            _contactRelRepository = contactRelationship;
            _configRepository = configRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSiteWorkRecordPageAsync(PageInput<SiteWorkRecordSearchDto> pageInput)
        {
            try
            {
                var search = pageInput?.search;

                var query = DoSearchSiteWorkRecord(search);
                //var sql = query.ToQueryString();
                // 默认排序
                if (string.IsNullOrEmpty(pageInput?.sort_field))
                {
                    pageInput.sort_field = "create_date";
                    pageInput.sort_type = "desc";
                }

                var result = await query.GetPageResultAsync(pageInput);
                result.data = await SetSiteWorkRecordWorker(result.data);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据时间获取工地工作记录
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSiteWorkRecordByDateAsync(SiteWorkRecordSearchDto search)
        {
            try
            {
                var query = DoSearchSiteWorkRecord(search);

                var result = await query.ToListAsync();

                result = await SetSiteWorkRecordWorker(result);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 构建工地工作记录IQueryable
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private IQueryable<SiteWorkRecordListDto> DoSearchSiteWorkRecord(SiteWorkRecordSearchDto search)
        {
            // 源数据
            var records = _repository
                .FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                .AsNoTracking();

            var rollings = _rollingProgramRepository
                .FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                .Select(p => new { p.id, p.content, p.percentage })
                .AsNoTracking();

            var contracts = _contractRepository
                .FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                .Select(p => new { p.id, p.contract_no, p.name_eng, p.name_cht, p.company_id })
                .AsNoTracking();

            var sites = _siteRepository
                .FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                .Select(p => new { p.id, p.name_sho })
                .AsNoTracking();

            var workers = _siteWorkerRepository
                .FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid && p.record_id.HasValue)
                .Select(p => new { p.record_id, p.time_in, p.time_out, p.time_in_adj, p.time_out_adj, p.work_type_id, p.is_sic, p.contact_id })
                .AsNoTracking();

            // 关联权限：当前用户需在报价的联系人关系表中（relation_type=0）
            var quotations = _quotationRepository
                .FindAsIQueryable(q => q.delete_status == (int)SystemDataStatus.Valid)
                .AsNoTracking()
                .Select(q => new { q.id, q.contract_id });

            var relationships = _contactRelRepository
                .FindAsIQueryable(cr => cr.delete_status == (int)SystemDataStatus.Valid && cr.relation_type == 0)
                .AsNoTracking()
                .Select(cr => new { cr.relation_id, cr.contact_id });

            // 非超级管理员：同公司过滤（优先按公司）
            if (!UserContext.Current.IsSuperAdmin)
            {
                // 权限过滤：仅展示当前用户在报价联系人关系中的数据
                var currentUserContactId = UserContext.Current.UserInfo.Contact_Id;
                if (currentUserContactId.HasValue)
                {
                    var permittedContracts = from q in quotations
                                             join cr in relationships on q.id equals cr.relation_id
                                             where cr.contact_id == currentUserContactId.Value
                                             select q.contract_id;

                    records = from r in records
                              where r.contract_id.HasValue && permittedContracts.Contains(r.contract_id.Value)
                              select r;

                }
            }

            // 连接查询（改为子查询聚合，避免分组参数组合带来的翻译问题）
            var query = from r in records
                        join rp in rollings on r.rolling_program_id equals rp.id into rrp
                        from rolling in rrp.DefaultIfEmpty()
                        join c in contracts on r.contract_id equals c.id into rc
                        from contract in rc.DefaultIfEmpty()
                        join s in sites on r.site_id equals s.id into rs
                        from site in rs.DefaultIfEmpty()
                        //join sub in sites on r.site_id equals sub.id into rsub
                        //from subSite in rsub.DefaultIfEmpty()
                        select new SiteWorkRecordListDto
                        {
                            id = r.id,
                            contract_id = contract != null ? contract.id : null,
                            contract_no = contract != null ? contract.contract_no : null,
                            contract_name = contract != null ? (!string.IsNullOrEmpty(contract.name_eng) ? contract.name_eng : contract.name_cht) : null,
                            location = site != null ? site.name_sho : null,
                            site_id = r.site_id,
                            date = r.work_date,
                            //time_in = agg != null ? agg.time_in : null,
                            //time_out = agg != null ? agg.time_out : null,
                            //work_count = agg != null ? agg.work_count : 0,
                            //cpt_count = agg != null ? agg.cpt_count : 0,
                            //cpnt_count = agg != null ? agg.cpnt_count : 0,
                            //fm_count = agg != null ? agg.fm_count : 0,
                            //work_count = workers.Count(w => w.record_id == r.id),
                            //cpt_count = workers.Count(w => w.record_id == r.id && w.work_type_id.HasValue && cptIds.Contains(w.work_type_id.Value)),
                            //cpnt_count = workers.Count(w => w.record_id == r.id && w.work_type_id.HasValue && cpntIds.Contains(w.work_type_id.Value)),
                            //fm_count = workers.Count(w => w.record_id == r.id && w.work_type_id.HasValue && fmIds.Contains(w.work_type_id.Value)),
                            company_id = contract != null ? contract.company_id : null,
                            shift_index = ShiftTypeHelper.GetShiftTypeIndex(r.shift),
                            shift = ShiftTypeHelper.GetShiftTypeValue(r.shift),
                            rolling_program_id = r.rolling_program_id,
                            rolling_program_content = rolling.content,
                            duty_cp_id = r.duty_cp_id,
                            current_duty_cp_id = r.current_duty_cp_id,
                            //sic_id = workers.Where(w => w.record_id == r.id && w.is_sic == 1).Select(w => w.contact_id).FirstOrDefault()
                        };

            

            // 条件过滤
            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.contract_no))
                {
                    query = query.Where(p => p.contract_no.Contains(search.contract_no));
                }

                // 站点ID（可多个）筛选：通过 records 套筛，按 r.site_id in site_ids
                if (search.site_ids != null && search.site_ids.Count > 0)
                {
                    var sids = search.site_ids.Where(x => x != Guid.Empty).Distinct().ToList();
                    if (sids.Count > 0)
                    {
                        query = from q in query
                                join r in records on q.id equals r.id
                                where r.site_id.HasValue && sids.Contains(r.site_id.Value)
                                select q;
                    }
                }

                if (search.start_date.HasValue)
                {
                    var start = search.start_date.Value.Date;
                    query = query.Where(p => p.date >= start);
                }

                if (search.end_date.HasValue)
                {
                    // 包含当日：小于 next day
                    var endNext = search.end_date.Value.Date.AddDays(1);
                    query = query.Where(p => p.date < endNext);
                }

                if (search.contract_id.HasValue)
                {
                    // 通过 records 套筛，避免反查contract_id，直接关联条件
                    var cid = search.contract_id.Value;
                    query = from q in query
                            join r in records on q.id equals r.id
                            where r.contract_id == cid
                            select q;
                }

                if (search.date.HasValue)
                {
                    var startData = search.date.Value.Date;
                    var endData = search.date.Value.Date.AddDays(1);
                    query = query.Where(p => p.date >= startData && p.date < endData);
                }

                if (search.shift.HasValue)
                {
                    query = query.Where(p => p.shift_index == search.shift.Value);
                }

                if (search.rolling_program_id.HasValue)
                {
                    query = query.Where(p => p.rolling_program_id == search.rolling_program_id);
                }
            }

            return query;
        }

        public async Task<List<SiteWorkRecordListDto>> SetSiteWorkRecordWorker(List<SiteWorkRecordListDto> data)
        {
            // 读取配置：config_type == 5，key 为 CPT/CPNT/FM，对应 Sys_Work_Type.type_name
            var dbContext = DBServerProvider.DbContext;
            var cfgList = await _configRepository
                .FindAsIQueryable(p => p.config_type == 5 && (p.config_key == "CPT" || p.config_key == "CPNT" || p.config_key == "FM"))
                .AsNoTracking()
                .Select(p => new { p.config_key, p.config_value })
                .ToListAsync();

            var cptNames = cfgList.Where(x => x.config_key == "CPT")
                .Select(x => x.config_value)
                .Where(v => !string.IsNullOrEmpty(v)).Distinct().ToList();
            var cpntNames = cfgList.Where(x => x.config_key == "CPNT")
                .Select(x => x.config_value)
                .Where(v => !string.IsNullOrEmpty(v)).Distinct().ToList();
            var fmNames = cfgList.Where(x => x.config_key == "FM")
                .Select(x => x.config_value)
                .Where(v => !string.IsNullOrEmpty(v)).Distinct().ToList();

            var typeIds = await dbContext.Set<Sys_Work_Type>()
                .AsNoTracking()
                .Where(p => (cptNames.Contains(p.type_name) || cpntNames.Contains(p.type_name) || fmNames.Contains(p.type_name)))
                .Select(p => new { p.id, p.type_name })
                .ToListAsync();

            var cptIds = typeIds.Where(t => cptNames.Contains(t.type_name)).Select(t => t.id).ToList();
            var cpntIds = typeIds.Where(t => cpntNames.Contains(t.type_name)).Select(t => t.id).ToList();
            var fmIds = typeIds.Where(t => fmNames.Contains(t.type_name)).Select(t => t.id).ToList();

            var lstRecordIds = data.Select(p => p.id).ToList();
            var lstWorkers = await _siteWorkerRepository
                .FindAsIQueryable(p => p.record_id.HasValue && lstRecordIds.Contains(p.record_id.Value) && p.delete_status == (int)SystemDataStatus.Valid)
                .AsNoTracking()
                .ToListAsync();

            var dicWorkers = new Dictionary<Guid, List<Biz_Site_Work_Record_Worker>>();
            foreach (var item in lstWorkers)
            {
                if (dicWorkers.ContainsKey(item.record_id.Value))
                {
                    dicWorkers[item.record_id.Value].Add(item);
                }
                else
                {
                    dicWorkers.Add(item.record_id.Value, new List<Biz_Site_Work_Record_Worker> { item });
                }
            }

            foreach (var item in data)
            {
                var isOk = dicWorkers.TryGetValue(item.id, out var recordWorkerData);
                if (!isOk)
                {
                    continue;
                }
                var lstTimeOuts = new List<DateTime?>();
                var lstTimeIns = new List<DateTime?>();
                foreach (var time in recordWorkerData)
                {
                    lstTimeIns.Add(time.time_in_adj.HasValue ? time.time_in_adj: time.time_in);
                    lstTimeOuts.Add(time.time_out_adj.HasValue ? time.time_out_adj : time.time_out);
                }
                item.time_in = lstTimeIns.Min();
                item.time_out = lstTimeOuts.Max();
                item.work_count = recordWorkerData.Count(w => w.work_type_id.HasValue && !typeIds.Select(p => p.id).Contains(w.work_type_id.Value));
                item.cpt_count = recordWorkerData.Count(w => w.work_type_id.HasValue && cptIds.Contains(w.work_type_id.Value));
                item.cpnt_count = recordWorkerData.Count(w => w.work_type_id.HasValue && cpntIds.Contains(w.work_type_id.Value));
                item.fm_count = recordWorkerData.Count(w => w.work_type_id.HasValue && fmIds.Contains(w.work_type_id.Value));
                item.sic_id = recordWorkerData.Where(p => p.is_sick == 1).Select(p => p.contact_id).FirstOrDefault();
            }

            return data;
        }

        /// <summary>
        /// 根据id获取工地记录详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSiteWorkRecordByIdAsync(Guid id)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;

                // 源数据（只取指定id的记录）
                var records = _repository
                    .FindAsIQueryable(r => r.delete_status == valid && r.id == id)
                    .AsNoTracking();

                var contracts = _contractRepository
                    .FindAsIQueryable(c => c.delete_status == valid)
                    .Select(c => new { c.id, c.contract_no, c.name_cht, c.name_eng, c.project_id, c.company_id })
                    .AsNoTracking();

                var projects = _projectRepository
                    .FindAsIQueryable(p => p.delete_status == valid)
                    .Select(p => new { p.id, p.project_no, p.name_cht, p.name_eng })
                    .AsNoTracking();

                var sites = _siteRepository
                    .FindAsIQueryable(s => s.delete_status == valid)
                    .Select(s => new { s.id, s.name_sho })
                    .AsNoTracking();

                var contacts = _contactRepository
                    .FindAsIQueryable(ct => ct.delete_status == valid)
                    .Select(ct => new { ct.id, ct.name_cht, ct.name_eng })
                    .AsNoTracking();

                var companies = _companyRepository
                    .FindAsIQueryable(co => co.delete_status == valid)
                    .Select(co => new { co.id, co.company_name, co.company_name_eng })
                    .AsNoTracking();

                // 连接查询，组装详情DTO
                var query = from r in records
                            join c in contracts on r.contract_id equals c.id into rc
                            from contract in rc.DefaultIfEmpty()
                            join p in projects on contract.project_id equals p.id into cp
                            from project in cp.DefaultIfEmpty()
                            join s in sites on r.site_id equals s.id into rs
                            from site in rs.DefaultIfEmpty()
                            join ss in sites on r.sub_site_id equals ss.id into rss
                            from subSite in rss.DefaultIfEmpty()
                            join ct in contacts on r.current_duty_cp_id equals ct.id into rct
                            from contact in rct.DefaultIfEmpty()
                            join co1 in companies on contract.company_id equals co1.id into cco
                            from contractCompany in cco.DefaultIfEmpty()
                            select new SiteWorkRecordDetailsDto
                            {
                                id = r.id,
                                contract_id = r.contract_id,
                                contract_no = contract != null ? contract.contract_no : null,
                                contract_name = contract != null ? contract.name_cht : null,
                                project_id = contract != null ? contract.project_id : null,
                                project_no = project != null ? project.project_no : null,
                                project_name = project != null ? project.name_cht : null,
                                company_id = contract != null ? contract.company_id : null,
                                company_name_cht = contractCompany != null ? contractCompany.company_name : null,
                                company_name_eng = contractCompany != null ? contractCompany.company_name_eng : null,
                                site_id = r.site_id,
                                site_sho = site != null ? site.name_sho : null,
                                sub_site_id = r.sub_site_id,
                                sub_site_sho = subSite != null ? subSite.name_sho : null,
                                current_duty_id = r.current_duty_cp_id,
                                current_duty_name_cht = contact != null ? contact.name_cht : null,
                                current_duty_name_eng = contact != null ? contact.name_eng : null,
                                job_duties = r.job_duties,
                                shift_code = r.shift,
                                is_track = r.is_track.HasValue && r.is_track.Value == 1,
                                work_date = r.work_date
                            };

                var dto = await query.FirstOrDefaultAsync();
                if (dto == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }

                //var cpWorker = await _siteWorkerRepository.FindAsyncFirst(p => p.id == dto.current_duty_id);
                //dto.is_operate = !UserContext.Current.IsSuperAdmin &&
                //                 cpWorker.contact_id != UserContext.Current.UserInfo.Contact_Id
                //                 ? 0 : 1;
                var recordData = await _repository.FindAsyncFirst(p => p.id == id);
                if (recordData != null && string.IsNullOrEmpty(recordData.check_config))
                {
                    recordData.check_config = await SetRecordCcheckList();
                    _repository.Update(recordData);
                    await _repository.SaveChangesAsync();
                }
               
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), dto);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 编辑工地工作记录
        /// </summary>
        /// <param name="editInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditSiteWorkRecordAsync(EditSiteWorkRecordDto editInput)
        {
            try
            {
                var recordData = await _repository.FindFirstAsync(p => p.id == editInput.id);
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }

                //var result = await CheckIsOperate(recordData);
                //if (!result.status)
                //{
                //    return result;
                //}

                recordData.work_date = editInput.work_date;
                recordData.shift = editInput.shift;
                recordData.current_duty_cp_id = editInput.current_duty_id;
                recordData.site_id = editInput.site_id;

                recordData.modify_id = UserContext.Current.UserInfo.User_Id;
                recordData.modify_name = UserContext.Current.UserInfo.UserName;
                recordData.modify_date = DateTime.Now;

                _repository.Update(recordData);
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
        /// 删除工地工作记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteSiteWorkRecordByIdAsync(Guid id)
        {
            try
            {
                var recordData = await _repository.FindFirstAsync(p => p.id == id);
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }

                //var result = await CheckIsOperate(recordData);
                //if (!result.status)
                //{
                //    return result;
                //}

                recordData.delete_status = (int)SystemDataStatus.Invalid;
                recordData.modify_id = UserContext.Current.UserInfo.User_Id;
                recordData.modify_name = UserContext.Current.UserInfo.UserName;
                recordData.modify_date = DateTime.Now;

                _repository.Update(recordData);
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
        /// 设置工程进度完成百分比
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SetProjectProgressAsync(SetProgressDto input)
        {
            try
            {
                var recordData = await _repository.FindFirstAsync(p => p.id == input.id);
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }
                var data = await _rollingProgramRepository.FindFirstAsync(p => p.id == recordData.rolling_program_id);
                if (data == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }
                data.percentage = input.percentage;
                _rollingProgramRepository.Update(data);
                await _rollingProgramRepository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取工程进度完成百分比
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectProgressAsync(Guid recordId)
        {
            try
            {
                var recordData = await _repository.FindFirstAsync(p => p.id == recordId);
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }
                var data = await _rollingProgramRepository.FindFirstAsync(p => p.id == recordData.rolling_program_id);
                if (data == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data.percentage.HasValue ? data.percentage : 0.00);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 判断是否能够操作
        /// </summary>
        /// <param name="recordData"></param>
        /// <returns></returns>
        private async Task<WebResponseContent> CheckIsOperate(Biz_Site_Work_Record recordData)
        {
            var lstWorker = await _siteWorkerRepository
                   .FindAsIQueryable(p => p.record_id == recordData.id)
                   .AsNoTracking()
                   .ToListAsync();

            var cpWorker = lstWorker.FirstOrDefault(p => p.contact_id == recordData.current_duty_cp_id);
            if (cpWorker != null)
            {
                if (!UserContext.Current.IsSuperAdmin && cpWorker.contact_id != UserContext.Current.UserInfo.Contact_Id)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("record_operate_is_no_cp"));
                }
            }

            if (lstWorker.Any(p => p.time_out.HasValue || p.time_out_adj.HasValue))
            {
                return WebResponseContent.Instance.Error(_localizationService.GetString("record_no_edit_because_worker"));
            }

            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
        }

        /// <summary>
        /// 获取工地工作记录检查清单
        /// </summary>
        /// <returns></returns>
        private async Task<string> SetRecordCcheckList()
        {
            // 0:BRF（Briefing Record）
            // 1:SCR（Safety Working Cycle）
            // 2:CPD（CPDAS）
            // 3:QDC（Quality Checklist）
            // 4:CP（SIC CP(T)）
            // 5:SIC（SIC）
            
            // 清单内容
            var checkItemList = await _sysCheckItemRepository
                .FindAsIQueryable(p => p.enable == 1)
                .AsNoTracking()
                .Select(i => new { i.level, i.item_code, i.master_id, i.global_code, i.name_cht, i.name_eng, i.order_no })
                .ToListAsync();

            var lstBRF = checkItemList.Where(p => !string.IsNullOrEmpty(p.global_code) && p.global_code.StartsWith("B"))
                .Select(p => p.item_code).ToList();

            bool HasPrefix(string code, string[] setting) => !string.IsNullOrEmpty(code) && setting.Any(p => code.StartsWith(p));

            var scrSetting = new[] { "S101", "S102", "S103", "S104", "S105", "S106", "S107" };
            var lstSCR = checkItemList.Where(i => HasPrefix(i.global_code, scrSetting)).Select(p => p.item_code).ToList();

            var cpdPrefixes = new[] { "C101", "C102", "C103" };
            var lstCPD = checkItemList.Where(i => HasPrefix(i.global_code, cpdPrefixes)).Select(p => p.item_code).ToList();

            var qdcPrefixes = new[] { "C201", "C202", "C203", "C204", "C205" };
            var lstQDC = checkItemList.Where(i => HasPrefix(i.global_code, qdcPrefixes)).Select(p => p.item_code).ToList();

            var cpPrefixes = new[] { "SI101", "SI102", "SI103", "SI104" };
            var lstCP = checkItemList.Where(i => HasPrefix(i.global_code, cpPrefixes)).Select(p => p.item_code).ToList();

            var sicPrefixes = new[] { "SI201", "SI202", "SI203", "SI204" };
            var lstSIC = checkItemList.Where(i => HasPrefix(i.global_code, sicPrefixes)).Select(p => p.item_code).ToList();

            var data = new CheckCodeSetting
            {
                brf_setting = lstBRF,
                scr_setting = lstSCR,
                cpd_setting = lstCPD,
                qdc_setting = lstQDC,
                cp_setting = lstCP,
                sic_setting = lstSIC,
            };

            return JsonConvert.SerializeObject(data);
        }


        #region 图片

        /// <summary>
        /// 根据工地记录id获取图片列表
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetPhotoListByRecordAsync(Guid recordId)
        {
            try
            {
                var lstFileCode = new List<int>
                {
                    (int)SubmissionFileEnum.BeforeWork,
                    (int)SubmissionFileEnum.Working,
                    (int)SubmissionFileEnum.AfterWork,
                };

                var photoData = await _projectfilesRepository
                    .FindAsync(p => p.relation_id == recordId && 
                                    p.file_type.HasValue && 
                                    lstFileCode.Contains(p.file_type.Value) && 
                                    p.delete_status == (int)SystemDataStatus.Valid);


                var data = new SiteWorkRecordPhotoDto
                {
                    after_work = [.. photoData.Where(p => p.file_type == (int)SubmissionFileEnum.AfterWork).Select(p => new PhotoDto
                    {
                        id = p.id,
                        file_name = p.file_name,
                        //file_path = p.file_path.ToUrl(),
                        file_thumb_name = p.file_thumbnail_name,
                        //file_thumb_path = p.file_thumbnail_path.ToUrl(),
                        index = p.file_index.HasValue? p.file_index.Value : 0
                    }).OrderBy(p => p.index)],
                    before_work = [.. photoData.Where(p => p.file_type == (int)SubmissionFileEnum.BeforeWork).Select(p => new PhotoDto
                    {
                        id = p.id,
                        file_name = p.file_name,
                        //file_path = p.file_path.ToUrl(),
                        file_thumb_name = p.file_thumbnail_name,
                        //file_thumb_path = p.file_thumbnail_path.ToUrl(),
                        index = p.file_index.HasValue? p.file_index.Value : 0,
                    }).OrderBy(p => p.index)],
                    working = [.. photoData.Where(p => p.file_type == (int)SubmissionFileEnum.Working).Select(p => new PhotoDto
                    {
                        id = p.id,
                        file_name = p.file_name,
                        //file_path = p.file_path.ToUrl(),
                        file_thumb_name = p.file_thumbnail_name,
                        //file_thumb_path = p.file_thumbnail_path.ToUrl(),
                        index = p.file_index.HasValue? p.file_index.Value : 0,
                    }).OrderBy(p => p.index)],
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据工地记录和类型获取图片列表
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="photoType"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetPhotoListByTypeAsync(Guid recordId, int photoType)
        {
            try
            {
                var rows = await _projectfilesRepository
                    .FindAsIQueryable(p => p.relation_id == recordId && p.file_type == photoType && p.delete_status == (int)SystemDataStatus.Valid)
                    .Select(p => new { p.id, p.file_name, p.file_path, p.file_thumbnail_name, p.file_thumbnail_path, p.file_index })
                    .AsNoTracking()
                    .ToListAsync();

                var photoData = rows
                    .Select(p => new PhotoDto
                    {
                        id = p.id,
                        file_name = p.file_name,
                        //file_path = p.file_path.ToUrl(),
                        file_thumb_name = p.file_thumbnail_name,
                        //file_thumb_path = p.file_thumbnail_path.ToUrl(),
                        index = p.file_index.HasValue ? p.file_index.Value : 0,
                    })
                    .OrderBy(p => p.index)
                    .ToList();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), photoData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="photoType"></param>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadPhotoAsync(Guid recordId, int photoType, List<IFormFile> lstFiles)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }

                var isPic = _projectfilesService.CheckFileIsPhoto(lstFiles);
                if (!isPic)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("site_work_photo_type_error"));
                }

                var data = await _projectfilesRepository
                   .FindAsIQueryable(p => p.relation_id == recordId && p.file_type == photoType && p.delete_status == (int)SystemDataStatus.Valid)
                   .OrderByDescending(p => p.file_index)
                   .Select(p => new { p.id, p.file_name, p.file_path, p.file_thumbnail_name, p.file_thumbnail_path, p.file_index })
                   .AsNoTracking()
                   .FirstOrDefaultAsync();

                var index = data != null && data.file_index.HasValue ? data.file_index.Value + 1 : 0;
                var strPhotoType = SubmissionTypeEnumHelper.GetSubmissionFileStr(photoType);

                // 获取工地记录
                var recordData = await _repository.FindAsIQueryable(p => p.id == recordId).AsNoTracking().FirstOrDefaultAsync();
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }
                var ctrData = await _contractRepository.FindFirstAsync(p => p.id == recordData.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }
                var siteData = await _siteRepository.FindFirstAsync(p => p.id == recordData.site_id);


                // 处理文件夹信息
                var strPhotoFileType = SubmissionTypeEnumHelper.GetSubmissionFileStrType(photoType);
                var getFolderResult = await _projectfilesService.GetFileFolderByContactIdAsync(recordData.contract_id.Value, strPhotoFileType);
                if (!getFolderResult.status)
                {
                    return getFolderResult;
                }
                var strRelFolder = getFolderResult.data as string; // 相对路径
                if (siteData != null && !string.IsNullOrEmpty(siteData.name_sho))
                {
                    strRelFolder = Path.Combine(strRelFolder, siteData.name_sho);
                }
                if (recordData.work_date.HasValue)
                {
                    strRelFolder = Path.Combine(strRelFolder, recordData.work_date?.ToString("yyyy-MM-dd"));
                }
                var strAbsFolder = Path.Combine(AppSetting.FileSaveSettings.FolderPath, strRelFolder); // 绝对路径
                if (!Directory.Exists(strAbsFolder))
                {
                    Directory.CreateDirectory(strAbsFolder);
                }
                // 保存文件
                var lstPhotoData = new List<Biz_Project_Files>();
                foreach (var item in lstFiles)
                {
                    var strExt = Path.GetExtension(item.FileName).TrimStart('.').ToLower(); // 扩展名
                    var strFileName = $"{strPhotoType}_image_{index}.{strExt}";             // 文件名
                    var strFileRelPath = Path.Combine(strRelFolder, strFileName);           // 保存数据库中的相对路径
                    var strFileAbsPath = Path.Combine(strAbsFolder, strFileName);           // 文件绝对路径

                    // 如果文件存在则添加(+1)
                    if (File.Exists(strFileAbsPath))
                    {
                        strFileName = FileHelper.EnsureUniqueFileName(strAbsFolder, strFileName);
                        strFileRelPath = Path.Combine(strRelFolder, strFileName);           // 保存数据库中的相对路径
                        strFileAbsPath = Path.Combine(strAbsFolder, strFileName);           // 文件绝对路径
                    }

                    using (var stream = new FileStream(strFileAbsPath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    // 处理文件，之后转换成缩略图
                    if (!Directory.Exists(Path.Combine(strAbsFolder, $"thumbnail")))
                    {
                        Directory.CreateDirectory(strAbsFolder);
                    }

                    var strThumFileName = $"_{strPhotoType}_image_{index}.{strExt}";                       // 缩略图文件名
                    var strThumFileRelPath = Path.Combine(strRelFolder, $"thumbnail\\{strThumFileName}");  // 保存数据库中的缩略图相对路径
                    var strThumFileAbsPath = Path.Combine(strAbsFolder, $"thumbnail\\{strThumFileName}");  // 保存数据库中的缩略图绝对路径

                    if (File.Exists(strThumFileAbsPath))
                    {
                        strThumFileName = FileHelper.EnsureUniqueFileName($"{strAbsFolder}\\thumbnail", strThumFileName);
                        strThumFileRelPath = Path.Combine(strRelFolder, $"thumbnail\\{strThumFileName}");   // 保存数据库中的缩略图相对路径
                        strThumFileAbsPath = Path.Combine(strAbsFolder, $"thumbnail\\{strThumFileName}");   // 保存数据库中的缩略图绝对路径
                    }
                    // 进行缩略图
                    ImageHelper.CreateThumbnail(strFileAbsPath, strThumFileAbsPath, 256, 192);

                    lstPhotoData.Add(new Biz_Project_Files
                    {
                        id = Guid.NewGuid(),
                        create_id = UserContext.Current.UserId,
                        create_date = DateTime.Now,
                        create_name = UserContext.Current.UserName,
                        delete_status = (int)SystemDataStatus.Valid,

                        project_id = ctrData.project_id,
                        relation_id = recordData.id,
                        file_type = photoType,
                        file_index = index,
                        file_name = strFileName,
                        file_thumbnail_name = strThumFileName,
                        file_path = strFileRelPath,
                        file_thumbnail_path = strThumFileRelPath,
                        file_ext = strExt,
                        file_size = (int)item.Length,
                        upload_status = 1,
                        version = 1,
                        inner_status = 0,
                    });


                    index++;
                }


                _projectfilesRepository.AddRange(lstPhotoData);
                await _repository.SaveChangesAsync();

                var photoData = lstPhotoData
                 .Select(p => new PhotoDto
                 {
                     id = p.id,
                     file_name = p.file_name,
                     //file_path = p.file_path.ToUrl(),
                     file_thumb_name = p.file_thumbnail_name,
                     //file_thumb_path = p.file_thumbnail_path.ToUrl(),
                     index = p.file_index.HasValue ? p.file_index.Value : 0,
                 })
                 .OrderBy(p => p.index)
                 .ToList();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), photoData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region  -- 获取工程进度表表格数据 --

        public async Task<WebResponseContent> GetProjectProgressTableDataAsync(Guid contractId, Guid taskId)
        {
            try
            {
                // 查询符合条件的滚动计划记录
                var rollingPrograms = await _rollingProgramRepository
                    .WhereIF(true, rp => rp.contract_id == contractId && rp.task_id == taskId && rp.start_date != null && rp.end_date != null)
                    .ToListAsync();

                // 查询相关的工作记录
                var workRecordIds = rollingPrograms.Select(rp => rp.id).ToList();
                var siteWorkRecords = await _repository
                    .WhereIF(true, swr => workRecordIds.Contains((Guid)swr.rolling_program_id))
                    .ToListAsync();

                // 获取所有涉及的负责人ID
                var directorIds = rollingPrograms.Where(rp => rp.director != null).Select(rp => rp.director.Value).ToList();
                // 查询负责人用户信息
                var userInfos = new Dictionary<int, string>();
                if (directorIds.Any())
                {
                    var users = await _userRepository
                        .WhereIF(true, u => directorIds.Contains(u.user_id))
                        .Select(u => new { u.user_id, u.user_name })
                        .ToListAsync();

                    foreach (var user in users)
                    {
                        userInfos[user.user_id] = user.user_name;
                    }
                }

                // 生成结果列表
                var result = new List<object>();
                foreach (var program in rollingPrograms)
                {
                    var programRecords = siteWorkRecords.Where(swr => swr.rolling_program_id == program.id);
                    var startDate = program.start_date ?? DateTime.MinValue;
                    var endDate = program.end_date ?? DateTime.MaxValue;

                    // 计算日期范围内的天数
                    int days = (endDate - startDate).Days + 1;

                    for (int i = 0; i < days; i++)
                    {
                        DateTime currentDate = startDate.AddDays(i);
                        string directorName = program.director.HasValue && userInfos.ContainsKey(program.director.Value)
                            ? userInfos[program.director.Value]
                            : string.Empty;

                        // 查找当天的工作记录
                        var dayRecord = programRecords.FirstOrDefault(swr => swr.work_date == currentDate.Date);

                        result.Add(new
                        {
                            id = dayRecord?.id,
                            title = program.content,
                            start = currentDate.ToString("yyyy-MM-dd"),
                            end = currentDate.ToString("yyyy-MM-dd"),
                            backgroundColor = "#378006",
                            extendedProps = new
                            {
                                record_id = dayRecord?.id,
                                work_date = currentDate.ToString("yyyy-MM-dd"),
                                shift = dayRecord?.shift,
                                rolling_program_id = program.id,
                                rolling_program_content = program.content,
                                percentage = program.percentage,
                                director_name = directorName
                            }
                        });
                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_RecordService.GetProjectProgressTableDataAsync", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取滚动计划数据，返回指定格式的计划列表
        /// </summary>
        /// <param name="contractId">合约ID</param>
        /// <param name="taskId">任务ID</param>
        /// <returns>计划列表数据</returns>
        public async Task<WebResponseContent> GetRollingProgramScheduleDataAsync(Guid contractId, Guid taskId)
        {
            try
            {
                // 查询符合条件的滚动计划记录
                var rollingPrograms = await _rollingProgramRepository
                    .WhereIF(true, rp => rp.contract_id == contractId && rp.task_id == taskId && rp.start_date != null && rp.end_date != null)
                    .ToListAsync();

                if (!rollingPrograms.Any())
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), new List<object>());
                }

                // 获取所有涉及的负责人ID
                var directorIds = rollingPrograms.Where(rp => rp.director.HasValue).Select(rp => rp.director.Value).Distinct().ToList();

                // 查询负责人用户信息
                var userInfos = new Dictionary<int, string>();
                if (directorIds.Any())
                {
                    var users = await _userRepository
                        .WhereIF(true, u => directorIds.Contains(u.user_id))
                        .Select(u => new { u.user_id, u.user_name })
                        .ToListAsync();

                    foreach (var user in users)
                    {
                        userInfos[user.user_id] = user.user_name;
                    }
                }

                var site = await _siteRepository
                    .WhereIF(true, t => t.id == taskId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                // 生成结果列表
                var result = new List<object>();
                foreach (var program in rollingPrograms)
                {
                    // 获取负责人姓名
                    string directorName = program.director.HasValue && userInfos.ContainsKey(program.director.Value)
                        ? userInfos[program.director.Value]
                        : string.Empty;

                    // 计算天数
                    int dayCount = 0;
                    if (program.start_date.HasValue && program.end_date.HasValue)
                    {
                        dayCount = (program.end_date.Value - program.start_date.Value).Days + 1;
                    }

                    // 获取百分比显示文本
                    string percentageLabel = program.percentage.HasValue
                        ? $"{program.percentage.Value}%"
                        : "0%";

                    // 添加结果项
                    result.Add(new
                    {
                        id = program.id,
                        rowBg = program.color ?? "#FF851B", // 使用默认颜色如果没有指定
                        name = directorName,
                        label = percentageLabel,
                        todoItems = "0", // 无效字段，按要求设置为0
                        startDateAndEndDate = new
                        {
                            startDate = program.start_date?.ToString("yyyy-MM-dd HH:mm") ?? "",
                            endDate = program.end_date?.ToString("yyyy-MM-dd HH:mm") ?? ""
                        },
                        dayCount = dayCount.ToString(),
                        assignee = directorName,
                        location = site?.name_sho ?? ""
                    });
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_RecordService.GetRollingProgramScheduleDataAsync", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        #endregion
    }
}
