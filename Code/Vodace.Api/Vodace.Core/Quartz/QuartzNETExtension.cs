using MathNet.Numerics.LinearAlgebra.Factorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vodace.Core.EFDbContext;
using Vodace.Core.Quartz;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;

namespace Vodace.Core.Quartz
{
    public static class QuartzNETExtension
    {
        private static List<Sys_QuartzOptions> _taskList = new List<Sys_QuartzOptions>();

        /// <summary>
        /// 初始化作业
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        //public static IApplicationBuilder UseQuartz(this IApplicationBuilder applicationBuilder, IWebHostEnvironment env)
        //{
        //    IServiceProvider services = applicationBuilder.ApplicationServices;
        //    ISchedulerFactory _schedulerFactory = services.GetService<ISchedulerFactory>();
        //    try
        //    {
        //        _taskList = services.GetService<VOLContext>().Set<Sys_QuartzOptions>().Where(x => true).ToList();

        //        _taskList.ForEach(options =>
        //        {
        //            options.AddJob(_schedulerFactory, jobFactory: services.GetService<IJobFactory>()).GetAwaiter().GetResult();
        //        });
        //    }
        //    catch (Exception ex)
        //    { 
        //        Console.WriteLine($"作业启动异常:{ex.Message + ex.StackTrace}");
        //    }
        //    return applicationBuilder;
        //}

        public static IApplicationBuilder UseQuartz(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var services = app.ApplicationServices;
            var schedulerFactory = services.GetService<ISchedulerFactory>();
            var jobFactory = services.GetService<IJobFactory>();
            var dbContext = services.GetService<VOLContext>();

            try
            {
                var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
                scheduler.JobFactory = jobFactory;

                // 从数据库读取任务配置
                _taskList = dbContext.Set<Sys_QuartzOptions>().Where(d=>d.delete_status == 0 && d.status ==1).ToList();

                foreach (var options in _taskList)
                {
                    options.AddJob(schedulerFactory, jobFactory).GetAwaiter().GetResult();
                }

                // ✅ 关键启动
                scheduler.Start().GetAwaiter().GetResult();

                Console.WriteLine("✅ Quartz 定时任务启动成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Quartz启动异常: {ex.Message}\n{ex.StackTrace}");
            }
            return app;
        }

