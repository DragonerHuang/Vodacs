
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
    [Entity(TableCnName = "角色权限",TableName = "Sys_Role_Auth")]
    public partial class Sys_Role_Auth:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="auth_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int auth_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="auth_value")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string auth_value { get; set; }

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
       [Display(Name ="Create_name")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
       [Editable(true)]
       public string Create_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="menu_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int menu_id { get; set; }

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
       ///
       /// </summary>
       [Display(Name ="role_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? role_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="user_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? user_id { get; set; }

       
    }
}