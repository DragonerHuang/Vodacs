/*
 *所有关于Sys_QuartzOptions类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*Sys_QuartzOptionsService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
*/
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Hubs;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Quartz;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;


namespace Vodace.Sys.Services
{
    public partial class Sys_QuartzOptionsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_QuartzOptionsRepository _repository;//访问数据库
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly IHomePageMessageSender _messageSender;
        [ActivatorUtilitiesConstructor]
        public Sys_QuartzOptionsService(
            ISys_QuartzOptionsRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ISchedulerFactory schedulerFactory
,
            ILocalizationService localizationService,
            IMapper mapper,
            IHomePageMessageSender messageSender)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _schedulerFactory = schedulerFactory;
            _localizationService = localizationService;
            _mapper = mapper;
            _messageSender = messageSender;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public override PageGridData<Sys_QuartzOptions> GetPageData(PageDataOptions options)
        {
            var result = base.GetPageData(options);
            return result;
        }

        WebResponseContent webResponse = new WebResponseContent();
        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            AddOnExecuting = (Sys_QuartzOptions options, object list) =>
            {
                options.status = (int)TriggerState.Paused;
                return webResponse.OK();
            };
            Sys_QuartzOptions ops = null;
            AddOnExecuted = (Sys_QuartzOptions options, object list) =>
            {
                ops = options;
                return webResponse.OK();
            };
            var result = base.Add(saveDataModel);
            if (result.status)
            {
                ops.AddJob(_schedulerFactory).GetAwaiter().GetResult();
            }
            return result;
        }

        public override WebResponseContent Del(object[] keys, bool delList = true)
        {
            var ids = keys.Select(s => (Guid)(s.GetGuid())).ToArray();

            repository.FindAsIQueryable(x => ids.Contains(x.id)).ToList().ForEach(options =>
            {
                _schedulerFactory.Remove(options).GetAwaiter().GetResult();
            });

            return base.Del(keys, delList);
        }

        public override WebResponseContent Update(SaveModel saveModel)
        {

            UpdateOnExecuted = (Sys_QuartzOptions options, object addList, object updateList, List<object> delKeys) =>
            {
                _schedulerFactory.Update(options).GetAwaiter().GetResult();
                return webResponse.OK();
            };
            return base.Update(saveModel);
        }

        /// <summary>
        /// 手动执行一次
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public async Task<object> Run(Sys_QuartzOptions taskOptions)
        {
            return await _schedulerFactory.Run(taskOptions);
        }
        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public async Task<object> Start(Sys_QuartzOptions taskOptions)
        {
            var result = await _schedulerFactory.Start(taskOptions);
            if (taskOptions.status != (int)TriggerState.Normal)
            {
                taskOptions.status = (int)TriggerState.Normal;
                _repository.Update(taskOptions, x => new { x.status }, true);
            }
            return result;
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public async Task<object> Pause(Sys_QuartzOptions taskOptions)
        {
            //  var result = await _schedulerFactory.Remove(taskOptions);
            var result = await _schedulerFactory.Pause(taskOptions);
            taskOptions.status = (int)TriggerState.Paused;
            _repository.Update(taskOptions, x => new { x.status }, true);
            return result;
        }

        public async void SendMsg(string userId,string title,string message)
        {
            Log4NetHelper.Info($"消息推送：接收人{userId}，标题：{title}，消息：{message}");
            await _messageSender.SendMessageToUser(userId, title,message);
        }

        public async Task<WebResponseContent> GetListByPage(PageInput<TaskQuery> query)
        {
            PageGridData<TaskListDto> pageGridData = new PageGridData<TaskListDto>();
            var queryPam = query.search;
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid &&
            (queryPam.status == -1 || queryPam.status == null ? true : d.status == queryPam.status) &&
            (string.IsNullOrEmpty(queryPam.task_name) ? true : d.task_name.Contains(queryPam.task_name))).Select(d => new TaskListDto
            {
                id = d.id,
                task_name = d.task_name,
                group_name = d.group_name,
                api_url = d.api_url,
                auth_key = d.auth_key,
                auth_value = d.auth_value,
                cron_expression = d.cron_expression,
                describe = d.describe,
                last_run_time = d.last_run_time,
                method = d.method,
                status = d.status,
                time_out = d.time_out,
                post_data = d.post_data,
                create_date = d.create_date,
                create_name = d.create_name,
                modify_date = d.modify_date,
                modify_name = d.modify_name
            });
            query.sort_field = "task_name";
            query.sort_type = "asc";
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        public WebResponseContent Add(TaskAddDto dto)
        {
            try
            {
                if (dto == null) return WebResponseContent.Instance.Error(_localizationService.GetString("com_name_exist！"));
                var checkComName = _repository.Exists(d => d.task_name == dto.task_name);
                if (checkComName) return WebResponseContent.Instance.Error(_localizationService.GetString("task_name_exist！"));

                Sys_QuartzOptions quaetz = _mapper.Map<Sys_QuartzOptions>(dto);
                quaetz.status = (int)AuditEnum.WaitAudit;
                quaetz.delete_status = (int)SystemDataStatus.Valid;
                quaetz.id = Guid.NewGuid();
                quaetz.create_date = DateTime.Now;
                quaetz.create_name = UserContext.Current.UserName;
                _repository.Add(quaetz, true);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), quaetz);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent Edit(TaskEditDto dto)
        {
            try
            {
                if (dto.id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var oldData = _repository.Find(d => d.id == dto.id).FirstOrDefault();
                if (oldData != null)
                {
                    oldData.task_name = dto.task_name;
                    oldData.group_name = dto.group_name;
                    oldData.api_url = dto.api_url;
                    oldData.auth_key = dto.auth_key;
                    oldData.auth_value = dto.auth_value;
                    oldData.cron_expression = dto.cron_expression;
                    oldData.describe = dto.describe;
                    //oldData.last_run_time = dto.last_run_time;
                    oldData.method = dto.method;
                    oldData.status = dto.status;
                    oldData.time_out = dto.time_out;
                    oldData.post_data = dto.post_data;
                    oldData.modify_date = DateTime.Now;
                    oldData.modify_id = UserContext.Current.UserId;
                    oldData.modify_name = UserContext.Current.UserName;
                    var res = _repository.Update(oldData, true);
                    if (res > 0) {
                        _schedulerFactory.Update(oldData).GetAwaiter().GetResult();
                        return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                    }
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> SwitchStatus(Guid id) 
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var oldData = _repository.Find(d => d.id == id).FirstOrDefault();
                oldData.status = oldData.status == 0 ? 1 : 0;
                oldData.modify_date = DateTime.Now;
                oldData.modify_id = UserContext.Current.UserId;
                oldData.modify_name = UserContext.Current.UserName;
                var res = _repository.Update(oldData, true);
                if (res > 0)
                {
                    if (oldData.status == 0) { await _schedulerFactory.Pause(oldData); }
                    else { await _schedulerFactory.Start(oldData); }
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                else return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent DelData(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var entModel = _repository.FindAsIQueryable(x => x.id == id).FirstOrDefault();
                if (UserContext.Current.IsSuperAdmin)
                {
                    entModel.delete_status = (int)SystemDataStatus.Invalid;
                    entModel.modify_name = UserContext.Current.UserName;
                    entModel.modify_date = DateTime.Now;
                    var res = _repository.Update(entModel, true);
                    if (res > 0) 
                    {
                        _schedulerFactory.Remove(entModel).GetAwaiter().GetResult();
                    }
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                }
                else
                {
                    //权限不足
                    return WebResponseContent.Instance.Error(_localizationService.GetString("insufficient_permissions"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }
    }
}
