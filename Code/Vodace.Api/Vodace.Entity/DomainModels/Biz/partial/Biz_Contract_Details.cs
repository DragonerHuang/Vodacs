
using Microsoft.AspNetCore.Http;
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
    
    public partial class Biz_Contract_Details
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class ContractDetailDataDto: AddQnRequestDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 合同id
        /// </summary>
        public Guid contract_id { get; set; }

        ///// <summary>
        ///// 合同类型（项目来源）
        ///// </summary>
        //public string contract_type { get; set; }

        /// <summary>
        /// 合同详情id
        /// </summary>
        public Guid contract_details_id { get; set; }

        /// <summary>
        /// 是否兴趣（0：是；1：否）
        /// </summary>
        public int? is_interest { get; set; }

        /// <summary>
        /// 兴趣原因
        /// </summary>
        public string interest_reason { get; set; }
    }

    /// <summary>
    /// 标书资料
    /// </summary>
    public class TenderDataDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 合同id
        /// </summary>
        public Guid contract_id { get; set; }

        /// <summary>
        /// 所属项目id
        /// </summary>
        public Guid? project_id { get; set; }

        /// <summary>
        /// 合同详情id
        /// </summary>
        public Guid contract_details_id { get; set; }

        /// <summary>
        /// 标书发布日期
        /// </summary>
        public DateTime? tender_start_date { get; set; }

        /// <summary>
        /// 招标截止时间
        /// </summary>
        public DateTime? tender_end_date { get; set; }

        /// <summary>
        /// 台同参考号
        /// </summary>
        public string contract_ref_no { get; set; }

        /// <summary>
        /// 合同标题
        /// </summary>
        public string contract_title { get; set; }

        /// <summary>
        /// 合同类别
        /// </summary>
        public string contract_category { get; set; }

        /// <summary>
        /// 往来文件
        /// </summary>
        public string contract_trade { get; set; }

        /// <summary>
        /// 招标标题
        /// </summary>
        public string tender_title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 投标补充
        /// </summary>
        public List<TenderAddendumDto> tender_addendums { get; set; } = new List<TenderAddendumDto>();
    }

    /// <summary>
    /// 投标补充
    /// </summary>
    public class TenderAddendumDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid? id { get; set; }

        /// <summary>
        /// 发行日期
        /// </summary>
        public DateTime? issue_date { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        public List<ContractQnFileDto> upload_file_info { get; set; } = new List<ContractQnFileDto>();
    }

   
}