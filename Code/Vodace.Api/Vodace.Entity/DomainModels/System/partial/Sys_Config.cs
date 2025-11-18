
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
    
    public partial class Sys_Config
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class ConfigQuery 
    {
        /// <summary>
        ///配置类型（0：消息提醒配置;1:文件存放路径）
        /// </summary>
        public int? config_type { get; set; }

        /// <summary>
        ///配置项键
        /// </summary>
        public string config_key { get; set; }
    }

    public class ConfigListDto 
    {
        public Guid id { get; set; }

        /// <summary>
        ///配置类型（0：消息提醒配置;1:文件存放路径）
        /// </summary>
        public int? config_type { get; set; }

        /// <summary>
        ///配置项键
        /// </summary>
        public string config_key { get; set; }
        /// <summary>
        ///配置项值
        /// </summary>
        public string config_value { get; set; }
        /// <summary>
        ///是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }
        public string create_name { get; set; }
        public DateTime? create_date { get; set; }
        public string modify_name { get; set; }
        public DateTime? modify_date { get; set; }
    }

    public class ConfigAddDto 
    {
        /// <summary>
        ///配置类型（0：消息提醒配置;1:文件存放路径）
        /// </summary>
        public int? config_type { get; set; }

        /// <summary>
        ///配置项键
        /// </summary>
        public string config_key { get; set; }
        /// <summary>
        ///配置项值
        /// </summary>
        public string config_value { get; set; }
    }

    public class ConfigEditDto
    {
        public Guid id { get; set; }

        /// <summary>
        ///配置类型（0：消息提醒配置;1:文件存放路径）
        /// </summary>
        public int? config_type { get; set; }

        /// <summary>
        ///配置项键
        /// </summary>
        public string config_key { get; set; }
        /// <summary>
        ///配置项值
        /// </summary>
        public string config_value { get; set; }
    }
}