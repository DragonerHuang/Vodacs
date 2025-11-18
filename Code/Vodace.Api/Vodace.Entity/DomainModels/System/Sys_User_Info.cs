
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
    [Entity(TableCnName = "用户管理新",TableName = "Sys_User_Info")]
    public partial class Sys_User_Info:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid Id { get; set; }

       /// <summary>
       ///公司id
       /// </summary>
       [Display(Name ="公司id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? Com_Id { get; set; }

       /// <summary>
       ///联系人id
       /// </summary>
       [Display(Name ="联系人id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? Cont_Id { get; set; }
       public int Role_Id { get; set; }
        /// <summary>
        ///账号
        /// </summary>
        [Display(Name ="账号")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Account { get; set; }

       /// <summary>
       ///密码
       /// </summary>
       [Display(Name ="密码")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Password { get; set; }

       /// <summary>
       ///暂定(未想到使用的地方)
       /// </summary>
       [Display(Name ="暂定(未想到使用的地方)")]
       [MaxLength(50)]
       [Column(TypeName="varchar(50)")]
       [Editable(true)]
       public string Org_Password { get; set; }

       /// <summary>
       ///用户名
       /// </summary>
       [Display(Name ="用户名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string UserName { get; set; }

       /// <summary>
       ///用户使用语言 1：英文 2中文
       /// </summary>
       [Display(Name ="用户使用语言 1：英文 2中文")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Lang { get; set; }

       /// <summary>
       ///是否删除（0：是；1：否）
       /// </summary>
       [Display(Name ="是否删除（0：是；1：否）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Enable { get; set; }
        public string Token { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [Display(Name ="备注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? CreateID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_name")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string Creator { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyID")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? ModifyID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Modifier")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string Modifier { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="ModifyDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? ModifyDate { get; set; }

       
    }
}