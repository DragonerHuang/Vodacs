
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
    [Entity(TableCnName = "Sys_User_Relation",TableName = "Sys_User_Relation")]
    public partial class Sys_User_Relation:BaseEntity
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
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? delete_status { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string remark { get; set; }

       /// <summary>
       ///关联类型：0：用户工种
       /// </summary>
       [Display(Name ="关联类型：0：用户工种")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? relation_type { get; set; }

       /// <summary>
       ///用户注册id
       /// </summary>
       [Display(Name ="用户注册id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? user_register_Id { get; set; }

       /// <summary>
       ///关联id
       /// </summary>
       [Display(Name ="关联id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? relation_id { get; set; }


        /// <summary>
        ///白天薪资
        /// </summary>
        [Display(Name = "白天薪资")]
        [DisplayFormat(DataFormatString = "18,2")]
        [Column(TypeName = "decimal")]
        public decimal? day_salary { get; set; }

        /// <summary>
        ///夜班薪资
        /// </summary>
        [Display(Name = "夜班薪资")]
        [DisplayFormat(DataFormatString = "18,2")]
        [Column(TypeName = "decimal")]
        public decimal? night_salary { get; set; }

    }
}