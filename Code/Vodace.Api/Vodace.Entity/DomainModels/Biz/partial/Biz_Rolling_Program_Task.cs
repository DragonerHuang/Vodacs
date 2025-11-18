
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
    
    public partial class Biz_Rolling_Program_Task
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class RollingProgramTaskDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? project_id { get; set; }
        /// <summary>
        /// 合约id
        /// </summary>
        public Guid? contract_id { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public string customer { get; set; }
        /// <summary>
        /// 工程类型
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 滚动计划任务名称
        /// </summary>
        public string task_name { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}