/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Entity.SystemModels;

namespace Vodace.Entity.DomainModels
{
    
    public partial class Sys_QuartzOptions
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class TaskQuery
    {
        public int? status { get; set; }
        public string task_name { get; set; }

    }
    public class TaskListDto 
    {
        public Guid id { get; set; }
        /// <summary>
        ///任务名称
        /// </summary>
        public string task_name { get; set; }

        /// <summary>
        ///任务分组
        /// </summary>
        public string group_name { get; set; }

        /// <summary>
        ///Corn表达式
        /// </summary>
        public string cron_expression { get; set; }

        /// <summary>
        ///请求方式
        /// </summary>
        public string method { get; set; }

        /// <summary>
        ///Url地址
        /// </summary>
        public string api_url { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string auth_key { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string auth_value { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string describe { get; set; }

        /// <summary>
        ///最后执行执行
        /// </summary>
        public DateTime? last_run_time { get; set; }

        /// <summary>
        ///运行状态
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        ///post参数
        /// </summary>
        public string post_data { get; set; }

        /// <summary>
        ///超时时间(秒)
        /// </summary>
        public int? time_out { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string create_name { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime? create_date { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string modify_name { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        public DateTime? modify_date { get; set; }
    }

    public class TaskAddDto 
    {
        /// <summary>
        ///任务名称
        /// </summary>
        public string task_name { get; set; }

        /// <summary>
        ///任务分组
        /// </summary>
        public string group_name { get; set; }

        /// <summary>
        ///Corn表达式
        /// </summary>
        public string cron_expression { get; set; }

        /// <summary>
        ///请求方式
        /// </summary>
        public string method { get; set; }

        /// <summary>
        ///Url地址
        /// </summary>
        public string api_url { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string auth_key { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string auth_value { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string describe { get; set; }

        /// <summary>
        ///运行状态
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        ///post参数
        /// </summary>
        public string post_data { get; set; }

        /// <summary>
        ///超时时间(秒)
        /// </summary>
        public int? time_out { get; set; }
    }

    public class TaskEditDto
    {
        public Guid id { get; set; }
        /// <summary>
        ///任务名称
        /// </summary>
        public string task_name { get; set; }
        /// <summary>
        ///任务分组
        /// </summary>
        public string group_name { get; set; }
        /// <summary>
        ///Corn表达式
        /// </summary>
        public string cron_expression { get; set; }
        /// <summary>
        ///请求方式
        /// </summary>
        public string method { get; set; }
        /// <summary>
        ///Url地址
        /// </summary>
        public string api_url { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string auth_key { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string auth_value { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string describe { get; set; }

        /// <summary>
        ///运行状态
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        ///post参数
        /// </summary>
        public string post_data { get; set; }

        /// <summary>
        ///超时时间(秒)
        /// </summary>
        public int? time_out { get; set; }
    }
}