
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
    
    public partial class Biz_Task_Group
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class TaskGroupDto
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
        ///
        /// </summary>
        [Display(Name = "master_id")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        public Guid? master_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "name_eng")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string name_eng { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "name_cht")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string name_cht { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "name_chs")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string name_chs { get; set; }

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