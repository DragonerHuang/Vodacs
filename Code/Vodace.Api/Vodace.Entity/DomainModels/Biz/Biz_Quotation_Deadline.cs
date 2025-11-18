
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
    [Entity(TableCnName = "Biz_Quotation_Deadline",TableName = "Biz_Quotation_Deadline")]
    public partial class Biz_Quotation_Deadline:BaseEntity
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

       /// <summary>
       ///报价的id
       /// </summary>
       [Display(Name ="报价的id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? qn_id { get; set; }

       /// <summary>
       ///期限类型
       /// </summary>
       [Display(Name ="期限类型")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string term_type { get; set; }

       /// <summary>
       ///负责人id
       /// </summary>
       [Display(Name ="负责人id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? handler_id { get; set; }

       /// <summary>
       ///截止时间
       /// </summary>
       [Display(Name ="截止时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? closing_date { get; set; }

       /// <summary>
       ///是否完成（0：否；1：完成）
       /// </summary>
       [Display(Name ="是否完成（0：否；1：完成）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_complete { get; set; }

       /// <summary>
       ///实际完成时间
       /// </summary>
       [Display(Name ="实际完成时间")]
       [Column(TypeName="datetime")]
       public DateTime? complete_date { get; set; }

       /// <summary>
       ///关联表id
       /// </summary>
       [Display(Name ="关联表id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? relation_id { get; set; }

       /// <summary>
       ///预计完成时间
       /// </summary>
       [Display(Name ="预计完成时间")]
       [Column(TypeName="datetime")]
       public DateTime? exp_complete_date { get; set; }

       
    }
}