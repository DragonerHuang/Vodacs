
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
    
    public partial class Sys_Message_Notification
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class MessageNotificationDto 
    {
        public Guid id { get; set; }

        /// <summary>
        ///消息类型（0：通知；1：消息；2：代办）
        /// </summary>
        public int msg_type { get; set; }
        public string msg_type_str { get; set; }

        /// <summary>
        ///消息标题
        /// </summary>
        public string msg_title { get; set; }
        /// <summary>
        ///消息内容
        /// </summary>
        public string msg_content { get; set; }
        /// <summary>
        ///消息状态（0：未读；1：已读；2：已处理；3：已逾期）
        /// </summary>
        public int status { get; set; }
        public string status_str { get; set; }
        /// <summary>
        ///接收人
        /// </summary>
        public string receive_user { get; set; }
        public DateTime? modify_date { get; set; }
        public Guid? relation_id { get; set; }
        public DateTime create_date { get; set; }
    }

    public class MessageNotificationAddDto
    {
        /// <summary>
        ///消息类型（0：通知；1：消息；2：代办）
        /// </summary>
        public int msg_type { get; set; }
        /// <summary>
        ///消息标题
        /// </summary>
        public string msg_title { get; set; }
        /// <summary>
        ///消息内容
        /// </summary>
        public string msg_content { get; set; }
        /// <summary>
        ///消息状态（0：未读；1：已读；2：已处理；3：已逾期）
        /// </summary>
        public int status { get; set; }
        /// <summary>
        ///接收人
        /// </summary>
        public string receive_user { get; set; }
        public Guid? relation_id { get; set; }
    }

    public class MailDto 
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string MailTo { get; set; }
        public bool IsExpected { get; set; }
        public string UserName { get; set; }
        public int? DiffDays { get; set; }
    }

    public class EventQnDto 
    {
        /// <summary>
        /// 合同编号
        /// </summary>
        public string qn_no { get; set; }
        /// <summary>
        /// 合同阶段(3：招标前；4：现场考察；5：招标)
        /// </summary>
        public int qn_type { get; set; }
        /// <summary>
        /// 截至日志
        /// </summary>
        public string closing_date { get; set; }

    }

    public class MailQnTasks
    {
        public string ContractNo { get; set; }
        public string Stage { get; set; }
        public string Deadline { get; set; }
        public string Status { get; set; }
        public string Reminder { get; set; }
        public int? DiffDays { get; set; }
        public bool IsExpected { get; set; }
    }
}