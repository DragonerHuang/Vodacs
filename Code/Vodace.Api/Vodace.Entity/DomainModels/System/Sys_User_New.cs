
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
    [Entity(TableCnName = "用户管理新",TableName = "Sys_User_New")]
    public partial class Sys_User_New:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>

       [Display(Name ="id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Key]
        [Display(Name ="user_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int user_id { get; set; }

       /// <summary>
       ///公司ID
       /// </summary>
       [Display(Name ="公司ID")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? company_id { get; set; }

       /// <summary>
       ///联系人ID
       /// </summary>
       [Display(Name ="联系人ID")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? contact_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="role_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int role_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="phone_no")]
       [MaxLength(11)]
       [Column(TypeName="nvarchar(11)")]
       [Editable(true)]
       public string phone_no { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="remark")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="user_name")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string user_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="user_true_name")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string user_true_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="user_pwd")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string user_pwd { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="user_name_eng")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       public string user_name_eng { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="lang")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? lang { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="email")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string email { get; set; }

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
       [Display(Name ="gender")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? gender { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="last_login_date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? last_login_date { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="address")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string address { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="token")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string token { get; set; }

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
        /// 用户注册表Id
        /// </summary>
       public Guid? user_register_id { get; set; }
        /// <summary>
        /// 来源（0：平台；1：APP）
        /// </summary>
        public int source { get; set; }
        /// <summary>
        /// 工号（存储数字）
        /// </summary>
        public string user_no { get; set; }
        public string login_ip { get; set; }

        /// <summary>
        /// 入职时间（Date of joining）
        /// </summary>
        public DateTime? doj { get; set; }
        /// <summary>
        /// 是否在职 1-是 0-否
        /// </summary>
        public int? is_current { get; set; }
    }
}