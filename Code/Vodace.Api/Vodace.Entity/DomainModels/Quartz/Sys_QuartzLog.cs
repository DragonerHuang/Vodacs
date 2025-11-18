
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
    [Entity(TableCnName = "执行记录",TableName = "Sys_QuartzLog")]
    public partial class Sys_QuartzLog:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="log_id")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid log_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? id { get; set; }

       /// <summary>
       ///任务名称
       /// </summary>
       [Display(Name ="任务名称")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       public string task_name { get; set; }

       /// <summary>
       ///耗时(秒)
       /// </summary>
       [Display(Name ="耗时(秒)")]
       [Column(TypeName="int")]
       public int? elapsed_time { get; set; }

       /// <summary>
       ///开始时间
       /// </summary>
       [Display(Name ="开始时间")]
       [Column(TypeName="datetime")]
       public DateTime? strat_date { get; set; }

       /// <summary>
       ///结束时间
       /// </summary>
       [Display(Name ="结束时间")]
       [Column(TypeName="datetime")]
       public DateTime? end_date { get; set; }

       /// <summary>
       ///执行结果
       /// </summary>
       [Display(Name ="执行结果")]
       [Column(TypeName="int")]
       public int? result { get; set; }

       /// <summary>
       ///返回内容
       /// </summary>
       [Display(Name ="返回内容")]
       [Column(TypeName="nvarchar(max)")]
       public string response_content { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="error_msg")]
       [Column(TypeName="nvarchar(max)")]
       public string error_msg { get; set; }

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

       
    }
}