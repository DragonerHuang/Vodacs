
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
    [Entity(TableCnName = "Biz_Project_Files",TableName = "Biz_Project_Files")]
    public partial class Biz_Project_Files:BaseEntity
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
       ///关联id
       /// </summary>
       [Display(Name ="关联id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? relation_id { get; set; }

       /// <summary>
       ///文件类型（0：开工前，1：施工中，2：完工后）
       /// </summary>
       [Display(Name ="文件类型（0：开工前，1：施工中，2：完工后）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? file_type { get; set; }

       /// <summary>
       ///文件排序号
       /// </summary>
       [Display(Name ="文件排序号")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? file_index { get; set; }

       /// <summary>
       ///文件名称
       /// </summary>
       [Display(Name ="文件名称")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string file_name { get; set; }

       /// <summary>
       ///图片缩略图
       /// </summary>
       [Display(Name ="图片缩略图")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string file_thumbnail_name { get; set; }

       /// <summary>
       ///文件路径
       /// </summary>
       [Display(Name ="文件路径")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string file_path { get; set; }

       /// <summary>
       ///图片缩略图路径
       /// </summary>
       [Display(Name ="图片缩略图路径")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string file_thumbnail_path { get; set; }

       /// <summary>
       ///文件后缀名
       /// </summary>
       [Display(Name ="文件后缀名")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
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
       ///上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）
       /// </summary>
       [Display(Name ="上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? upload_status { get; set; }

       /// <summary>
       ///项目ID
       /// </summary>
       [Display(Name ="项目ID")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? project_id { get; set; }

       /// <summary>
       ///版本
       /// </summary>
       [Display(Name ="版本")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int version { get; set; }

       /// <summary>
       ///内部状态（0：编辑中；1：审核中；2：已提交；3：取消；）
       /// </summary>
       [Display(Name ="内部状态（0：编辑中；1：审核中；2：已提交；3：取消；）")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int inner_status { get; set; }

       /// <summary>
       ///提交时间
       /// </summary>
       [Display(Name ="提交时间")]
       [Column(TypeName="datetime")]
       public DateTime? submit_date { get; set; }

       /// <summary>
       ///提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）
       /// </summary>
       [Display(Name ="提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）")]
       [Column(TypeName="int")]
       public int? submit_status { get; set; }

       /// <summary>
       ///检查清单
       /// </summary>
       [Display(Name ="检查清单")]
       [MaxLength(300)]
       [Column(TypeName="nvarchar(300)")]
       public string check_list { get; set; }

       /// <summary>
       ///审核日期
       /// </summary>
       [Display(Name ="审核日期")]
       [Column(TypeName="datetime")]
       public DateTime? approved_date { get; set; }

       /// <summary>
       ///审核人
       /// </summary>
       [Display(Name ="审核人")]
       [Column(TypeName="int")]
       public int? approved_id { get; set; }

       /// <summary>
       ///文件版本
       /// </summary>
       [Display(Name ="文件版本")]
       [Column(TypeName="int")]
       public int? file_version { get; set; }

       
    }
}