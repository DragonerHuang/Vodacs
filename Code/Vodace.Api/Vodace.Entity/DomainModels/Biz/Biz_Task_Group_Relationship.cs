
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
    [Entity(TableCnName = "Biz_Task_Group_Relationship ",TableName = "Biz_Task_Group_Relationship")]
    public partial class Biz_Task_Group_Relationship:BaseEntity
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
       ///任务分组id
       /// </summary>
       [Display(Name ="任务分组id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? task_group_id { get; set; }

       /// <summary>
       ///任务分组父节点
       /// </summary>
       [Display(Name ="任务分组父节点")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? task_group_master_id { get; set; }

       /// <summary>
       ///任务id
       /// </summary>
       [Display(Name ="任务id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? task_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="customer_id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? customer_id { get; set; }

       /// <summary>
       ///工程类别
       /// </summary>
       [Display(Name ="工程类别")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string project_category_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="site_id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? site_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="des")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string des { get; set; }

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

       
    }
}