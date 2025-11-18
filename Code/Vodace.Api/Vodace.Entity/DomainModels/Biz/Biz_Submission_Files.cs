
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
    [Entity(TableCnName = "提交管理",TableName = "Biz_Submission_Files")]
    public partial class Biz_Submission_Files:BaseEntity
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
       ///提交编号
       /// </summary>
       [Display(Name ="提交编号")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string file_no { get; set; }

       /// <summary>
       ///版本
       /// </summary>
       [Display(Name ="版本")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int version { get; set; }

       /// <summary>
       ///内部状态（0：编辑中；1：审核中；2：已提交；3：取消；）
       /// </summary>
       [Display(Name ="内部状态（0：编辑中；1：审核中；2：已提交；3：取消；）")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int inner_status { get; set; }

       /// <summary>
       ///描述
       /// </summary>
       [Display(Name ="描述")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string describe { get; set; }

       /// <summary>
       ///审核日期
       /// </summary>
       [Display(Name ="审核日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? approved_date { get; set; }

       /// <summary>
       ///预计上传日期
       /// </summary>
       [Display(Name ="预计上传日期")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? expected_upload_date { get; set; }

       /// <summary>
       ///实际上传日期
       /// </summary>
       [Display(Name ="实际上传日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? actual_upload_date { get; set; }

       /// <summary>
       ///提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）
       /// </summary>
       [Display(Name ="提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? submit_status { get; set; }

       /// <summary>
       ///合约Id
       /// </summary>
       [Display(Name ="合约Id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? contract_id { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? delete_status { get; set; }

       /// <summary>
       ///品牌/型号
       /// </summary>
       [Display(Name ="品牌/型号")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string brand { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="remark")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
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
       ///提交时间
       /// </summary>
       [Display(Name ="提交时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? submit_date { get; set; }

       /// <summary>
       ///制作人id
       /// </summary>
       [Display(Name ="制作人id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? producer_id { get; set; }

       /// <summary>
       ///审核人id
       /// </summary>
       [Display(Name ="审核人id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? approved_id { get; set; }

       /// <summary>
       ///文件类型-大类
       /// </summary>
       [Display(Name ="文件类型-大类")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string main_type { get; set; }

       /// <summary>
       ///文件类型-子类
       /// </summary>
       [Display(Name ="文件类型-子类")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string child_type { get; set; }

       /// <summary>
       ///收件人
       /// </summary>
       [Display(Name ="收件人")]
       [Column(TypeName="int")]
       public int? to_email_user { get; set; }

       /// <summary>
       ///抄送人
       /// </summary>
       [Display(Name ="抄送人")]
       [MaxLength(1000)]
       [Column(TypeName="varchar(1000)")]
       public string cc_email_users { get; set; }

       
    }
}