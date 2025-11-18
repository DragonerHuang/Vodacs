
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
    [Entity(TableCnName = "任务明细表",TableName = "Biz_Task_Detail")]
    public partial class Biz_Task_Detail:BaseEntity
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
       ///
       /// </summary>
       [Display(Name ="task_id")]
       [Column(TypeName= "uniqueidentifier")]
       public Guid? task_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="name_eng")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string name_eng { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="name_cht")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string name_cht { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="name_chs")]
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

       
    }
}