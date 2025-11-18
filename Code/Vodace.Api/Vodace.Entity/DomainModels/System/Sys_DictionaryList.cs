
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
    [Entity(TableCnName = "字典明细",TableName = "Sys_DictionaryList",DBServer = "SysDbContext")]
    public partial class Sys_Dictionary_List:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="dic_list_id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int dic_list_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="dic_name")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string dic_name { get; set; }
        /// </summary>
        [Display(Name = "dic_name_eng")]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]

        public string dic_name_eng { get; set; }
        /// <summary>
        ///
        /// </summary>
        [Display(Name ="dic_value")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string dic_value { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="dic_id")]
       [Column(TypeName="int")]
       public int? dic_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="enable")]
       [Column(TypeName="tinyint")]
       public byte? enable { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="order_no")]
       [Column(TypeName="int")]
       public int? order_no { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="remark")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       public string remark { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       public int? delete_status { get; set; }

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