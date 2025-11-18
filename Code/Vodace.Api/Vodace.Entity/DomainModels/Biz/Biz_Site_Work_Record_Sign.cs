
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
    [Entity(TableCnName = "Biz_Site_Work_Record_Sign",TableName = "Biz_Site_Work_Record_Sign")]
    public partial class Biz_Site_Work_Record_Sign:BaseEntity
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
       ///工地工作记录id
       /// </summary>
       [Display(Name ="工地工作记录id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? record_id { get; set; }

       /// <summary>
       ///对应的表ID
       /// </summary>
       [Display(Name ="对应的表ID")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? relation_id { get; set; }

       /// <summary>
       ///签名类型（0：工人签名）
       /// </summary>
       [Display(Name ="签名类型（0：工人签名）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? relation_type { get; set; }

       /// <summary>
       ///路径
       /// </summary>
       [Display(Name ="路径")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string file_path { get; set; }

       /// <summary>
       ///模糊图片的路径
       /// </summary>
       [Display(Name ="模糊图片的路径")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string file_blurry_path { get; set; }

        /// <summary>
        ///签名人
        /// </summary>
        [Display(Name = "签名人")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string sign_name { get; set; }
    }
}