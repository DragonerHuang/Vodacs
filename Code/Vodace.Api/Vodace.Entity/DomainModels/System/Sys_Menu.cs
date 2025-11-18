
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
    [Entity(TableCnName = "菜单管理",TableName = "Sys_Menu")]
    public partial class Sys_Menu:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Display(Name ="icon")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string icon { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="menu_id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int menu_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="menu_name")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Required(AllowEmptyStrings=false)]
       public string menu_name { get; set; }

       /// <summary>
       ///菜单名称_英文
       /// </summary>
       [Display(Name ="菜单名称_英文")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string menu_name_us { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="menu_name_tw")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string menu_name_tw { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="auth")]
       [MaxLength(4000)]
       [Column(TypeName="nvarchar(4000)")]
       public string auth { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="component")]
       [MaxLength(80)]
       [Column(TypeName="varchar(80)")]
       public string component { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="description")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string description { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="enable")]
       [Column(TypeName="tinyint")]
       public byte? enable { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="orderNo")]
       [Column(TypeName="int")]
       public int? orderNo { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="table_name")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string table_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="parent_id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int parent_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="url")]
       [MaxLength(4000)]
       [Column(TypeName="nvarchar(4000)")]
       public string url { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="menu_type")]
       [Column(TypeName="int")]
       public int? menu_type { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       public int? delete_status { get; set; }

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
       [Display(Name ="modify_name")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       public string modify_name { get; set; }

        [Display(Name = "parent_title")]
        [MaxLength(30)]
        [Column(TypeName = "nvarchar(50)")]
        public string parent_title { get; set; }

        [Display(Name = "hidden")]
        [Column(TypeName = "tinyint")]
        public byte? hidden { get; set; }
        /// <summary>
        ///
        /// </summary>
        [Display(Name ="modify_date")]
       [Column(TypeName="datetime")]
       public DateTime? modify_date { get; set; }
        public List<Sys_Actions> Actions { get; set; }


    }
}