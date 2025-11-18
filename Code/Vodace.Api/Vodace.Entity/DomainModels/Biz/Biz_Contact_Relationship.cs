
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
    [Entity(TableCnName = "公司联系人关系表",TableName = "Biz_Contact_Relationship")]
    public partial class Biz_Contact_Relationship:BaseEntity
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
       ///公司ID
       /// </summary>
       [Display(Name ="公司ID")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? company_id { get; set; }

       /// <summary>
       ///联系人ID
       /// </summary>
       [Display(Name ="联系人ID")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? contact_id { get; set; }

       /// <summary>
       ///角色id
       /// </summary>
       [Display(Name ="角色id")]
       [Column(TypeName="int")]
       public int? role_id { get; set; }

       /// <summary>
       ///联络类型：0：qn联系人、1：公司联系人
       /// </summary>
       [Display(Name ="联络类型：0：qn联系人、1：公司联系人")]
       [Column(TypeName="int")]
       public int? relation_type { get; set; }

       /// <summary>
       ///发送邮件类型，Gen，Pay ，Safe ，PD， ABWF ，EL， FS（多选，用,号隔开）
       /// </summary>
       [Display(Name ="发送邮件类型，Gen，Pay ，Safe ，PD， ABWF ，EL， FS（多选，用,号隔开）")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string mail_to { get; set; }

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
       ///对应的表ID
       /// </summary>
       [Display(Name ="对应的表ID")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? relation_id { get; set; }

       /// <summary>
       ///联络人姓名
       /// </summary>
       [Display(Name ="联络人姓名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string name { get; set; }

       /// <summary>
       ///联络人电话
       /// </summary>
       [Display(Name ="联络人电话")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string tel { get; set; }

       /// <summary>
       ///联络人传真
       /// </summary>
       [Display(Name ="联络人传真")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string fax { get; set; }

       /// <summary>
       ///联络人邮箱
       /// </summary>
       [Display(Name ="联络人邮箱")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string email { get; set; }

       /// <summary>
       ///职位、头衔
       /// </summary>
       [Display(Name ="职位、头衔")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string title { get; set; }

       /// <summary>
       ///部门id
       /// </summary>
       [Display(Name ="部门id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? department_id { get; set; }

       /// <summary>
       ///部门名称
       /// </summary>
       [Display(Name ="部门名称")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string department_name { get; set; }

       /// <summary>
       ///角色名称
       /// </summary>
       [Display(Name ="角色名称")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string role_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="org_id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? org_id { get; set; }

       
    }
}