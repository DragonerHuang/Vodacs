
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
    
    public partial class Biz_Contract
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class ContractDto
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
        ///项目ID
        /// </summary>
        public Guid? project_id { get; set; }

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
        public DateTime? create_date { get; set; } = DateTime.Now;
        /// <summary>
        ///合同标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///合同类别
        /// </summary>
        public string category { get; set; }

        /// <summary>
        ///投标类型
        /// </summary>
        public string tender_type { get; set; }

        /// <summary>
        ///合同编号/投标参考编号
        /// </summary>
        public string ref_no { get; set; }

        /// <summary>
        ///vo和wo类型（wo不纳入金额统计），如果都不是则为空
        /// </summary>
        public string vo_wo_type { get; set; }

        /// <summary>
        ///上一层级合约id
        /// </summary>
        public Guid? master_id { get; set; }

        /// <summary>
        ///所属公司id
        /// </summary>
        public Guid? company_id { get; set; }
    }

    /// <summary>
    /// 合约列表Dto
    /// </summary>
    public class ContractListDto : ContractDto
    {
        /// <summary>
        /// 合约组织数量
        /// </summary>
        public int? contract_org {  get; set; }
        /// <summary>
        /// 合约组织数量
        /// </summary>
        public string submit_file_code { get; set; }
        /// <summary>
        /// 施工地点名称
        /// </summary>
        public string strSiteName { get; set; }

        /// <summary>
        /// 报价编码
        /// </summary>
        public string strQuotationNo { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string strProjectName { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string project_no { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string strCompanyName { get; set; }

        /// <summary>
        /// 项目期望开始时间
        /// </summary>
        public DateTime? exp_start_date { get; set; }

        /// <summary>
        /// 项目实际开始时间
        /// </summary>
        public DateTime? act_start_date { get; set; }

        /// <summary>
        /// 项目期望结束时间
        /// </summary>
        public DateTime? exp_end_date { get; set; }

        /// <summary>
        /// 项目实际结束时间
        /// </summary>
        public DateTime? act_end_date { get; set; }
    }

    public class ContractDetailDto : ContractListDto
    {
        public string strMasterName { get; set; }
        public string strMasterNo { get; set; }
        public string project_no { get; set; }
        public int create_id { get; set; }
        public string create_name { get; set; }
        public int modify_id { get; set; }
        public string modify_name { get; set; }
        public DateTime? modify_date { get; set; }
    }


    /// <summary>
    /// 查询条件
    /// </summary>
    public class ContractSearchDto
    {
        /// <summary>
        /// vo和wo类型（wo不纳入金额统计），如果都不是则为空
        /// </summary>
        public string vo_wo_type { get; set; }
        
        /// <summary>
        /// 合同名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 招标主题
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 合同编号/投标参考编号
        /// </summary>
        public string ref_no { get; set; }

        /// <summary>
        /// 办公地点（合约那个公司的办公地点）
        /// </summary>
        public string contract_address { get; set; }

        /// <summary>
        /// 工程类别
        /// </summary>
        public string work_type_code { get; set; }

        /// <summary>
        /// 合同标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 合同类别
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// 贸易
        /// </summary>
        public string trade { get; set; }
        /// <summary>
        /// 报价编码
        /// </summary>
        public string qn_no { get; set; }

        /// <summary>
        /// 站点位置
        /// </summary>
        public string strSiteName { get; set; }
    }

    public class SetContractMasterDto
    {
        /// <summary>
        /// 报价单id
        /// </summary>
        public Guid? qn_id { get; set; }
        /// <summary>
        /// 报价编码
        /// </summary>
        public string qn_no { get; set; }
        /// <summary>
        /// 合同id
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// 上一层级合约id
        /// </summary>
        public Guid? master_id { get; set; }
        /// <summary>
        /// vo和wo类型（wo不纳入金额统计），如果都不是则为空
        /// </summary>
        public string vo_wo_type { get; set; }
        /// <summary>
        /// 0-删除 1-设置
        /// </summary>
        public int intSetType { get; set; }
    }

    public class ChildContractSearchDto
    {
        /// <summary>
        /// 合同id
        /// </summary>
        public Guid? contract_id { get; set; }
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid? quotation_id { get; set; }
        /// <summary>
        /// 合同名称
        /// </summary>
        public string contract_name { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string contract_no { get; set; }
        /// <summary>
        /// 合同标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// vo和wo类型（wo不纳入金额统计），如果都不是则为空
        /// </summary>
        public string vo_wo_type { get; set; }
        /// <summary>
        /// 报价编码
        /// </summary>
        public string qn_no { get; set; }
        /// <summary>
        /// 是否包含当前合约 1-是 0-否
        /// </summary>
        public int contract_include { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? project_id { get; set; }
    }

    public class ChildContractDto
    {
        /// <summary>
        /// 合同id
        /// </summary>
        public Guid? contract_id { get; set; }
        /// <summary>
        /// 合约组织数量
        /// </summary>
        public int? contract_org { get; set; }
        /// <summary>
        /// 提交文件代码
        /// </summary>
        public string submit_file_code { get; set; }
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid? quotation_id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? project_id { get; set; }
        /// <summary>
        /// 报价编号
        /// </summary>
        public string qn_no { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string contract_no { get; set; }
        /// <summary>
        /// vo和wo类型（wo不纳入金额统计），如果都不是则为空
        /// </summary>
        public string vo_wo_type { get; set; }
        /// <summary>
        /// 合约中文名
        /// </summary>
        public string name_cht { get; set; }
        /// <summary>
        /// 合约英文名
        /// </summary>
        public string name_eng { get; set; }
        /// <summary>
        /// 确认报价金额
        /// </summary>
        public decimal? confirm_amt { get; set; }
        /// <summary>
        /// 内部报价金额
        /// </summary>
        public decimal? qn_amt { get; set; }
    }
}