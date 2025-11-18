
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
    
    public partial class Biz_Quotation_Record
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class QuotationRecordAddDto
    {
        /// <summary>
        ///版本号
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 报价金额(HK$)
        /// </summary>
        public decimal? amount { get; set; }
        /// <summary>
        /// 文档名
        /// </summary>
        public string file_name { get; set; }
        /// <summary>
        ///报价ID
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 报价单创建人（联系人id）
        /// </summary>
        public Guid? create_id_by_contract { get; set; }
        /// <summary>
        /// 报价单创建人（联系人名）
        /// </summary>
        public string create_name_by_contract { get; set; }
        /// <summary>
        /// 报价单创建时间 （联系人名）
        /// </summary>
        public DateTime? create_name_by_date { get; set; }
        /// <summary>
        /// 报价单修改人（联系人id）
        /// </summary>
        public Guid? update_id_by_contract { get; set; }
        /// <summary>
        /// 报价单修改人（联系人名）
        /// </summary>
        public string update_name_by_contract { get; set; }
        /// <summary>
        /// 报价单修改时间（联系人名）
        /// </summary>
        public DateTime? update_id_by_date { get; set; }
    }

    public class QuotationRecordEditDto : QuotationRecordAddDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// 是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; } = 0;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_date { get; set; } = DateTime.Now;
    }

    public class QuotationRecordListDto: QuotationRecordEditDto
    {
        /// <summary>
        /// 报价编码
        /// </summary>
        public string qn_no { get; set; }
        /// <summary>
        /// 合约状态
        /// </summary>
        public string status_code { get; set; }


        public string create_name { get; set; }
        public int? modify_id { get; set; }
        public string modify_name { get; set; }
        public DateTime? modify_date { get; set; }

    }

    public class SearchQuotationRecordDto
    {
        /// <summary>
        ///版本号
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 报价金额(HK$)
        /// </summary>
        public double amount { get; set; }
        /// <summary>
        /// 文档名
        /// </summary>
        public string file_name { get; set; }
        /// <summary>
        ///报价ID
        /// </summary>
        public Guid? qn_id { get; set; }
    }
}