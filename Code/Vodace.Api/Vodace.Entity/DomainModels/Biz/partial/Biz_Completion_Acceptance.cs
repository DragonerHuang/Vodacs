
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
    
    public partial class Biz_Completion_Acceptance
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public partial class CompletionAcceptanceListDto 
    {
        public Guid id { get; set; }
        /// <summary>
        ///验收编号
        /// </summary>
        public string acceptance_no { get; set; }

        /// <summary>
        ///版本
        /// </summary>
        public int version { get; set; }
        public string version_str { get; set; }

        /// <summary>
        ///内部状态（0：编辑中；1：审核中；2：已提交；3：取消；）
        /// </summary>
        public int inner_status { get; set; }
        public Guid? file_id { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        public string describe { get; set; }

        /// <summary>
        ///制作人
        /// </summary>
        public int? producer_id { get; set; }
        public string producer_name { get; set; }

        /// <summary>
        ///审核人
        /// </summary>
        public int? approved_id { get; set; }
        public string approved_name { get; set; }

        /// <summary>
        ///审核日期
        /// </summary>
        public DateTime? approved_date { get; set; }

        /// <summary>
        ///审核状态
        /// </summary>
        public int? approved_status { get; set; }

        /// <summary>
        ///文件发行日期
        /// </summary>
        public DateTime? file_publish_date { get; set; }

        /// <summary>
        ///实际检验日期
        /// </summary>
        public DateTime? actual_inspection_date { get; set; }

        /// <summary>
        ///提交状态（0：代办；1：已提交；2：已批准；3：条件批准；4：驳回；5：取消；6：不需要回应）
        /// </summary>
        public int? submit_status { get; set; }

        /// <summary>
        ///合约Id
        /// </summary>
        public Guid? contract_id { get; set; }

        /// <summary>
        ///检验结果（0：不及格；1：及格）
        /// </summary>
        public int? test_result { get; set; }

        /// <summary>
        ///验收人
        /// </summary>
        public Guid? inspector_id { get; set; }

        /// <summary>
        ///客户工程师许可日期
        /// </summary>
        public DateTime? engineer_permit_date { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }
    }

    public partial class CompletionAcceptanceModelDto: CompletionAcceptanceEditDto
    {
        public int internal_img_count { get; set; }
        public int customer_img_count { get; set; }
        public DateTime? approved_date { get; set; }
    }
    public partial class CompletionAcceptanceEditDto : CompletionAcceptanceAddDto
    { 
        public Guid id { get; set; }
        public DateTime? file_publish_date { get; set; }
        public DateTime? actual_inspection_date { get; set; }
        public int? test_result { get; set; }
        public string inspector { get; set; }
        public DateTime? engineer_permit_date { get; set; }
        public string remark { get; set; }
    }
    public partial class CompletionAcceptanceAddDto 
    {
        public Guid? contract_id { get; set; }
        /// <summary>
        ///验收编号
        /// </summary>
        public string acceptance_no { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        public string describe { get; set; }

        /// <summary>
        ///制作人
        /// </summary>
        public int? producer_id { get; set; }

        /// <summary>
        ///审核人
        /// </summary>
        public int? approved_id { get; set; }
    }
}