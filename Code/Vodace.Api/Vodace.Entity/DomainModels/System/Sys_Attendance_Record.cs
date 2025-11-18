
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
    [Entity(TableCnName = "打卡管理",TableName = "Sys_Attendance_Record")]
    public partial class Sys_Attendance_Record:BaseEntity
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
       ///
       /// </summary>
       [Display(Name ="user_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int user_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="user_no")]
       [MaxLength(255)]
       [Column(TypeName="varchar(255)")]
       [Editable(true)]
       public string user_no { get; set; }

       /// <summary>
       ///打卡时间
       /// </summary>
       [Display(Name ="打卡时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? punch_time { get; set; }

       /// <summary>
       ///设备ID（例如 打卡机：1，手机：2）
       /// </summary>
       [Display(Name ="设备ID（例如 打卡机：1，手机：2）")]
       [Column(TypeName = "int")]
       [Editable(true)]
       public int device_source { get; set; }

       /// <summary>
       ///地点ID
       /// </summary>
       [Display(Name ="地点ID")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? location_id { get; set; }

       /// <summary>
       ///纬度
       /// </summary>
       [Display(Name ="纬度")]
       [DisplayFormat(DataFormatString="10,6")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? latitude { get; set; }

       /// <summary>
       ///经度
       /// </summary>
       [Display(Name ="经度")]
       [DisplayFormat(DataFormatString="10,6")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? longitude { get; set; }

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
       ///打卡地点
       /// </summary>
       [Display(Name ="打卡地点")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string adderss { get; set; }

       
    }
}