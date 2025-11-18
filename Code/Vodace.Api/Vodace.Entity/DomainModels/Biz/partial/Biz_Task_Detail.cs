
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

    public partial class Biz_Task_Detail
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
        /// <summary>
        /// 炒作类型
        /// </summary>
        public int? operation_type { get; set; }
    }
    public  class TaskDetailDto
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
        [Display(Name = "id")]
        public Guid ?id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "task_id")]
        [Column(TypeName = "int")]
        public Guid? task_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "name_eng")]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string name_eng { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "name_cht")]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string name_cht { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "name_chs")]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string name_chs { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "des")]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public string des { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "remark")]
        [MaxLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string remark { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public int? operation_type { get; set; }
    }
}