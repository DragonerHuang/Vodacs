
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
    
    public partial class Biz_Quotation_QA
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class QuotationQADto
    {
        public Guid? id { get; set; }
        public Guid? qn_id { get; set; }
        public int? type { get; set; }
        public string subject { get; set; }
        public DateTime? issue_date { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? submit_date { get; set; }
        public string remark { get; set; }
        public Guid[] FileIds { get; set; } = new Guid[0];
    }

    public class QuotationQAWithFileAddDto
    {
        public Guid? qn_id { get; set; }
        public int? type { get; set; }
        public string subject { get; set; }
        public DateTime? issue_date { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? submit_date { get; set; }
        public string remark { get; set; }
    }
    public class QuotationQAWithFileEditDto
    {
        public Guid id { get; set; }
        public Guid? qn_id { get; set; }
        public int? type { get; set; }
        public string subject { get; set; }
        public DateTime? issue_date { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? submit_date { get; set; }
        public string remark { get; set; }
    }

    public class QuotationQA_Query
    {
        public Guid? qn_id { get; set; }
        public int? type { get; set; }

    }
    public class QuotationQAListDto 
    {
        public Guid id { get; set; }
        public Guid? qn_id { get; set; }
        public int? type { get; set; }
        public string subject { get; set; }
        public DateTime? issue_date { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? submit_date { get; set; }
        public string file_name { get; set; }
        public string remark { get; set; }

        public string type_name_cht { get; set; }

        public string type_name_eng { get; set; }

    }

    public class EditQuotationQADto : QuotationQAWithFileEditDto
    {
        /// <summary>
        /// 上传Q&A问答文件
        /// </summary>
        public List<ContractQnFileDto> upload_qa_file_info { get; set; } = new List<ContractQnFileDto>();

        /// <summary>
        /// 上传完成文件
        /// </summary>
        public List<ContractQnFileDto> upload_finish_file_info { get; set; } = new List<ContractQnFileDto>();
    }
}