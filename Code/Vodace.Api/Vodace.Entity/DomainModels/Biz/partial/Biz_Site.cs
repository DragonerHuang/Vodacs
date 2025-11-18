
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

    public partial class Biz_Site
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class SiteDto
    {
        /// <summary>
        ///站点Id
        /// </summary>
        [Key]
        [Display(Name = "站点id")]
        [Column(TypeName = "uniqueidentifier")]
        [Required(AllowEmptyStrings = false)]
        public Guid id { get; set; }

        /// <summary>
        ///站点名称（英文）
        /// </summary>
        [Display(Name = "站点名称（英文）")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string name_eng { get; set; }

        /// <summary>
        ///站点名称（繁体）
        /// </summary>
        [Display(Name = "站点名称（繁体）")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string name_cht { get; set; }

        /// <summary>
        ///站点名称（简体）
        /// </summary>
        [Display(Name = "站点名称（简体）")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string name_chs { get; set; }

        /// <summary>
        ///Site（站点） 所属线路
        /// </summary>
        [Display(Name = "Site（站点） 所属线路")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string line { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [Display(Name = "备注")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string remark { get; set; }
        /// <summary>
        ///Site（站点） 缩写
        /// </summary>
        [Display(Name = "Site（站点） 缩写")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string name_sho { get; set; }
    }

    /// <summary>
    /// 返回site表中的id，缩写，英文名，简繁体，创建人，创建时间、
    /// </summary>
    public class SiteDataDto
    {
        /// <summary>
        /// site_id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 站点名称（英文）
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        /// 站点名称（繁体）
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        /// 站点名称（简体）
        /// </summary>
        public string name_chs { get; set; }

        /// <summary>
        /// Site（站点） 缩写
        /// </summary>
        public string name_sho { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_date { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public int? create_id { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string create_name { get; set; }
    }

    /// <summary>
    /// site下拉列表
    /// </summary>
    public class SiteDownDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 名称(存的别名)
        /// </summary>
        public string name { get; set; }
    }
}