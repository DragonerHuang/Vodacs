
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
    [Entity(TableCnName = "Biz_Completion_Acceptance",TableName = "Biz_Completion_Acceptance")]
    public partial class Biz_Completion_Acceptance:BaseEntity
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
       ///验收编号
       /// </summary>
       [Display(Name ="验收编号")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string acceptance_no { get; set; }

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
       ///描述
       /// </summary>
       [Display(Name ="描述")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string describe { get; set; }

       /// <summary>
       ///制作人
       /// </summary>
       [Display(Name ="制作人")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? producer_id { get; set; }

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
       ///文件发行日期
       /// </summary>
       [Display(Name ="文件发行日期")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? file_publish_date { get; set; }

       /// <summary>
       ///实际检验日期
       /// </summary>
       [Display(Name ="实际检验日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? actual_inspection_date { get; set; }

       /// <summary>
       ///提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）
       /// </summary>
       [Display(Name ="提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? submit_status { get; set; }

       /// <summary>
       ///合约Id
       /// </summary>
       [Display(Name ="合约Id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? contract_id { get; set; }

       /// <summary>
       ///检验结果（0：不及格；1：及格）
       /// </summary>
       [Display(Name ="检验结果（0：不及格；1：及格）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? test_result { get; set; }

       /// <summary>
       ///客户工程师许可日期
       /// </summary>
       [Display(Name ="客户工程师许可日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? engineer_permit_date { get; set; }

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
       ///验收人
       /// </summary>
       [Display(Name ="验收人")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string inspector { get; set; }

       
    }
}