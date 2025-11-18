
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
    [Entity(TableCnName = "Sys_Leave_Balance_Record",TableName = "Sys_Leave_Balance_Record")]
    public partial class Sys_Leave_Balance_Record:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid id { get; set; }

       /// <summary>
       ///员工id
       /// </summary>
       [Display(Name ="员工id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? user_id { get; set; }

       /// <summary>
       ///工号
       /// </summary>
       [Display(Name ="工号")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string user_no { get; set; }

       /// <summary>
       ///年份
       /// </summary>
       [Display(Name ="年份")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? year { get; set; }

       /// <summary>
       ///假期代号
       /// </summary>
       [Display(Name ="假期代号")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string leave_type_code { get; set; }

       /// <summary>
       ///假期名字
       /// </summary>
       [Display(Name ="假期名字")]
       [MaxLength(120)]
       [Column(TypeName="nvarchar(120)")]
       [Editable(true)]
       public string leave_type_name { get; set; }

       /// <summary>
       ///是否假期类型 0:不是假期，1：是假期
       /// </summary>
       [Display(Name ="是否假期类型 0:不是假期，1：是假期")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_leave { get; set; }

       /// <summary>
       ///0：有工资，1：没有工资
       /// </summary>
       [Display(Name ="0：有工资，1：没有工资")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? pay_type { get; set; }

       /// <summary>
       ///消费天数
       /// </summary>
       [Display(Name ="消费天数")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? spend { get; set; }

       /// <summary>
       ///剩余假期
       /// </summary>
       [Display(Name ="剩余假期")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? remaing_leave { get; set; }

       /// <summary>
       ///请假id
       /// </summary>
       [Display(Name ="请假id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? leave_balance_id { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? delete_status { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="remark")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? create_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_name")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string create_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? create_date { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? modify_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_name")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string modify_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? modify_date { get; set; }

       
    }
}