        private static async Task<bool> CheckTask(Sys_QuartzOptions taskOptions, ISchedulerFactory schedulerFactory)
        {
            string groupName = "group";
            string taskName = taskOptions.id.ToString();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            List<JobKey> jobKeys = (await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName))).ToList();
            if (jobKeys == null || jobKeys.Count() == 0)
            {
                return false;
            }
            JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result
                            .Any(x => (x as CronTriggerImpl).Name == taskName))
                            .FirstOrDefault();
            if (jobKey == null)
            {
                return false;
            }
            var triggers = await scheduler.GetTriggersOfJob(jobKey);
            ITrigger trigger = triggers?.Where(x => (x as CronTriggerImpl).Name == taskName).FirstOrDefault();

            if (trigger == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 添加作业
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <param name="schedulerFactory"></param>
        /// <param name="init">是否初始化,否=需要重新生成配置文件，是=不重新生成配置文件</param>
        /// <returns></returns>
        public static async Task<object> AddJob(this Sys_QuartzOptions taskOptions, ISchedulerFactory schedulerFactory, IJobFactory jobFactory = null)
        {
            string msg = null;
            try
            {
                if (await CheckTask(taskOptions, schedulerFactory))
                {
                    await schedulerFactory.TriggerAction(JobAction.开启, taskOptions);
                    return new { status = true };
                }
                if (!_taskList.Exists(x => x.id == taskOptions.id))
                {
                    _taskList.Add(taskOptions);
                }
                else
                {
                    _taskList = _taskList.Where(x => x.id != taskOptions.id).ToList();
                    _taskList.Add(taskOptions);
                }
                taskOptions.group_name = "group";
                (bool, string) validExpression = taskOptions.cron_expression.IsValidExpression();
                if (!validExpression.Item1)
                {
                    msg = $"添加作业失败，作业:{taskOptions.task_name},表达式不正确:{ taskOptions.cron_expression}";
                    Console.WriteLine(msg);
                    QuartzFileHelper.Error(msg);
                    return new { status = false, msg = validExpression.Item2 };
                }

                IJobDetail job = JobBuilder.Create<HttpResultfulJob>()
               .WithIdentity(taskOptions.id.ToString(), "group").Build();
                ITrigger trigger = TriggerBuilder.Create()
                   .WithIdentity(taskOptions.id.ToString(), "group")
                   // .st()
                   .WithDescription(taskOptions.describe)
                   .WithCronSchedule(taskOptions.cron_expression)
                   .Build();

                IScheduler scheduler = await schedulerFactory.GetScheduler();

                if (jobFactory == null)
                {
                    jobFactory = HttpContext.Current.RequestServices.GetService<IJobFactory>();
                }

                if (jobFactory != null)
                {
                    scheduler.JobFactory = jobFactory;
                }

                await scheduler.ScheduleJob(job, trigger);
                Console.WriteLine(taskOptions.status);
                //if (taskOptions.status == 0)
                //{
                    Console.WriteLine("启动了：" + taskOptions.task_name);
                    await scheduler.Start();
                //}
                //else
                //{
                //    await scheduler.PauseJob(trigger.JobKey);
                //}
                msg = $"作业启动:{taskOptions.task_name}";
                Console.WriteLine(msg);
                QuartzFileHelper.Error(msg);
            }
            catch (Exception ex)
            {
                msg = $"作业启动异常:{taskOptions.task_name},{ex.Message}";
                Console.WriteLine(msg);
                QuartzFileHelper.Error(msg);
                return new { status = false, msg = ex.Message };
            }
            return new { status = true };
        }

        /// <summary>
        /// 移除作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static Task<WebResponseContent> Remove(this ISchedulerFactory schedulerFactory, Sys_QuartzOptions taskOptions)
        {
            return schedulerFactory.TriggerAction(JobAction.删除, taskOptions);
        }

        /// <summary>
        /// 更新作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public static Task<WebResponseContent> Update(this ISchedulerFactory schedulerFactory, Sys_QuartzOptions taskOptions)
        {
            return schedulerFactory.TriggerAction(JobAction.修改, taskOptions);
        }

        /// <summary>
        /// 暂停作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public static Task<WebResponseContent> Pause(this ISchedulerFactory schedulerFactory, Sys_QuartzOptions taskOptions)
        {
            return schedulerFactory.TriggerAction(JobAction.暂停, taskOptions);
        }

        /// <summary>
        /// 启动作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public static Task<WebResponseContent> Start(this ISchedulerFactory schedulerFactory, Sys_QuartzOptions taskOptions)
        {
            return schedulerFactory.TriggerAction(JobAction.开启, taskOptions);
            //  return taskOptions.AddJob(schedulerFactory);
            //  return  schedulerFactory.TriggerAction(JobAction.开启, taskOptions);
        }

        /// <summary>
        /// 立即执行一次作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public static async Task<WebResponseContent> Run(this ISchedulerFactory schedulerFactory, Sys_QuartzOptions taskOptions)
        {
            //var data = WebResponseContent.Instance.OK("",await schedulerFactory.TriggerAction(JobAction.立即执行, taskOptions));
            return await schedulerFactory.TriggerAction(JobAction.立即执行, taskOptions);
        }

        /// <summary>
        /// 触发新增、删除、修改、暂停、启用、立即执行事件
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <param name="action"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public static async Task<WebResponseContent> TriggerAction(
            this ISchedulerFactory schedulerFactory,
            JobAction action,
            Sys_QuartzOptions taskOptions = null)
        {
            string errorMsg = "";
            try
            {
                //string groupName = string.IsNullOrEmpty( taskOptions.group_name)? "group": taskOptions.group_name;
                string groupName ="group";
                string taskName = taskOptions.id.ToString();
                IScheduler scheduler = await schedulerFactory.GetScheduler();
                List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)).Result.ToList();
                if (jobKeys == null || jobKeys.Count() == 0)
                {
                    errorMsg = $"未找到分组[{groupName}]";
                    return WebResponseContent.Instance.Error(errorMsg); //new { status = false, msg = errorMsg };
                }
                JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result
                                .Any(x => (x as CronTriggerImpl).Name == taskName))
                                .FirstOrDefault();
                if (jobKey == null)
                {
                    errorMsg = $"未找到触发器[{taskName}]";
                    return WebResponseContent.Instance.Error(errorMsg);//new { status = false, msg = errorMsg };
                }
                var triggers = await scheduler.GetTriggersOfJob(jobKey);
                ITrigger trigger = triggers?.Where(x => (x as CronTriggerImpl).Name == taskName).FirstOrDefault();

                if (trigger == null)
                {
                    errorMsg = $"未找到触发器[{taskName}]";
                    return WebResponseContent.Instance.Error(errorMsg); //new { status = false, msg = errorMsg };
                }
                object result = null;
                switch (action)
                {
                    case JobAction.删除:

                        await scheduler.PauseTrigger(trigger.Key);
                        await scheduler.UnscheduleJob(trigger.Key);// 移除触发器
                        await scheduler.DeleteJob(trigger.JobKey);
                        break;
                    case JobAction.修改:
                        CronExpression cron = new CronExpression(taskOptions.cron_expression);
                        trigger = trigger.GetTriggerBuilder()
                            .WithIdentity(taskOptions.id.ToString(), "group")
                            .WithSchedule(CronScheduleBuilder.CronSchedule(cron))
                            .Build();
                        // 更新触发器
                        _taskList.ForEach(x =>
                        {
                            if (x.id == taskOptions.id)
                            {
                                x.task_name = taskOptions.task_name;
                                x.api_url = taskOptions.api_url;
                                x.auth_key = taskOptions.auth_key;
                                x.auth_value = taskOptions.auth_value;
                                x.describe = taskOptions.describe;
                                x.post_data = taskOptions.post_data;
                                x.method = taskOptions.method;
                                x.describe = taskOptions.describe;
                            }
                        });
                        await scheduler.RescheduleJob(trigger.Key, trigger);
                        break;
                    case JobAction.暂停:
                        await scheduler.PauseJob(jobKey);
                        if (action == JobAction.暂停)
                        {
                            taskOptions.status = (int)JobAction.暂停;
                            Log4NetHelper.Info($"Quartz框架-暂停任务：{taskName}");
                        }
                        break;
                    case JobAction.立即执行:
                    case JobAction.开启:
                        TriggerState state = await scheduler.GetTriggerState(new TriggerKey(jobKey.Name));

                        Log4NetHelper.Info($"Quartz框架-启动任务：{taskName}");
                        if (state == TriggerState.None)
                        {
                            await scheduler.Start();
                        }
                        if (action == JobAction.立即执行)
                        {
                            await scheduler.TriggerJob(jobKey);
                        }
                        else
                        {
                            await scheduler.ResumeJob(jobKey);
                        }
                        break;
                }
                return   WebResponseContent.Instance.OK($"作业{action.ToString()}成功"); //result ?? new { status = true, msg = $"作业{action.ToString()}成功" };
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return WebResponseContent.Instance.Error(errorMsg); //new { status = false, msg = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>通过作业上下文获取作业对应的配置参数
        /// <returns></returns>
        public static Sys_QuartzOptions GetTaskOptions(this IJobExecutionContext context)
        {
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            Sys_QuartzOptions taskOptions = _taskList.Where(x => x.id.ToString() == trigger.Name).FirstOrDefault();
            return taskOptions ?? _taskList.Where(x => x.id.ToString() == trigger.JobName).FirstOrDefault();
        }

        /// <summary>
        /// 作业是否存在
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <param name="init">初始化的不需要判断</param>
        /// <returns></returns>
        public static (bool, object) Exists(this Sys_QuartzOptions taskOptions, bool init)
        {
            if (!init && _taskList.Any(x => x.task_name == taskOptions.task_name && x.group_name == taskOptions.group_name))
            {
                return (false,
                    new
                    {
                        status = false,
                        msg = $"作业:{taskOptions.task_name},分组：{taskOptions.group_name}已经存在"
                    });
            }
            return (true, null);
        }

        public static (bool, string) IsValidExpression(this string cronExpression)
        {
            try
            {
                CronTriggerImpl trigger = new CronTriggerImpl();
                trigger.CronExpressionString = cronExpression;
                DateTimeOffset? date = trigger.ComputeFirstFireTimeUtc(null);
                return (date != null, date == null ? $"请确认表达式{cronExpression}是否正确!" : "");
            }
            catch (Exception e)
            {
                return (false, $"请确认表达式{cronExpression}是否正确!{e.Message}");
            }
        }
    }

}
