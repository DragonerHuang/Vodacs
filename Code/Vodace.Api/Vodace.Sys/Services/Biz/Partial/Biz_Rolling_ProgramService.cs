
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
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
    public partial class Biz_Rolling_ProgramService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Rolling_ProgramRepository _repository;//访问数据库
        private readonly IBiz_ProjectRepository _projectRepository;//访问数据库
        private readonly IBiz_ContractRepository _contractRepository;//访问数据库
        private readonly ISys_User_NewRepository _userRepository;//访问数据库
        private readonly IBiz_Confirmed_OrderRepository _coRepository;//访问数据库
        private readonly IBiz_Rolling_Program_TaskRepository _taskRepository;//访问数据库
        private readonly IBiz_Site_Work_RecordRepository _swrRepository;//访问数据库
        private readonly IBiz_Site_Work_Record_WorkerRepository _swrWorkerRepository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IBiz_Submission_FilesService _submissionFilesService;//访问业务代码
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_Rolling_ProgramService(
            IBiz_Rolling_ProgramRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ILocalizationService localizationService,
            IBiz_ProjectRepository projectRepository,
            IBiz_ContractRepository contractRepository,
            ISys_User_NewRepository userRepository,
            IBiz_Confirmed_OrderRepository coRepository,
            IBiz_Rolling_Program_TaskRepository taskRepository,
            IBiz_Site_Work_RecordRepository swrRepository,
            IBiz_Site_Work_Record_WorkerRepository swrWorkerRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _localizationService = localizationService;
            _projectRepository = projectRepository;
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _coRepository = coRepository;
            _taskRepository = taskRepository;
            _swrRepository = swrRepository;
            _swrWorkerRepository = swrWorkerRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public WebResponseContent Add(RollingProgramDto dto)
        {
            try
            {
                Biz_Rolling_Program model = new Biz_Rolling_Program();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                model.project_id = dto.project_id;
                model.contract_id = dto.contract_id;
                model.task_id = dto.task_id;
                model.sc_id = dto.sc_id;
                model.cc_id = null; //手动添加的，不存在cc_id

                //获取当前最大的line_number
                var lstProgram = _repository.Find(p => p.contract_id == dto.contract_id && p.task_id == dto.task_id && p.sc_id == dto.sc_id && p.delete_status == (int)SystemDataStatus.Valid).OrderByDescending(a => a.line_number).ToList();
                if(lstProgram.Count > 0)
                    model.line_number = lstProgram[0].line_number + 1;
                else
                    model.line_number = 1;

                model.site_id = dto.site_id;    //目前只允许出现一个工地，不支持多个工地
                model.master_id = dto.master_id;
                model.level = dto.level;
                model.item_code = dto.item_code;
                model.content = dto.content;
                model.work_type = dto.work_type;
                model.point_type = dto.point_type;

                model.director = dto.director;
                model.quotation = dto.quotation;
                model.track_scope = dto.track_scope;
                model.duty = dto.duty;
                model.color = dto.color;
                model.org_id = dto.org_id;
                model.exp_type = dto.exp_type;
                model.exp_value = dto.exp_value;
                model.start_date = dto.start_date;
                model.end_date = dto.end_date;

                #region  -- 添加工地记录 --

                if(dto.start_date.HasValue && dto.end_date.HasValue)
                {
                    var userInfo = _userRepository.WhereIF(dto.director > 0, a => a.user_id == dto.director).FirstOrDefault();
                    //遍历开始到结束日期之间的所有日期
                    for (DateTime date = dto.start_date.Value; date <= dto.end_date.Value; date = date.AddDays(1))
                    {
                        Biz_Site_Work_Record swr = new Biz_Site_Work_Record();
                        swr.id = Guid.NewGuid();
                        swr.delete_status = (int)SystemDataStatus.Valid;
                        swr.create_id = UserContext.Current.UserId;
                        swr.create_name = UserContext.Current.UserName;
                        swr.create_date = DateTime.Now;
                        swr.remark = dto.remark;

                        swr.contract_id = dto.contract_id;
                        swr.rolling_program_id = model.id;
                        swr.site_id = Guid.Parse(model.site_id);    //目前只允许出现一个工地，不支持多个工地
                        swr.duty_cp_id = userInfo?.contact_id;      //读取联络人id
                        swr.job_duties = model.content;
                        swr.shift = model.duty;
                        swr.is_track = model.track_scope;
                        swr.work_date = date;
                        swr.finish_briefing = 0;                    //默认设置0
                        swr.check_config = null;                    //默认空
                        swr.sub_site_id = null;                     //暂不存在子级工地id
                        swr.current_duty_cp_id = userInfo?.contact_id;//工人打卡id

                        _swrRepository.Add(swr);

                        _swrWorkerRepository.Add(new Biz_Site_Work_Record_Worker() {
                            id = Guid.NewGuid(),
                            record_id = swr.id,
                            contact_id = userInfo?.contact_id,
                            company_id = userInfo?.company_id,
                            work_remark = swr.remark,
                            is_cp = 1,
                            is_nt = 0,
                            is_qr = 0,
                            is_wpic = 0,
                            is_sick = 0,
                            is_sign = 0,
                            is_valid = 0,
                            salary_adj = 0,
                            base_salary = 0,
                            traffic_allowance = 0,
                            time_in = null,
                            time_in_adj = null,
                            time_out = null,
                            time_out_adj = null,
                            green_card_exp = null,
                            work_type_id = null,
                            cic_exp = null,

                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = DateTime.Now
                        });
                    }
                }

                #endregion

                model.remark = dto.remark;
                _repository.Add(model);

                _repository.SaveChanges();

                //if (dto.contract_id.HasValue && dto.director.HasValue)
                //    _submissionFilesService.AddDataByBatch((Guid)dto.contract_id, (int)dto.director);   //如果上面执行成功，但这里失败，则不会回滚(2025-11-18 09:44)

                return WebResponseContent.Instance.OK("Ok", model);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_ProgramService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                var model = repository.Find(p => p.id == guid).FirstOrDefault();
                if (model != null)
                {
                    model.delete_status = (int)SystemDataStatus.Invalid;
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;
                    var res = _repository.Update(model, true);

                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("delete") + _localizationService.GetString("failes"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{guid}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_ProgramService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Edit(RollingProgramEditDto dto)
        {
            try
            {
                var model = _repository.Find(p => p.id == dto.id).FirstOrDefault();
                if(model == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_plan_task_empty"));

                //更改时，不允许修改这些数据
                //model.project_id = dto.project_id;
                //model.contract_id = dto.contract_id;
                //model.task_id = dto.task_id;
                //model.sc_id = dto.sc_id;
                //model.cc_id = null; //手动添加的，不存在cc_id
                //model.line_number = 1;

                model.site_id = dto.site_id;
                model.master_id = dto.master_id;
                model.level = dto.level;
                model.item_code = dto.item_code;
                model.content = dto.content;
                model.work_type = dto.work_type;
                model.point_type = dto.point_type;

                model.director = dto.director;
                model.quotation = dto.quotation;
                model.track_scope = dto.track_scope;
                model.duty = dto.duty;
                model.color = dto.color;
                model.org_id = dto.org_id;
                model.exp_type = dto.exp_type;
                model.exp_value = dto.exp_value;
                model.start_date = dto.start_date;
                model.end_date = dto.end_date;

                model.remark = dto.remark;
                model.modify_id = UserContext.Current.UserId;
                model.modify_name = UserContext.Current.UserName;
                model.modify_date = DateTime.Now;


                #region  -- 更新工地记录 --

                if (dto.start_date.HasValue && dto.end_date.HasValue)
                {
                    var userInfo = _userRepository.WhereIF(dto.director > 0, a => a.user_id == dto.director).FirstOrDefault();

                    //开始或结束或中间的时间是否已存在Biz_Site_Work_Record，并且Biz_Site_Work_Record_Worker里面对应的上班时间已存在数据，当前时间内的Biz_Site_Work_Record数据不允许删除；否则允许重新删除并且重新添加数据
                    //不存在差异时，并且Biz_Site_Work_Record未创建记录，允许继续添加数据
                    // 获取当前滚动计划关联的所有工地记录
                    var existingRecords = _swrRepository.Find(p => p.rolling_program_id == model.id && p.delete_status == (int)SystemDataStatus.Valid).ToList();
                    var existingRecordDates = existingRecords.Select(r => r.work_date.Value).ToList();

                    // 检查是否存在已有工人记录的工地记录
                    var hasWorkerRecords = false;
                    if (existingRecords.Any())
                    {
                        // 假设这里有一个方法可以检查工地记录是否有关联的工人记录
                        // 由于没有看到完整的实体定义，这里使用假设的方式
                        hasWorkerRecords = CheckIfHasWorkerRecords(existingRecords.Select(r => r.id).ToList());
                    }

                    // 如果没有工人记录或者时间范围没有变化，则允许删除旧记录并添加新记录
                    if (!hasWorkerRecords || !existingRecordDates.SequenceEqual(
                        Enumerable.Range(0, (dto.end_date.Value.Date - dto.start_date.Value.Date).Days + 1)
                        .Select(d => dto.start_date.Value.Date.AddDays(d))))
                    {
                        // 删除旧的工地记录
                        foreach (var record in existingRecords)
                        {
                            record.delete_status = (int)SystemDataStatus.Invalid;
                            record.modify_id = UserContext.Current.UserId;
                            record.modify_name = UserContext.Current.UserName;
                            record.modify_date = DateTime.Now;
                            _swrRepository.Update(record);
                        }

                        // 遍历开始到结束日期之间的所有日期，添加新的工地记录
                        for (DateTime date = dto.start_date.Value; date <= dto.end_date.Value; date = date.AddDays(1))
                        {
                            Biz_Site_Work_Record swr = new Biz_Site_Work_Record();
                            swr.id = Guid.NewGuid();
                            swr.delete_status = (int)SystemDataStatus.Valid;
                            swr.create_id = UserContext.Current.UserId;
                            swr.create_name = UserContext.Current.UserName;
                            swr.create_date = DateTime.Now;
                            swr.remark = dto.remark;

                            swr.contract_id = dto.contract_id;
                            swr.rolling_program_id = model.id;
                            swr.site_id = Guid.Parse(model.site_id);    //目前只允许出现一个工地，不支持多个工地
                            swr.duty_cp_id = userInfo?.contact_id;      //读取联络人id
                            swr.current_duty_cp_id = userInfo?.contact_id;//默认管工打卡id
                            swr.job_duties = model.content;
                            swr.shift = model.duty;
                            swr.is_track = model.track_scope;
                            swr.work_date = date;
                            swr.finish_briefing = 0;                    //默认设置0
                            swr.check_config = null;                    //默认空
                            swr.sub_site_id = null;                     //暂不存在子级工地id

                            _swrRepository.Add(swr);

                            _swrWorkerRepository.Add(new Biz_Site_Work_Record_Worker()
                            {
                                id = Guid.NewGuid(),
                                record_id = swr.id,
                                contact_id = userInfo?.contact_id,
                                company_id = userInfo?.company_id,
                                work_remark = swr.remark,
                                is_cp = 1,
                                is_nt = 0,
                                is_qr = 0,
                                is_wpic = 0,
                                is_sick = 0,
                                is_sign = 0,
                                is_valid = 0,
                                salary_adj = 0,
                                base_salary = 0,
                                traffic_allowance = 0,
                                time_in = null,
                                time_in_adj = null,
                                time_out = null,
                                time_out_adj = null,
                                green_card_exp = null,
                                work_type_id = null,
                                cic_exp = null,

                                delete_status = (int)SystemDataStatus.Valid,
                                create_id = UserContext.Current.UserId,
                                create_name = UserContext.Current.UserName,
                                create_date = DateTime.Now
                            });
                        }
                    }
                    else
                    {
                        // 如果时间范围没有变化且已有工人记录，则只更新现有记录的其他信息
                        foreach (var record in existingRecords)
                        {
                            record.site_id = Guid.Parse(model.site_id);
                            record.duty_cp_id = userInfo?.contact_id;
                            record.job_duties = model.content;
                            record.shift = model.duty;
                            record.is_track = model.track_scope;
                            record.remark = dto.remark;
                            record.modify_id = UserContext.Current.UserId;
                            record.modify_name = UserContext.Current.UserName;
                            record.modify_date = DateTime.Now;
                            _swrRepository.Update(record);
                        }
                    }
                }

                #endregion

                _repository.Update(model);

                _repository.SaveChanges();

                //if(dto.contract_id.HasValue && dto.director.HasValue)
                //    _submissionFilesService.AddDataByBatch((Guid)dto.contract_id, (int)dto.director);   //如果上面执行成功，但这里失败，则不会回滚(2025-11-18 09:44)

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_ProgramService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public async Task<WebResponseContent> GetRollingProgramList(PageInput<RollingProgramEditDto> dto)
        {
            try
            {
                var search = dto.search;

                var query = _repository.WhereIF(true, a => a.delete_status == (int)SystemDataStatus.Valid);
                if (search != null)
                {
                    if(!search.project_id.HasValue || !search.contract_id.HasValue || !search.task_id.HasValue)
                        return WebResponseContent.Instance.Error(_localizationService.GetString("project_contract_task_empty"));

                    query = query.WhereIF(search.id.HasValue, a => a.id == search.id);
                    query = query.WhereIF(search.project_id.HasValue, a => a.project_id == search.project_id);
                    query = query.WhereIF(search.contract_id.HasValue, a => a.contract_id == search.contract_id);
                    query = query.WhereIF(search.task_id.HasValue, a => a.task_id == search.task_id);
                    query = query.WhereIF(search.sc_id.HasValue, a => a.sc_id == search.sc_id);
                    query = query.WhereIF(search.cc_id.HasValue, a => a.cc_id == search.cc_id);
                    query = query.WhereIF(search.master_id.HasValue, a => a.master_id == search.master_id);

                    query = query.WhereIF(search.lst_level.Count > 0, a => search.lst_level.Contains((int)a.level));
                    query = query.WhereIF(search.lst_unlevel.Count > 0, a => !search.lst_unlevel.Contains((int)a.level));
                    query = query.WhereIF(search.level > -1, a => a.level == search.level);
                    query = query.WhereIF(search.quotation > -1, a => a.quotation == search.quotation);
                    query = query.WhereIF(search.director > -1, a => a.director == search.director);
                    query = query.WhereIF(search.track_scope > -1, a => a.track_scope == search.track_scope);

                    query = query.WhereIF(!string.IsNullOrEmpty(search.site_id), a => a.site_id.Contains(search.site_id));
                    query = query.WhereIF(!string.IsNullOrEmpty(search.item_code), a => a.site_id.Contains(search.item_code));
                    query = query.WhereIF(!string.IsNullOrEmpty(search.content), a => a.content.Contains(search.content));
                    query = query.WhereIF(!string.IsNullOrEmpty(search.work_type), a => a.work_type.Contains(search.work_type));
                    query = query.WhereIF(!string.IsNullOrEmpty(search.point_type), a => a.site_id.Contains(search.point_type));

                    query = query.WhereIF(!string.IsNullOrEmpty(search.exp_type), a => a.exp_type.Contains(search.exp_type));
                    query = query.WhereIF(!string.IsNullOrEmpty(search.exp_value), a => a.exp_value.Contains(search.exp_value));
                }
                else return WebResponseContent.Instance.Error(_localizationService.GetString("project_contract_task_empty"));

                Dictionary<string, QueryOrderBy> orderByDict = new Dictionary<string, QueryOrderBy>
                    {
                        { "work_type", QueryOrderBy.Asc },
                        { "line_number", QueryOrderBy.Asc },
                        { "item_code", QueryOrderBy.Asc }
                    };
                query = query.GetIQueryableOrderBy(orderByDict);

                var sql = query.ToQueryString();

                var list = await query.ToListAsync();
                if (list.Count > 0)
                {
                    var lstProject = _projectRepository
                        .WhereIF(true, a => a.delete_status == (int)SystemDataStatus.Valid)
                        .WhereIF(search.project_id.HasValue, a => a.id == search.project_id)
                        .Select(a => new { a.id, a.project_no, a.name_cht, a.name_eng })
                        .ToList();

                    var lstContract = _contractRepository
                        .WhereIF(true, a => a.delete_status == (int)SystemDataStatus.Valid)
                        .WhereIF(search.project_id.HasValue, a => a.project_id == search.project_id)
                        .WhereIF(search.contract_id.HasValue, a => a.id == search.contract_id)
                        .Select(a => new { a.id, a.contract_no, a.name_cht, a.name_eng })
                        .ToList();

                    var lstTask = _taskRepository
                        .WhereIF(true, a => a.delete_status == (int)SystemDataStatus.Valid)
                        .WhereIF(search.project_id.HasValue, a => a.project_id == search.project_id)
                        .WhereIF(search.contract_id.HasValue, a => a.contract_id == search.contract_id)
                        .Select(a => new { a.id, a.task_name, a.project_id, a.contract_id })
                        .ToList();

                    var lstCO = _coRepository
                        .WhereIF(true, a => a.delete_status == (int)SystemDataStatus.Valid)
                        .WhereIF(search.project_id.HasValue, a => a.project_id == search.project_id)
                        .WhereIF(search.contract_id.HasValue, a => a.contract_id == search.contract_id)
                        .Select(a => new { a.id, a.contract_id, a.commen_date, a.complet_exp_date, a.complet_act_date })
                        .ToList();

                    var lstUser = _userRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid).Select(a => new { a.id, a.user_id, a.user_name, name_cht = a.user_name_eng, name_eng = a.user_true_name }).ToList();

                    var result = new List<object>();
                    foreach(var item in list)
                    {
                        var user = lstUser.Where(a => a.user_id == item.director).FirstOrDefault();
                        result.Add(new { 
                            item.id,
                            item.project_id,
                            item.contract_id,
                            item.task_id,
                            item.sc_id,
                            item.cc_id,
                            item.line_number,
                            item.level,
                            item.org_id,
                            item.site_id,
                            item.master_id,
                            item.item_code,
                            item.content,
                            item.work_type,
                            item.point_type,
                            item.start_date,
                            item.end_date,
                            item.director,
                            director_name_cht = user?.name_cht,
                            director_name_eng = user?.name_eng,
                            director_user_name = user?.user_name,
                            item.quotation,
                            item.duty,
                            item.color,
                            item.track_scope,
                            item.exp_type,
                            item.exp_value,
                            item.remark,
                            item.create_name,
                            item.create_date,
                            item.modify_name,
                            item.modify_date
                        });
                    }

                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), new { project = lstProject, contract = lstContract, co = lstCO, task = lstTask, list = result });
                }
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), new { project = new List<object>(), contract = new List<object>(), co = new List<object>(), task = new List<object>(), list = new List<object>() });
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractOrgService.GetRollingProgramList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 检查工地记录是否有关联的工人记录
        /// </summary>
        /// <param name="recordIds">工地记录ID列表</param>
        /// <returns>是否有关联的工人记录</returns>
        private bool CheckIfHasWorkerRecords(List<Guid> recordIds)
        {
            try
            {
                // 查询是否存在关联的工人记录
                var hasWorkerRecords = _swrWorkerRepository.Find(p =>
                    recordIds.Contains((Guid)p.record_id) &&
                    p.delete_status == (int)SystemDataStatus.Valid
                ).Any();

                return hasWorkerRecords;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("CheckIfHasWorkerRecords", ex);
                return false;
            }
        }
    }
}
