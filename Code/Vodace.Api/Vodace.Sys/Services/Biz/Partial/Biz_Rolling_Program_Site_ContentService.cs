
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Me.Solutions.WorkingTimeSchedule.EndWorkingTime;
using Microsoft.Graph.Models;
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

namespace Vodace.Sys.Services
{
    public partial class Biz_Rolling_Program_Site_ContentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Rolling_Program_Site_ContentRepository _repository;//访问数据库
        private readonly IBiz_Rolling_Program_TaskRepository _taskRepository;//访问数据库
        private readonly IBiz_Rolling_ProgramRepository _programRepository;//访问数据库
        private readonly ISys_Construction_Content_InitRepository _initRepository;
        private readonly IBiz_SiteRepository _siteRepository;
        private readonly ILocalizationService _localizationService;

        [ActivatorUtilitiesConstructor]
        public Biz_Rolling_Program_Site_ContentService(
            IBiz_Rolling_Program_Site_ContentRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IBiz_Rolling_Program_TaskRepository taskRepository,
            IBiz_Rolling_ProgramRepository programRepository,
            ISys_Construction_Content_InitRepository initRepository,
            IBiz_SiteRepository siteRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _taskRepository = taskRepository;
            _programRepository = programRepository;
            _initRepository = initRepository;
            _siteRepository = siteRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public WebResponseContent Add(RollingProgramSiteContentAddDto dto)
        {
            try
            {
                if (dto.rollingProgramTaskDto == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_plan_empty"));
                }
                if (dto.rollingProgramSiteContentDtos.Count <= 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_plan_task_empty"));
                }

                var delete_status = (int)SystemDataStatus.Valid;
                var userId = UserContext.Current.UserId;
                var userName = UserContext.Current.UserName;
                var time = DateTime.Now;
                int site_line_number = 0;
                int program_line_number = 0;
                string empty = string.Empty;

                #region  -- task --

                var task = dto.rollingProgramTaskDto;
                Biz_Rolling_Program_Task model = new Biz_Rolling_Program_Task();
                model.id = Guid.NewGuid();
                model.delete_status = delete_status;
                model.create_id = userId;
                model.create_name = userName;
                model.create_date = time;

                //读取当前合约是否已存在版本号
                var list = _taskRepository.Find(a => a.delete_status == delete_status && a.project_id == task.project_id && a.contract_id == task.contract_id).OrderByDescending(a => a.create_date).ToList();
                if (list.Count > 0)
                    model.version = (int.Parse(list[0].version.Split('.')[0]) + 1) + ".0";    //每次数字追加
                else
                    model.version = "1.0";

                model.project_id = task.project_id;
                model.contract_id = task.contract_id;
                model.customer = task.customer;
                model.category = task.category;
                model.task_name = task.task_name;
                model.remark = task.remark;
                _taskRepository.Add(model);

                #endregion

                //读取初始化内容
                var lstInit = _initRepository.Find(a => a.delete_status == delete_status && a.work_type == ConstructionContentWorkTypeEnum.SiteWork).ToList();

                #region  -- site content --

                foreach (var item in dto.rollingProgramSiteContentDtos)
                {
                    site_line_number++;
                    Biz_Rolling_Program_Site_Content site_content = new Biz_Rolling_Program_Site_Content();
                    site_content.project_id = task.project_id;
                    site_content.contract_id = task.contract_id;
                    site_content.task_id = model.id;
                    site_content.site_id = item.site_id;
                    site_content.cc_id = item.cc_id;

                    var init = lstInit.Where(a => a.id == item.cc_id).FirstOrDefault();
                    site_content.item_code = init?.item_code;
                    site_content.content = init?.content;
                    site_content.work_type = init?.work_type;
                    site_content.point_type = init?.point_type;

                    site_content.number = item.number;
                    site_content.is_generate = item.is_generate;
                    site_content.line_number = site_line_number;

                    site_content.id = Guid.NewGuid();
                    site_content.delete_status = delete_status;
                    site_content.create_id = userId;
                    site_content.create_name = userName;
                    site_content.create_date = time;

                    _repository.Add(site_content);

                    //创建任务
                    if(item.is_generate == 1)
                    {
                        string rolling = AddRollingProgram(lstInit, (Guid)item.cc_id, (Guid)task.project_id, (Guid)task.contract_id, model.id, site_content.id, (Guid)item.site_id, ref program_line_number);
                        if(!string.IsNullOrEmpty(rolling))
                            return WebResponseContent.Instance.Error(rolling);
                    }
                }

                #endregion

                _repository.SaveChanges();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_Program_Site_ContentService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                var model = _repository.Find(p => p.id == guid).FirstOrDefault();
                if (model != null)
                {
                    //判断是否已存在滚动计划
                    var pro = _programRepository.Find(a => a.sc_id == model.task_id && a.delete_status == (int)SystemDataStatus.Valid).ToList();
                    if(pro.Count > 0)
                        return WebResponseContent.Instance.Error(_localizationService.GetString("rolling_program_not_allowed"));

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
                Log4NetHelper.Error("Biz_Rolling_Program_Site_ContentService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Edit(RollingProgramSiteContentAddDto dto)
        {
            try
            {
                if (dto.rollingProgramTaskDto == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_plan_empty"));
                }
                if (dto.rollingProgramSiteContentDtos.Count <= 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_plan_task_empty"));
                }

                var delete_status = (int)SystemDataStatus.Valid;
                var userId = UserContext.Current.UserId;
                var userName = UserContext.Current.UserName;
                var time = DateTime.Now;
                int site_line_number = 0;
                int program_line_number = 0;
                string empty = string.Empty;

                #region  -- task --

                var task = dto.rollingProgramTaskDto;
                var model = _taskRepository.Find(a => a.id == task.id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                if(model == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_plan_null"));

                //更改时，版本号不修改
                //var list = _taskRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid && a.project_id == task.project_id && a.contract_id == task.contract_id).OrderByDescending(a => a.create_date).ToList();
                //if (list.Count > 0)
                //    model.version = (int.Parse(list[0].version.Split('.')[0]) + 1) + ".0";    //每次数字追加
                //else
                //    model.version = "1.0";
                //model.project_id = task.project_id;
                //model.contract_id = task.contract_id;

                model.customer = task.customer;
                model.category = task.category;
                model.task_name = task.task_name;
                model.remark = task.remark;

                model.modify_id = userId;
                model.modify_name = userName;
                model.modify_date = time;
                _taskRepository.Update(model);

                #endregion

                //读取初始化内容
                var lstInit = _initRepository.Find(a => a.delete_status == delete_status && a.work_type == ConstructionContentWorkTypeEnum.SiteWork).ToList();
                var lstSiteContent = _repository.Find(a => a.delete_status == delete_status && a.task_id == task.id).ToList();

                #region  -- site content --

                foreach (var item in dto.rollingProgramSiteContentDtos)
                {
                    if (item.id.HasValue)
                    {
                        var site_content = lstSiteContent.Where(a => a.id == item.id).FirstOrDefault();
                        if (site_content != null)
                        {
                            site_content.site_id = item.site_id;
                            site_content.cc_id = item.cc_id;
                            var init = lstInit.Where(a => a.id == item.cc_id).FirstOrDefault();
                            site_content.item_code = init?.item_code;
                            site_content.content = init?.content;
                            site_content.work_type = init?.work_type;
                            site_content.point_type = init?.point_type;

                            site_content.number = item.number;
                            site_content.is_generate = item.is_generate;
                            site_content.line_number = site_line_number;    //序号重新更新

                            site_content.modify_id = userId;
                            site_content.modify_name = userName;
                            site_content.modify_date = time;
                            _repository.Update(site_content);

                            //原来不创建，但现在选择创建的
                            if(site_content.is_generate == 0 && item.is_generate == 1)
                            {
                                string rolling = AddRollingProgram(lstInit, (Guid)item.cc_id, (Guid)task.project_id, (Guid)task.contract_id, model.id, site_content.id, (Guid)item.site_id, ref program_line_number);
                                if (!string.IsNullOrEmpty(rolling))
                                    return WebResponseContent.Instance.Error(rolling);
                            }

                        }else return WebResponseContent.Instance.Error(_localizationService.GetString("contract_plan_task_empty")); 
                    }
                    else
                    {
                        site_line_number++;
                        Biz_Rolling_Program_Site_Content site_content = new Biz_Rolling_Program_Site_Content();
                        site_content.project_id = task.project_id;
                        site_content.contract_id = task.contract_id;
                        site_content.task_id = model.id;
                        site_content.site_id = item.site_id;
                        site_content.cc_id = item.cc_id;

                        var init = lstInit.Where(a => a.id == item.cc_id).FirstOrDefault();
                        site_content.item_code = init?.item_code;
                        site_content.content = init?.content;
                        site_content.work_type = init?.work_type;
                        site_content.point_type = init?.point_type;

                        site_content.number = item.number;
                        site_content.is_generate = item.is_generate;
                        site_content.line_number = site_line_number;

                        site_content.id = Guid.NewGuid();
                        site_content.delete_status = delete_status;
                        site_content.create_id = userId;
                        site_content.create_name = userName;
                        site_content.create_date = time;
                        _repository.Add(site_content);

                        //创建任务
                        if (item.is_generate == 1)
                        {
                            string rolling = AddRollingProgram(lstInit, (Guid)item.cc_id, (Guid)task.project_id, (Guid)task.contract_id, model.id, site_content.id, (Guid)item.site_id, ref program_line_number);
                            if (!string.IsNullOrEmpty(rolling))
                                return WebResponseContent.Instance.Error(rolling);
                        }
                    }
                }

                #endregion

                _repository.SaveChanges();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_Program_Site_ContentService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent GetRollingProgramSiteContentList(PageInput<RollingProgramSiteContentSearchDto> dto)
        {
            try
            {
                var search = dto.search;

                var taskQuery = _taskRepository.WhereIF(true, a => a.delete_status == (int)SystemDataStatus.Valid);
                taskQuery = taskQuery.WhereIF(search.project_id.HasValue, a => a.project_id == search.project_id);
                taskQuery = taskQuery.WhereIF(search.contract_id.HasValue, a => a.contract_id == search.contract_id);
                taskQuery = taskQuery.WhereIF(search.id.HasValue, a => a.id == search.id);
                taskQuery = taskQuery.WhereIF(!string.IsNullOrEmpty(search.version), a => a.version == search.version);
                var task = taskQuery.Select(a => new { a.id, a.customer, a.category, a.task_name, a.version}).ToList();
                var taskIds = taskQuery.Select(a => a.id).ToList();

                if (task.Count <= 0)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_plan_null"));

                var lstSite = _siteRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid).Select(a => new { a.id, a.name_sho }).ToList();
                var lstProgram = _programRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid && a.master_id == null && a.level == 1).Select(a => new { a.id, a.sc_id, a.level, a.item_code, a.content }).ToList();

                var querySiteContent = _repository.WhereIF(true, a => a.delete_status == (int)SystemDataStatus.Valid);
                querySiteContent = querySiteContent.WhereIF(search.project_id.HasValue, a => a.project_id == search.project_id);
                querySiteContent = querySiteContent.WhereIF(search.contract_id.HasValue, a => a.contract_id == search.contract_id);
                querySiteContent = querySiteContent.WhereIF(search.site_id.HasValue, a => a.site_id == search.site_id);
                querySiteContent = querySiteContent.Where(a => taskIds.Contains((Guid)a.task_id));
                var lstSieContent = querySiteContent.ToList();

                var result = new List<object>();
                if(lstSieContent.Count > 0)
                {
                    foreach(var item in lstSieContent)
                    {
                        var site = lstSite.Where(a => a.id == item.site_id).FirstOrDefault();
                        result.Add(new { 
                            item.id,
                            item.is_generate,
                            site_name = site?.name_sho,
                            item.item_code,
                            item.content,
                            item.work_type,
                            item.point_type,
                            item.number
                        });
                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), new { task = task, result = result });
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_Program_Site_ContentService.GetRollingProgramSiteContentList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public async Task<WebResponseContent> GetVersion(RollingProgramSiteContentDto dto)
        {
            try
            {
                var result = await _taskRepository.FindAsIQueryable(a => a.delete_status == (int)SystemDataStatus.Valid && a.contract_id == dto.contract_id)
                    .Select(a => a.version)
                    .ToListAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_Program_Site_ContentService.GetVersion", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #region  -- 私有 --

        private string AddRollingProgram(List<Sys_Construction_Content_Init> list, Guid id, Guid project_id, Guid contract_id, Guid task_id, Guid site_content_id, Guid site_id, ref int program_line_number)
        {
            string res = string.Empty;

            try
            {
                // 查找当前ID对应的节点
                var currentInit = list.FirstOrDefault(a => a.id == id);
                if (currentInit == null)
                {
                    return _localizationService.GetString("engineering_task_null"); // 如果找不到当前节点，直接返回
                }

                // 添加当前节点
                program_line_number++;
                Biz_Rolling_Program program = new Biz_Rolling_Program();
                program.project_id = project_id;
                program.contract_id = contract_id;
                program.task_id = task_id;
                program.sc_id = site_content_id;

                program.line_number = program_line_number;
                program.site_id = site_id.ToString();
                program.cc_id = currentInit.id;
                program.master_id = currentInit.master_id;   //根据循环获取
                program.level = currentInit.level;
                program.item_code = currentInit.item_code;
                program.content = currentInit.content;
                program.work_type = currentInit.work_type;
                program.point_type = currentInit.point_type;

                program.director = 0;
                program.quotation = 0;  //默认为0
                program.track_scope = 0;  //默认为0
                program.duty = res;
                program.color = res;
                program.org_id = res;
                program.exp_type = res;
                program.exp_value = res;
                //program.start_date = DateTime.Now;    //创建时未添加
                //program.end_date = DateTime.Now;      //创建时未添加

                program.id = Guid.NewGuid();
                program.delete_status = (int)SystemDataStatus.Valid;
                program.create_id = UserContext.Current.UserId;
                program.create_name = UserContext.Current.UserInfo.UserName;
                program.create_date = DateTime.Now;

                _programRepository.Add(program);

                // 递归添加所有子节点（master_id等于当前id的记录）
                var childNodes = list.Where(a => a.master_id.HasValue && a.master_id.Value == id).ToList();
                foreach (var child in childNodes)
                {
                    // 递归调用，继续添加子节点
                    string childRes = AddRollingProgram(list, child.id, project_id, contract_id, task_id, site_content_id, site_id, ref program_line_number);
                    if (!string.IsNullOrEmpty(childRes) && string.IsNullOrEmpty(res))
                    {
                        return childRes; // 只保留第一个错误信息
                    }
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_Program_Site_ContentService.AddRollingProgram", e);
                return _localizationService.GetString("system_error");
            }

            return res;
        }

        #endregion
    }
}
