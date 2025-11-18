
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
    [Entity(TableCnName = "组织架构",TableName = "Sys_Department",DBServer = "SysDbContext")]
    public partial class Sys_Department:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="id")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid id { get; set; }

       /// <summary>
       ///是否启用
       /// </summary>
       [Display(Name ="是否启用")]
       [Column(TypeName="int")]
       public int? enable { get; set; }

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
       ///是否删除（0：正常；1：删除；2：数据库手删除；3：完成关闭）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除；3：完成关闭）")]
       [Column(TypeName="int")]
       public int? delete_status { get; set; }

       /// <summary>
       ///上级数据id
       /// </summary>
       [Display(Name ="上级数据id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? master_id { get; set; }

       /// <summary>
       ///英文名称
       /// </summary>
       [Display(Name ="英文名称")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string name_eng { get; set; }

       /// <summary>
       ///中文名称
       /// </summary>
       [Display(Name ="中文名称")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string name_cht { get; set; }

       /// <summary>
       ///简称
       /// </summary>
       [Display(Name ="简称")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string name_sho { get; set; }

       /// <summary>
       ///别名
       /// </summary>
       [Display(Name ="别名")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string name_ali { get; set; }

       
    }
}