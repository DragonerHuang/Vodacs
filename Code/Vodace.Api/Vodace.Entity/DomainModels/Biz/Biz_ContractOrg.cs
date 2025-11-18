
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
    [Entity(TableCnName = "Biz_ContractOrg",TableName = "Biz_ContractOrg")]
    public partial class Biz_ContractOrg:BaseEntity
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
       ///上级数据id
       /// </summary>
       [Display(Name ="上级数据id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? master_id { get; set; }

       /// <summary>
       ///合约id
       /// </summary>
       [Display(Name ="合约id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? contract_id { get; set; }

       /// <summary>
       ///特殊职位（一人之下万人之上）
       /// </summary>
       [Display(Name ="特殊职位（一人之下万人之上）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_special { get; set; }

       /// <summary>
       ///用户id
       /// </summary>
       [Display(Name ="用户id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? user_id { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? delete_status { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="remark")]
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
       ///组织id
       /// </summary>
       [Display(Name ="组织id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? org_id { get; set; }

       /// <summary>
       ///提交文件编码
       /// </summary>
       [Display(Name ="提交文件编码")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string submit_file_code { get; set; }

       
    }
}