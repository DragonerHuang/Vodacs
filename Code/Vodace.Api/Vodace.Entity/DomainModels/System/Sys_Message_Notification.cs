
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
    [Entity(TableCnName = "消息通知",TableName = "Sys_Message_Notification")]
    public partial class Sys_Message_Notification:BaseEntity
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
       ///消息类型（0：通知；1：消息；2：代办）
       /// </summary>
       [Display(Name ="消息类型（0：通知；1：消息；2：代办）")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int msg_type { get; set; }

       /// <summary>
       ///消息标题
       /// </summary>
       [Display(Name ="消息标题")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string msg_title { get; set; }

       /// <summary>
       ///消息内容
       /// </summary>
       [Display(Name ="消息内容")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string msg_content { get; set; }

       /// <summary>
       ///消息状态（0：未读；1：已读；2：已处理；3：已逾期）
       /// </summary>
       [Display(Name ="消息状态（0：未读；1：已读；2：已处理；3：已逾期）")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int status { get; set; }

       /// <summary>
       ///接收人
       /// </summary>
       [Display(Name ="接收人")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       public string receive_user { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int create_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_name")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string create_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime create_date { get; set; }

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
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
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
       ///关系表Id
       /// </summary>
       [Display(Name ="关系表Id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? relation_id { get; set; }

       /// <summary>
       ///剩余天数
       /// </summary>
       [Display(Name ="剩余天数")]
       [Column(TypeName="int")]
       public int? days_left_to_close { get; set; }

       
    }
}