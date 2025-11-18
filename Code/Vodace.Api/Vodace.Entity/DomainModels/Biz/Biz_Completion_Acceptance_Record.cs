
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
    [Entity(TableCnName = "Biz_Completion_Acceptance_Record",TableName = "Biz_Completion_Acceptance_Record")]
    public partial class Biz_Completion_Acceptance_Record:BaseEntity
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
       ///验收提交id
       /// </summary>
       [Display(Name ="验收提交id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid acceptance_id { get; set; }

       /// <summary>
       ///版本
       /// </summary>
       [Display(Name ="版本")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int version { get; set; }

       /// <summary>
       ///内部状态（0：编辑中；1：审核中；2：已提交；3：取消；）
       /// </summary>
       [Display(Name ="内部状态（0：编辑中；1：审核中；2：已提交；3：取消；）")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int inner_status { get; set; }

       /// <summary>
       ///提交时间
       /// </summary>
       [Display(Name ="提交时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? submit_date { get; set; }

       /// <summary>
       ///提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）
       /// </summary>
       [Display(Name ="提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? submit_status { get; set; }

       /// <summary>
       ///提交文件
       /// </summary>
       [Display(Name ="提交文件")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string file_name { get; set; }

       /// <summary>
       ///文件路径
       /// </summary>
       [Display(Name ="文件路径")]
       [MaxLength(300)]
       [Column(TypeName="nvarchar(300)")]
       [Editable(true)]
       public string file_path { get; set; }

       /// <summary>
       ///文件类型（0：主文件；1：编辑文件；2：客户评语；3：参考文件；4：内部验收凭证；5：客户验收凭证）
       /// </summary>
       [Display(Name ="文件类型（0：主文件；1：编辑文件；2：客户评语；3：参考文件；4：内部验收凭证；5：客户验收凭证）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? file_type { get; set; }

       /// <summary>
       ///文件扩展
       /// </summary>
       [Display(Name ="文件扩展")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Editable(true)]
       public string file_ext { get; set; }

       /// <summary>
       ///文件大小、单位是字节
       /// </summary>
       [Display(Name ="文件大小、单位是字节")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? file_size { get; set; }

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
       ///检查清单
       /// </summary>
       [Display(Name ="检查清单")]
       [MaxLength(300)]
       [Column(TypeName="nvarchar(300)")]
       [Editable(true)]
       public string check_list { get; set; }

       /// <summary>
       ///审核人
       /// </summary>
       [Display(Name ="审核人")]
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
       ///上传人
       /// </summary>
       [Display(Name ="上传人")]
       [Column(TypeName="int")]
       public int? upload_user_id { get; set; }

       /// <summary>
       ///上传时间
       /// </summary>
       [Display(Name ="上传时间")]
       [Column(TypeName="datetime")]
       public DateTime? upload_date { get; set; }

       
    }
}