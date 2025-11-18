
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
    [Entity(TableCnName = "工种管理",TableName = "Sys_Work_Type")]
    public partial class Sys_Work_Type:BaseEntity
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
       ///上一层级
       /// </summary>
       [Display(Name ="上一层级")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? master_id { get; set; }

       /// <summary>
       ///工种名称
       /// </summary>
       [Display(Name ="工种名称")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string type_name { get; set; }

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
       ///白天薪资
       /// </summary>
       [Display(Name ="白天薪资")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? day_salary { get; set; }

       /// <summary>
       ///夜班薪资
       /// </summary>
       [Display(Name ="夜班薪资")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? night_salary { get; set; }

       
    }
}