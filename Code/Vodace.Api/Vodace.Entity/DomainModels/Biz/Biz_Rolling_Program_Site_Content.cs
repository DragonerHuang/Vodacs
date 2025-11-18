
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
    [Entity(TableCnName = "Biz_Rolling_Program_Site_Content",TableName = "Biz_Rolling_Program_Site_Content")]
    public partial class Biz_Rolling_Program_Site_Content:BaseEntity
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
       ///序号
       /// </summary>
       [Display(Name ="序号")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? line_number { get; set; }

       /// <summary>
       ///项目id
       /// </summary>
       [Display(Name ="项目id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? project_id { get; set; }

       /// <summary>
       ///合约id
       /// </summary>
       [Display(Name ="合约id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? contract_id { get; set; }

       /// <summary>
       ///滚动计划任务id
       /// </summary>
       [Display(Name ="滚动计划任务id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? task_id { get; set; }

       /// <summary>
       ///工地id
       /// </summary>
       [Display(Name ="工地id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? site_id { get; set; }

       /// <summary>
       ///施工具体内容初始化id
       /// </summary>
       [Display(Name ="施工具体内容初始化id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? cc_id { get; set; }

       /// <summary>
       ///数量
       /// </summary>
       [Display(Name ="数量")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? number { get; set; }

       /// <summary>
       ///是否生成任务 1-是 0-否
       /// </summary>
       [Display(Name ="是否生成任务 1-是 0-否")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_generate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="delete_status")]
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
       ///内容编码
       /// </summary>
       [Display(Name ="内容编码")]
       [MaxLength(250)]
       [Column(TypeName="nvarchar(250)")]
       public string item_code { get; set; }

       /// <summary>
       ///内容
       /// </summary>
       [Display(Name ="内容")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       public string content { get; set; }

       /// <summary>
       ///工作类型
       /// </summary>
       [Display(Name ="工作类型")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string work_type { get; set; }

       /// <summary>
       ///节点类型
       /// </summary>
       [Display(Name ="节点类型")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string point_type { get; set; }

       
    }
}