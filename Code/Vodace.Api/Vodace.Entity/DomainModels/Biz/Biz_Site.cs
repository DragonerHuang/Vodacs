
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
    [Entity(TableCnName = "工地表",TableName = "Biz_Site")]
    public partial class Biz_Site:BaseEntity
    {
        /// <summary>
       ///站点Id
       /// </summary>
       [Key]
       [Display(Name ="站点Id")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid id { get; set; }

       /// <summary>
       ///站点名称（英文）
       /// </summary>
       [Display(Name ="站点名称（英文）")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string name_eng { get; set; }

       /// <summary>
       ///站点名称（繁体）
       /// </summary>
       [Display(Name ="站点名称（繁体）")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string name_cht { get; set; }

       /// <summary>
       ///站点名称（简体）
       /// </summary>
       [Display(Name ="站点名称（简体）")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string name_chs { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
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
       ///Site（站点） 缩写
       /// </summary>
       [Display(Name ="Site（站点） 缩写")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string name_sho { get; set; }

       
    }
}