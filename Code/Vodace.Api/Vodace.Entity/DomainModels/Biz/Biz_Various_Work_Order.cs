
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
    [Entity(TableCnName = "新增的合约",TableName = "Biz_Various_Work_Order")]
    public partial class Biz_Various_Work_Order:BaseEntity
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
       ///确认订单ID（M_Confirmed_Order.id）
       /// </summary>
       [Display(Name ="确认订单ID（M_Confirmed_Order.id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? co_id { get; set; }

       /// <summary>
       ///采购单ID
       /// </summary>
       [Display(Name ="采购单ID")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? po_id { get; set; }

       /// <summary>
       ///报价单ID（M_Quotation.id）
       /// </summary>
       [Display(Name ="报价单ID（M_Quotation.id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? qn_id { get; set; }

       /// <summary>
       ///合约ID（M_Contract.id）
       /// </summary>
       [Display(Name ="合约ID（M_Contract.id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? contract_id { get; set; }

       /// <summary>
       ///合约编码
       /// </summary>
       [Display(Name ="合约编码")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       public string vo_wo_no { get; set; }

       /// <summary>
       ///合约类型：VO、WO
       /// </summary>
       [Display(Name ="合约类型：VO、WO")]
       [MaxLength(10)]
       [Column(TypeName="nvarchar(20)")]
       public string vo_wo_type { get; set; }

       /// <summary>
       ///内部定价金额
       /// </summary>
       [Display(Name ="内部定价金额")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? internal_qn_amt { get; set; }

       /// <summary>
       ///确认订单金额
       /// </summary>
       [Display(Name ="确认订单金额")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? confirmed_amt { get; set; }

       /// <summary>
       ///简称
       /// </summary>
       [Display(Name ="简称")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(200)")]
       public string name_sho { get; set; }

       /// <summary>
       ///英文名
       /// </summary>
       [Display(Name ="英文名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       public string name_eng { get; set; }

       /// <summary>
       ///中文名
       /// </summary>
       [Display(Name ="中文名")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(200)")]
       public string name_cht { get; set; }

       /// <summary>
       ///别名
       /// </summary>
       [Display(Name ="别名")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(200)")]
       public string name_ali { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       public int? delete_status { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="remark")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
       public string remark { get; set; }

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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string create_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_date")]
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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string modify_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_date")]
       [Column(TypeName="datetime")]
       public DateTime? modify_date { get; set; }

       
    }
}