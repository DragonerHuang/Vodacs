
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
    [Entity(TableCnName = "确认报价",TableName = "Biz_Confirmed_Order")]
    public partial class Biz_Confirmed_Order:BaseEntity
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
       ///co编码（年份-流水号）
       /// </summary>
       [Display(Name ="co编码（年份-流水号）")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string co_no { get; set; }

       /// <summary>
       ///所属公司id
       /// </summary>
       [Display(Name ="所属公司id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? company_id { get; set; }

       /// <summary>
       ///项目id
       /// </summary>
       [Display(Name ="项目id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? project_id { get; set; }

       /// <summary>
       ///报价的id
       /// </summary>
       [Display(Name ="报价的id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? qn_id { get; set; }

       /// <summary>
       ///客户id
       /// </summary>
       [Display(Name ="客户id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? customer_id { get; set; }

       /// <summary>
       ///确定中标时间
       /// </summary>
       [Display(Name ="确定中标时间")]
       [Column(TypeName="date")]
       public DateTime? confirm_date { get; set; }

       /// <summary>
       ///确认价格
       /// </summary>
       [Display(Name ="确认价格")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? confirm_amt { get; set; }

       /// <summary>
       ///合约开始时间
       /// </summary>
       [Display(Name ="合约开始时间")]
       [Column(TypeName="date")]
       public DateTime? commen_date { get; set; }

       /// <summary>
       ///负责人id（确认中标人）
       /// </summary>
       [Display(Name ="负责人id（确认中标人）")]
       [Column(TypeName="int")]
       public int? enter_id { get; set; }

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
       ///合约结束时间（预计）
       /// </summary>
       [Display(Name ="合约结束时间（预计）")]
       [Column(TypeName="date")]
       public DateTime? complet_exp_date { get; set; }

       /// <summary>
       ///合约表id
       /// </summary>
       [Display(Name ="合约表id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? contract_id { get; set; }

       /// <summary>
       ///采购订单号（PO编号）
       /// </summary>
       [Display(Name ="采购订单号（PO编号）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string po_no { get; set; }

       /// <summary>
       ///合约结束时间（实际）
       /// </summary>
       [Display(Name ="合约结束时间（实际）")]
       [Column(TypeName="date")]
       public DateTime? complet_act_date { get; set; }

       /// <summary>
       ///负责人id（合同的）
       /// </summary>
       [Display(Name ="负责人id（合同的）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? head_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="confirm_currency")]
       [MaxLength(100)]
       [Column(TypeName="varchar(100)")]
       public string confirm_currency { get; set; }

       
    }
}