
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
    [Entity(TableCnName = "用户注册",TableName = "Sys_User_Register")]
    public partial class Sys_User_Register:BaseEntity
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
       ///注册账户
       /// </summary>
       [Display(Name ="注册账户")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string user_account { get; set; }

       /// <summary>
       ///密码
       /// </summary>
       [Display(Name ="密码")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string user_password { get; set; }

       /// <summary>
       ///移动电话
       /// </summary>
       [Display(Name ="移动电话")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       [Required(AllowEmptyStrings=false)]
       public string user_phone { get; set; }

       /// <summary>
       ///中文名称
       /// </summary>
       [Display(Name ="中文名称")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Required(AllowEmptyStrings=false)]
       public string name_cht { get; set; }

       /// <summary>
       ///英文姓名
       /// </summary>
       [Display(Name ="英文姓名")]
       [MaxLength(20)]
       [Column(TypeName="varchar(20)")]
       [Required(AllowEmptyStrings=false)]
       public string name_eng { get; set; }

       /// <summary>
       ///身份证
       /// </summary>
       [Display(Name ="身份证")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Required(AllowEmptyStrings=false)]
       public string id_no { get; set; }

       /// <summary>
       ///电子邮箱
       /// </summary>
       [Display(Name ="电子邮箱")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Required(AllowEmptyStrings=false)]
       public string user_email { get; set; }

       /// <summary>
       ///性别 0：女；1：男
       /// </summary>
       [Display(Name ="性别 0：女；1：男")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int user_gender { get; set; }

       /// <summary>
       ///执照编号
       /// </summary>
       [Display(Name ="公司编号")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       public string company_no { get; set; }

       /// <summary>
       ///0:公司；1：一般个人；2：施工工人
       /// </summary>
       [Display(Name ="0:公司；1：一般个人；2：施工工人")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int user_type { get; set; }

       /// <summary>
       ///是否删除（0：是；1：否）
       /// </summary>
       [Display(Name ="是否删除（0：是；1：否）")]
       [Column(TypeName="int")]
       public int? delete_status { get; set; }

       /// <summary>
       ///注册状态（0：暂存；1：完成）
       /// </summary>
       [Display(Name ="注册状态（0：暂存；1：完成）")]
       [Column(TypeName="int")]
       public int? register_status { get; set; }

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

       
    }
}