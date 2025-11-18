
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
    
    public partial class Biz_Various_Work_Order
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    /// <summary>
    /// 新增、修改时使用
    /// </summary>
    public class Various_Work_OrderDto
    {
        public Guid master_id { get; set; }

        public ContractByvwDto dtoContract { get; set; }

        public QuotationByvwDto dtoQuotation { get; set; }

        public VariousWorkByvwDto dtoVarious_Work_Order { get; set; }
    }


    public class ContractByvwDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///采购单ID
        /// </summary>
        public Guid? po_id { get; set; }

        /// <summary>
        ///合约编码
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        ///简称
        /// </summary>
        public string name_sho { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        ///别名
        /// </summary>
        public string name_ali { get; set; }

        /// <summary>
        ///是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; } = 0;

        /// <summary>
        ///
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? create_date { get; set; } = DateTime.Now;

        /// <summary>
        ///发行日期
        /// </summary>
        public DateTime? issue_date { get; set; }

        /// <summary>
        ///截止日期
        /// </summary>
        public DateTime? end_date { get; set; }

        /// <summary>
        ///合同标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///合同类别
        /// </summary>
        public string category { get; set; }

        /// <summary>
        ///贸易
        /// </summary>
        public string trade { get; set; }

        /// <summary>
        ///资格预审申请截止日期(Closing Date of Anticipated Date of Prequalification Submission
        /// </summary>
        public DateTime? antic_pql_sub_close_date { get; set; }

        /// <summary>
        ///预期资格预审提交日期(Anticipated Date of Prequalification Submission)
        /// </summary>
        public string antic_pql_sub_date { get; set; }

        /// <summary>
        ///预期发出投标邀请日期(Anticipated Date of Invitation of Tender)
        /// </summary>
        public string antic_inv_tndr_date { get; set; }

        /// <summary>
        ///合同授予预期日期(Anticipated Date for Contract Award)
        /// </summary>
        public string antic_cntr_awd_date { get; set; }

        /// <summary>
        ///投标类型
        /// </summary>
        public string tender_type { get; set; }

        /// <summary>
        ///成本范围
        /// </summary>
        public string range_cost { get; set; }

        /// <summary>
        ///姓名(合同的联络人)
        /// </summary>
        public string contact_name { get; set; }

        /// <summary>
        ///头衔、职称(合同的联络人)
        /// </summary>
        public string contact_title { get; set; }

        /// <summary>
        ///邮箱(合同的联络人)
        /// </summary>
        public string contact_email { get; set; }

        /// <summary>
        ///电话(合同的联络人)
        /// </summary>
        public string contact_tel { get; set; }

        /// <summary>
        ///传真(合同的联络人)
        /// </summary>
        public string contact_fax { get; set; }

        /// <summary>
        ///合同编号/投标参考编号
        /// </summary>
        public string ref_no { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>
        public Guid? project_id { get; set; }

        /// <summary>
        ///办公地点（合约那个公司的办公地点）
        /// </summary>
        public string contract_address { get; set; }

        /// <summary>
        ///工程类别
        /// </summary>
        public string work_type_code { get; set; }

        /// <summary>
        ///项目来源
        /// </summary>
        public string project_source_code { get; set; }

        /// <summary>
        ///vo和wo类型（wo不纳入金额统计），如果都不是则为空
        /// </summary>
        public string vo_wo_type { get; set; }

        /// <summary>
        /// 上一层级合约id
        /// </summary>
        public Guid? master_id { get; set; }
    }

    public class QuotationByvwDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///qn_no报价号码(qn-年份+流水号)
        /// </summary>
        public string qn_no { get; set; }

        /// <summary>
        ///年份（香港财政年从4月1日开始，超过这日期需要往下1年，例如：24年的财政报告从24年4月1日-2025年3月31日）
        /// </summary>
        public int? qn_year { get; set; }

        /// <summary>
        ///是否兴趣（0：是；1：否）
        /// </summary>
        public int? is_interest { get; set; }

        /// <summary>
        ///兴趣原因
        /// </summary>
        public string interest_reason { get; set; }

        /// <summary>
        ///确认金额
        /// </summary>
        public decimal? confirm_amt { get; set; }

        /// <summary>
        ///报价金额
        /// </summary>
        public decimal? qn_amt { get; set; }

        /// <summary>
        ///确认金额时间
        /// </summary>
        public DateTime? confirm_amt_date { get; set; }

        /// <summary>
        ///报价金额时间
        /// </summary>
        public DateTime? qn_amt_date { get; set; }

        /// <summary>
        ///是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; } = 0;

        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? create_date { get; set; } = DateTime.Now;

        /// <summary>
        ///合约状态（字段中DicValue）
        /// </summary>
        public string status_code { get; set; }

        /// <summary>
        ///客户id（M_Sub_Contractors.id）
        /// </summary>
        public Guid? customer_id { get; set; }

        /// <summary>
        ///招标前负责人（联系人表id）
        /// </summary>
        public Guid? pq_handler_id { get; set; }

        /// <summary>
        ///现场考察负责人（联系人表id）
        /// </summary>
        public Guid? pe_handler_id { get; set; }

        /// <summary>
        ///招标负责人（联系人表id）
        /// </summary>
        public Guid? tender_handler_id { get; set; }

        /// <summary>
        ///招标前截止日期
        /// </summary>
        public DateTime? pq_handler_closing_date { get; set; }

        /// <summary>
        ///现场考察截止日期
        /// </summary>
        public DateTime? pe_handler_closing_date { get; set; }

        /// <summary>
        ///招标截止日期
        /// </summary>
        public DateTime? tender_handler_closing_date { get; set; }

        /// <summary>
        ///办公地址
        /// </summary>
        public string office_address { get; set; }

        /// <summary>
        ///托盘
        /// </summary>
        public string contract_tray { get; set; }
    }

    public class VariousWorkByvwDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///确认订单ID（M_Confirmed_Order.id）
        /// </summary>
        public Guid? co_id { get; set; }

        /// <summary>
        ///采购单ID
        /// </summary>
        public Guid? po_id { get; set; }

        /// <summary>
        ///合约ID（M_Contract.id）
        /// </summary>
        public Guid? contract_id { get; set; }

        /// <summary>
        ///合约编码
        /// </summary>
        public string vo_wo_no { get; set; }

        /// <summary>
        ///合约类型：VO、WO
        /// </summary>
        public string vo_wo_type { get; set; }

        /// <summary>
        ///内部定价金额
        /// </summary>
        public decimal? internal_qn_amt { get; set; }

        /// <summary>
        ///确认订单金额
        /// </summary>
        public decimal? confirmed_amt { get; set; }

        /// <summary>
        ///简称
        /// </summary>
        public string name_sho { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        ///别名
        /// </summary>
        public string name_ali { get; set; }

        /// <summary>
        ///是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? create_date { get; set; }

    }
}