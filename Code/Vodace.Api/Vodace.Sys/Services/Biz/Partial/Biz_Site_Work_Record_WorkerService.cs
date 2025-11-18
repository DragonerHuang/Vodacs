
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Vodace.Sys.Services
{
    public partial class Biz_Site_Work_Record_WorkerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Site_Work_Record_WorkerRepository _repository;//访问数据库

        private readonly IBiz_ContractRepository _contractRepository;                         // 合同仓储
        private readonly IBiz_ProjectRepository _projectRepository;                           // 项目仓储
        private readonly IBiz_SiteRepository _siteRepository;                                 // 工地仓储
        private readonly ISys_ContactRepository _contactRepository;                           // 联系人仓储
        private readonly ISys_CompanyRepository _companyRepository;                           // 公司仓储
        private readonly IBiz_Site_Work_RecordRepository _siteWorkRecordRepository;           // 工地记录仓储
        private readonly ISys_Work_TypeRepository _sysWorkTypeRepository;                     // 工种仓储
        private readonly IBiz_Site_Work_Record_SignRepository _siteWorkRecordSignRepository;  // 工地签名仓储
        private readonly ISys_User_NewRepository _userRepository;                             // 用户仓储
        private readonly ISys_Worker_RegisterRepository _workerRegisterRepository;            // 工人注册信息仓储
        private readonly ISys_Work_TypeRepository _workTypeRepository;                        // 工种仓储
        private readonly ISys_User_RelationRepository _userRelationRepository;                // 用户工种关系仓储

        private readonly ILocalizationService _localizationService;                           // 国际化服务
        private readonly IMapper _mapper;                                                     // atuomapper映射服务

        [ActivatorUtilitiesConstructor]
        public Biz_Site_Work_Record_WorkerService(
            IBiz_Site_Work_Record_WorkerRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IBiz_ContractRepository contractRepository,
            IBiz_ProjectRepository projectRepository,
            IBiz_SiteRepository siteRepository,
            ISys_ContactRepository contactRepository,
            ISys_CompanyRepository companyRepository,
            IBiz_Site_Work_RecordRepository siteWorkRecordRepository,
            ISys_Work_TypeRepository sysWorkTypeRepository,
            IBiz_Site_Work_Record_SignRepository siteWorkRecordSignRepository,
            IMapper mapper,
            ISys_Worker_RegisterRepository workerRegisterRepository,
            ISys_User_NewRepository userRepository,
            ISys_Work_TypeRepository workTypeRepository,
            ISys_User_RelationRepository userRelationRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _contractRepository = contractRepository;
            _projectRepository = projectRepository;
            _siteRepository = siteRepository;
            _contactRepository = contactRepository;
            _companyRepository = companyRepository;
            _siteWorkRecordRepository = siteWorkRecordRepository;
            _sysWorkTypeRepository = sysWorkTypeRepository;

            _siteWorkRecordSignRepository = siteWorkRecordSignRepository;
            _mapper = mapper;
            _workerRegisterRepository = workerRegisterRepository;
            _userRepository = userRepository;
            _workTypeRepository = workTypeRepository;
            _userRelationRepository = userRelationRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 根据工地记录获取工人列表（分页）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetWorkerListPageAsync(PageInput<SiteWorkerSearchDto> search)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;

                var workers = _repository
                    .FindAsIQueryable(w => w.delete_status == valid)
                    .AsNoTracking();


                var records = _siteWorkRecordRepository
                    .FindAsIQueryable(r => r.delete_status == valid)
                    .Select(r => new { r.id })
                    .AsNoTracking();

                var contacts = _contactRepository
                    .FindAsIQueryable(ct => ct.delete_status == valid)
                    .Select(ct => new { ct.id, ct.name_cht, ct.name_eng })
                    .AsNoTracking();

                var workTypes = _sysWorkTypeRepository
                    .FindAsIQueryable(t => t.delete_status == valid)
                    .Select(t => new { t.id, t.type_name })
                    .AsNoTracking();

                var companies = _companyRepository
                    .FindAsIQueryable(co => co.delete_status == valid)
                    .Select(co => new { co.id, co.company_name, co.company_name_eng })
                    .AsNoTracking();

                var query = from w in workers
                            join r in records on w.record_id equals r.id into wr
                            from rec in wr.DefaultIfEmpty()
                            join ct in contacts on w.contact_id equals ct.id into wc
                            from contact in wc.DefaultIfEmpty()
                            join wt in workTypes on w.work_type_id equals wt.id into wtw
                            from workType in wtw.DefaultIfEmpty()
                            join co in companies on w.company_id equals co.id into wco
                            from company in wco.DefaultIfEmpty()
                            select new { w, contact, workType, company };

                // 过滤条件
                if (search?.search != null && search.search.record_id.HasValue)
                {
                    var rid = search.search.record_id.Value;
                    query = query.Where(x => x.w.record_id == rid);
                }

                // 构造列表数据（投影）
                var lstData = query.Select(x => new SiteWorkerDto
                {
                    id = x.w.id,
                    record_id = x.w.record_id,
                    contact_id = x.w.contact_id,
                    contact_name_cht = x.contact != null ? x.contact.name_cht : null,
                    contact_name_eng = x.contact != null ? x.contact.name_eng : null,
                    green_card_no = x.w.green_card_no,
                    green_card_exp = x.w.green_card_exp,
                    is_valid = x.w.is_valid.HasValue && x.w.is_valid.Value == 1,
                    work_type_id = x.w.work_type_id,
                    work_type = x.workType != null ? x.workType.type_name : null,
                    is_wpic = x.w.is_wpic.HasValue && x.w.is_wpic.Value == 1,
                    is_cp = x.w.is_cp.HasValue && x.w.is_cp.Value == 1,
                    is_nt = x.w.is_nt.HasValue && x.w.is_nt.Value == 1,
                    work_remark = x.w.work_remark,
                    company_id = x.w.company_id,
                    company_name_cht = x.company != null ? x.company.company_name : null,
                    company_name_eng = x.company != null ? x.company.company_name_eng : null,
                    is_sick = x.w.is_sick.HasValue && x.w.is_sick.Value == 1,
                    is_sign = x.w.is_sign.HasValue && x.w.is_sign.Value == 1,
                    //sign_image = x.w.sign_image,
                });

                // 统一分页：固定按创建时间降序
                search.sort_field = "create_date";
                search.sort_type = "desc";
                var result = await lstData.GetPageResultAsync(search);

                // 根据工人id与record_id获取签名的模糊图片，读取为字节存到sign_image
                if (result?.data != null && result.data.Count > 0 && search.search.is_show_image)
                {
                    var validStatus = (int)SystemDataStatus.Valid;
                    var workerIds = result.data.Select(d => d.id).ToList();

                    var signQuery = _siteWorkRecordSignRepository
                        .FindAsIQueryable(s => s.delete_status == validStatus
                                              && s.relation_type == 0
                                              && s.relation_id.HasValue
                                              && workerIds.Contains(s.relation_id.Value))
                        .Select(s => new { s.relation_id, s.record_id, s.file_blurry_path })
                        .AsNoTracking();

                    var signs = await signQuery.ToListAsync();
                    var baseFolder = AppSetting.FileSaveSettings?.FolderPath ?? string.Empty;

                    foreach (var w in result.data)
                    {
                        var sign = signs.FirstOrDefault(s => s.relation_id == w.id && s.record_id == w.record_id);
                        var relPath = sign?.file_blurry_path;
                        if (!string.IsNullOrEmpty(relPath))
                        {
                            // 修正：如果相对路径以路径分隔符开头，Path.Combine会忽略baseFolder
                            // 先去掉前导分隔符，再进行拼接
                            var relPathFixed = relPath.TrimStart('\\', '/');
                            var fullPath = Path.Combine(baseFolder, relPathFixed).ReplacePath();
                            try
                            {
                                if (File.Exists(fullPath))
                                {
                                    w.sign_image = await File.ReadAllBytesAsync(fullPath);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据工地记录获取工人出勤列表（分页）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCICAttendancePageAsync(PageInput<SiteWorkerSearchDto> search)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;

                var workers = _repository
                    .FindAsIQueryable(w => w.delete_status == valid)
                    .AsNoTracking();

                var records = _siteWorkRecordRepository
                    .FindAsIQueryable(r => r.delete_status == valid)
                    .Select(r => new { r.id })
                    .AsNoTracking();

                var contacts = _contactRepository
                    .FindAsIQueryable(ct => ct.delete_status == valid)
                    .Select(ct => new { ct.id, ct.name_cht, ct.name_eng })
                    .AsNoTracking();

                var query = from w in workers
                            join r in records on w.record_id equals r.id into wr
                            from rec in wr.DefaultIfEmpty()
                            join ct in contacts on w.contact_id equals ct.id into wc
                            from contact in wc.DefaultIfEmpty()
                            select new { w, contact };

                // 过滤条件
                if (search?.search != null && search.search.record_id.HasValue)
                {
                    var rid = search.search.record_id.Value;
                    query = query.Where(x => x.w.record_id == rid);
                }

                // 构造列表数据（投影），上/下班时间优先展示调整后的时间
                var lstData = query.Select(x => new CICAttendanceDto
                {
                    id = x.w.id,
                    record_id = x.w.record_id,
                    contact_id = x.w.contact_id,
                    contact_name_cht = x.contact != null ? x.contact.name_cht : null,
                    contact_name_eng = x.contact != null ? x.contact.name_eng : null,
                    cic_no = x.w.cic_no,
                    cic_card_no = x.w.cic_card_no,
                    time_in = x.w.time_in_adj ?? x.w.time_in,
                    time_out = x.w.time_out_adj ?? x.w.time_out,
                });

                // 统一分页：固定按创建时间降序
                search.sort_field = "create_date";
                search.sort_type = "desc";
                var result = await lstData.GetPageResultAsync(search);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #region -- 出勤与工人资料 --

        /// <summary>
        /// 完成CIC记录（总的，联系人的）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSiteAttendanceRecords(PageInput<SiteAttendanceRecordDto> dto)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;

                // 获取各表数据
                var workers = _repository
                    .FindAsIQueryable(w => w.delete_status == valid)
                    .AsNoTracking();

                var siteWorkRecords = _siteWorkRecordRepository
                    .FindAsIQueryable(r => r.delete_status == valid)
                    .AsNoTracking();

                var contracts = _contractRepository
                    .FindAsIQueryable(c => c.delete_status == valid)
                    .AsNoTracking();

                var companies = _companyRepository
                    .FindAsIQueryable(co => co.delete_status == valid)
                    .AsNoTracking();

                var contacts = _contactRepository
                    .FindAsIQueryable(ct => ct.delete_status == valid)
                    .AsNoTracking();

                var workTypes = _sysWorkTypeRepository
                    .FindAsIQueryable(wt => wt.delete_status == valid)
                    .AsNoTracking();

                var users = _userRepository
                    .FindAsIQueryable(u => u.delete_status == valid)
                    .AsNoTracking();

                var workerRegisters = _workerRegisterRepository
                    .FindAsIQueryable(wr => wr.delete_status == valid)
                    .AsNoTracking();

                var sites = _siteRepository
                    .FindAsIQueryable(s => s.delete_status == valid)
                    .AsNoTracking();

                // 构建Linq查询，实现多表连接
                var query = from w in workers
                            join swr in siteWorkRecords on w.record_id equals swr.id into wswr
                            from siteWorkRecord in wswr.DefaultIfEmpty()
                            join c in contracts on siteWorkRecord.contract_id equals c.id into wswrc
                            from contract in wswrc.DefaultIfEmpty()
                            join co in companies on contract.company_id equals co.id into wswrcco
                            from company in wswrcco.DefaultIfEmpty()
                            join ct in contacts on w.contact_id equals ct.id into wct
                            from contact in wct.DefaultIfEmpty()
                            join wt in workTypes on w.work_type_id equals wt.id into wwt
                            from workType in wwt.DefaultIfEmpty()
                            join un in users on contact.id equals un.contact_id into ctun
                            from user in ctun.DefaultIfEmpty()
                            join wr in workerRegisters on user.user_register_id equals wr.id into unwr
                            from workerRegister in unwr.DefaultIfEmpty()
                            join s in sites on siteWorkRecord.site_id equals s.id into swrs
                            from site in swrs.DefaultIfEmpty()
                            select new { w, siteWorkRecord, contract, company, contact, workType, user, workerRegister, site };

                // 应用搜索条件
                var search = dto.search;
                if (search != null)
                {
                    if (search.contract_id.HasValue)
                    {
                        query = query.Where(x =>
                            (x.contract != null && (x.contract.id == search.contract_id))
                        );
                    }

                    if (search.contact_id.HasValue)
                    {
                        query = query.Where(x =>
                            (x.w != null && (x.w.contact_id == search.contact_id))
                        );
                    }

                    if (!string.IsNullOrEmpty(search.contact_name))
                    {
                        query = query.Where(x =>
                            (x.contact != null && (x.contact.name_cht.Contains(search.contact_name) || x.contact.name_eng.Contains(search.contact_name)))
                        );
                    }

                    if (!string.IsNullOrEmpty(search.company_name))
                    {
                        query = query.Where(x =>
                            (x.company != null && (x.company.company_name.Contains(search.company_name) || x.company.company_name_eng.Contains(search.company_name)))
                        );
                    }

                    if (search.work_date_start.HasValue)
                    {
                        //query = query.Where(x =>
                        //    (x.w.time_in_adj ?? x.w.time_in) >= search.work_date_start.Value
                        //);

                        query = query.Where(x =>
                            x.siteWorkRecord.work_date >= search.work_date_start.Value
                        );
                    }

                    if (search.work_date_end.HasValue)
                    {
                        //query = query.Where(x =>
                        //    (x.w.time_out_adj ?? x.w.time_out) <= search.work_date_end.Value
                        //);

                        query = query.Where(x =>
                            x.siteWorkRecord.work_date <= search.work_date_end.Value
                        );
                    }

                    if (!string.IsNullOrEmpty(search.site_name))
                    {
                        query = query.Where(x =>
                            (x.site != null && (x.site.name_cht.Contains(search.site_name) || x.site.name_eng.Contains(search.site_name)))
                        );
                    }

                    if(search.site_ids != null && search.site_ids.Count > 0)
                    {
                        query = query.Where(x => search.site_ids.Contains(x.site.id));
                    }
                }

                // 统一分页设置
                if (string.IsNullOrEmpty(dto.sort_field))
                {
                    dto.sort_field = "work_date";
                    dto.sort_type = "desc";
                }
                else query = query.OrderByDescending(x => x.w.create_date);

                var sql = query.ToQueryString();

                // 构造结果数据
                var lstData = query.Select(x => new
                {
                    contact_id = x.w.contact_id,            //联系人id
                    contact_name_cht = x.contact.name_cht,  //联系人中文名
                    contact_name_eng = x.contact.name_eng,  //联系人英文名
                    record_id = x.w.record_id,              //出勤id
                    contract_no = x.contract.contract_no,   //合约编码
                    company_name = x.company.company_name,  //公司名称
                    company_name_eng = x.company.company_name_eng,//公司英文名
                    wrc_no = x.workerRegister.wrc_no,       //工人注册证
                    month = x.siteWorkRecord != null ? x.siteWorkRecord.work_date.ToString("yyyy-MM") : null,                   //月份
                    work_date = x.siteWorkRecord.work_date, //开工日期
                    attend_date = x.siteWorkRecord != null ? (x.w.time_in_adj ?? x.w.time_in).ToString("yyyy-MM-dd") : null,    //出席日期
                    time_in = (x.w.time_in_adj ?? x.w.time_in), //上班时间
                    time_out = (x.w.time_out_adj ?? x.w.time_out),//下班时间
                    site_name_cht = x.site.name_cht,        //工地中文名
                    site_name_eng = x.site.name_eng,        //工地英文名
                    work_type_name = x.workType.type_name,  //工种
                    base_salary = x.w.salary_adj.HasValue? x.w.salary_adj : x.w.base_salary,//基本薪资
                    traffic_allowance = x.w.traffic_allowance,//交通补贴
                    salary_adj = (x.w.base_salary ?? x.w.salary_adj ?? 0) + (x.w.traffic_allowance ?? 0),
                    //salary_adj = SumSalary(x.w.base_salary, x.w.salary_adj, x.w.traffic_allowance),//总薪资
                    training_attendance = x.siteWorkRecord != null && x.siteWorkRecord.finish_briefing == 1 ? "已开入职纸" : string.Empty,   //培训出席记录
                    remark = x.w.work_remark                     //备注
                });

                var result = await lstData.GetPageResultAsync(dto);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_Record_WorkerService.GetSiteAttendanceRecords", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> GetContactAttendanceActualSalary(PageInput<SiteAttendanceRecordDto> dto)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;

                // 首先获取所有需要的数据到内存中
                var workerQuery = from w in _repository.FindAsIQueryable(w => w.delete_status == valid).AsNoTracking()
                                  join swr in _siteWorkRecordRepository.FindAsIQueryable(r => r.delete_status == valid).AsNoTracking()
                                  on w.record_id equals swr.id into workerSiteWorkRecords
                                  from siteWorkRecord in workerSiteWorkRecords.DefaultIfEmpty()
                                  join ct in _contactRepository.FindAsIQueryable(ct => ct.delete_status == valid).AsNoTracking()
                                  on w.contact_id equals ct.id into workerContacts
                                  from contact in workerContacts.DefaultIfEmpty()
                                  join wt in _sysWorkTypeRepository.FindAsIQueryable(wt => wt.delete_status == valid).AsNoTracking()
                                  on w.work_type_id equals wt.id into workerWorkTypes
                                  from workType in workerWorkTypes.DefaultIfEmpty()
                                  join un in _userRepository.FindAsIQueryable(un => un.delete_status == valid).AsNoTracking()
                                  on w.contact_id equals un.contact_id into workerUsers
                                  from userNew in workerUsers.DefaultIfEmpty()
                                  join wr in _workerRegisterRepository.FindAsIQueryable(wr => wr.delete_status == valid).AsNoTracking()
                                  on userNew.user_register_id equals wr.id into workerRegisters
                                  from workerRegister in workerRegisters.DefaultIfEmpty()
                                  where siteWorkRecord != null && contact != null && workType != null
                                  select new
                                  {
                                      w.contact_id,
                                      w.base_salary,
                                      siteWorkRecord.work_date,
                                      siteWorkRecord.site_id,
                                      contact.name_cht,
                                      contact.name_eng,
                                      workerRegister.wrc_no,
                                      workType.type_name,
                                      siteWorkRecord.shift,
                                      workType.day_salary,
                                      workType.night_salary
                                  };

                // 应用搜索条件（如果需要）
                var search = dto.search;
                if (search != null)
                {
                    if (search.contact_id.HasValue)
                    {
                        workerQuery = workerQuery.Where(x => x.contact_id == search.contact_id);
                    }

                    if (!string.IsNullOrEmpty(search.contact_name))
                    {
                        workerQuery = workerQuery.Where(x =>
                            x.name_cht.Contains(search.contact_name) ||
                            x.name_eng.Contains(search.contact_name));
                    }

                    if (search.work_date_start.HasValue)
                    {
                        workerQuery = workerQuery.Where(x => x.work_date >= search.work_date_start.Value);
                    }

                    if (search.work_date_end.HasValue)
                    {
                        workerQuery = workerQuery.Where(x => x.work_date <= search.work_date_end.Value);
                    }

                    if (search.site_ids != null && search.site_ids.Count > 0)
                    {
                        workerQuery = workerQuery.Where(x => search.site_ids.Contains((Guid)x.site_id));
                    }
                }

                // 执行查询并加载到内存中
                var workerData = await workerQuery.ToListAsync();

                // 统一分页设置
                if (string.IsNullOrEmpty(dto.sort_field))
                {
                    dto.sort_field = "month";
                    dto.sort_type = "desc";
                }

                // 在内存中进行分组和格式化处理
                var groupedData = workerData
                    .GroupBy(x => new
                    {
                        Month = x.work_date.ToString("yyyy-MM"),
                        x.contact_id,
                        x.base_salary,
                        x.name_cht,
                        x.name_eng,
                        x.wrc_no,
                        x.type_name,
                        x.shift,
                        x.day_salary,
                        x.night_salary
                    }, x => x)
                    .Select(g => new
                    {
                        month = g.Key.Month,
                        totalDays = g.Count(),
                        name_cht = g.Key.name_cht,
                        name_eng = g.Key.name_eng,
                        wrc_no = g.Key.wrc_no,
                        type_name = g.Key.type_name,
                        shift = g.Key.shift,
                        base_salary = (g.Key.base_salary.HasValue && g.Key.base_salary.Value > 0) ?
                            (g.Key.base_salary.Value * g.Count()) :
                            (g.Key.shift == "Night Shift" ?
                                (g.Key.night_salary * g.Count()) :
                                (g.Key.day_salary * g.Count())),
                        day_salary = g.Key.day_salary,
                        night_salary = g.Key.night_salary
                    });

                // 排序处理
                if (dto.sort_type.ToLower() == "desc")
                {
                    groupedData = groupedData.OrderByDescending(x => x.month);
                }
                else
                {
                    groupedData = groupedData.OrderBy(x => x.month);
                }

                // 执行分页查询
                var result = groupedData.GetPageResult(dto);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_Record_WorkerService.GetContactAttendanceInfo", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> GetContactAttendanceSalaryCalculationStandard(PageInput<SiteAttendanceRecordDto> dto)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;

                // 获取各表数据
                var workers = _repository
                    .FindAsIQueryable(w => w.delete_status == valid)
                    .AsNoTracking();

                var siteWorkRecords = _siteWorkRecordRepository
                    .FindAsIQueryable(r => r.delete_status == valid)
                    .AsNoTracking();

                var contacts = _contactRepository
                    .FindAsIQueryable(ct => ct.delete_status == valid)
                    .AsNoTracking();

                var workTypes = _sysWorkTypeRepository
                    .FindAsIQueryable(wt => wt.delete_status == valid)
                    .AsNoTracking();

                var users = _userRepository
                    .FindAsIQueryable(u => u.delete_status == valid)
                    .AsNoTracking();

                var workerRegisters = _workerRegisterRepository
                    .FindAsIQueryable(wr => wr.delete_status == valid)
                    .AsNoTracking();

                // 构建Linq查询，实现多表连接
                var query = from w in workers
                            join swr in siteWorkRecords on w.record_id equals swr.id into wswr
                            from siteWorkRecord in wswr.DefaultIfEmpty()
                            join c in contacts on w.contact_id equals c.id into wct
                            from contact in wct.DefaultIfEmpty()
                            join wt in workTypes on w.work_type_id equals wt.id into wwt
                            from workType in wwt.DefaultIfEmpty()
                            join un in users on w.contact_id equals un.contact_id into ctun
                            from user in ctun.DefaultIfEmpty()
                            join wr in workerRegisters on user.user_register_id equals wr.id into unwr
                            from workerRegister in unwr.DefaultIfEmpty()
                            select new { contact, workerRegister, workType, siteWorkRecord };

                // 应用搜索条件
                var search = dto.search;
                if (search != null)
                {
                    if (search.contact_id.HasValue)
                    {
                        query = query.Where(x => x.contact != null && x.contact.id == search.contact_id.Value);
                    }

                    if (!string.IsNullOrEmpty(search.contact_name))
                    {
                        query = query.Where(x =>
                            x.contact != null && (x.contact.name_cht.Contains(search.contact_name) || x.contact.name_eng.Contains(search.contact_name))
                        );
                    }

                    if (search.work_date_start != null)
                    {
                        query = query.Where(x => x.siteWorkRecord.work_date >= search.work_date_start.Value);
                    }

                    if (search.work_date_end != null)
                    {

                        query = query.Where(x => x.siteWorkRecord.work_date <= search.work_date_end.Value);
                    }

                    if (search.site_ids != null && search.site_ids.Count > 0)
                    {
                        query = query.Where(x => search.site_ids.Contains((Guid)x.siteWorkRecord.site_id));
                    }

                    // 可以根据需要添加更多的搜索条件
                }

                // 分组并投影结果
                var groupedQuery = query
                    .GroupBy(x => new
                    {
                        NameCht = x.contact.name_cht,
                        NameEng = x.contact.name_eng,
                        WrcNo = x.workerRegister.wrc_no,
                        TypeId = x.workType.id.ToString(),
                        TypeName = x.workType.type_name,
                        DaySalary = x.workType.day_salary,
                        NightSalary = x.workType.night_salary
                    })
                    .Select(g => new
                    {
                        name_cht = g.Key.NameCht,
                        name_eng = g.Key.NameEng,
                        wrc_no = g.Key.WrcNo,
                        type_id = g.Key.TypeId,
                        type_name = g.Key.TypeName,
                        day_salary = g.Key.DaySalary,
                        night_salary = g.Key.NightSalary
                    })
                    .OrderBy(x => x.type_name);

                // 处理分页
                var result = await groupedQuery.GetPageResultAsync(dto);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_Record_WorkerService.GetContactAttendanceInfo", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> GetSiteAttendanceTotal(PageInput<SiteAttendanceRecordDto> dto)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;

                // 构建主查询
                var query = from record in _siteWorkRecordRepository.FindAsIQueryable(r => r.delete_status == valid).AsNoTracking()
                            select record;

                // 应用搜索条件
                if (dto.search != null)
                {
                    // contract_id 过滤
                    if (dto.search.contract_id.HasValue)
                    {
                        query = query.Where(r => r.contract_id == dto.search.contract_id.Value);
                    }

                    // 联系人名称搜索过滤（对应子查询）
                    if (!string.IsNullOrEmpty(dto.search.contact_name))
                    {
                        var contactQuery = _contactRepository.FindAsIQueryable(c => c.delete_status == valid &&
                            (c.name_eng.Contains(dto.search.contact_name) || c.name_cht.Contains(dto.search.contact_name)));
                        var contactIds = contactQuery.Select(c => c.id).ToList();
                        if (contactIds.Any())
                        {
                            query = query.Where(r => contactIds.Contains(r.id));
                        }
                    }

                    // site_id 过滤
                    if (dto.search.site_id.HasValue)
                    {
                        query = query.Where(r => r.site_id == dto.search.site_id.Value);
                    }

                    // 工地名称搜索过滤（对应子查询）
                    if (!string.IsNullOrEmpty(dto.search.site_name))
                    {
                        var siteQuery = _siteRepository.FindAsIQueryable(s => s.delete_status == valid &&
                            (s.name_chs.Contains(dto.search.site_name) ||
                             s.name_eng.Contains(dto.search.site_name) ||
                             s.name_sho.Contains(dto.search.site_name)));
                        var siteIds = siteQuery.Select(s => s.id).ToList();
                        if (siteIds.Any())
                        {
                            query = query.Where(r => siteIds.Contains((Guid)r.site_id));
                        }
                    }

                    if (dto.search.site_ids != null && dto.search.site_ids.Count > 0)
                    {
                        query = query.Where(r => dto.search.site_ids.Contains((Guid)r.site_id));
                    }

                    // 工作日期范围过滤
                    if (dto.search.work_date_start.HasValue)
                    {
                        query = query.Where(r => r.work_date >= dto.search.work_date_start.Value);
                    }
                    if (dto.search.work_date_end.HasValue)
                    {
                        query = query.Where(r => r.work_date <= dto.search.work_date_end.Value);
                    }
                }

                // 设置默认排序
                query = query.OrderByDescending(r => r.work_date);

                // 分页处理
                var pageResult = await query.GetPageResultAsync(dto);

                // 获取所有记录数据
                var result = await query.GetPageResultAsync(dto);

                // 获取所有月份并按降序排序
                var months = result.data.Select(a => a.work_date.ToString("yyyy-MM")).Distinct().OrderByDescending(m => m).ToList();

                // 构建标题数组
                var title = new List<string> { "name" };
                title.AddRange(months);
                title.Add("total");

                // 获取所有相关数据
                // 获取合同信息
                var lstContract = _contractRepository
                    .WhereIF(true, a => a.delete_status == valid)
                    .WhereIF(dto.search.contract_id.HasValue, a => a.id == dto.search.contract_id || (!string.IsNullOrEmpty(a.vo_wo_type) && a.master_id == dto.search.contract_id))
                    .Select(a => new { a.id, a.contract_no, a.create_date })
                    .OrderByDescending(a => a.create_date)
                    .ToList();

                // 获取站点信息
                var lstSite = _siteRepository.Find(a => a.delete_status == valid).Select(a => new { a.id, a.name_cht, a.name_eng, a.name_sho }).ToList();

                // 获取联系人信息
                var lstContact = _contactRepository.Find(a => a.delete_status == valid).Select(a => new { a.id, a.name_cht, a.name_eng }).ToList();

                // 获取工作记录工人信息
                var lstWorkRe = _repository.Find(a => a.delete_status == valid && (a.time_out != null || a.time_out_adj != null))
                    .Select(a => new { a.id, a.record_id, a.contact_id }).ToList();

                // 构建合同->站点->工人的层级结构
                var contractItems = new List<dynamic>();

                foreach (var contract in lstContract)
                {
                    // 获取该合同下的所有工作记录
                    var contractRecords = result.data.Where(r => r.contract_id == contract.id).ToList();
                    if (!contractRecords.Any()) continue;

                    // 构建站点集合
                    var siteItems = new List<dynamic>();
                    var contractMonthTotals = new Dictionary<string, int>();

                    // 初始化合同月份计数
                    foreach (var month in months)
                    {
                        contractMonthTotals[month] = 0;
                    }

                    int contractTotal = 0;

                    // 按站点分组处理
                    var siteGroups = contractRecords.GroupBy(r => r.site_id);
                    foreach (var siteGroup in siteGroups)
                    {
                        var siteId = siteGroup.Key;
                        var siteInfo = lstSite.FirstOrDefault(s => s.id == siteId);
                        if (siteInfo == null) continue;

                        // 获取该站点下的所有记录ID
                        var siteRecordIds = siteGroup.Select(r => r.id).ToList();

                        // 获取该站点下的所有工人记录
                        var siteWorkerRecords = lstWorkRe.Where(w => siteRecordIds.Contains((Guid)w.record_id)).ToList();

                        // 构建工人集合
                        var workerItems = new List<dynamic>();
                        var siteMonthTotals = new Dictionary<string, int>();

                        // 初始化站点月份计数
                        foreach (var month in months)
                        {
                            siteMonthTotals[month] = 0;
                        }

                        int siteTotal = 0;

                        // 按工人分组处理
                        var workerGroups = siteWorkerRecords.GroupBy(w => w.contact_id);
                        foreach (var workerGroup in workerGroups)
                        {
                            var contactId = workerGroup.Key;
                            var contactInfo = lstContact.FirstOrDefault(c => c.id == contactId);
                            if (contactInfo == null) continue;

                            // 创建工人数据对象
                            var workerObj = new Dictionary<string, object>();
                            workerObj["name"] = contactInfo.name_cht;

                            var workerMonthTotals = new Dictionary<string, int>();
                            int workerTotal = 0;

                            // 计算每个月份的工人出勤次数
                            foreach (var month in months)
                            {
                                // 获取该月份下该工人的出勤记录数
                                var monthRecordIds = siteGroup
                                    .Where(r => r.work_date.ToString("yyyy-MM") == month)
                                    .Select(r => r.id)
                                    .ToList();

                                int monthCount = workerGroup.Count(w => monthRecordIds.Contains((Guid)w.record_id));
                                workerMonthTotals[month] = monthCount;
                                workerObj[month] = monthCount;
                                workerTotal += monthCount;
                            }

                            workerObj["total"] = workerTotal;
                            workerItems.Add(workerObj);

                            // 累加到站点和合同的月份计数
                            foreach (var month in months)
                            {
                                siteMonthTotals[month] += workerMonthTotals[month];
                                contractMonthTotals[month] += workerMonthTotals[month];
                            }
                            siteTotal += workerTotal;
                            contractTotal += workerTotal;
                        }

                        // 创建站点数据对象
                        var siteObj = new Dictionary<string, object>();
                        siteObj["name"] = siteInfo.name_cht;
                        foreach (var month in months)
                        {
                            siteObj[month] = siteMonthTotals[month];
                        }
                        siteObj["total"] = siteTotal;
                        siteObj["child"] = workerItems;
                        siteItems.Add(siteObj);
                    }

                    // 创建合同数据对象
                    var contractObj = new Dictionary<string, object>();
                    contractObj["name"] = contract.contract_no;
                    foreach (var month in months)
                    {
                        contractObj[month] = contractMonthTotals[month];
                    }
                    contractObj["total"] = contractTotal;
                    contractObj["child"] = siteItems;
                    contractItems.Add(contractObj);
                }

                // 构建最终返回结构
                var responseData = new
                {
                    title = title,
                    list = contractItems
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), responseData);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_Record_WorkerService.GetSiteAttendanceRecords", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 地盘出勤统计（工资统计）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSiteAttendanceSalaryTotal(PageInput<SiteAttendanceRecordDto> dto)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;

                // 确保有contract_id过滤条件
                //if (dto.search == null || !dto.search.contract_id.HasValue)
                //{
                //    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_plan_task_empty"));
                //}

                // 获取当前合约及子合约ID列表
                var contractIds = new List<Guid>();
                if(dto.search != null && dto.search.contract_id.HasValue)
                    contractIds.Add(dto.search.contract_id.Value);

                // 获取子合约（假设vo_wo_type表示子合约，master_id关联主合约）
                var childContracts = await _contractRepository
                    .FindAsIQueryable(c => c.delete_status == valid)
                    .WhereIF(dto.search.contract_id.HasValue, c => c.master_id == dto.search.contract_id.Value)
                    .Select(c => c.id)
                    .ToListAsync();

                contractIds.AddRange(childContracts);

                // 构建主查询 - 获取当前合约及子合约下的所有工作记录及相关工人信息
                var query = from record in _siteWorkRecordRepository.FindAsIQueryable(r => r.delete_status == valid).WhereIF(contractIds.Count > 0, r => contractIds.Contains((Guid)r.contract_id)).AsNoTracking()
                            join workerRecord in _repository.FindAsIQueryable(w => w.delete_status == valid).AsNoTracking()
                            on record.id equals workerRecord.record_id
                            select new
                            {
                                Record = record,
                                WorkerRecord = workerRecord
                            };

                // 应用其他搜索条件
                if (dto.search != null)
                {
                    // site_id 过滤
                    if (dto.search.site_id.HasValue)
                    {
                        query = query.Where(r => r.Record.site_id == dto.search.site_id.Value);
                    }
                    if (dto.search.site_ids != null && dto.search.site_ids.Count > 0)
                    {
                        query = query.Where(r => dto.search.site_ids.Contains((Guid)r.Record.site_id));
                    }

                    // 工作日期范围过滤
                    if (dto.search.work_date_start.HasValue)
                    {
                        query = query.Where(r => r.Record.work_date >= dto.search.work_date_start.Value);
                    }
                    if (dto.search.work_date_end.HasValue)
                    {
                        query = query.Where(r => r.Record.work_date <= dto.search.work_date_end.Value);
                    }
                }

                // 设置默认排序
                query = query.OrderByDescending(r => r.Record.work_date);

                // 获取所有数据
                var allData = await query.ToListAsync();

                // 获取所有月份并按降序排序
                var months = allData.Select(a => a.Record.work_date.ToString("yyyy-MM")).Distinct().OrderByDescending(m => m).ToList();

                // 构建标题数组
                var title = new List<string> { "name" };
                title.AddRange(months);
                title.Add("total");

                // 获取相关数据
                var lstContact = await _contactRepository.FindAsIQueryable(c => c.delete_status == valid).Select(c => new { c.id, c.name_cht, c.name_eng }).ToListAsync();

                // 按工人分组统计工资
                var workerItems = new List<dynamic>();
                var workerGroups = allData.GroupBy(r => r.WorkerRecord.contact_id);

                foreach (var workerGroup in workerGroups)
                {
                    var contactId = workerGroup.Key;
                    var contactInfo = lstContact.FirstOrDefault(c => c.id == contactId);
                    if (contactInfo == null) continue;

                    // 创建工人数据对象
                    var workerObj = new Dictionary<string, object>();
                    workerObj["name"] = contactInfo.name_cht ?? contactInfo.name_eng;

                    decimal workerTotal = 0;

                    // 计算每个月份的工资
                    foreach (var month in months)
                    {
                        // 获取该月份下该工人的所有记录
                        var monthRecords = workerGroup.Where(r => r.Record.work_date.ToString("yyyy-MM") == month).ToList();

                        // 计算该月份工资总额
                        decimal monthSalary = 0;
                        foreach (var record in monthRecords)
                        {
                            // 薪资计算规则：若salary_adj非空则以该字段作为基础工资，否则使用base_salary + traffic_allowance
                            if (record.WorkerRecord.salary_adj.HasValue)
                            {
                                //monthSalary += record.WorkerRecord.salary_adj.Value;
                                monthSalary += record.WorkerRecord.salary_adj.Value + (record.WorkerRecord.traffic_allowance ?? 0);
                            }
                            else
                            {
                                monthSalary += (record.WorkerRecord.base_salary ?? 0) + (record.WorkerRecord.traffic_allowance ?? 0);
                            }
                        }

                        workerObj[month] = monthSalary;
                        workerTotal += monthSalary;
                    }

                    workerObj["total"] = workerTotal;
                    workerItems.Add(workerObj);
                }

                // 构建最终返回结构 - 只包含工人层级数据
                var responseData = new
                {
                    title = title,
                    list = workerItems // 直接返回工人列表，不再包含合约层级
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), responseData);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_Record_WorkerService.GetSiteAttendanceSalaryTotal", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 查询工人资料-实际薪资（分页）
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContactWorkTypeSalaryByMonthAsync(PageInput<ContactWorkTypeSearchDto> searchDto)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;
                var search = searchDto?.search;
                if (search == null || search.contact_id == Guid.Empty)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contact_null"));
                }

                var workers = _repository
                    .FindAsIQueryable(w => w.delete_status == valid && w.contact_id == search.contact_id)
                    .AsNoTracking();

                var siteRecords = _siteWorkRecordRepository
                    .FindAsIQueryable(r => r.delete_status == valid)
                    .AsNoTracking();

                var users = _userRepository
                    .FindAsIQueryable(u => u.delete_status == valid)
                    .AsNoTracking();

                var userRels = _userRelationRepository
                    .FindAsIQueryable(ur => ur.delete_status == valid && ur.relation_type == 0)
                    .AsNoTracking();

                var workTypes = _workTypeRepository
                    .FindAsIQueryable(wt => wt.delete_status == valid)
                    .AsNoTracking();

                var baseQuery = from w in workers
                                join r in siteRecords on w.record_id equals r.id into wr
                                from rec in wr.DefaultIfEmpty()
                                join u in users on new { comp = w.company_id, cid = w.contact_id } equals new { comp = u.company_id, cid = u.contact_id } into wu
                                from user in wu.DefaultIfEmpty()
                                join ur in userRels on new { uid = user.user_register_id, rid = w.work_type_id } equals new { uid = ur.user_register_Id, rid = ur.relation_id } into uur
                                from urel in uur.DefaultIfEmpty()
                                join wt in workTypes on w.work_type_id equals wt.id into wtw
                                from wtype in wtw.DefaultIfEmpty()
                                where rec != null && rec.work_date.HasValue
                                select new
                                {
                                    Year = rec.work_date.Value.Year,
                                    Month = rec.work_date.Value.Month,
                                    rec.work_date,
                                    Shift = rec.shift,
                                    SalaryAdj = w.salary_adj,
                                    BaseSalary = w.base_salary,
                                    TrafficAllowance = w.traffic_allowance,
                                    UserDaySalary = urel.day_salary,
                                    UserNightSalary = urel.night_salary,
                                    TypeDaySalary = wtype.day_salary,
                                    TypeNightSalary = wtype.night_salary
                                };

                baseQuery = baseQuery.WhereIF(search.work_date_start.HasValue, x => x.work_date >= search.work_date_start.Value);
                baseQuery = baseQuery.WhereIF(search.work_date_end.HasValue, x => x.work_date <= search.work_date_end.Value);

                var groupedQuery = baseQuery
                    .GroupBy(x => new { x.Year, x.Month })
                    .Select(g => new
                    {
                        year = g.Key.Year,
                        month = g.Key.Month,
                        count = g.Count(),
                        time = (g.Key.Month < 10 ? ("0" + g.Key.Month) : g.Key.Month.ToString()) + "/" + g.Key.Year.ToString(),
                        total_salary = g.Sum(x =>
                            ((x.SalaryAdj ?? x.BaseSalary) ??
                             (x.Shift == ShiftTypeHelper.GetShiftTypeStr((int)ShiftTypeEnum.Night)
                                ? (x.UserNightSalary ?? x.TypeNightSalary)
                                : (x.UserDaySalary ?? x.TypeDaySalary)))
                             + (x.TrafficAllowance ?? 0))
                    })
                    .OrderByDescending(x => x.year)
                    .ThenByDescending(x => x.month);

                if (string.IsNullOrEmpty(searchDto.sort_field))
                {
                    searchDto.sort_field = "create_date";
                    searchDto.sort_type = "desc";
                }

                var result = await groupedQuery.GetPageResultAsync(searchDto);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 分页查询工人资料-计薪标准
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContactWorkTypeSalaryAsync(PageInput<ContactWorkTypeSearchDto> searchDto)
        {
            try
            {
                var search = searchDto.search;

                var contactData = await _contactRepository
                  .FindAsIQueryable(p => p.id == search.contact_id)
                  .AsNoTracking()
                  .FirstOrDefaultAsync();
                if (contactData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contact_null"));
                }

                var userData = await _userRepository
                    .FindAsIQueryable(p => p.contact_id == contactData.id && p.company_id == contactData.company_id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (userData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contact_null"));
                }

                // 连表查询用户工种表
                var workTypes = _sysWorkTypeRepository
                    .FindAsIQueryable(wt => wt.delete_status == (int)SystemDataStatus.Valid)
                    .AsNoTracking();
                var userWorkTypes = _userRelationRepository
                    .FindAsIQueryable(p => p.user_register_Id == userData.user_register_id &&
                                           p.relation_type == 0 &&
                                           p.delete_status == (int)SystemDataStatus.Valid)
                    .AsNoTracking();

                var query = from uw in userWorkTypes
                            join wt in workTypes on uw.relation_id equals wt.id into utw
                            from userRelWorkType in utw.DefaultIfEmpty()
                            select new { uw, userRelWorkType };

                if (!string.IsNullOrEmpty(search.work_type_name))
                {
                    query = query.Where(x => x.userRelWorkType.type_name.Contains(search.work_type_name));
                }

                var lstData = query.Select(x => new
                {
                    id = x.uw.id,
                    contact_id = contactData.id,
                    work_type_id = x.userRelWorkType.id,
                    work_type_name = x.userRelWorkType.type_name,
                    day_salary = x.uw.day_salary.HasValue ? x.uw.day_salary : x.userRelWorkType.day_salary,
                    night_salary = x.uw.night_salary.HasValue ? x.uw.night_salary : x.userRelWorkType.night_salary,
                });
                if (string.IsNullOrEmpty(searchDto.sort_field))
                {
                    searchDto.sort_field = "create_date";
                    searchDto.sort_type = "desc";
                }
                var result = await lstData.GetPageResultAsync(searchDto);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 设置工人资料工种对应的薪资
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SetContactWorkTypeSalaryAsync(SetContactWorkTypeSalaryDto input)
        {
            try
            {
                var userWorkTypeData = await _userRelationRepository
                    .FindAsyncFirst(p => p.id == input.id);
                if (userWorkTypeData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contact_null"));
                }

                userWorkTypeData.day_salary = input.day_salary;
                userWorkTypeData.night_salary = input.night_salary;
                userWorkTypeData.modify_id = UserContext.Current.UserInfo.User_Id;
                userWorkTypeData.modify_name = UserContext.Current.UserInfo.UserName;
                userWorkTypeData.modify_date = DateTime.Now;

                _userRelationRepository.Update(userWorkTypeData);
                await _userRelationRepository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region  -- 已弃用 --

        private async Task<WebResponseContent> GetSiteAttendanceRecords_bak(PageInput<SiteAttendanceRecordDto> dto)
        {
            try
            {
                string where = " where swrw.delete_status=0 and swr.delete_status=0 ";

                var search = dto.search;
                if (search != null)
                {
                    if (!string.IsNullOrEmpty(search.contact_name))
                    {
                        where += $" and (contact.name_cht like '%{search.contact_name}%' or contact.name_eng like '%{search.contact_name}%')";
                    }
                    if (!string.IsNullOrEmpty(search.company_name))
                    {
                        where += $" and (company.company_name like '%{search.company_name}%' or company.company_name_eng like '%{search.company_name}%')";
                    }
                    if (!search.work_date_start.HasValue)
                    {
                        where += $" and (case when swrw.time_in_adj is not null then swrw.time_in_adj else swrw.time_in end)>='{search.work_date_start}'";
                    }
                    if (!search.work_date_end.HasValue)
                    {
                        where += $" and (case when swrw.time_out_adj is not null then swrw.time_out_adj else swrw.time_out end)<='{search.work_date_end}'";
                    }
                    if (!string.IsNullOrEmpty(search.site_name))
                    {
                        where += $" and (site.name_cht like '%{search.contact_name}%' or site.name_eng like '%{search.contact_name}%')";
                    }
                }

                string sql = @$"select 
                            swrw.contact_id '联系人id',
                            contact.name_cht '联系人中文名',contact.name_eng '联系人英文名',
                            swrw.record_id '出勤id',
                            c.contract_no '合约编码',
                            company.company_name '公司名称',company.company_name_eng '公司英文名',
                            wr.wrc_no '工人注册证',
                            format(swr.work_date, 'yyyy-MM') '月份',
                            swr.work_date '开工日期',
                            format((case when swrw.time_in_adj is not null then swrw.time_in_adj else swrw.time_in end), 'yyyy-MM-dd') '出席日期',
                            (case when swrw.time_in_adj is not null then swrw.time_in_adj else swrw.time_in end) '上班时间',
                            (case when swrw.time_out_adj is not null then swrw.time_out_adj else swrw.time_out end) '下班时间',
                            site.name_cht '工地中文名',site.name_eng '工地英文名',
                            wt.type_name '工种',
                            swrw.base_salary '基本薪资',
                            swrw.traffic_allowance '交通补贴',
                            swrw.salary_adj '总薪资',
                            (case when swr.finish_briefing = 1 then N'已开入职纸' else '' end) '培训出席记录',
                            swrw.remark '备注'
                        from Biz_Site_Work_Record_Worker swrw
                        left join Biz_Site_Work_Record swr on swrw.record_id=swr.id
                        left join Biz_Contract c on swr.contract_id=c.id
                        left join Sys_Company company on c.company_id=company.id
                        left join Sys_Contact contact on swrw.contact_id=contact.id
                        left join Sys_Work_Type wt on swrw.work_type_id=wt.id
                        left join Sys_User_New un on contact.id=un.contact_id
                        left join Sys_Worker_Register wr on un.user_register_id=wr.id
                        left join Biz_Site site on swr.site_id=site.id {where}";


                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_Record_WorkerService.GetSiteAttendanceRecords", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        private async Task<WebResponseContent> GetContactAttendanceInfo_bak(PageInput<SiteAttendanceRecordDto> dto)
        {
            try
            {
                string sql = @"select format(b.work_date, 'yyyy-MM') [month], count(a.contact_id) totalDays, c.name_cht, d.type_name, b.shift,
                        (case when a.base_salary is not null or a.base_salary <= 0 then (a.base_salary * count(a.contact_id)) else (
                        (case when b.shift='Night Shift' then (d.night_salary * count(a.contact_id)) else (d.day_salary * count(a.contact_id)) end)
                        ) end)base_salary,
                        d.day_salary, d.night_salary 
                        from Biz_Site_Work_Record_Worker a
                        left join Biz_Site_Work_Record b on a.record_id=b.id
                        left join Sys_Contact c on a.contact_id=c.id
                        left join Sys_Work_Type d on a.work_type_id=d.id 
                        group by b.work_date, a.contact_id, a.base_salary,c.name_cht, d.type_name, b.shift, a.base_salary, d.day_salary, d.night_salary
                        order by format(b.work_date, 'yyyy-MM')";
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_Record_WorkerService.GetSiteAttendanceRecords", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        private async Task<WebResponseContent> GetContactAttendanceSalaryCalculationStandard_bak(PageInput<SiteAttendanceRecordDto> dto)
        {
            try
            {
                string sql = @"select c.name_cht, c.name_eng, wr.wrc_no, d.type_name,d.day_salary, d.night_salary 
                        from Biz_Site_Work_Record_Worker a
                        left join Biz_Site_Work_Record b on a.record_id=b.id
                        left join Sys_Contact c on a.contact_id=c.id
                        left join Sys_Work_Type d on a.work_type_id=d.id 
                        left join Sys_User_New un on a.contact_id=un.contact_id
                        left join Sys_Worker_Register wr on un.user_register_id=wr.id
                        where a.delete_status=0 and b.delete_status=0 and c.delete_status=0 and d.delete_status=0 and un.delete_status=0 --and wr.delete_status=0
                        group by c.name_cht,c.name_eng, wr.wrc_no, d.type_name, d.day_salary, d.night_salary
                        order by d.type_name";

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_Record_WorkerService.GetContactAttendanceInfo", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        private async Task<WebResponseContent> GetSiteAttendanceTotal_bak(PageInput<SiteAttendanceRecordDto> dto)
        {
            try
            {
                #region  -- sql语句 --

                string sql = @"select 
                        a.contract_id, c.contract_no, 
                        a.site_id,d.name_cht site_name_cht,d.name_eng site_name_eng,d.name_sho site_name_sho,
                        a.work_date,
                        c.name_cht contact_name_cht,c.name_eng contact_name_eng,
                        b.base_salary '工人当前开工基础工资',
                        b.salary_adj '工资调整（调整后的）',
                        b.traffic_allowance '交通津贴'
                        from Biz_Site_Work_Record a
                        left join Biz_Site_Work_Record_Worker b on b.record_id=b.id
                        left join Biz_Contract c on a.contract_id=c.id
                        left join Biz_Site d on a.site_id=d.id
                        left join Sys_Contact e on b.contact_id=e.id";

                #endregion

                var valid = (int)SystemDataStatus.Valid;

                // 构建主查询
                var query = from record in _siteWorkRecordRepository.FindAsIQueryable(r => r.delete_status == valid).AsNoTracking()
                            select record;

                // 应用搜索条件
                if (dto.search != null)
                {
                    // contract_id 过滤
                    if (dto.search.contract_id.HasValue)
                    {
                        query = query.Where(r => r.contract_id == dto.search.contract_id.Value);
                    }

                    // 联系人名称搜索过滤（对应子查询）
                    if (!string.IsNullOrEmpty(dto.search.contact_name))
                    {
                        var contactQuery = _contactRepository.FindAsIQueryable(c => c.delete_status == valid &&
                            (c.name_eng.Contains(dto.search.contact_name) || c.name_cht.Contains(dto.search.contact_name)));
                        var contactIds = contactQuery.Select(c => c.id).ToList();
                        if (contactIds.Any())
                        {
                            query = query.Where(r => contactIds.Contains(r.id));
                        }
                    }

                    // site_id 过滤
                    if (dto.search.site_id.HasValue)
                    {
                        query = query.Where(r => r.site_id == dto.search.site_id.Value);
                    }

                    // 工地名称搜索过滤（对应子查询）
                    if (!string.IsNullOrEmpty(dto.search.site_name))
                    {
                        var siteQuery = _siteRepository.FindAsIQueryable(s => s.delete_status == valid &&
                            (s.name_chs.Contains(dto.search.site_name) ||
                             s.name_eng.Contains(dto.search.site_name) ||
                             s.name_sho.Contains(dto.search.site_name)));
                        var siteIds = siteQuery.Select(s => s.id).ToList();
                        if (siteIds.Any())
                        {
                            query = query.Where(r => siteIds.Contains((Guid)r.site_id));
                        }
                    }

                    // 工作日期范围过滤
                    if (dto.search.work_date_start.HasValue)
                    {
                        query = query.Where(r => r.work_date >= dto.search.work_date_start.Value);
                    }
                    if (dto.search.work_date_end.HasValue)
                    {
                        query = query.Where(r => r.work_date <= dto.search.work_date_end.Value);
                    }
                }

                // 设置默认排序
                query = query.OrderByDescending(r => r.work_date);

                // 分页处理
                var pageResult = await query.GetPageResultAsync(dto);

                // 分页处理
                var result = await query.GetPageResultAsync(dto);
                var month = new List<string>();
                var contract = new List<int>();
                var site = new List<int>();
                var contact = new List<int>();

                if (result.data.Count > 0)
                {
                    //获取合同内的所有月份
                    month = result.data.Select(a => a.work_date.ToString("yyyy-MM")).Distinct().ToList();


                    var list = result.data.Select(a => new { a.id, a.contract_id, a.site_id, a.work_date }).ToList();
                    //获取合同id与编号
                    var lstContract = _contractRepository
                        .WhereIF(true, a => a.delete_status == valid)
                        .WhereIF(dto.search.contract_id.HasValue, a => a.id == dto.search.contract_id || (!string.IsNullOrEmpty(a.vo_wo_type) && a.master_id == dto.search.contract_id))
                        .Select(a => new { a.id, a.contract_no, a.create_date })
                        .OrderByDescending(a => a.create_date)
                        .ToList();
                    //获取站点与名称
                    var lstSite = _siteRepository.Find(a => a.delete_status == valid).Select(a => new { a.id, a.name_cht, a.name_eng, a.name_sho }).ToList();
                    //获取所有联络人id与名称
                    var lstContact = _contactRepository.Find(a => a.delete_status == valid).Select(a => new { a.id, a.name_cht, a.name_eng }).ToList();
                    //获取所有出勤记录
                    var lstWorkRe = _repository.Find(a => a.delete_status == valid && (a.time_out != null || a.time_out_adj != null)).Select(a => new { a.id, a.record_id, a.contact_id }).ToList();

                    var contractObj = new List<object>();
                    var siteObj = new List<object>();
                    var contactObj = new List<object>();

                    //循环所有合约
                    foreach (var item in lstContract)
                    {
                        siteObj = new List<object>();
                        //遍历总月列表
                        foreach (var m in month)
                        {
                            //遍历所有任务
                            foreach (var r in list.Where(a => a.contract_id == item.id).ToList())
                            {
                                contactObj = new List<object>();
                                //遍历所有出勤人数
                                foreach (var w in lstWorkRe.Where(a => a.record_id == r.id).ToList())
                                {
                                    var c = lstContact.Where(a => a.id == w.contact_id).FirstOrDefault();
                                    contactObj.Add(new
                                    {
                                        c.name_cht,
                                        c.name_eng,
                                        //total = 获取数量
                                    });

                                }

                                var s = lstSite.Where(a => a.id == r.site_id).FirstOrDefault();
                                siteObj.Add(new
                                {
                                    s.name_cht,
                                    s.name_eng,
                                    s.name_sho,
                                    contact = contactObj
                                });
                            }
                        }


                        contractObj.Add(new
                        {
                            item.contract_no,
                            site = siteObj,
                        });
                    }


                    //foreach(var item in month)
                    for (var i = 0; i < month.Count; i++)
                    {
                        var item = month[i];
                        var detail = list.Where(a => a.work_date.ToString("yyyy-MM") == item).ToList();
                        foreach (var index in detail)
                        {
                            var siteDetail = lstSite.Where(a => a.id == index.site_id).FirstOrDefault();

                        }


                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), new { data = result, month = month });
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Site_Work_Record_WorkerService.GetSiteAttendanceRecords", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        #region 添加、编辑、删除工人

        /// <summary>
        /// 添加工人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> AddWorkerByDataAsync(AddWorkerDto input)
        {
            try
            {
                var contactData = await _contactRepository
                    .FindAsIQueryable(p => p.id == input.contact_id && p.delete_status == (int)SystemDataStatus.Valid)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (contactData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contact_null"));
                }
                var isAnyWorker = await _repository
                    .FindAsIQueryable(p => p.contact_id == input.contact_id && p.delete_status == (int)SystemDataStatus.Valid)
                    .AnyAsync();
                if (isAnyWorker)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contact_null"));
                }

                var workerData = new Biz_Site_Work_Record_Worker
                {
                    create_id = UserContext.Current.UserId,
                    create_date = DateTime.Now,
                    create_name = UserContext.Current.UserName,
                    delete_status = (int)SystemDataStatus.Valid,

                    is_valid = input.is_valid,
                    work_type_id = input.work_type_id,
                    is_wpic = input.is_wpic,
                    is_cp = input.is_cp,
                    is_nt = input.is_nt,
                    is_qr = 0,
                    is_sign = 0,
                    is_sic = 0,
                    is_sick = input.is_sick,
                    work_remark = input.work_remark,
                    time_in = input.time_in,
                    time_out = input.time_out,

                    green_card_no = "",
                    green_card_exp = null,
                    cic_no = "",
                    cic_exp = null,
                    cic_card_no = "",
                    company_id = contactData.company_id
                };

                // 补充用户信息
                workerData = await SetWorkerData(workerData);

                _repository.Add(workerData);

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
        /// 编辑工人信息（app端）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditWorkerDataAsync(EditWorkerDto input)
        {
            try
            {
                var workerData = await _repository.FindAsyncFirst(p => p.id == input.id);
                if (workerData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_null"));
                }
                var recordData = await _siteWorkRecordRepository.FindFirstAsync(p => p.id == workerData.record_id);
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }


                workerData.work_type_id = input.work_type_id;
                workerData.is_wpic = input.is_wpic;
                workerData.is_sick = input.is_sick;
                workerData.work_remark = input.work_remark;

                // 判断工种是否为CP
                workerData.is_cp = await CheckWorkerTypeIsCP(workerData.work_type_id.Value, recordData) ? 1 : 0;

                workerData.modify_id = UserContext.Current.UserInfo.User_Id;
                workerData.modify_name = UserContext.Current.UserInfo.UserName;
                workerData.modify_date = DateTime.Now;

                _repository.Update(workerData);
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
        /// 编辑工人信息（web端）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditWorkerDataByWebAsync(EditWorkByWebDto input)
        {
            try
            {
                var workerData = await _repository.FindAsyncFirst(p => p.id == input.id);
                if (workerData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_null"));
                }
                var recordData = await _siteWorkRecordRepository.FindFirstAsync(p => p.id == workerData.record_id);
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }

                workerData.green_card_no = input.green_card_no;
                workerData.green_card_exp = input.green_card_exp;
                workerData.is_wpic = input.is_wpic;
                workerData.work_type_id = input.work_type_id;
                workerData.is_cp = input.is_cp;
                workerData.is_nt = input.is_nt;
                workerData.is_sick = input.is_sick;
                workerData.work_remark = input.work_remark;

                // 判断工种是否为CP
                if (workerData.is_cp == 0)
                {
                    workerData.is_cp = await CheckWorkerTypeIsCP(workerData.work_type_id.Value, recordData) ? 1 : 0;
                }

                workerData.modify_id = UserContext.Current.UserInfo.User_Id;
                workerData.modify_name = UserContext.Current.UserInfo.UserName;
                workerData.modify_date = DateTime.Now;

                _repository.Update(workerData);
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
        /// 根据二维码添加工人
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="contectId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> AddWorkerByQRCodeAsync(Guid recordId, Guid contectId)
        {
            try
            {
                return await AddWorkerDataAsync(new AddWorkerByContactDto
                {
                    contact_ids = new List<Guid> { contectId },
                    record_id = recordId
                }, true, true);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 批量添加工人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> AddWorkerByContactListAsync(AddWorkerByContactDto input)
        {
            try
            {
                return await AddWorkerDataAsync(input, false, true);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 添加工人
        /// </summary>
        /// <param name="input">联系人数据</param>
        /// <param name="isQR">是否二维码</param>
        /// <param name="isTimeIn">是否添加就上班</param>
        /// <returns></returns>
        private async Task<WebResponseContent> AddWorkerDataAsync(AddWorkerByContactDto input, bool isQR, bool isTimeIn)
        {
            try
            {
                if (input.contact_ids.Count == 0)
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }

                var lstContactData = await _contactRepository
                    .FindAsIQueryable(p => input.contact_ids.Contains(p.id))
                    .AsNoTracking()
                    .ToListAsync();

                // 批量逻辑：基于联系人批量补充用户与注册信息并一次性插入
                var valid = (int)SystemDataStatus.Valid;
                var contacts = lstContactData.Select(c => new { c.id, c.company_id }).ToList();
                if (contacts.Count == 0)
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }

                var contactIds = contacts.Select(c => c.id).ToList();
                var existedContactIds = await _repository
                    .FindAsIQueryable(w => w.record_id == input.record_id &&
                                           w.delete_status == valid &&
                                           w.contact_id.HasValue &&
                                           contactIds.Contains(w.contact_id.Value))
                    .AsNoTracking()
                    .Select(w => w.contact_id.Value)
                    .ToListAsync();

                var newContacts = contacts.Where(c => !existedContactIds.Contains(c.id)).ToList();
                if (newContacts.Count == 0)
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }

                var newContactIds = newContacts.Select(c => c.id).ToList();

                var users = await _userRepository
                    .FindAsIQueryable(u => newContactIds.Contains(u.contact_id.Value))
                    .Select(u => new { u.contact_id, u.company_id, u.user_register_id })
                    .AsNoTracking()
                    .ToListAsync();

                var userIndex = users
                    .GroupBy(u => new { u.contact_id, u.company_id })
                    .ToDictionary(g => (g.Key.contact_id, g.Key.company_id), g => g.Select(x => x.user_register_id).FirstOrDefault());

                var registerIds = users
                    .Select(u => u.user_register_id)
                    .Where(id => id.HasValue)
                    .Select(id => id.Value)
                    .Distinct()
                    .ToList();
                var registers = registerIds.Count == 0
                    ? new List<Sys_Worker_Register>()
                    : await _workerRegisterRepository
                        .FindAsIQueryable(r => r.user_register_Id.HasValue && registerIds.Contains(r.user_register_Id.Value))
                        .AsNoTracking()
                        .ToListAsync();

                var registerIndex = registers.ToDictionary(r => r.user_register_Id, r => r);

                var now = DateTime.Now;
                var toInsert = new List<Biz_Site_Work_Record_Worker>();
                foreach (var c in newContacts)
                {
                    Guid? urid = null;
                    if (userIndex.TryGetValue((c.id, c.company_id), out var uid))
                    {
                        urid = uid;
                    }

                    string greenNo = "";
                    DateTime? greenExp = null;
                    string cicNo = "";
                    DateTime? cicExp = null;

                    if (urid.HasValue && registerIndex.TryGetValue(urid.Value, out var reg))
                    {
                        greenNo = reg.stc_no;
                        greenExp = reg.stc_issued_end_date;
                        cicNo = reg.wrc_no;
                        cicExp = reg.wrc_issued_end_date;
                    }

                    var worker = new Biz_Site_Work_Record_Worker
                    {
                        id = Guid.NewGuid(),
                        create_id = UserContext.Current.UserId,
                        create_name = UserContext.Current.UserName,
                        create_date = now,
                        delete_status = valid,

                        record_id = input.record_id,
                        contact_id = c.id,
                        company_id = c.company_id,
                        time_in = isTimeIn ? now : null,

                        is_qr = isQR ? 1 : 0,

                        green_card_no = greenNo,
                        green_card_exp = greenExp,
                        cic_no = cicNo,
                        cic_exp = cicExp,
                        cic_card_no = ""
                    };

                    toInsert.Add(worker);
                }

                _repository.AddRange(toInsert);
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
        /// 补充工人的绿卡、CIC卡信息
        /// </summary>
        /// <param name="workerData"></param>
        /// <returns></returns>
        private async Task<Biz_Site_Work_Record_Worker> SetWorkerData(Biz_Site_Work_Record_Worker workerData)
        {
            var userData = await _userRepository
                .FindAsIQueryable(p => p.contact_id == workerData.contact_id &&
                                       p.company_id == workerData.company_id &&
                                       p.delete_status == (int)SystemDataStatus.Valid)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (userData == null)
            {
                return workerData;
            }

            var registerData = await _workerRegisterRepository
                .FindAsIQueryable(p => p.user_register_Id == userData.user_register_id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (registerData == null)
            {
                return workerData;
            }

            workerData.green_card_no = registerData.stc_no;
            workerData.green_card_exp = registerData.stc_issued_end_date;
            workerData.cic_no = registerData.wrc_no;
            workerData.cic_exp = registerData.wrc_issued_end_date;

            if (!workerData.is_cp.HasValue || !workerData.work_type_id.HasValue)
            {
                workerData.is_cp = 0;
                return workerData;
            }

            // 判断工种是否为管工类型
            var recordData = await _siteWorkRecordRepository.FindFirstAsync(p => p.id == workerData.record_id);
            if (recordData == null)
            {
                return workerData;
            }

            workerData.is_cp = await CheckWorkerTypeIsCP(workerData.work_type_id.Value, recordData) ? 1 : 0;
            //workerData.is_cp = cfgList.Contains(workTypeData.type_name) ? 1 : 0;
            return workerData;
        }

        /// <summary>
        /// 判断工种是否为管工
        /// </summary>
        /// <param name="workerTypeId"></param>
        /// <returns></returns>
        private async Task<bool> CheckWorkerTypeIsCP(Guid workerTypeId, Biz_Site_Work_Record recordData)
        {
            // CPT是可以下轨道工程的管工
            // CPNT是管工

            string key = recordData.is_track == 1 ? "CPT" : "CPNT";

            var cfgList = await DBServerProvider.DbContext.Set<Sys_Config>()
              .AsNoTracking()
              .Where(p => p.config_type == 5 && p.config_key == key)
              .Select(p => p.config_value)
              .ToListAsync();

            var workTypeData = await _workTypeRepository
                .FindAsIQueryable(p => p.id == workerTypeId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (workTypeData == null)
            {
                return false;
            }

            return cfgList.Contains(workTypeData.type_name);
        }

        /// <summary>
        /// 根据工人id删除工人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteWorkerByIdAsync(Guid id)
        {
            try
            {
                var workerData = await _repository.FindFirstAsync(p => p.id == id);
                if (workerData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_null"));
                }
                var recordData = await _siteWorkRecordRepository.FindFirstAsync(p => p.id == workerData.record_id);
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }
                if (recordData.current_duty_cp_id == workerData.contact_id)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_delete_no_del"));
                }
                if (workerData.time_out.HasValue || workerData.time_out_adj.HasValue)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_delete_no_time_out"));
                }

                workerData.delete_status = (int)SystemDataStatus.Invalid;
                workerData.modify_id = UserContext.Current.UserInfo.User_Id;
                workerData.modify_name = UserContext.Current.UserInfo.UserName;
                workerData.modify_date = DateTime.Now;

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
        /// 根据工人id下班打卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> WorkerTimeOutByIdAsync(Guid id)
        {
            return await WorkerTimeOutAsync(new List<Guid> { id });
        }

        /// <summary>
        /// 根据工地工作记录id将未下班的工人进行下班打卡
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> WorkerTimeOutByAllAsync(Guid recordId)
        {
            try
            {
                var lstWorkerData = await _repository.FindAsync(p => p.record_id == recordId &&
                                                                     p.delete_status == (int)SystemDataStatus.Valid);
                var lstTimeOutWorkerIds = lstWorkerData
                    .Where(p => !p.time_out.HasValue && !p.time_out_adj.HasValue)
                    .Select(p => p.id)
                    .ToList();
                if (lstTimeOutWorkerIds.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_time_out_all"));
                }

                return await WorkerTimeOutAsync(lstTimeOutWorkerIds);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 工人下班打卡
        /// </summary>
        /// <param name="workerIds"></param>
        /// <returns></returns>
        private async Task<WebResponseContent> WorkerTimeOutAsync(List<Guid> workerIds)
        {
            try
            {
                var workerData = await _repository.FindAsync(p => workerIds.Contains(p.id));
                if (workerData.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }
                var recordData = await _siteWorkRecordRepository.FindFirstAsync(p => p.id == workerData[0].record_id);
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }

                var nowTime = DateTime.Now;
                foreach (var item in workerData)
                {
                    item.time_out = nowTime;
                    item.modify_id = UserContext.Current.UserInfo.User_Id;
                    item.modify_name = UserContext.Current.UserInfo.UserName;
                    item.modify_date = DateTime.Now;
                }
                if (workerData.Count > 0)
                {
                    workerData = await CalculateWorkerSalary(workerData, recordData);

                    _repository.UpdateRange(workerData);
                    await _repository.SaveChangesAsync();
                }
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 计算工人的薪资
        /// </summary>
        /// <param name="lstWorker"></param>
        /// <param name="recordData"></param>
        /// <returns></returns>
        private async Task<List<Biz_Site_Work_Record_Worker>> CalculateWorkerSalary(
            List<Biz_Site_Work_Record_Worker> lstWorker,
            Biz_Site_Work_Record recordData,
            bool isChange = false)
        {
            var lstContectIds = lstWorker.Where(p => p.contact_id.HasValue).Select(p => p.contact_id.Value).ToList();
            var lstWorkTypeIds = lstWorker.Where(p => p.work_type_id.HasValue).Select(p => p.work_type_id.Value).ToList();

            var lstContect = await _contactRepository
                .FindAsIQueryable(p => lstContectIds.Contains(p.id))
                .AsNoTracking()
                .ToDictionaryAsync(p => p.id);
            var lstWorkType = await _workTypeRepository
                .FindAsIQueryable(p => lstWorkTypeIds.Contains(p.id))
                .AsNoTracking()
                .ToDictionaryAsync(p => p.id);

            var users = await _userRepository
                   .FindAsIQueryable(u => lstContectIds.Contains(u.contact_id.Value))
                   .Select(u => new { u.contact_id, u.company_id, u.user_register_id })
                   .AsNoTracking()
                   .ToListAsync();

            var userIndex = users
                .GroupBy(u => new { u.contact_id, u.company_id })
                .ToDictionary(g => (g.Key.contact_id, g.Key.company_id), g => g.Select(x => x.user_register_id).FirstOrDefault());

            var lstRegisterIds = userIndex.Values.ToList();

            var lstUserWorkType = await _userRelationRepository
                .FindAsIQueryable(u => u.relation_type == 0 && lstRegisterIds.Contains(u.user_register_Id))
                .AsNoTracking()
                .ToListAsync();

            foreach (var item in lstWorker)
            {
                var contact = item.contact_id.HasValue && lstContect.TryGetValue(item.contact_id.Value, out Sys_Contact value)
                    ? value : null;
                var registerId = contact != null && userIndex.TryGetValue((contact.id, contact.company_id), out var registerValue)
                    ? registerValue : null;

                item.modify_id = UserContext.Current.UserInfo.User_Id;
                item.modify_name = UserContext.Current.UserInfo.UserName;
                item.modify_date = DateTime.Now;

                // 先获取联系人设置的工种薪资
                if (registerId != null)
                {
                    var userWorkType = lstUserWorkType
                        .FirstOrDefault(p => p.user_register_Id == registerId && p.relation_id == item.work_type_id);
                    if (userWorkType != null)
                    {
                        if (isChange)
                        {
                            item.salary_adj = recordData.shift == ShiftTypeHelper.GetShiftTypeStr((int)ShiftTypeEnum.Night) ?
                                userWorkType.day_salary : userWorkType.night_salary;
                        }
                        else
                        {
                            item.base_salary = recordData.shift == ShiftTypeHelper.GetShiftTypeStr((int)ShiftTypeEnum.Night) ?
                                userWorkType.day_salary : userWorkType.night_salary;
                        }
                        continue;
                    }
                }
                // 再获取工种的薪资（如果联系人有设置，则不获取工种的薪资）
                if (!item.work_type_id.HasValue)
                {
                    if (isChange)
                    {
                        item.salary_adj = 0;
                    }
                    else
                    {
                        item.base_salary = 0;
                    }
                    continue;
                }
                if (!item.base_salary.HasValue)
                {
                    var workType = lstWorkType.TryGetValue(item.work_type_id.Value, out var workTypeValue) ? workTypeValue : null;
                    if (workType == null)
                    {
                        continue;
                    }
                    if (isChange)
                    {
                        item.salary_adj = recordData.shift == ShiftTypeHelper.GetShiftTypeStr((int)ShiftTypeEnum.Night) ?
                          workType.day_salary : workType.night_salary;
                    }
                    else
                    {
                        item.base_salary = recordData.shift == ShiftTypeHelper.GetShiftTypeStr((int)ShiftTypeEnum.Night) ?
                          workType.day_salary : workType.night_salary;
                    }

                }
            }


            return lstWorker;
        }

        /// <summary>
        /// 工人上下班时间调整
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> WorkerTimeAdjustmentAsync(WorkerTimeChangeDto input)
        {
            try
            {
                var workerData = await _repository.FindAsyncFirst(p => p.id == input.id);
                if (workerData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_null"));
                }
                if (input.time_type == 0)
                {
                    workerData.time_in_adj = input.time_change;
                }
                else
                {
                    workerData.time_out_adj = input.time_change;
                }
                workerData.modify_id = UserContext.Current.UserInfo.User_Id;
                workerData.modify_name = UserContext.Current.UserInfo.UserName;
                workerData.modify_date = DateTime.Now;

                _repository.Update(workerData);
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
        /// 手动调整工地工作记录工人薪资
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> WorkerSalaryChangeAsync(WorkerSalaryChangeDto input)
        {
            try
            {
                var workerData = await _repository.FindAsyncFirst(p => p.id == input.id);
                if (workerData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_null"));
                }
                if (input.change_type == 0)
                {
                    workerData.salary_adj = input.salary;
                }
                else if (input.change_type == 1)
                {
                    var recordData = await _siteWorkRecordRepository.FindFirstAsync(p => p.id == workerData.record_id);
                    if (recordData == null)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                    }
                    var data = await CalculateWorkerSalary(new List<Biz_Site_Work_Record_Worker> { workerData }, recordData, true);

                    workerData = data[0];
                }

                _repository.Update(workerData);
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
        /// 设置当前值更管理人员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SetSiteWorkRecordCPAsync(Guid id, Guid recordId)
        {
            try
            {
                var recordData = await _siteWorkRecordRepository.FindFirstAsync(p => p.id == recordId);
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }

                var workerData = await _repository.FindAsyncFirst(p => p.id == id);
                if (workerData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_null"));
                }

                if (workerData.is_cp == 0)
                {
                    var isCP = await CheckWorkerTypeIsCP(workerData.work_type_id.Value, recordData);
                    if (!isCP)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("worker_no_cp"));
                    }
                }

                recordData.current_duty_cp_id = workerData.contact_id;
                recordData.modify_id = UserContext.Current.UserInfo.User_Id;
                recordData.modify_name = UserContext.Current.UserInfo.UserName;
                recordData.modify_date = DateTime.Now;

                _siteWorkRecordRepository.Update(recordData);
                await _siteWorkRecordRepository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据工地工作记录id获取工人管工列表
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSiteWorkCPAsync(Guid recordId)
        {
            try
            {
                var lstWorkers = await _repository.FindAsync(p => p.record_id == recordId && p.is_cp == 1);
                var lstWorkerIds = lstWorkers.Select(p => p.contact_id).ToList();
                var lstContact = await _contactRepository.FindAsync(p => lstWorkerIds.Contains(p.id));

                var data = new List<WorkerContactDto>();
                foreach (var item in lstWorkers)
                {
                    var contactData = lstContact.FirstOrDefault(p => p.id == item.contact_id);
                    data.Add(new WorkerContactDto
                    {
                        id = item.id,
                        contact_id = contactData == null ? Guid.Empty : contactData.id,
                        name_cht = contactData?.name_cht,
                        name_eng = contactData?.name_eng,
                    });
                }
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 编辑工地工作记录工人考勤记录
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditWorkerAttendanceAsync(EditWorkerAttendanceDto inputDto)
        {
            try
            {
                var workerData = await _repository.FindAsyncFirst(p => p.id == inputDto.id);
                if (workerData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("worker_null"));
                }

                workerData.cic_no = inputDto.cic_no;
                workerData.cic_card_no = inputDto.cic_card_no;
                workerData.time_in_adj = inputDto.time_in;
                workerData.time_out = inputDto.time_out;

                _repository.Update(workerData);
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
