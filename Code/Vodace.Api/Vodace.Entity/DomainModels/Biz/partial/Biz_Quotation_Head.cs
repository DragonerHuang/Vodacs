
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
    
    public partial class Biz_Quotation_Head
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    /// <summary>
    /// 负责人信息
    /// </summary>
    public class QnHeadDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid? qn_id { get; set; }

        /// <summary>
        /// 负责人列表id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///  负责类型中文
        /// </summary>
        public string handler_type_cht { get; set; }

        /// <summary>
        ///  负责类型英文
        /// </summary>
        public string handler_type_eng { get; set; }

        /// <summary>
        /// 负责人id
        /// </summary>
        public Guid? handler_id { get; set; }

        /// <summary>
        /// 负责人中文名
        /// </summary>
        public string handler_cht { get; set; }

        /// <summary>
        /// 负责人英文名
        /// </summary>
        public string handler_eng { get; set; }

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
    }

    /// <summary>
    /// 编辑负责人信息
    /// </summary>
    public class QnHeadInputDto
    {
        /// <summary>
        /// 负责人列表id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 选择的负责人id
        /// </summary>
        public Guid contact_id { get; set; }
    }
}