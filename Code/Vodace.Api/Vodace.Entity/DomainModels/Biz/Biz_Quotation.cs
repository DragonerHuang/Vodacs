
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
    [Entity(TableCnName = "报价管理",TableName = "Biz_Quotation")]
    public partial class Biz_Quotation:BaseEntity
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
       ///qn_no报价号码(qn-年份+流水号)
       /// </summary>
       [Display(Name ="qn_no报价号码(qn-年份+流水号)")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string qn_no { get; set; }

       /// <summary>
       ///年份（香港财政年从4月1日开始，超过这日期需要往下1年，例如：24年的财政报告从24年4月1日-2025年3月31日）
       /// </summary>
       [Display(Name ="年份（香港财政年从4月1日开始，超过这日期需要往下1年，例如：24年的财政报告从24年4月1日-2025年3月31日）")]
       [Column(TypeName="int")]
       public int? qn_year { get; set; }

       /// <summary>
       ///是否兴趣（0：是；1：否）
       /// </summary>
       [Display(Name ="是否兴趣（0：是；1：否）")]
       [Column(TypeName="int")]
       public int? is_interest { get; set; }

       /// <summary>
       ///兴趣原因
       /// </summary>
       [Display(Name ="兴趣原因")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string interest_reason { get; set; }

       /// <summary>
       ///确认金额
       /// </summary>
       [Display(Name ="确认金额")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? confirm_amt { get; set; }

       /// <summary>
       ///报价金额
       /// </summary>
       [Display(Name ="报价金额")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? qn_amt { get; set; }

       /// <summary>
       ///确认金额时间
       /// </summary>
       [Display(Name ="确认金额时间")]
       [Column(TypeName="datetime")]
       public DateTime? confirm_amt_date { get; set; }

       /// <summary>
       ///报价金额时间
       /// </summary>
       [Display(Name ="报价金额时间")]
       [Column(TypeName="datetime")]
       public DateTime? qn_amt_date { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       public int? delete_status { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
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
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
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
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string modify_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_date")]
       [Column(TypeName="datetime")]
       public DateTime? modify_date { get; set; }

       /// <summary>
       ///合约状态（字段中DicValue）
       /// </summary>
       [Display(Name ="合约状态（字段中DicValue）")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string status_code { get; set; }

       /// <summary>
       ///客户id（M_Sub_Contractors.Id）
       /// </summary>
       [Display(Name ="客户id（M_Sub_Contractors.Id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? customer_id { get; set; }

       /// <summary>
       ///招标前负责人（联系人表id）
       /// </summary>
       [Display(Name ="招标前负责人（联系人表id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? pq_handler_id { get; set; }

       /// <summary>
       ///现场考察负责人（联系人表id）
       /// </summary>
       [Display(Name ="现场考察负责人（联系人表id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? pe_handler_id { get; set; }

       /// <summary>
       ///招标负责人（联系人表id）
       /// </summary>
       [Display(Name ="招标负责人（联系人表id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? tender_handler_id { get; set; }

       /// <summary>
       ///招标前截止日期
       /// </summary>
       [Display(Name ="招标前截止日期")]
       [Column(TypeName="datetime")]
       public DateTime? pq_handler_closing_date { get; set; }

       /// <summary>
       ///现场考察截止日期
       /// </summary>
       [Display(Name ="现场考察截止日期")]
       [Column(TypeName="datetime")]
       public DateTime? pe_handler_closing_date { get; set; }

       /// <summary>
       ///招标截止日期
       /// </summary>
       [Display(Name ="招标截止日期")]
       [Column(TypeName="datetime")]
       public DateTime? tender_handler_closing_date { get; set; }

       /// <summary>
       ///办公地址
       /// </summary>
       [Display(Name ="办公地址")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string office_address { get; set; }

       /// <summary>
       ///托盘
       /// </summary>
       [Display(Name ="托盘")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string contract_tray { get; set; }

       /// <summary>
       ///合约id
       /// </summary>
       [Display(Name ="合约id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? contract_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="confirm_currency")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       public string confirm_currency { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="qn_currency")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       public string qn_currency { get; set; }

       
    }
}