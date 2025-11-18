
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
    [Entity(TableCnName = "工人注册",TableName = "Sys_Worker_Register")]
    public partial class Sys_Worker_Register:BaseEntity
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
       ///
       /// </summary>
       [Display(Name ="user_register_Id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? user_register_Id { get; set; }

       /// <summary>
       ///建造行业安全训练证明书
       /// </summary>
       [Display(Name ="建造行业安全训练证明书")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Required(AllowEmptyStrings=false)]
       public string stc_no { get; set; }

       /// <summary>
       ///签发日期
       /// </summary>
       [Display(Name ="签发日期")]
       [Column(TypeName="date")]
       [Required(AllowEmptyStrings=false)]
       public DateTime stc_issued_start_date { get; set; }

       /// <summary>
       ///有效日期
       /// </summary>
       [Display(Name ="有效日期")]
       [Column(TypeName="date")]
       [Required(AllowEmptyStrings=false)]
       public DateTime stc_issued_end_date { get; set; }

       /// <summary>
       ///工人注册证
       /// </summary>
       [Display(Name ="工人注册证")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Required(AllowEmptyStrings=false)]
       public string wrc_no { get; set; }

       /// <summary>
       ///签发日期
       /// </summary>
       [Display(Name ="签发日期")]
       [Column(TypeName="date")]
       [Required(AllowEmptyStrings=false)]
       public DateTime wrc_issued_start_date { get; set; }

       /// <summary>
       ///有效日期
       /// </summary>
       [Display(Name ="有效日期")]
       [Column(TypeName="date")]
       [Required(AllowEmptyStrings=false)]
       public DateTime wrc_issued_end_date { get; set; }

       /// <summary>
       ///专业工种
       /// </summary>
       [Display(Name ="专业工种")]
       [Column(TypeName="uniqueidentifier")]
       [Required(AllowEmptyStrings=false)]
       public Guid work_type { get; set; }

       /// <summary>
       ///是否删除（0：是；1：否）
       /// </summary>
       [Display(Name ="是否删除（0：是；1：否）")]
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

       
    }
}