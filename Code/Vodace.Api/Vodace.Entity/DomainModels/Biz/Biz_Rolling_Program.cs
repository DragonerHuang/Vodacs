
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
    [Entity(TableCnName = "Biz_Rolling_Program",TableName = "Biz_Rolling_Program")]
    public partial class Biz_Rolling_Program:BaseEntity
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
       [Display(Name ="project_id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? project_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="contract_id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? contract_id { get; set; }

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
       [Display(Name ="remark")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string remark { get; set; }

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
       ///滚动计划任务id
       /// </summary>
       [Display(Name ="滚动计划任务id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? task_id { get; set; }

       /// <summary>
       ///滚动计划工地内容id
       /// </summary>
       [Display(Name ="滚动计划工地内容id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? sc_id { get; set; }

       /// <summary>
       ///施工具体内容初始化id
       /// </summary>
       [Display(Name ="施工具体内容初始化id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? cc_id { get; set; }

       /// <summary>
       ///内容行号
       /// </summary>
       [Display(Name ="内容行号")]
       [Column(TypeName="int")]
       public int? line_number { get; set; }

       /// <summary>
       ///内容级别：1级、2级、3级，目前最高三级数据
       /// </summary>
       [Display(Name ="内容级别：1级、2级、3级，目前最高三级数据")]
       [Column(TypeName="int")]
       public int? level { get; set; }

       /// <summary>
       ///内容所属组织ids，多个之间以逗号分割
       /// </summary>
       [Display(Name ="内容所属组织ids，多个之间以逗号分割")]
       [MaxLength(500)]
       [Column(TypeName="varchar(500)")]
       public string org_id { get; set; }

       /// <summary>
       ///工地ids，多个之间以逗号之间分割
       /// </summary>
       [Display(Name ="工地ids，多个之间以逗号之间分割")]
       [MaxLength(500)]
       [Column(TypeName="varchar(500)")]
       public string site_id { get; set; }

       /// <summary>
       ///上级数据id
       /// </summary>
       [Display(Name ="上级数据id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? master_id { get; set; }

       /// <summary>
       ///施工内容编码
       /// </summary>
       [Display(Name ="施工内容编码")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string item_code { get; set; }

       /// <summary>
       ///施工内容
       /// </summary>
       [Display(Name ="施工内容")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       public string content { get; set; }

       /// <summary>
       ///开始时间
       /// </summary>
       [Display(Name ="开始时间")]
       [Column(TypeName="datetime")]
       public DateTime? start_date { get; set; }

       /// <summary>
       ///结束时间
       /// </summary>
       [Display(Name ="结束时间")]
       [Column(TypeName="datetime")]
       public DateTime? end_date { get; set; }

       /// <summary>
       ///相差报价细分金额
       /// </summary>
       [Display(Name ="相差报价细分金额")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? quotation { get; set; }

       /// <summary>
       ///值更：早班、中班、晚班
       /// </summary>
       [Display(Name ="值更：早班、中班、晚班")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string duty { get; set; }

       /// <summary>
       ///色块
       /// </summary>
       [Display(Name ="色块")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string color { get; set; }

       /// <summary>
       ///是否路轨范围工作，0-否 1-是
       /// </summary>
       [Display(Name ="是否路轨范围工作，0-否 1-是")]
       [Column(TypeName="int")]
       public int? track_scope { get; set; }

       /// <summary>
       ///扩展类型：text,radio,checkbox,file....
       /// </summary>
       [Display(Name ="扩展类型：text,radio,checkbox,file....")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       public string exp_type { get; set; }

       /// <summary>
       ///扩展类型值
       /// </summary>
       [Display(Name ="扩展类型值")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       public string exp_value { get; set; }

       /// <summary>
       ///工作类型
       /// </summary>
       [Display(Name ="工作类型")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string work_type { get; set; }

       /// <summary>
       ///节点类型
       /// </summary>
       [Display(Name ="节点类型")]
       [MaxLength(60)]
       [Column(TypeName="nvarchar(60)")]
       public string point_type { get; set; }

       /// <summary>
       ///负责人
       /// </summary>
       [Display(Name ="负责人")]
       [Column(TypeName="int")]
       public int? director { get; set; }

        /// <summary>
        ///完成百分比
        /// </summary>
        [Display(Name = "完成百分比")]
        [DisplayFormat(DataFormatString = "18,2")]
        [Column(TypeName = "decimal")]
        public decimal? percentage { get; set; }
    }
}