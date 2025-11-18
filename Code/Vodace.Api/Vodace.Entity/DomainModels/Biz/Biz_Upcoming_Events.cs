
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
    [Entity(TableCnName = "活动列表",TableName = "Biz_Upcoming_Events")]
    public partial class Biz_Upcoming_Events:BaseEntity
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
       ///活动名称
       /// </summary>
       [Display(Name ="活动名称")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string event_name { get; set; }

       /// <summary>
       ///截至日期
       /// </summary>
       [Display(Name ="截至日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime closing_date { get; set; }

       /// <summary>
       ///剩余天数
       /// </summary>
       [Display(Name ="剩余天数")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int days_left_to_close { get; set; }

       /// <summary>
       ///活动类型（0：报价；1：项目；2：滚动计划）
       /// </summary>
       [Display(Name ="活动类型（0：报价；1：项目；2：滚动计划）")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int event_type { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
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
       ///活动编号
       /// </summary>
       [Display(Name ="活动编号")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string event_no { get; set; }

       /// <summary>
       ///活动关联id
       /// </summary>
       [Display(Name ="活动关联id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? relation_id { get; set; }

       /// <summary>
       ///消息推送接收人Id
       /// </summary>
       [Display(Name ="消息推送接收人Id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? recipient_user_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="event_name_eng")]
       [MaxLength(255)]
       [Column(TypeName= "nvarchar(350)")]
       public string event_name_eng { get; set; }

    }
}