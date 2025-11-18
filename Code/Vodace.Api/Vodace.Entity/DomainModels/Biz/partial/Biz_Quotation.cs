
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
    
    public partial class Biz_Quotation
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    #region Request

    /// <summary>
    /// 创建报价请求DTO
    /// </summary>
    public class AddQnRequestDto
    {
        /// <summary>
        /// 项目持有人信息
        /// </summary>
        public TenderQnDto dto_qn_tender { get; set; }

        /// <summary>
        /// 合同1信息
        /// </summary>
        public ContractQnDto dto_qn_contract { get; set; }

        /// <summary>
        /// 联系人信息
        /// </summary>
        public ContactQnDto dto_qn_contact { get; set; }

        /// <summary>
        /// 联系人列表信息
        /// </summary>
        public List<ContactQnDto> dto_qn_contacts { get; set; } = new List<ContactQnDto>();

        /// <summary>
        /// 客户类型
        /// </summary>
        public string customer_type { get; set; }

        /// <summary>
        /// 合同类型(项目来源)
        /// </summary>
        public string project_source { get; set; }
    }

    /// <summary>
    /// 报价（qn）中公开招标、邀请招标信息
    /// </summary>
    public class TenderQnDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? pro_id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string pro_name { get; set; }

        /// <summary>
        /// 参考编号（邀请招标）
        /// </summary>
        public string tender_ref { get; set; }

        /// <summary>
        /// 招标标题（公开招标）
        /// </summary>
        public string tender_title { get; set; }

        /// <summary>
        /// 招标主题（邀请招标）
        /// </summary>
        public string tender_subject { get; set; }

        /// <summary>
        /// 招标公布日期
        /// </summary>
        public DateTime? tender_issue_date { get; set; }

        /// <summary>
        /// 招标截止日期
        /// </summary>
        public DateTime? tender_end_date { get; set; }

        /// <summary>
        /// 主题（邀请招标）
        /// </summary>
        public string tender_info { get; set; }

    }

    /// <summary>
    /// 报价（qn）中合同信息
    /// </summary>
    public class ContractQnDto
    {
        /// <summary>
        /// 合同名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 合同/招标参考编号
        /// </summary>
        public string contract_ref_no { get; set; }

        /// <summary>
        /// 原合同编号
        /// </summary>
        public Guid? master_id { get; set; }

        /// <summary>
        /// vo和wo类型（wo不纳入金额统计），如果都不是则为空
        /// </summary>
        public string vo_wo_type { get; set; }

        /// <summary>
        /// 合同标题
        /// </summary>
        public string contract_title { get; set; }

        /// <summary>
        /// 合同类别
        /// </summary>
        public string contract_category { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string contract_description { get; set; }

        /// <summary>
        /// 往来文件
        /// </summary>
        public string contract_trade { get; set; }

        /// <summary>
        /// 预审资格截止日期
        /// </summary>
        public DateTime? closing_date { get; set; }

        /// <summary>
        /// 预计预审资格提交日期
        /// </summary>
        public string anticipated_date { get; set; }

        /// <summary>
        /// 预计招标标邀请日期
        /// </summary>
        public string ant_send_inv_tender_date { get; set; }

        /// <summary>
        /// 招标公布日期
        /// </summary>
        public string ant_contract_award_date { get; set; }

        /// <summary>
        /// 投标类型
        /// </summary>
        public string tender_type { get; set; }

        /// <summary>
        /// 成本范围
        /// </summary>
        public string cost_range { get; set; }


        /// <summary>
        /// 上传文件
        /// </summary>
        public List<ContractQnFileDto> upload_file_info { get; set; } = new List<ContractQnFileDto>();

    }

    /// <summary>
    /// 联系人信息
    /// </summary>
    public class ContactQnDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid? id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string contact_name { get; set; }

        /// <summary>
        /// 示题
        /// </summary>
        public string contact_tilte { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string contact_phone { get; set; }

        /// <summary>
        /// 电子邮件地址
        /// </summary>
        public string contact_mail { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string contact_faxno { get; set; }

        /// <summary>
        /// 抄送邮件群组，Gen，Pay ，Safe ，PD， ABWF ，EL， FS（多选，用,号隔开）
        /// </summary>
        public string contact_mail_to { get; set; }
    }

    /// <summary>
    /// 报价上传时合同文件信息
    /// </summary>
    public class ContractQnFileDto
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public Guid? file_id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        /// 文件描述
        /// </summary>
        public string file_remark { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int file_size { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string file_path { get; set; }

    }

    /// <summary>
    /// 修改报价请求dto信息
    /// </summary>
    public class EditQnRequestDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

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
        /// 上一层级合约编码
        /// </summary>
        public Guid? master_id { get; set; }

        /// <summary>
        /// vo和wo类型（wo不纳入金额统计），如果都不是则为空
        /// </summary>
        public string vo_wo_type { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public Guid? customer_id { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string customer_type { get; set; }

        /// <summary>
        /// 合约状态编码
        /// </summary>
        public string qn_stuts_code { get; set; }

        /// <summary>
        /// 现场位置id集合
        /// </summary>
        public List<Guid?> site_ids { get; set; } = [];

        /// <summary>
        /// 年份
        /// </summary>
        public int? year { get; set; }

        /// <summary>
        /// 确认金额
        /// </summary>
        public decimal? confirm_amt { get; set; }

        /// <summary>
        /// 报价金额
        /// </summary>
        public decimal? qn_amt { get; set; }

        /// <summary>
        /// 招标前负责人信息
        /// </summary>
        public QnHeaderDto pq_handler { get; set; }

        /// <summary>
        /// 现场考察负责人信息
        /// </summary>
        public QnHeaderDto pe_handler { get; set; }

        /// <summary>
        /// 招标负责人信息
        /// </summary>
        public QnHeaderDto tender_handler { get; set; }
    }

    /// <summary>
    /// qn查询条件
    /// </summary>
    public class QnSearchDto
    {
        /// <summary>
        /// 报价编号
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 现场位置名称
        /// </summary>
        public string site_names { get; set; }

        /// <summary>
        /// 站点位置
        /// </summary>
        public List<Guid> lst_site_ids { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string customer_name { get; set; }

        /// <summary>
        /// 项目来源（其实新增时的合同类型）
        /// </summary>
        public string project_form { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string status_code { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? year { get; set; }
    }


    #endregion


    #region Response

    /// <summary>
    /// 总的报价统计
    /// </summary>
    public class QnSumAmtDto
    {
        /// <summary>
        /// 总的确认金额
        /// </summary>
        public decimal total_confirm_amt { get; set; }  

        /// <summary>
        /// 总的报价金额
        /// </summary>
        public decimal total_qn_amt { get; set; }
    }

    /// <summary>
    /// 报价数据
    /// </summary>
    public class QnDataDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 报价编码
        /// </summary>
        public string qn_no { get; set; }

        /// <summary>
        /// 合约id
        /// </summary>
        public Guid? contract_id { get; set; }

        /// <summary>
        /// 报价状态code
        /// </summary>
        public string qn_status_code { get; set; }

        /// <summary>
        /// 报价状态
        /// </summary>
        public string qn_status_value { get; set; }

        /// <summary>
        /// 合约编号
        /// </summary>
        public string contract_no { get; set; } 

        /// <summary>
        /// 合约名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 现场位置
        /// </summary>
        public List<SiteDataDto> lst_dto_sites { get; set; } = new List<SiteDataDto>();

        /// <summary>
        /// 确认金额
        /// </summary>
        public decimal? confirm_amt { get; set; }

        /// <summary>
        /// 报价金额
        /// </summary>
        public decimal? qn_amt { get; set; }

        /// <summary>
        /// 审核截止日期
        /// </summary>
        public DateTime? pq_closing_date { get; set; }

        /// <summary>
        /// 招标截止日期
        /// </summary>
        public DateTime? tender_closing_date { get; set; }

        /// <summary>
        /// 视察日期
        /// </summary>
        public DateTime? pe_closing_date { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public Guid? customer_id {  get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string customer_name { get; set; }

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
        /// 项目来源（投标类型（合同））
        /// </summary>
        public string contract_type { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int? qn_year { get; set; }

        /// <summary>
        /// 确认价格币种
        /// </summary>
        public string confirm_currency { get; set; }

        /// <summary>
        /// 报价价格币种
        /// </summary>
        public string qn_currency { get; set; }
    }

    /// <summary>
    /// 报价详细信息
    /// </summary>
    public class QnDetailDataDto: EditQnRequestDto
    {
        /// <summary>
        /// 已确认订单编码
        /// </summary>
        public string co_no { get; set; }

        /// <summary>
        /// 报价单号
        /// </summary>
        public string pn_no { get; set; }

        /// <summary>
        /// 合约参考编码
        /// </summary>
        public string contract_ref_no { get; set; }

        /// <summary>
        /// 合约状态
        /// </summary>
        public string contract_status_code { get; set; }

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
        /// 修改人id
        /// </summary>
        public int? modify_id { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string modify_name { get; set; }

        /// <summary>
        /// 确认价格币种
        /// </summary>
        public string confirm_currency { get; set; }

        /// <summary>
        /// 报价价格币种
        /// </summary>
        public string qn_currency { get; set; }
    }

    /// <summary>
    /// 合约状态
    /// </summary>
    public class ContractStatusDto
    {
        /// <summary>
        /// 字典id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? index { get; set; }

        /// <summary>
        /// 代号
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 英语
        /// </summary>
        public string value_eng { get; set; }
    }

    /// <summary>
    /// 报价下拉信息
    /// </summary>
    public class ContractDropListDto
    {
        /// <summary>
        /// 合约id
        /// </summary>
        public Guid id { get; set; } 
        
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 报价编码
        /// </summary>
        public string qn_no { get; set; }

        /// <summary>
        /// 合约编号
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 合约名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 确认金额
        /// </summary>
        public decimal? confirm_amt { get; set; }

        /// <summary>
        /// 报价金额
        /// </summary>
        public decimal? qn_amt { get; set; }
        public string submit_file_code { get; set; }
    }

    #endregion

    #region 合同资料

    /// <summary>
    /// 合同资料
    /// </summary>
    public class ContractQnDataDto: ContractQnSaveDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? project_id { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string project_no { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string project_name { get; set; }

        /// <summary>
        /// 合约id
        /// </summary>
        public Guid contract_id { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 合约名称
        /// </summary>
        public string contract_name { get; set; }
    }

    /// <summary>
    /// 合同资料保存
    /// </summary>
    public class ContractQnSaveDto
    {
        /// <summary>
        /// qnid
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 项目来源
        /// </summary>
        public string project_source_code { get; set; }

        /// <summary>
        /// 工程类别
        /// </summary>
        public string project_Type_code { get; set; }

        /// <summary>
        /// 办公地址
        /// </summary>
        public string office_address { get; set; }

        /// <summary>
        /// 工作地点
        /// </summary>
        public List<Guid?> contract_sites { get; set; } = new List<Guid?>();

        /// <summary>
        /// 文件往来
        /// </summary>
        public string contract_trade { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        //public string contract_remark { get; set; }

        /// <summary>
        /// 是否兴趣（0：是；1：否）
        /// </summary>
        public int is_interest { get; set; }

        /// <summary>
        /// 兴趣原因
        /// </summary>
        public string interest_reason { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }  
    }

    public class TenderQnDataDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 标书发布日期
        /// </summary>
        public DateTime? tender_release_date { get; set; }

        /// <summary>
        /// 合约截止日期
        /// </summary>
        public DateTime? tender_end_date { get; set; }

        /// <summary>
        /// 合约编号
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string project_name { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string project_ali { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string project_no { get; set; }

        /// <summary>
        /// 工程类别
        /// </summary>
        public string project_Type_code { get; set; }

        /// <summary>
        /// 文件往来
        /// </summary>
        public string contract_trade { get; set; }

        /// <summary>
        /// 工作类型
        /// </summary>
        public string work_type { get; set; }

        /// <summary>
        /// 报价金额
        /// </summary>
        public decimal? qn_amt { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

    #endregion

    #region 负责人

    /// <summary>
    /// 负责人信息
    /// </summary>
    public class QnHeaderDto
    {
        /// <summary>
        /// 负责人id
        /// </summary>
        public Guid? contact_id { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? end_date { get; set; }

    }

    #endregion

    #region 报价单

    /// <summary>
    /// 报告单读取文件（夹）信息
    /// </summary>
    public class GetFileInfoDto
    {
        public string strDirectoryPath { get; set; }
        public string qn_no { get; set; }
        public Guid? qn_id { get; set; }
    }

    #endregion

    #region 提交文件

    /// <summary>
    /// 提交文件展示列表
    /// </summary>
    public class QnSubmitFileDto: ContractQnFileDto
    {
        /// <summary>
        /// 上传人id
        /// </summary>
        public int? upload_id { get; set; }

        /// <summary>
        /// 上传人
        /// </summary>
        public string upload_name { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime? upload_time { get; set; }
    }

    #endregion
}