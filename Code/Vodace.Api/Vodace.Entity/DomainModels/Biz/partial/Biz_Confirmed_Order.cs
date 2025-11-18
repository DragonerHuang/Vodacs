
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
    
    public partial class Biz_Confirmed_Order
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    #region Request

    /// <summary>
    /// 确认订单
    /// </summary>
    public class ConfirmDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string po_no { get; set; }

        /// <summary>
        /// 确认日期
        /// </summary>
        public DateTime? confirm_date { get; set; }

        /// <summary>
        /// 负责人id
        /// </summary>
        public Guid? head_id { get; set; }

        /// <summary>
        /// 合约开始时间
        /// </summary>
        public DateTime? commen_date { get; set; }

        /// <summary>
        /// 合约结束时间（预计）
        /// </summary>
        public DateTime? complet_exp_date { get; set; }

        /// <summary>
        ///合约结束时间（实际）
        /// </summary>
        public DateTime? complet_act_date { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        public List<ContractQnFileDto> upload_file_info { get; set; } = new List<ContractQnFileDto>();
    }

    /// <summary>
    /// 确认订单查询
    /// </summary>
    public class CoSearchDto
    {
        /// <summary>
        /// 报价编号
        /// </summary>
        public string qn_no { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 站点位置
        /// </summary>
        public List<Guid> lst_site_ids { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string customer_name { get; set; }

        /// <summary>
        /// co的编号
        /// </summary>
        public string co_no { get; set;}

        /// <summary>
        /// 状态
        /// </summary>
        public string status_code { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? year { get; set; }

        /// <summary>
        /// 投标类型（合同来源）
        /// </summary>
        public string contract_type { get; set; }
    }

    /// <summary>
    /// 确认订单修改
    /// </summary>
    public class CoInputDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid co_id { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 合同编码
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public Guid? customer_id { get; set; }

        /// <summary>
        /// 现场位置id集合
        /// </summary>
        public List<Guid?> site_ids { get; set; } = [];

        /// <summary>
        /// 合约状态编码
        /// </summary>
        public string qn_stuts_code { get; set; }

        /// <summary>
        /// 参考编号
        /// </summary>
        public string contract_ref_no { get; set; }

        /// <summary>
        /// 采购订单号（po编号）
        /// </summary>
        public string po_no { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? year { get; set; }

        /// <summary>
        /// 确认日期
        /// </summary>
        public DateTime? confirm_date { get; set; }

        /// <summary>
        /// 确认金额
        /// </summary>
        public decimal? confirm_amt { get; set; }

        /// <summary>
        /// 合约开始时间
        /// </summary>
        public DateTime? commen_date { get; set; }

        /// <summary>
        /// 合约结束时间（预计）
        /// </summary>
        public DateTime? complet_exp_date { get; set; }

        /// <summary>
        ///合约结束时间（实际）
        /// </summary>
        public DateTime? complet_act_date { get; set; }

        /// <summary>
        /// 负责人id
        /// </summary>
        public Guid? head_id { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        public List<ContractQnFileDto> upload_file_info { get; set; } = new List<ContractQnFileDto>();
    }

    #endregion


    #region Response

    /// <summary>
    /// co信息列表数据
    /// </summary>
    public class CoDataDto
    {
        /// <summary>
        /// coid
        /// </summary>
        public Guid co_id { get; set; }

        /// <summary>
        /// CO编码
        /// </summary>
        public string co_no { get; set; }

        /// <summary>
        /// 报价id
        /// </summary>
        public Guid? qn_id { get; set; }

        /// <summary>
        /// 报价编码
        /// </summary>
        public string qn_no { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public Guid? customer_id { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string customer_name { get; set; }

        /// <summary>
        /// 现场位置
        /// </summary>
        public List<SiteDataDto> lst_dto_sites { get; set; } = new List<SiteDataDto>();

        /// <summary>
        /// 合约id
        /// </summary>
        public Guid? contract_id { get; set; }

        /// <summary>
        /// 合约编号
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 参考编号
        /// </summary>
        public string contract_ref_no { get; set; }

        /// <summary>
        /// PO编号
        /// </summary>
        public string po_no { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 确认日期
        /// </summary>
        public DateTime? confirm_date { get; set; }

        /// <summary>
        /// 确认金额
        /// </summary>
        public decimal? confirm_amount { get; set; }

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
    
        /// <summary>
        /// 合同进度（报价进度）
        /// </summary>
        public string status_code { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? qn_year { get; set; }

        /// <summary>
        /// 投标类型
        /// </summary>
        public string contract_type { get; set; }

        /// <summary>
        /// 负责人id（合同）
        /// </summary>
        public Guid? head_id { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        public string head_name { get; set; }

        /// <summary>
        /// 合约开始时间
        /// </summary>
        public DateTime? commen_date { get; set; }

        /// <summary>
        /// 合约结束时间（预计）
        /// </summary>
        public DateTime? complet_exp_date { get; set; }

        /// <summary>
        ///合约结束时间（实际）
        /// </summary>
        public DateTime? complet_act_date { get; set; }

        /// <summary>
        /// 确认价格币种
        /// </summary>
        public string confirm_currency { get; set; }
    }

    /// <summary>
    /// CO详情信息
    /// </summary>
    public class CoDetailsDto
    {
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

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modify_date { get; set; }

        /// <summary>
        /// 修改id
        /// </summary>
        public int? modify_id { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string modify_name { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public Guid co_id { get; set; }

        /// <summary>
        /// 确认订单号
        /// </summary>
        public string co_no { get; set; }

        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 报价单号
        /// </summary>
        public string qn_no { get; set; }

        /// <summary>
        /// 合约id
        /// </summary>
        public Guid contract_id { get; set; }

        /// <summary>
        /// 合约名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 合约编码
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public Guid? customer_id { get; set; }

        /// <summary>
        /// 现场位置id集合
        /// </summary>
        public List<Guid?> site_ids { get; set; } = [];

        /// <summary>
        /// 合约状态编码
        /// </summary>
        public string qn_stuts_code { get; set; }

        /// <summary>
        /// 合约参考编码
        /// </summary>
        public string contract_ref_no { get; set; }

        /// <summary>
        /// 采购订单号
        /// </summary>
        public string po_no { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? year { get; set; }

        /// <summary>
        /// 确认日期
        /// </summary>
        public DateTime? confirm_date { get; set; }

        /// <summary>
        /// 确认金额
        /// </summary>
        public decimal? confirm_amt { get; set; }

        /// <summary>
        /// 合约开始时间
        /// </summary>
        public DateTime? commen_date { get; set; }

        /// <summary>
        /// 合约结束时间（预计）
        /// </summary>
        public DateTime? complet_exp_date { get; set; }

        /// <summary>
        ///合约结束时间（实际）
        /// </summary>
        public DateTime? complet_act_date { get; set; }

        /// <summary>
        /// 负责人id
        /// </summary>
        public Guid? head_id { get; set; }

        /// <summary>
        /// 确认价格币种
        /// </summary>
        public string confirm_currency { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        public List<ContractQnFileDto> upload_file_info { get; set; } = new List<ContractQnFileDto>();
    }

    #endregion


}