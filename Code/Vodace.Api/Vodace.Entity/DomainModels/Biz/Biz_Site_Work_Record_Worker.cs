
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
    [Entity(TableCnName = "Biz_Site_Work_Record_Worker",TableName = "Biz_Site_Work_Record_Worker")]
    public partial class Biz_Site_Work_Record_Worker:BaseEntity
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
       ///工地工作记录id
       /// </summary>
       [Display(Name ="工地工作记录id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? record_id { get; set; }

       /// <summary>
       ///联系人id
       /// </summary>
       [Display(Name ="联系人id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? contact_id { get; set; }

       /// <summary>
       ///valid（0：否；1：是）
       /// </summary>
       [Display(Name ="valid（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_valid { get; set; }

       /// <summary>
       ///工种id
       /// </summary>
       [Display(Name ="工种id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? work_type_id { get; set; }

       /// <summary>
       ///wpic（0：否；1：是）
       /// </summary>
       [Display(Name ="wpic（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_wpic { get; set; }

       /// <summary>
       ///cp（0：否；1：是）
       /// </summary>
       [Display(Name ="cp（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_cp { get; set; }

       /// <summary>
       ///NT/T（0：否；1：是）
       /// </summary>
       [Display(Name ="NT/T（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_nt { get; set; }

       /// <summary>
       ///二维码打卡（0：否；1：是）
       /// </summary>
       [Display(Name ="二维码打卡（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_qr { get; set; }

       /// <summary>
       ///是否生病（0：否；1：是）
       /// </summary>
       [Display(Name ="是否生病（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_sick { get; set; }

       /// <summary>
       ///是否签名（0：否；1：是）
       /// </summary>
       [Display(Name ="是否签名（0：否；1：是）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? is_sign { get; set; }

       /// <summary>
       ///工作备注
       /// </summary>
       [Display(Name ="工作备注")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string work_remark { get; set; }

       /// <summary>
       ///交通津贴
       /// </summary>
       [Display(Name ="交通津贴")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? traffic_allowance { get; set; }

       /// <summary>
       ///工资调整
       /// </summary>
       [Display(Name ="工资调整")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? salary_adj { get; set; }

       /// <summary>
       ///上班时间
       /// </summary>
       [Display(Name ="上班时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? time_in { get; set; }

       /// <summary>
       ///上班时间调整
       /// </summary>
       [Display(Name ="上班时间调整")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? time_in_adj { get; set; }

       /// <summary>
       ///下班时间
       /// </summary>
       [Display(Name ="下班时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? time_out { get; set; }

       /// <summary>
       ///下班时间调整
       /// </summary>
       [Display(Name ="下班时间调整")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? time_out_adj { get; set; }

       /// <summary>
       ///绿卡编号
       /// </summary>
       [Display(Name ="绿卡编号")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string green_card_no { get; set; }

       /// <summary>
       ///绿卡过期时间
       /// </summary>
       [Display(Name ="绿卡过期时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? green_card_exp { get; set; }

       /// <summary>
       ///cic编号
       /// </summary>
       [Display(Name ="cic编号")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string cic_no { get; set; }

       /// <summary>
       ///cic过期时间
       /// </summary>
       [Display(Name ="cic过期时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? cic_exp { get; set; }

       /// <summary>
       ///工人当前所属公司id
       /// </summary>
       [Display(Name ="工人当前所属公司id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? company_id { get; set; }

       /// <summary>
       ///工人当前开工基础工资
       /// </summary>
       [Display(Name ="工人当前开工基础工资")]
       [DisplayFormat(DataFormatString="18,2")]
       [Column(TypeName="decimal")]
       public decimal? base_salary { get; set; }

       /// <summary>
       ///cic序列号
       /// </summary>
       [Display(Name ="cic序列号")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       public string cic_card_no { get; set; }

        /// <summary>
        ///是否SIC（0：否；1：是）
        /// </summary>
        [Display(Name = "是否SIC（0：否；1：是）")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? is_sic { get; set; }
    }
}