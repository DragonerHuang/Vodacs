
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
    
    public partial class Biz_Quotation_Deadline
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    /// <summary>
    /// 期限管理查询条件
    /// </summary>
    public class QnDeadlineSearchDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 期限类型
        /// </summary>
        public string deadline_type { get; set; }
    }

    /// <summary>
    /// 期限管理查询内容
    /// </summary>
    public class QnDeadlineDto
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
        /// 期限类型
        /// </summary>
        public string term_type { get; set; }

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
        /// 截止时间
        /// </summary>
        public DateTime? closing_date { get; set; }

        /// <summary>
        /// 是否完成（0：否；1：完成）
        /// </summary>
        public int? is_complete { get; set; } = 0;

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
        /// 实际完成时间
        /// </summary>
        public DateTime? complete_date { get; set; }

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime? exp_complete_date { get; set; }
    }

    /// <summary>
    /// 创建期限管理
    /// </summary>
    public class AddQnDeadlineDto
    {
        /// <summary>
        /// 报价id
        /// </summary>
        public Guid qn_id { get; set; }

        /// <summary>
        /// 期限类型
        /// </summary>
        public string term_type { get; set; }

        /// <summary>
        /// 负责人id
        /// </summary>
        public Guid? handler_id { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? closing_date { get; set; }

        /// <summary>
        /// 是否完成（0：否；1：完成）
        /// </summary>
        public int? is_complete { get; set; } = 0;

        /// <summary>
        /// 关联表id
        /// </summary>
        public Guid? relation_id { get; set; }
    }

    /// <summary>
    /// 编辑期限管理
    /// </summary>
    public class EditQnDeadlineDto
    {
        public Guid id { get; set; }

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime? exp_complete_date { get; set; }
    }

   
}