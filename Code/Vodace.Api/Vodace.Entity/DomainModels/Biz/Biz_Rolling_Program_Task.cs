
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
    [Entity(TableCnName = "Biz_Rolling_Program_Task",TableName = "Biz_Rolling_Program_Task")]
    public partial class Biz_Rolling_Program_Task:BaseEntity
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

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="contract_id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? contract_id { get; set; }

       /// <summary>
       ///项目id
       /// </summary>
       [Display(Name ="项目id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? project_id { get; set; }

       /// <summary>
       ///客户
       /// </summary>
       [Display(Name ="客户")]
       [MaxLength(40)]
       [Column(TypeName="nvarchar(40)")]
       public string customer { get; set; }

       /// <summary>
       ///工程类型
       /// </summary>
       [Display(Name ="工程类型")]
       [MaxLength(40)]
       [Column(TypeName="nvarchar(40)")]
       public string category { get; set; }

       /// <summary>
       ///滚动计划任务名称
       /// </summary>
       [Display(Name ="滚动计划任务名称")]
       [MaxLength(40)]
       [Column(TypeName="nvarchar(40)")]
       public string task_name { get; set; }

       /// <summary>
       ///版本号
       /// </summary>
       [Display(Name ="版本号")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string version { get; set; }

       
    }
}