
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Entity.SystemModels;

namespace Vodace.Entity.DomainModels
{
    [Entity(TableCnName = "定时任务",TableName = "Sys_QuartzOptions")]
    public partial class Sys_QuartzOptions:BaseEntity
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
       ///任务名称
       /// </summary>
       [Display(Name ="任务名称")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
        [Editable(true)]
        [Required(AllowEmptyStrings=false)]
       public string task_name { get; set; }

       /// <summary>
       ///任务分组
       /// </summary>
       [Display(Name ="任务分组")]
       [MaxLength(500)]
        [Editable(true)]
        [Column(TypeName="nvarchar(500)")]
       [Required(AllowEmptyStrings=false)]
       public string group_name { get; set; }

       /// <summary>
       ///Corn表达式
       /// </summary>
       [Display(Name ="Corn表达式")]
       [MaxLength(100)]
        [Editable(true)]
        [Column(TypeName="varchar(100)")]
       [Required(AllowEmptyStrings=false)]
       public string cron_expression { get; set; }

       /// <summary>
       ///请求方式
       /// </summary>
       [Display(Name ="请求方式")]
       [MaxLength(50)]
        [Editable(true)]
        [Column(TypeName="varchar(50)")]
       public string method { get; set; }

       /// <summary>
       ///Url地址
       /// </summary>
       [Display(Name ="Url地址")]
       [MaxLength(2000)]
        [Editable(true)]
        [Column(TypeName="nvarchar(2000)")]
       public string api_url { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="auth_key")]
       [MaxLength(200)]
        [Editable(true)]
        [Column(TypeName="nvarchar(200)")]
       public string auth_key { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="auth_value")]
       [MaxLength(200)]
        [Editable(true)]
        [Column(TypeName="nvarchar(200)")]
       public string auth_value { get; set; }

       /// <summary>
       ///描述
       /// </summary>
       [Display(Name ="描述")]
       [MaxLength(2000)]
        [Editable(true)]
        [Column(TypeName="nvarchar(2000)")]
       public string describe { get; set; }

       /// <summary>
       ///最后执行执行
       /// </summary>
       [Display(Name ="最后执行执行")]
       [Column(TypeName="datetime")]
       public DateTime? last_run_time { get; set; }

       /// <summary>
       ///运行状态
       /// </summary>
       [Display(Name ="运行状态")]
       [Column(TypeName="int")]
       public int? status { get; set; }

       /// <summary>
       ///post参数
       /// </summary>
       [Display(Name ="post参数")]
        [Editable(true)]
        [Column(TypeName="nvarchar(max)")]
       public string post_data { get; set; }

       /// <summary>
       ///超时时间(秒)
       /// </summary>
       [Display(Name ="超时时间(秒)")]
       [Column(TypeName="int")]
        [Editable(true)]
        public int? time_out { get; set; }

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
       ///创建时间
       /// </summary>
       [Display(Name ="创建时间")]
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
       ///修改时间
       /// </summary>
       [Display(Name ="修改时间")]
       [Column(TypeName="datetime")]
       public DateTime? modify_date { get; set; }

        /// <summary>
        ///是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        [Display(Name = "是否删除（0：正常；1：删除；2：数据库手删除）")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? delete_status { get; set; }
    }
}