
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
    [Entity(TableCnName = "角色管理",TableName = "Sys_Role",DBServer = "SysDbContext")]
    public partial class Sys_Role:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="role_id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int role_id { get; set; }

       /// <summary>
       ///角色名称
       /// </summary>
       [Display(Name ="角色名称")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string role_name { get; set; }

       /// <summary>
       ///描述
       /// </summary>
       [Display(Name ="描述")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       public string remark { get; set; }

       /// <summary>
       ///是否启用
       /// </summary>
       [Display(Name ="是否启用")]
       [Column(TypeName="tinyint")]
       public byte? enable { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_date")]
       [Column(TypeName="datetime")]
       public DateTime? create_date { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_name")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string create_name { get; set; }

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
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       public int? delete_status { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="order_no")]
       [Column(TypeName="int")]
       public int? order_no { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="delete_name")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string delete_name { get; set; }

       
    }
}