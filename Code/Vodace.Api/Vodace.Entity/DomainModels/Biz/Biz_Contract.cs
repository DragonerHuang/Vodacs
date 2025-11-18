
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
    [Entity(TableCnName = "合约",TableName = "Biz_Contract")]
    public partial class Biz_Contract:BaseEntity
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
       ///采购单ID
       /// </summary>
       [Display(Name ="采购单ID")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? po_id { get; set; }

       /// <summary>
       ///合约编码
       /// </summary>
       [Display(Name ="合约编码")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       public string contract_no { get; set; }

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
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string name_eng { get; set; }

       /// <summary>
       ///中文名
       /// </summary>
       [Display(Name ="中文名")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(400)")]
       public string name_cht { get; set; }

       /// <summary>
       ///别名
       /// </summary>
       [Display(Name ="别名")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(400)")]
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

       /// <summary>
       ///合同标题
       /// </summary>
       [Display(Name ="合同标题")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string title { get; set; }

       /// <summary>
       ///合同类别
       /// </summary>
       [Display(Name ="合同类别")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string category { get; set; }

       /// <summary>
       ///投标类型
       /// </summary>
       [Display(Name ="投标类型")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       public string tender_type { get; set; }

       /// <summary>
       ///合同编号/投标参考编号
       /// </summary>
       [Display(Name ="合同编号/投标参考编号")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       public string ref_no { get; set; }

       /// <summary>
       ///项目ID
       /// </summary>
       [Display(Name ="项目ID")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? project_id { get; set; }

       /// <summary>
       ///vo和wo类型（wo不纳入金额统计），如果都不是则为空
       /// </summary>
       [Display(Name ="vo和wo类型（wo不纳入金额统计），如果都不是则为空")]
       [MaxLength(10)]
       [Column(TypeName="nvarchar(10)")]
       public string vo_wo_type { get; set; }

       /// <summary>
       ///上一层级合约id
       /// </summary>
       [Display(Name ="上一层级合约id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? master_id { get; set; }

       /// <summary>
       ///所属公司id
       /// </summary>
       [Display(Name ="所属公司id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? company_id { get; set; }

       
    }
}