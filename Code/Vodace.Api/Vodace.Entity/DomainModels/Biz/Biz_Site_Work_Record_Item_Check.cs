
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
    [Entity(TableCnName = "Biz_Site_Work_Record_Item_Check",TableName = "Biz_Site_Work_Record_Item_Check")]
    public partial class Biz_Site_Work_Record_Item_Check:BaseEntity
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
       ///工地工作记录id
       /// </summary>
       [Display(Name ="工地工作记录id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? record_id { get; set; }

       /// <summary>
       ///代码（指向表中的type_code）
       /// </summary>
       [Display(Name ="代码（指向表中的type_code）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? check_code { get; set; }

       /// <summary>
       ///子代码（指向表中的type_code）
       /// </summary>
       [Display(Name ="子代码（指向表中的type_code）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? check_sub_code { get; set; }

       /// <summary>
       ///单选框结果（1：第一个，2：第二个以此类推）
       /// </summary>
       [Display(Name ="单选框结果（1：第一个，2：第二个以此类推）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? radio_result { get; set; }

       /// <summary>
       ///是否勾选（0：否；1：是）
       /// </summary>
       [Display(Name ="是否勾选（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? check_result { get; set; }

       /// <summary>
       ///文本框填写内容
       /// </summary>
       [Display(Name ="文本框填写内容")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string text_result { get; set; }

       /// <summary>
       ///选项类型（0：brf；1：scr；2：cpd；3：qdc；4：cp；5：sic）
       /// </summary>
       [Display(Name ="选项类型（0：brf；1：scr；2：cpd；3：qdc；4：cp；5：sic）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? check_type { get; set; }

        /// <summary>
        ///检查时间（选择类型4，5使用）
        /// </summary>
        [Display(Name = "检查时间（选择类型4，5使用）")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? time_result { get; set; }
    }
}