
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
    [Entity(TableCnName = "Biz_Site_Work_Record",TableName = "Biz_Site_Work_Record")]
    public partial class Biz_Site_Work_Record:BaseEntity
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
       ///所属合同id
       /// </summary>
       [Display(Name ="所属合同id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? contract_id { get; set; }

       /// <summary>
       ///所属工程进度id
       /// </summary>
       [Display(Name ="所属工程进度id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? rolling_program_id { get; set; }

       /// <summary>
       ///站点id
       /// </summary>
       [Display(Name ="站点id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? site_id { get; set; }

       /// <summary>
       ///子站点id（暂无）
       /// </summary>
       [Display(Name ="子站点id（暂无）")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? sub_site_id { get; set; }

       /// <summary>
       ///管工id
       /// </summary>
       [Display(Name ="管工id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? duty_cp_id { get; set; }

       /// <summary>
       ///当前值更管工id
       /// </summary>
       [Display(Name ="当前值更管工id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? current_duty_cp_id { get; set; }

       /// <summary>
       ///工作内容
       /// </summary>
       [Display(Name ="工作内容")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string job_duties { get; set; }

       /// <summary>
       ///值更
       /// </summary>
       [Display(Name ="值更")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string shift { get; set; }

       /// <summary>
       ///是否进入轨道（0：否；1：是）
       /// </summary>
       [Display(Name ="是否进入轨道（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_track { get; set; }

       /// <summary>
       ///工作日期
       /// </summary>
       [Display(Name ="工作日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? work_date { get; set; }

       /// <summary>
       ///是否完成安全简介（0：否；1：是）
       /// </summary>
       [Display(Name ="是否完成安全简介（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? finish_briefing { get; set; }

       /// <summary>
       ///选择的配置（Sys_Site_Work_Chk_Item）
       /// </summary>
       [Display(Name ="选择的配置（Sys_Site_Work_Chk_Item）")]
       [MaxLength(3000)]
       [Column(TypeName="nvarchar(3000)")]
       [Editable(true)]
       public string check_config { get; set; }

        /// <summary>
        ///检查表参考编号
        /// </summary>
        [Display(Name = "检查表参考编号")]
        [MaxLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        [Editable(true)]
        public string check_ref { get; set; }
    }
}