
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
    [Entity(TableCnName = "项目管理",TableName = "Biz_Project")]
    public partial class Biz_Project:BaseEntity
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
       ///所属公司id
       /// </summary>
       [Display(Name ="所属公司id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? company_id { get; set; }

       /// <summary>
       ///所属上级项目ID

       /// </summary>
       [Display(Name ="所属上级项目ID")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? master_id { get; set; }

       /// <summary>
       ///上级新增项目ID（vo/wo）
       /// </summary>
       [Display(Name ="上级新增项目ID（vo/wo）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? original_id { get; set; }

       /// <summary>
       ///客户id（M_Sub_Contractors.id）
       /// </summary>
       [Display(Name ="客户id（M_Sub_Contractors.id）")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? customer_id { get; set; }

       /// <summary>
       ///简称
       /// </summary>
       [Display(Name ="简称")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string name_sho { get; set; }

       /// <summary>
       ///英文名
       /// </summary>
       [Display(Name ="英文名")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string name_eng { get; set; }

       /// <summary>
       ///中文名
       /// </summary>
       [Display(Name ="中文名")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string name_cht { get; set; }

       /// <summary>
       ///别名
       /// </summary>
       [Display(Name ="别名")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string name_ali { get; set; }

       /// <summary>
       ///期望开始时间
       /// </summary>
       [Display(Name ="期望开始时间")]
       [Column(TypeName="date")]
       public DateTime? exp_start_date { get; set; }

       /// <summary>
       ///实际开始时间
       /// </summary>
       [Display(Name ="实际开始时间")]
       [Column(TypeName="date")]
       public DateTime? act_start_date { get; set; }

       /// <summary>
       ///期望结束时间
       /// </summary>
       [Display(Name ="期望结束时间")]
       [Column(TypeName="date")]
       public DateTime? exp_end_date { get; set; }

       /// <summary>
       ///实际结束时间
       /// </summary>
       [Display(Name ="实际结束时间")]
       [Column(TypeName="date")]
       public DateTime? act_end_date { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
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
       ///
       /// </summary>
       [Display(Name ="project_type_id")]
       [Column(TypeName="int")]
       public int? project_type_id { get; set; }

       /// <summary>
       ///项目编号
       /// </summary>
       [Display(Name ="项目编号")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       public string project_no { get; set; }

       
    }
}