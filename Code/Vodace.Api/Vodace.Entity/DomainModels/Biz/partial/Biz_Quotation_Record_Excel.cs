
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
    
    public partial class Biz_Quotation_Record_Excel
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class QuotationRecordExcelDto
    {
        /// <summary>
        ///报价单记录id
        /// </summary>
        public Guid? quotation_record_id { get; set; }

        /// <summary>
        ///excel表单名
        /// </summary>
        public string sheet_name { get; set; }

        /// <summary>
        ///项目（Item Code）
        /// </summary>
        public string item_code { get; set; }

        /// <summary>
        ///内容
        /// </summary>
        public string item_description { get; set; }

        /// <summary>
        ///数量
        /// </summary>
        public string quantity { get; set; }

        /// <summary>
        ///单位
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        ///单价
        /// </summary>
        public string unit_rage { get; set; }

        /// <summary>
        ///银码
        /// </summary>
        public string amount { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///行号
        /// </summary>
        public int? line_number { get; set; }
    }
    public class QuotationRecordExcelEditDto : QuotationRecordExcelDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }
    }

    public class QuotationRecordExcelListDto : QuotationRecordExcelEditDto
    {
        public string version { get; set; }
        public string amount { get; set; }
        public string file_name { get; set; }
        public string qn_no { get; set; }
        public int? delete_status { get; set; }
        public string remark { get; set; }
        public int? create_id { get; set; }
        public string create_name { get; set; }
        public DateTime? create_date { get; set; }
        public int? modify_id { get; set; }
        public string modify_name { get; set; }
        public DateTime? modify_date { get; set; }
    }

    public class QuotationRecordExcelDetailListDto
    {
        public string sheet_name { get; set; }
        public List<QuotationRecordExcelListDto> list { get; set; }
    }

    public class SearchQuotationRecordExcelInputDto
    {
        public string sheet_name { get; set; }
        public string item_code { get; set; }
        public string item_description { get; set; }
        public Guid? quotation_record_id { get; set; }
    }
}