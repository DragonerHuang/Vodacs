
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
    [Entity(TableCnName = "Biz_Deadline_Management",TableName = "Biz_Deadline_Management")]
    public partial class Biz_Deadline_Management:BaseEntity
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
       ///期限类型（3：预审；4：现场考察；5：招标；6：公开招标；7：预审问答；8：邀请招标；9：招标问答；10：面试	）
       /// </summary>
       [Display(Name ="期限类型（3：预审；4：现场考察；5：招标；6：公开招标；7：预审问答；8：邀请招标；9：招标问答；10：面试	）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? deadline_type { get; set; }

       /// <summary>
       ///主题
       /// </summary>
       [Display(Name ="主题")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string subject { get; set; }

       /// <summary>
       ///预计日期
       /// </summary>
       [Display(Name ="预计日期")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? estimated_date { get; set; }

       /// <summary>
       ///截至日期
       /// </summary>
       [Display(Name ="截至日期")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? deadline_date { get; set; }

       /// <summary>
       ///实际日期
       /// </summary>
       [Display(Name ="实际日期")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? actual_date { get; set; }

       /// <summary>
       ///客户联系人
       /// </summary>
       [Display(Name ="客户联系人")]
       [MaxLength(16)]
       [Column(TypeName="nvarchar(16)")]
       [Editable(true)]
       public string customer_contact { get; set; }

       /// <summary>
       ///负责人
       /// </summary>
       [Display(Name ="负责人")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? director_user_id { get; set; }

       /// <summary>
       ///审批人
       /// </summary>
       [Display(Name ="审批人")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? approved_id { get; set; }

       /// <summary>
       ///审核日期
       /// </summary>
       [Display(Name ="审核日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? approved_date { get; set; }

       /// <summary>
       ///状态（0：待审核；1：已批准；2：驳回）
       /// </summary>
       [Display(Name ="状态（0：待审核；1：已批准；2：驳回）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? status { get; set; }

       /// <summary>
       ///描述
       /// </summary>
       [Display(Name ="描述")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string describe { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string remark { get; set; }

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
       ///合约Id
       /// </summary>
       [Display(Name ="合约Id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? contract_id { get; set; }

       
    }
}