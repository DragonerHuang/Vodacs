
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
    [Entity(TableCnName = "文件管理",TableName = "Sys_File_Records")]
    public partial class Sys_File_Records:BaseEntity
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
       ///文件扩展
       /// </summary>
       [Display(Name ="文件扩展")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(40)")]
       public string file_ext { get; set; }

       /// <summary>
       ///文件大小、单位是字节
       /// </summary>
       [Display(Name ="文件大小、单位是字节")]
       [Column(TypeName="int")]
       public int? file_size { get; set; }

       /// <summary>
       ///文件路径
       /// </summary>
       [Display(Name ="文件路径")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string file_path { get; set; }

       /// <summary>
       ///文件类型代码
       /// </summary>
       [Display(Name ="文件类型代码")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string file_code { get; set; }

       /// <summary>
       ///文件所属id（在哪个地方上传的，例如项目、报价、报价确认啥的）
       /// </summary>
       [Display(Name ="文件所属id（在哪个地方上传的，例如项目、报价、报价确认啥的）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? master_id { get; set; }

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

       /// <summary>
       ///上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）
       /// </summary>
       [Display(Name ="上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）")]
       [Column(TypeName="int")]
       public int? upload_status { get; set; }

       /// <summary>
       ///文件名
       /// </summary>
       [Display(Name ="文件名")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string file_name { get; set; }

       
    }
}