
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
    
    public partial class Biz_Task_Group_Relationship
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class TaskGroupRelationshipDto
    {
        /// <summary>
        ///
        /// </summary>
        [Key]
        [Display(Name = "id")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public Guid id { get; set; }

        /// <summary>
        ///任务分组id
        /// </summary>
        [Display(Name = "任务分组id")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        public Guid? task_group_id { get; set; }

        /// <summary>
        ///任务分组父节点
        /// </summary>
        [Display(Name = "任务分组父节点")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        public Guid? task_group_master_id { get; set; }

        /// <summary>
        ///任务id
        /// </summary>
        [Display(Name = "任务id")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        public Guid? task_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "customer_id")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        public Guid? customer_id { get; set; }

        /// <summary>
        ///工程类别
        /// </summary>
        [Display(Name = "工程类别")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string project_category_name { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "site_id")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        public Guid? site_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "des")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        public string des { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [Display(Name = "备注")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        public string remark { get; set; }

    }
}