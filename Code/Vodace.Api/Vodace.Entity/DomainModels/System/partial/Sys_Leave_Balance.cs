
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
    
    public partial class Sys_Leave_Balance
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class LeaveBalanceDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string user_no { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public string year { get; set; }

        /// <summary>
        ///假期类型 连接Sys_Leave_Type
        /// </summary>
        public Guid? leave_type_id { get; set; }

        /// <summary>
        ///剩余假期
        /// </summary>
        public decimal? remaing_leave { get; set; }
    }

    public class LeaveBalanceEditDto : LeaveBalanceDto
    {
        public Guid? id { get; set; }
    }

    public class LeaveBalanceListDto : LeaveBalanceEditDto
    {
        /// <summary>
        ///是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int? create_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string create_name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? create_date { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int? modify_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string modify_name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? modify_date { get; set; }

        /// <summary>
        /// 假期代号
        /// </summary>
        public string leave_type_code { get; set; }

        /// <summary>
        /// 假期名字
        /// </summary>
        public string leave_type_name { get; set; }

        /// <summary>
        /// 是否假期类型
        /// </summary>
        public int is_leave { get; set; }
        /// <summary>
        /// 0：有工资，1：没有工资
        /// </summary>
        public int pay_type { get; set; }

        /// <summary>
        /// 员工名
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 员工真实名
        /// </summary>
        public string user_true_name { get; set; }
        /// <summary>
        /// 员工英文名
        /// </summary>
        public string user_name_eng { get; set; }
        /// <summary>
        /// 员工所属id公司
        /// </summary>
        public Guid? company_id { get; set; }

        /// <summary>
        /// 已用假期额度
        /// </summary>
        public decimal? used_leave { get; set; }
    }
}