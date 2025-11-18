
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
    [Entity(TableCnName = "Biz_Quotation_Record_Excel",TableName = "Biz_Quotation_Record_Excel")]
    public partial class Biz_Quotation_Record_Excel:BaseEntity
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
       ///报价单记录id
       /// </summary>
       [Display(Name ="报价单记录id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? quotation_record_id { get; set; }

       /// <summary>
       ///excel表单名
       /// </summary>
       [Display(Name ="excel表单名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string sheet_name { get; set; }

       /// <summary>
       ///项目（Item Code）
       /// </summary>
       [Display(Name ="项目（Item Code）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string item_code { get; set; }

       /// <summary>
       ///内容
       /// </summary>
       [Display(Name ="内容")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string item_description { get; set; }

       /// <summary>
       ///数量
       /// </summary>
       [Display(Name ="数量")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string quantity { get; set; }

       /// <summary>
       ///单位
       /// </summary>
       [Display(Name ="单位")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string unit { get; set; }

       /// <summary>
       ///单价
       /// </summary>
       [Display(Name ="单价")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string unit_rage { get; set; }

       /// <summary>
       ///银码
       /// </summary>
       [Display(Name ="银码")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string amount { get; set; }

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
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
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
       ///行号
       /// </summary>
       [Display(Name ="行号")]
       [Column(TypeName="int")]
       public int? line_number { get; set; }

       
    }
}