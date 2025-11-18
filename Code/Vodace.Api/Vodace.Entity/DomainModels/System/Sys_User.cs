using Newtonsoft.Json;

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
    [Entity(TableCnName = "用户管理", TableName = "Sys_User", DBServer = "SysDbContext")]
    public partial class Sys_User : BaseEntity
    {
        /// <summary>
        ///
        /// </summary>
        [Key]
        [Display(Name = "User_Id")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int User_Id { get; set; }

        /// <summary>
        ///帐号
        /// </summary>
        [Display(Name = "帐号")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }

        /// <summary>
        ///姓名
        /// </summary>
        [Display(Name = "姓名")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string UserTrueName { get; set; }

        /// <summary>
        ///密码
        /// </summary>
        [Display(Name = "密码")]
        [MaxLength(200)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string UserPwd { get; set; }

        /// <summary>
        ///角色
        /// </summary>
        [Display(Name = "角色")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Role_Id { get; set; }

        /// <summary>
        ///性别
        /// </summary>
        [Display(Name = "性别")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? Gender { get; set; }

        /// <summary>
        ///是否可用
        /// </summary>
        [Display(Name = "是否可用")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Enable { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string Email { get; set; }

        /// <summary>
        ///Token
        /// </summary>
        [Display(Name = "Token")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        public string Token { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [Display(Name = "备注")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        public string Remark { get; set; }

        /// <summary>
        ///手机号
        /// </summary>
        [Display(Name = "手机号")]
        [MaxLength(11)]
        [Column(TypeName = "nvarchar(11)")]
        [Editable(true)]
        public string PhoneNo { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "CreateID")]
        [Column(TypeName = "int")]
        public int? CreateID { get; set; }

        /// <summary>
        ///最后登陆时间
        /// </summary>
        [Display(Name = "最后登陆时间")]
        [Column(TypeName = "datetime")]
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        ///最后密码修改时间
        /// </summary>
        [Display(Name = "最后密码修改时间")]
        [Column(TypeName = "datetime")]
        public DateTime? LastModifyPwdDate { get; set; }

        /// <summary>
        ///创建人
        /// </summary>
        [Display(Name = "创建人")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Creator { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "ModifyID")]
        [Column(TypeName = "int")]
        public int? ModifyID { get; set; }

        /// <summary>
        ///注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        ///修改人
        /// </summary>
        [Display(Name = "修改人")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Modifier { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "Address")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Address { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "id")]
        [Column(TypeName = "uniqueidentifier")]
        [Required(AllowEmptyStrings = false)]
        public Guid Id { get; set; }

        /// <summary>
        ///公司表外键
        /// </summary>
        [Display(Name = "公司表外键")]
        [Column(TypeName = "uniqueidentifier")]
        public Guid? Com_Id { get; set; }

        /// <summary>
        ///联系人外键
        /// </summary>
        [Display(Name = "联系人外键")]
        [Column(TypeName = "uniqueidentifier")]
        public Guid? Cont_Id { get; set; }

        /// <summary>
        ///语言
        /// </summary>
        [Display(Name = "语言")]
        [Column(TypeName = "int")]
        public int? Lang { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "UserNameEng")]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string UserNameEng { get; set; }


    }
}
