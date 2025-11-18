
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
    [Entity(TableCnName = "Biz_Quotation_Record",TableName = "Biz_Quotation_Record")]
    public partial class Biz_Quotation_Record:BaseEntity
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
       ///版本号
       /// </summary>
       [Display(Name ="版本号")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Editable(true)]
       public string version { get; set; }

       /// <summary>
       ///报价金额(HK$)
       /// </summary>
       [Display(Name ="报价金额(HK$)")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? amount { get; set; }

       /// <summary>
       ///文档名
       /// </summary>
       [Display(Name ="文档名")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string file_name { get; set; }

       /// <summary>
       ///对应的报价
       /// </summary>
       [Display(Name ="对应的报价")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid qn_id { get; set; }

       /// <summary>
       ///报价单创建人（联系人id）
       /// </summary>
       [Display(Name ="报价单创建人（联系人id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? create_id_by_contract { get; set; }

       /// <summary>
       ///报价单创建人（联系人名）
       /// </summary>
       [Display(Name ="报价单创建人（联系人名）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string create_name_by_contract { get; set; }

       /// <summary>
       ///报价单创建时间
       /// </summary>
       [Display(Name ="报价单创建时间")]
       [Column(TypeName="datetime")]
       public DateTime? create_name_by_date { get; set; }

       /// <summary>
       ///报价单修改人（联系人id）
       /// </summary>
       [Display(Name ="报价单修改人（联系人id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? update_id_by_contract { get; set; }

       /// <summary>
       ///报价单修改人（联系人名）
       /// </summary>
       [Display(Name ="报价单修改人（联系人名）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string update_name_by_contract { get; set; }

       /// <summary>
       ///报价单修改时间
       /// </summary>
       [Display(Name ="报价单修改时间")]
       [Column(TypeName="datetime")]
       public DateTime? update_id_by_date { get; set; }

       
    }
}