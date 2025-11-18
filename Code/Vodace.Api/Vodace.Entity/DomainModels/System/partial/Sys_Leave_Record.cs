
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
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
    
    public partial class Sys_Leave_Record
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class SearchLeaveRecordDto
    {
        public Guid? id { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// 员工编码
        /// </summary>
        public string user_no { get; set; }
        /// <summary>
        /// 假期类型
        /// </summary>
        public Guid? leave_type_id { get; set; }
        /// <summary>
        ///假期开始日
        /// </summary>
        public DateTime? start_date { get; set; }
        /// <summary>
        ///假期结束日
        /// </summary>
        public DateTime? end_date { get; set; }
        /// <summary>
        /// 假期期间
        /// </summary>
        /// <remarks>
        /// 0：全日
        /// 1：上午
        /// 2：下午
        /// </remarks>
        public int? period { get; set; }
        /// <summary>
        ///请假原因
        /// </summary>
        public string reason { get; set; }
        /// <summary>
        ///批准者ID
        /// </summary>
        public Guid? approver_id { get; set; }
        /// <summary>
        ///假期批核状态
        /// </summary>
        /// <remarks>
        /// 1:申请中pending
        /// 2:已批准Approved
        /// 3:拒绝rejrcted
        /// </remarks>
        public int status { get; set; }

        /// <summary>
        /// 发送人邮箱地址
        /// </summary>
        public string from_email { get; set; }
        /// <summary>
        /// 接收人邮箱地址
        /// </summary>
        public string to_email { get; set; }
        /// <summary>
        /// 抄送人邮箱地址
        /// </summary>
        public string cc_email { get; set; }
        /// <summary>
        /// 密送人邮箱地址
        /// </summary>
        public string bcc_email { get; set; }
    }

    public class LeaveRecordDto : SearchLeaveRecordDto
    {
        /// <summary>
        /// 请假附件
        /// </summary>
        public List<IFormFile> file { get; set; }
    }

    public class LeaveAttachmentDto
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        public string file_name { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string file_path { get; set; }
        /// <summary>
        /// 副档类型(文件后缀)
        /// </summary>
        public string file_type { get; set; }
    }

    public class LeaveRecordEditDto : LeaveRecordDto
    {
        public Guid? id { get; set; }
    }
    public class LeaveRecordReviewDto
    {
        public Guid? id { get; set; }
        /// <summary>
        ///假期批核状态
        /// </summary>
        /// <remarks>
        /// 1:申请中pending
        /// 2:已批准Approved
        /// 3:拒绝rejrcted
        /// </remarks>
        public int status { get; set; }
        /// <summary>
        /// 审核人ID
        /// </summary>
        public Guid? approver_id { get; set; }
    }

    public class LeaveRecordListDto : Sys_Leave_Record
    {
        public string leave_type_code { get; set; }
        public string leave_type_name { get; set; }

        public string user_name { get; set; }
        public string user_true_name { get; set; }
        public string user_name_eng { get; set; }
        public Guid? company_id { get; set; }

        public string approver_true_name { get; set; }
        public string approver_name_eng { get; set; }
        public string approver_name { get; set; }
        public string approver_user_no { get; set; }


        public List<AttachmentDto> attachment { get; set; }
    }

    public class AttachmentDto
    {
        public Guid id { get; set; }
        public Guid leave_id { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public string file_type { get; set; }
    }
}