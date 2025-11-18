
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
    
    public partial class Biz_Task_Detail_Work_Type
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class TaskWorkTypeDto
    {
        /// <summary>
        ///
        /// </summary>
        [Display(Name = "code")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? code { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Key]
        [Display(Name = "id")]
        [Column(TypeName = "uniqueidentifier")]
        [Required(AllowEmptyStrings = false)]
        public Guid id { get; set; }

        /// <summary>
        ///明细任务Id
        /// </summary>
        [Display(Name = "明细任务Id")]
        [Column(TypeName = "int")]
        public int? task_detail_id { get; set; }

        /// <summary>
        ///小工种代码
        /// </summary>
        [Display(Name = "小工种代码")]
        [Column(TypeName = "int")]
        public int? work_type_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "remark")]
        [MaxLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string remark { get; set; }
    }
}