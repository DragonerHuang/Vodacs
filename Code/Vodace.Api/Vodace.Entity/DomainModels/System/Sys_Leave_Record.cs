
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
    [Entity(TableCnName = "请假记录",TableName = "Sys_Leave_Record")]
    public partial class Sys_Leave_Record:BaseEntity
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
       ///
       /// </summary>
       [Display(Name ="user_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int user_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="user_no")]
       [MaxLength(255)]
       [Column(TypeName="varchar(255)")]
       [Editable(true)]
       public string user_no { get; set; }

       /// <summary>
       ///假期开始日
       /// </summary>
       [Display(Name ="假期开始日")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime start_date { get; set; }

       /// <summary>
       ///假期结束日
       /// </summary>
       [Display(Name ="假期结束日")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime end_date { get; set; }

       /// <summary>
       ///请假原因
       /// </summary>
       [Display(Name ="请假原因")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string reason { get; set; }

       /// <summary>
       ///假期批核状态  1:申请中pending,2:已批准Approved,3:拒绝rejrcted,4:取消Canceled
       /// </summary>
       [Display(Name = "假期批核状态  1:申请中pending,2:已批准Approved,3:拒绝rejrcted,4:取消Canceled")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int status { get; set; }

       /// <summary>
       ///批准者ID
       /// </summary>
       [Display(Name ="批准者ID")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? approver_id { get; set; }

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
       [Display(Name ="create_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? create_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_name")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       [Editable(true)]
       public string modify_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? modify_date { get; set; }

       /// <summary>
       ///假期类型 连接Sys_Leave_Type
       /// </summary>
       [Display(Name ="假期类型 连接Sys_Leave_Type")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid leave_type_id { get; set; }

       /// <summary>
       ///假期期间 0：全日，1：上午，2：下午
       /// </summary>
       [Display(Name ="假期期间 0：全日，1：上午，2：下午")]
       [Column(TypeName="int")]
       public int? period { get; set; }

       /// <summary>
       ///发送人邮箱地址
       /// </summary>
       [Display(Name ="发送人邮箱地址")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string from_email { get; set; }

       /// <summary>
       ///接收人邮箱地址
       /// </summary>
       [Display(Name ="接收人邮箱地址")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string to_email { get; set; }

       /// <summary>
       ///抄送人邮箱地址
       /// </summary>
       [Display(Name ="抄送人邮箱地址")]
       [MaxLength(250)]
       [Column(TypeName="nvarchar(250)")]
       public string cc_email { get; set; }

       /// <summary>
       ///密送人邮箱地址
       /// </summary>
       [Display(Name ="密送人邮箱地址")]
       [MaxLength(250)]
       [Column(TypeName="nvarchar(250)")]
       public string bcc_email { get; set; }

       
    }
}