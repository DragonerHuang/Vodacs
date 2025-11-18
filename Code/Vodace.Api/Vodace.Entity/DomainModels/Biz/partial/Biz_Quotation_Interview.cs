
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
    
    public partial class Biz_Quotation_Interview
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    /// <summary>
    /// 列表查询条件
    /// </summary>
    public class QnInterviewSearchDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 回复日期
        /// </summary>
        public DateTime? time { get; set; }
    }

    /// <summary>
    /// 招标面试
    /// </summary>
    public class QnInterviewDataDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid? qn_id { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 面试时间
        /// </summary>
        public DateTime? interview_time { get; set; }

        /// <summary>
        /// 集合点
        /// </summary>
        public string meeting_point { get; set; }

        /// <summary>
        /// 联系人id
        /// </summary>
        public Guid? contact_id { get; set; }

        /// <summary>
        /// 负责人中文名
        /// </summary>
        public string interview_cht { get; set; }

        /// <summary>
        /// 负责人英文名
        /// </summary>
        public string interview_eng { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

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
        /// 回复时间
        /// </summary>
        public DateTime? reply_date { get; set; }

        /// <summary>
        /// 上传完成文件
        /// </summary>
        public List<ContractQnFileDto> upload_finish_file_info { get; set; } = new List<ContractQnFileDto>();
    }

    /// <summary>
    /// 添加招标面试dto
    /// </summary>
    public class AddQnInterview
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid? qn_id { get; set; }

        /// <summary>
        /// 面试时间
        /// </summary>
        public DateTime? interview_time { get; set; }

        /// <summary>
        /// 集合点
        /// </summary>
        public string meeting_point { get; set; }

        /// <summary>
        /// 联系人id
        /// </summary>
        public Guid? contact_id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 上传完成文件
        /// </summary>
        public List<ContractQnFileDto> upload_finish_file_info { get; set; } = new List<ContractQnFileDto>();
    }

    /// <summary>
    /// 编辑招标面试
    /// </summary>
    public class EditQnInterview: AddQnInterview
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid id { get; set; }
    }
}