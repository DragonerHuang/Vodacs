
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
    [Entity(TableCnName = "Sys_Construction_Content_Init",TableName = "Sys_Construction_Content_Init")]
    public partial class Sys_Construction_Content_Init:BaseEntity
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
       ///内容行号
       /// </summary>
       [Display(Name ="内容行号")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? line_number { get; set; }

       /// <summary>
       ///内容级别：1级、2级、3级，目前最高三级数据
       /// </summary>
       [Display(Name ="内容级别：1级、2级、3级，目前最高三级数据")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? level { get; set; }

       /// <summary>
       ///上级数据id
       /// </summary>
       [Display(Name ="上级数据id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? master_id { get; set; }

       /// <summary>
       ///外链接id
       /// </summary>
       [Display(Name ="外链接id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? external_link_id { get; set; }

       /// <summary>
       ///施工内容编码
       /// </summary>
       [Display(Name ="施工内容编码")]
       [MaxLength(250)]
       [Column(TypeName="nvarchar(250)")]
       [Editable(true)]
       public string item_code { get; set; }

       /// <summary>
       ///施工内容
       /// </summary>
       [Display(Name ="施工内容")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       [Editable(true)]
       public string content { get; set; }

       /// <summary>
       ///工程类型：Pre Work, Site Work, Site Survey, Sub-C.Work, T&C, O&M
       /// </summary>
       [Display(Name ="工程类型：Pre Work, Site Work, Site Survey, Sub-C.Work, T&C, O&M")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       [Editable(true)]
       public string work_type { get; set; }

       /// <summary>
       ///工作类型：Check Point, Whole Point, Daily Point
       /// </summary>
       [Display(Name ="工作类型：Check Point, Whole Point, Daily Point")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       [Editable(true)]
       public string point_type { get; set; }

       /// <summary>
       ///扩展属性
       /// </summary>
       [Display(Name ="扩展属性")]
       [MaxLength(2000)]
       [Column(TypeName="varchar(2000)")]
       [Editable(true)]
       public string extend_attr { get; set; }

       /// <summary>
       ///扩展类型：text,radio,checkbox,file....
       /// </summary>
       [Display(Name ="扩展类型：text,radio,checkbox,file....")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Editable(true)]
       public string exp_type { get; set; }

       /// <summary>
       ///扩展类型值
       /// </summary>
       [Display(Name ="扩展类型值")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string exp_value { get; set; }

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
       [Display(Name ="remark")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string remark { get; set; }

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

       
    }
}