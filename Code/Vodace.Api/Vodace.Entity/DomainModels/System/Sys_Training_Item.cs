
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
    [Entity(TableCnName = "Sys_Training_Item",TableName = "Sys_Training_Item")]
    public partial class Sys_Training_Item:BaseEntity
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
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? delete_status { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
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

       /// <summary>
       ///层级（1，2，3）
       /// </summary>
       [Display(Name ="层级（1，2，3）")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int level { get; set; }

       /// <summary>
       ///行级代码（自曾列）
       /// </summary>
       [Display(Name ="行级代码（自曾列）")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int item_code { get; set; }

       /// <summary>
       ///主层级id
       /// </summary>
       [Display(Name ="主层级id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? root_id { get; set; }

       /// <summary>
       ///父层级（item_code）
       /// </summary>
       [Display(Name ="父层级（item_code）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? master_id { get; set; }

       /// <summary>
       ///业务全局编码
       /// </summary>
       [Display(Name ="业务全局编码")]
       [MaxLength(255)]
       [Column(TypeName="varchar(255)")]
       [Editable(true)]
       public string global_code { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="type_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? type_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="is_qly")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_qly { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="is_others")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_others { get; set; }

       /// <summary>
       ///中文
       /// </summary>
       [Display(Name ="中文")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string name_cht { get; set; }

       /// <summary>
       ///英文
       /// </summary>
       [Display(Name ="英文")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string name_eng { get; set; }

       
    }
}