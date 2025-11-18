
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
    
    public partial class Sys_Work_Type
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class WorkTypeDto
    {
        public Guid? id { get; set; }
        public Guid? master_id { get; set; }
        /// <summary>
        /// 工作类型上一层名称
        /// </summary>
        public string master_type_name { get; set; }
        /// <summary>
        /// 工作类型名称
        /// </summary>
        public string type_name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 白班薪资
        /// </summary>
        public decimal? day_salary { get; set; }
        /// <summary>
        /// 夜班薪资
        /// </summary>
        public decimal? night_salary { get; set; }
    }
}