
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
    [Entity(TableCnName = "公司管理",TableName = "Sys_Company")]
    public partial class Sys_Company:BaseEntity
    {
        /// <summary>
       ///主键
       /// </summary>
       [Key]
       [Display(Name ="主键")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid id { get; set; }

       /// <summary>
       ///公司名称
       /// </summary>
       [Display(Name ="公司名称")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Required(AllowEmptyStrings=false)]
       public string company_name { get; set; }

       /// <summary>
       ///公司编号
       /// </summary>
       [Display(Name ="公司编号")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Required(AllowEmptyStrings=false)]
       public string company_no { get; set; }

       /// <summary>
       ///执照编号
       /// </summary>
       [Display(Name ="执照编号")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string license_no { get; set; }

       /// <summary>
       ///联系人
       /// </summary>
       [Display(Name ="联系人")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Required(AllowEmptyStrings=false)]
       public string contact { get; set; }

       /// <summary>
       ///联系人电话
       /// </summary>
       [Display(Name ="联系人电话")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       [Required(AllowEmptyStrings=false)]
       public string contact_phone { get; set; }

       /// <summary>
       ///联系人邮箱
       /// </summary>
       [Display(Name ="联系人邮箱")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Required(AllowEmptyStrings=false)]
       public string contact_email { get; set; }

       /// <summary>
       ///公司状态
       /// </summary>
       [Display(Name ="公司状态")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int status { get; set; }

       /// <summary>
       ///审核人
       /// </summary>
       [Display(Name ="审核人")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string audit_user { get; set; }

       /// <summary>
       ///审核时间
       /// </summary>
       [Display(Name ="审核时间")]
       [Column(TypeName="datetime")]
       public DateTime? audit_date { get; set; }

       /// <summary>
       ///审核意见
       /// </summary>
       [Display(Name ="审核意见")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string audit_remark { get; set; }

       /// <summary>
       ///是否删除（0：是；1：否）
       /// </summary>
       [Display(Name ="是否删除（0：是；1：否）")]
       [Column(TypeName="int")]
       public int? delete_status { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       public string remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_id")]
       [Column(TypeName="int")]
       public int? create_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_name")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string create_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_date")]
       [Column(TypeName="datetime")]
       public DateTime? create_date { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_id")]
       [Column(TypeName="int")]
       public int? modify_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_name")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string modify_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_date")]
       [Column(TypeName="datetime")]
       public DateTime? modify_date { get; set; }

       /// <summary>
       ///公司类型 0：公司承建商，1：一般个人
       /// </summary>
       [Display(Name ="公司类型 0：公司承建商，1：一般个人")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int company_type { get; set; }

       /// <summary>
       ///公司英文名
       /// </summary>
       [Display(Name ="公司英文名")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string company_name_eng { get; set; }

       
    }
}