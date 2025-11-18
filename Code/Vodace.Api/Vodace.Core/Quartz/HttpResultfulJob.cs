using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Vodace.Core.EFDbContext;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;

namespace Vodace.Core.Quartz
{
    public class HttpResultfulJob : IJob
    {
        readonly IHttpClientFactory _httpClientFactory;

        readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 2020.05.31增加构造方法
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="httpClientFactory"></param>
        public HttpResultfulJob(IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _serviceProvider = serviceProvider;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine(DateTime.Now);
            DateTime dateTime = DateTime.Now;
    
            Sys_QuartzOptions taskOptions = context.GetTaskOptions();
            string httpMessage = "";
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            if (taskOptions == null)
            {
                Console.WriteLine($"未获取到作业");
                return;
            }
            Console.WriteLine(taskOptions.task_name);
            Log4NetHelper.Info($"定时任务：{taskOptions.task_name}");
            //await Task.CompletedTask;
            //return;
            if (string.IsNullOrEmpty(taskOptions.api_url) || taskOptions.api_url == "/")
            {
                Console.WriteLine($"未配置作业:{taskOptions.task_name}的url地址");
                QuartzFileHelper.Error($"未配置作业:{taskOptions.task_name}的url地址");
                return;
            }
            string exceptionMsg = null;

            try
            {

                using (var dbContext = new VOLContext())
                {
                    var _taskOptions = dbContext.Set<Sys_QuartzOptions>().AsTracking()
                          .Where(x => x.id == taskOptions.id).FirstOrDefault();

                    if (_taskOptions != null)
                    {
                        dbContext.Update(_taskOptions);
                        var entry = dbContext.Entry(_taskOptions);
                        entry.State = EntityState.Unchanged;
                        entry.Property("last_run_time").IsModified = true;
                        _taskOptions.last_run_time = DateTime.Now;
                        dbContext.SaveChanges();
                    }
                }

                Dictionary<string, string> header = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(taskOptions.auth_key)
                    && !string.IsNullOrEmpty(taskOptions.auth_value))
                {
                    header.Add(taskOptions.auth_key.Trim(), taskOptions.auth_value.Trim());
                }

                httpMessage = await _httpClientFactory.SendAsync(
                    taskOptions.method?.ToLower() == "get" ? HttpMethod.Get : HttpMethod.Post,
                    taskOptions.api_url,
                    taskOptions.post_data,
                    taskOptions.time_out ?? 180,
                    header); ;
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message + ex.StackTrace;
            }
            finally
            {
                try
                {
                    var log = new Sys_QuartzLog
                    {
                        log_id = Guid.NewGuid(),
                        task_name = taskOptions.task_name,
                        id = taskOptions.id,
                        create_date = dateTime,
                        elapsed_time = Convert.ToInt32((DateTime.Now - dateTime).TotalSeconds),
                        response_content = httpMessage,
                        error_msg = exceptionMsg,
                        strat_date = dateTime,
                        result = exceptionMsg == null ? 1 : 0,
                        end_date = DateTime.Now
                    };
                    using (var dbContext = new VOLContext())
                    {
                        dbContext.Set<Sys_QuartzLog>().Add(log);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"日志写入异常:{taskOptions.task_name},{ex.Message}");
                    QuartzFileHelper.Error($"日志写入异常:{typeof(HttpResultfulJob).Name},{taskOptions.task_name},{ex.Message}");
                }
            }
            Console.WriteLine(trigger.FullName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + " " + httpMessage);
            return;
        }
    }
    public class TaskOptions
    {
        public string task_name { get; set; }
        public string GroupName { get; set; }
        public string Interval { get; set; }
        public string api_url { get; set; }
        public string auth_key { get; set; }
        public string auth_value { get; set; }
        public string Describe { get; set; }
        public string RequestType { get; set; } 
        public DateTime? last_run_time { get; set; }
        public int Status { get; set; }
    }
}
