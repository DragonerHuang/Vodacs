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
    [Entity(TableCnName = "Sys_Employee_Management ", TableName = "Sys_Employee_Management")]
    public partial class Sys_Employee_Management : BaseEntity
    {
        /// <summary>
        ///主键ID
        /// </summary>
        [Key]
        [Display(Name = "主键ID")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public Guid id { get; set; }

        /// <summary>
        ///是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        [Display(Name = "是否删除（0：正常；1：删除；2：数据库手删除）")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? delete_status { get; set; }

        /// <summary>
        ///创建人ID
        /// </summary>
        [Display(Name = "创建人ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? create_id { get; set; }

        /// <summary>
        ///创建人姓名
        /// </summary>
        [Display(Name = "创建人姓名")]
        [MaxLength(60)]
        [Column(TypeName = "nvarchar(60)")]
        [Editable(true)]
        public string create_name { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? create_date { get; set; }

        /// <summary>
        ///修改人ID
        /// </summary>
        [Display(Name = "修改人ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? modify_id { get; set; }

        /// <summary>
        ///修改人姓名
        /// </summary>
        [Display(Name = "修改人姓名")]
        [MaxLength(60)]
        [Column(TypeName = "nvarchar(60)")]
        [Editable(true)]
        public string modify_name { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? modify_date { get; set; }

        // /// <summary>
        // ///员工编号
        // /// </summary>
        // [Display(Name = "员工编号")]
        // [MaxLength(50)]
        // [Column(TypeName = "nvarchar(50)")]
        // [Editable(true)]
        // [Required(AllowEmptyStrings = false)]
        // public string sys_employee_management_number { get; set; }
        //
        // /// <summary>
        // ///中文名
        // /// </summary>
        // [Display(Name = "中文名")]
        // [MaxLength(100)]
        // [Column(TypeName = "nvarchar(100)")]
        // [Editable(true)]
        // public string chinese_name { get; set; }
        //
        // /// <summary>
        // ///英文名
        // /// </summary>
        // [Display(Name = "英文名")]
        // [MaxLength(100)]
        // [Column(TypeName = "nvarchar(100)")]
        // [Editable(true)]
        // public string english_name { get; set; }
        //
        // /// <summary>
        // ///身份证号
        // /// </summary>
        // [Display(Name = "身份证号")]
        // [MaxLength(50)]
        // [Column(TypeName = "nvarchar(50)")]
        // [Editable(true)]
        // public string card_no { get; set; }
        //
        // /// <summary>
        // ///电话
        // /// </summary>
        // [Display(Name = "电话")]
        // [MaxLength(50)]
        // [Column(TypeName = "nvarchar(50)")]
        // [Editable(true)]
        // public string phone { get; set; }
        //
        // /// <summary>
        // ///邮箱
        // /// </summary>
        // [Display(Name = "邮箱")]
        // [MaxLength(100)]
        // [Column(TypeName = "nvarchar(100)")]
        // [Editable(true)]
        // public string email { get; set; }


        /// <summary>
        ///基本工资
        /// </summary>
        [Display(Name = "基本工资")]
        [DisplayFormat(DataFormatString = "18,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        public decimal? basic_salary { get; set; }

        /// <summary>
        ///入职日期
        /// </summary>
        [Display(Name = "入职日期")]
        [Column(TypeName = "date")]
        [Editable(true)]
        public DateTime? record_date { get; set; }

        /// <summary>
        ///离职日期
        /// </summary>
        [Display(Name = "离职日期")]
        [Column(TypeName = "date")]
        [Editable(true)]
        public DateTime? leave_date { get; set; }

        /// <summary>
        ///休假结算日期
        /// </summary>
        [Display(Name = "休假结算日期")]
        [Column(TypeName = "date")]
        public DateTime? leave_settlement_date { get; set; }

        /// <summary>
        ///婚姻状态
        /// </summary>
        [Display(Name = "婚姻状态")]
        [Column(TypeName = "int")]
        public int? marital_status { get; set; }

        /// <summary>
        ///试用期(月)
        /// </summary>
        [Display(Name = "试用期(月)")]
        [Column(TypeName = "int")]
        public int? trial_period { get; set; }

        /// <summary>
        ///离职原因
        /// </summary>
        [Display(Name = "离职原因")]
        [MaxLength(400)]
        [Column(TypeName = "nvarchar(400)")]
        public string leave_reason { get; set; }

        /// <summary>
        ///科室
        /// </summary>
        [Display(Name = "科室")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string section { get; set; }

        /// <summary>
        ///部门
        /// </summary>
        [Display(Name = "部门")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        public Guid? department_id { get; set; }

        /// <summary>
        ///假期类型 连接Sys_Leave_Type
        /// </summary>
        [Display(Name = "假期类型 连接Sys_Leave_Type")]
        [Column(TypeName = "uniqueidentifier")]
        public Guid? leave_type_id { get; set; }

        /// <summary>
        ///工作时长(日/小时)
        /// </summary>
        [Display(Name = "工作时长(日/小时)")]
        [Column(TypeName = "int")]
        public int? work_hour { get; set; }

        /// <summary>
        ///工作天数(周/天)
        /// </summary>
        [Display(Name = "工作天数(周/天)")]
        [Column(TypeName = "int")]
        public int? work_day { get; set; }

        /// <summary>
        ///假期组别
        /// </summary>
        [Display(Name = "假期组别")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string leave_group { get; set; }

        /// <summary>
        ///工资类型
        /// </summary>
        [Display(Name = "工资类型")]
        [Column(TypeName = "int")]
        public int? salary_type { get; set; }

        /// <summary>
        ///工资金额
        /// </summary>
        [Display(Name = "工资金额")]
        [DisplayFormat(DataFormatString = "18,2")]
        [Column(TypeName = "decimal")]
        public decimal? salary_amount { get; set; }

        /// <summary>
        ///调整金额
        /// </summary>
        [Display(Name = "调整金额")]
        [DisplayFormat(DataFormatString = "18,2")]
        [Column(TypeName = "decimal")]
        public decimal? adjustment_amount { get; set; }

        /// <summary>
        ///生效日期
        /// </summary>
        [Display(Name = "生效日期")]
        [Column(TypeName = "date")]
        public DateTime? effective_date { get; set; }

        /// <summary>
        ///强基金计划
        /// </summary>
        [Display(Name = "强基金计划")]
        [Column(TypeName = "int")]
        public int? mpf_plan { get; set; }

        /// <summary>
        ///强基金类型
        /// </summary>
        [Display(Name = "强基金类型")]
        [Column(TypeName = "int")]
        public int? mpf_type { get; set; }

        /// <summary>
        ///雇主强积金
        /// </summary>
        [Display(Name = "雇主强积金")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string contribution_mpf_account { get; set; }

        /// <summary>
        ///雇员强积金
        /// </summary>
        [Display(Name = "雇员强积金")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string employee_mpf_account { get; set; }

        /// <summary>
        ///自愿性供款强积金
        /// </summary>
        [Display(Name = "自愿性供款强积金")]
        [Column(TypeName = "int")]
        public int? voluntary_mpf_contribution { get; set; }

        /// <summary>
        ///自愿性供款强积金类型
        /// </summary>
        [Display(Name = "自愿性供款强积金类型")]
        [Column(TypeName = "int")]
        public int? voluntary_mpf_contribution_type { get; set; }

        /// <summary>
        ///员工自愿性供款强积金
        /// </summary>
        [Display(Name = "员工自愿性供款强积金")]
        [DisplayFormat(DataFormatString = "18,2")]
        [Column(TypeName = "decimal")]
        public decimal? employee_voluntary_contribution { get; set; }

        /// <summary>
        ///雇主自愿性供款强积金
        /// </summary>
        [Display(Name = "雇主自愿性供款强积金")]
        [DisplayFormat(DataFormatString = "18,2")]
        [Column(TypeName = "decimal")]
        public decimal? employer_voluntary_contribution { get; set; }

        /// <summary>
        ///支付信息
        /// </summary>
        [Display(Name = "支付信息")]
        [MaxLength(1000)]
        [Column(TypeName = "nvarchar(1000)")]
        public string bank_info { get; set; }

        /// <summary>
        ///职位
        /// </summary>
        [Display(Name = "职位")]
        [Column(TypeName = "uniqueidentifier")]
        public Guid? organization_id { get; set; }

        /// <summary>
        ///用户id(guid)
        /// </summary>
        [Display(Name = "用户id(guid)")]
        [Column(TypeName = "uniqueidentifier")]
        public Guid? user_id { get; set; }

        /// <summary>
        ///年假天数
        /// </summary>
        [Display(Name = "年假天数")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? leave_day { get; set; }
    }
}