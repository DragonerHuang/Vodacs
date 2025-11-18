
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
    [Entity(TableCnName = "字典数据",TableName = "Sys_Dictionary",DetailTable =  new Type[] { typeof(Sys_Dictionary_List)},DetailTableCnName = "字典明细",DBServer = "SysDbContext")]
    public partial class Sys_Dictionary:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="dic_id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int dic_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="parent_id")]
       [Column(TypeName="int")]
        [Editable(true)]
        [Required(AllowEmptyStrings=false)]
       public int parent_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="config")]
       [MaxLength(4000)]
        [Editable(true)]
        [Column(TypeName="nvarchar(4000)")]
       public string config { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="db_server")]
       [MaxLength(4000)]
        [Editable(true)]
        [Column(TypeName="nvarchar(4000)")]
       public string db_server { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="db_sql")]
       [MaxLength(4000)]
       [Column(TypeName="nvarchar(4000)")]
       public string db_sql { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="dic_name")]
       [MaxLength(100)]
        [Editable(true)]
        [Column(TypeName="nvarchar(100)")]
       [Required(AllowEmptyStrings=false)]
       public string dic_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="dic_no")]
       [MaxLength(100)]
        [Editable(true)]
        [Column(TypeName="nvarchar(100)")]
       [Required(AllowEmptyStrings=false)]
       public string dic_no { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="enable")]
       [Column(TypeName="tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings=false)]
       public byte enable { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="remark")]
       [MaxLength(2000)]
        [Editable(true)]
        [Column(TypeName="nvarchar(2000)")]
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
       ///
       /// </summary>
       [Display(Name ="order_no")]
       [Column(TypeName="int")]
       public int? order_no { get; set; }

       [Display(Name ="字典明细")]
       [ForeignKey("dic_id")]
       public List<Sys_Dictionary_List> Sys_DictionaryList { get; set; }

    }
}