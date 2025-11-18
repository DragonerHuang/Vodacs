using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace Vodace.Entity.DomainModels
{
    public partial class Sys_Employee_Management
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class SysEmpMentCreateOrUpdateDto
    {
        [Required] public SysEmpMentInputDto SysEmpMentInput { get; set; }
    }

    /// <summary>
    /// 编辑返回
    /// </summary>
    public class SysEmpMentOutputDto : SysEmpMentInputDto
    {
    }

    /// <summary>
    /// 创建或修改
    /// </summary>
    public class SysEmpMentInputDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid? id { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string sys_employee_management_number { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string chinese_name { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string english_name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string card_no { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int? user_gender { get; set; }

        /// <summary>
        /// 婚姻状态
        /// </summary>
        public int? marital_status { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public Guid? department { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        public string section { get; set; }

        /// <summary>
        /// 职位(工种)
        /// </summary>
        public Guid? organization_id { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime? record_date { get; set; }

        /// <summary>
        /// 离职日期
        /// </summary>
        public DateTime? leave_date { get; set; }

        /// <summary>
        /// 离职原因
        /// </summary>
        public string leave_reason { get; set; }

        /// <summary>
        /// 试用期(月)
        /// </summary>
        public int? trial_period { get; set; }

        /// <summary>
        /// 工作时长(日/小时)
        /// </summary>
        public int? work_hour { get; set; }

        /// <summary>
        /// 工作天数(周/天)
        /// </summary>
        public int? work_day { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public decimal? basic_salary { get; set; }

        /// <summary>
        /// 工资类型 (例如：基本工资、绩效奖金、岗位津贴)
        /// Salary Type (e.g., Basic Salary, Performance Bonus, Position Allowance)
        /// </summary>
        public int? salary_type { get; set; }

        /// <summary>
        /// 工资金额
        /// Salary Amount
        /// </summary>
        public decimal? salary_amount { get; set; }

        /// <summary>
        /// 调整金额 (正数表示增加，负数表示减少)
        /// Adjustment Amount (Positive for increase, negative for decrease)
        /// </summary>
        public decimal? adjustment_amount { get; set; }

        /// <summary>
        /// 生效日期
        /// Effective Date
        /// </summary>
        public DateTime? effective_date { get; set; }

        /// <summary>
        /// 强基金计划
        /// </summary>
        public int? mpf_plan { get; set; }

        /// <summary>
        /// 强基金类型
        /// </summary>
        public int? mpf_type { get; set; }

        /// <summary>
        /// 雇主强积金
        /// </summary>
        public string contribution_mpf_account { get; set; }

        /// <summary>
        /// 催主强积金
        /// </summary>
        public string employee_mpf_account { get; set; }

        /// <summary>
        /// 自愿性供款强积金
        /// </summary>
        public int? voluntary_mpf_contribution { get; set; }

        /// <summary>
        /// 自愿性供款强积金类型
        /// </summary>
        public int? voluntary_mpf_contribution_type { get; set; }

        /// <summary>
        /// 员工自愿性供款强积金
        /// </summary>
        public decimal? employee_voluntary_contribution { get; set; }

        /// <summary>
        /// 催主自愿性供款强积金
        /// </summary>
        public decimal? employer_voluntary_contribution { get; set; }

        /// <summary>
        /// 假期类型 连接Sys_Leave_Type
        /// </summary>
        public Guid? leave_type_id { get; set; }

        /// <summary>
        /// 假期组别
        /// </summary>
        public string leave_group { get; set; }

        /// <summary>
        /// 休假结算日期
        /// </summary>
        public DateTime? leave_settlement_date { get; set; }

        /// <summary>
        /// 是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int? create_id { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string create_name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_date { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public int? modify_id { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string modify_name { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modify_date { get; set; }

        public Guid? file_id { get; set; }

        public IFormFile form_file { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public SysEmpFileUploadInput emp_photo => new()
        {
            file_id = file_id,
            form_file = form_file
        };

        /// <summary>
        ///银行信息
        /// </summary>
        public string bank_info { get; set; }

        /// <summary>
        ///年假天数
        /// </summary>
        public int? leave_day { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public Guid? user_id { get; set; }

        public List<BankAccountInfo> bank_info_list => !string.IsNullOrEmpty(bank_info) ? JsonConvert.DeserializeObject<List<BankAccountInfo>>(bank_info) : new List<BankAccountInfo>();
    }

    /// <summary>
    /// 分页查询实体
    /// </summary>
    public class SysEmpMentQueryDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? end_time { get; set; }
    }

    /// <summary>
    /// 分页查询返回实体
    /// </summary>
    public class SysEmpMentWebDto
    {
        public Guid id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public Guid? user_id { get; set; }

        /// <summary>
        ///员工编号
        /// </summary>
        public string sys_employee_management_number { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string chinese_name { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string english_name { get; set; }

        /// <summary>
        ///身份证号
        /// </summary>
        public string card_no { get; set; }

        /// <summary>
        ///电话
        /// </summary>

        public string phone { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///部门
        /// </summary>
        public Guid? department_id { get; set; }

        /// <summary>
        ///部门
        /// </summary>
        public string department_name { get; set; }

        /// <summary>
        ///工种
        /// </summary>
        public Guid? organization_id { get; set; }

        /// <summary>
        /// 工种名称
        /// </summary>
        public string organization_name { get; set; }

        /// <summary>
        ///入职日期
        /// </summary>
        public DateTime? record_date { get; set; }

        /// <summary>
        ///基本工资
        /// </summary>
        public decimal? basic_salary { get; set; }

        /// <summary>
        ///性别
        /// </summary>
        public int? user_gender { get; set; }
    }

    public class SysEmpFileUploadInput
    {
        public Guid? file_id { get; set; }

        public IFormFile form_file { get; set; }
    }

    public class BankAccountInfo
    {
        /// <summary>
        /// 模式
        /// </summary>
        public string pattern { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string bank_name { get; set; }

        /// <summary>
        /// 分行编号
        /// </summary>
        public string branch_code { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string bank_account { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public decimal? percentage { get; set; }
    }
}