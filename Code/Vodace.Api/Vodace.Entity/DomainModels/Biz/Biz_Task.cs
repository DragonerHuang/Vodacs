
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
    [Entity(TableCnName = "任务表",TableName = "Biz_Task")]
    public partial class Biz_Task:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Display(Name ="code")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? code { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="id")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid id { get; set; }

       /// <summary>
       ///任务大类名称（英文）
       /// </summary>
       [Display(Name ="任务大类名称（英文）")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string name_eng { get; set; }

       /// <summary>
       ///任务大类名称（繁体）
       /// </summary>
       [Display(Name ="任务大类名称（繁体）")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string name_cht { get; set; }

       /// <summary>
       ///任务大类名称（简体）
       /// </summary>
       [Display(Name ="任务大类名称（简体）")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string name_chs { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="des")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string des { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       public int? delete_status { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="remark")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string modify_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_date")]
       [Column(TypeName="datetime")]
       public DateTime? modify_date { get; set; }

       /// <summary>
       ///父级id
       /// </summary>
       [Display(Name ="父级id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? master_id { get; set; }

       /// <summary>
       ///负责人
       /// </summary>
       [Display(Name ="负责人")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string responsible { get; set; }

       /// <summary>
       ///0:个人，1：角色，2:组织
       /// </summary>
       [Display(Name ="0:个人，1：角色，2:组织")]
       [Column(TypeName="int")]
       public int? responsible_type { get; set; }

       /// <summary>
       ///是否存在详细工作项（0:不存在；1：存在...）
       /// </summary>
       [Display(Name ="是否存在详细工作项（0:不存在；1：存在...）")]
       [Column(TypeName="int")]
       public int? exists_work_item { get; set; }

       /// <summary>
       ///持续时间
       /// </summary>
       [Display(Name ="持续时间")]
       [Column(TypeName="float")]
       public float? duration { get; set; }

       
    }
}