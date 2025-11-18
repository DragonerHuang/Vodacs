
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
    
    public partial class Sys_Leave_Type
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class LeaveTypeQuery
    {
        public string leave_type_code { get; set; }

        /// <summary>
        ///假期名字
        /// </summary>
        public string leave_type_name { get; set; }

        /// <summary>
        ///是否假期类型 0:不是假期，1：是假期
        /// </summary>
        public int? is_leave { get; set; }
    }
    public class LeaveTypeListDto
    {
        public Guid id { get; set; }
        public string leave_type_code { get; set; }

        /// <summary>
        ///假期名字
        /// </summary>
        public string leave_type_name { get; set; }

        /// <summary>
        ///是否假期类型 0:不是假期，1：是假期
        /// </summary>
        public int is_leave { get; set; }

        /// <summary>
        ///0：有工资，1：没有工资
        /// </summary>
        public int pay_type { get; set; }

        public string create_name { get; set; }

        public DateTime? create_date { get; set; }
    }
    public class LeaveTypeEditDto : LeaveTypeAddDto
    {
        /// <summary>
        /// 假期类型id
        /// </summary>
        public Guid id { get; set; }
    }
    public class LeaveTypeAddDto 
    {
        /// <summary>
        ///假期代号
        /// </summary>
        public string leave_type_code { get; set; }

        /// <summary>
        ///假期名字
        /// </summary>
        public string leave_type_name { get; set; }

        /// <summary>
        ///是否假期类型 0:不是假期，1：是假期
        /// </summary>
        public int is_leave { get; set; }

        /// <summary>
        ///0：有工资，1：没有工资
        /// </summary>
        public int pay_type { get; set; }
    }
}