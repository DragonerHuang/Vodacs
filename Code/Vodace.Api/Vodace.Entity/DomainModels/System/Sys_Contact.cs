
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
    [Entity(TableCnName = "联系人管理",TableName = "Sys_Contact")]
    public partial class Sys_Contact:BaseEntity
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
       ///Sys_Company、所属公司\一般个人
       /// </summary>
       [Display(Name ="Sys_Company、所属公司一般个人")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? company_id { get; set; }

       /// <summary>
       ///简称
       /// </summary>
       [Display(Name ="简称")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string name_sho { get; set; }

       /// <summary>
       ///英文名
       /// </summary>
       [Display(Name ="英文名")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string name_eng { get; set; }

       /// <summary>
       ///中文名
       /// </summary>
       [Display(Name ="中文名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string name_cht { get; set; }

       /// <summary>
       ///别名
       /// </summary>
       [Display(Name ="别名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string name_ali { get; set; }

       /// <summary>
       ///所属部门
       /// </summary>
       [Display(Name ="所属部门")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? department_id { get; set; }

       /// <summary>
       ///头衔、职称
       /// </summary>
       [Display(Name ="头衔、职称")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string title { get; set; }

       /// <summary>
       ///地址
       /// </summary>
       [Display(Name ="地址")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string address { get; set; }

       /// <summary>
       ///‌行政区id
       /// </summary>
       [Display(Name ="‌行政区id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? district_id { get; set; }

       /// <summary>
       ///邮箱
       /// </summary>
       [Display(Name ="邮箱")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string email { get; set; }

       /// <summary>
       ///工人日薪
       /// </summary>
       [Display(Name ="工人日薪")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? daily_salary { get; set; }

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
       ///传真
       /// </summary>
       [Display(Name ="传真")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string fax { get; set; }

       /// <summary>
       ///组织
       /// </summary>
       [Display(Name ="组织")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? organization_id { get; set; }

       /// <summary>
       ///身份证
       /// </summary>
       [Display(Name ="身份证")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string id_no { get; set; }

       /// <summary>
       ///注册账户
       /// </summary>
       [Display(Name ="注册账户")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string user_account { get; set; }

       /// <summary>
       ///移动电话
       /// </summary>
       [Display(Name ="移动电话")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       public string user_phone { get; set; }

       /// <summary>
       ///性别 0：女；1：男
       /// </summary>
       [Display(Name ="性别 0：女；1：男")]
       [Column(TypeName="int")]
       public int? user_gender { get; set; }

       
    }
}