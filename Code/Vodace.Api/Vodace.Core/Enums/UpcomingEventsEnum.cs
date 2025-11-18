using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    public enum UpcomingEventsEnum
    {
        /// <summary>
        /// 报价
        /// </summary>
        [Description("报价")]
        Quotation = 0,

        /// <summary>
        /// 项目
        /// </summary>
        [Description("项目")]
        Project = 1,

        /// <summary>
        /// 滚动计划
        /// </summary>
        [Description("滚动计划")]
        Rolling = 2,

        /// <summary>
        ///  期限管理-预审
        /// </summary>
        [Description("预审")]
        QnPQ = 3,

        /// <summary>
        /// 期限管理-现场考察
        /// </summary>
        [Description("现场考察")]
        QnPE = 4,

        /// <summary>
        /// 期限管理-招标
        /// </summary>
        [Description("招标")]
        QnTender = 5,

        /// <summary>
        /// 期限管理-公开招标
        /// </summary>
        [Description("公开招标")]
        QnAdvertisement = 6,

        /// <summary>
        /// 期限管理-预审问答
        /// </summary>
        [Description("预审问答")]
        QnPQQA = 7,

        /// <summary>
        /// 期限管理-邀请招标
        /// </summary>
        [Description("邀请招标")]
        QnPEI = 8,

        /// <summary>
        /// 期限管理-招标问答
        /// </summary>
        [Description("招标问答")]
        QnTenderQA = 9,

        /// <summary>
        /// 期限管理-面试
        /// </summary>
        [Description("面试")]
        QnTenderInterview = 10,
    }

    public enum MessageTypeEnum 
    {
        /// <summary>
        /// 通知
        /// </summary>
        Notice=0,
        /// <summary>
        /// 消息
        /// </summary>
        Message=1,
        /// <summary>
        /// 代办
        /// </summary>
        Todo=2
    }

    public enum MessageStatus 
    {
        /// <summary>
        /// 未读
        /// </summary>
        Unread=0,
        /// <summary>
        /// 已读
        /// </summary>
        Read=1,
        /// <summary>
        /// 已处理
        /// </summary>
        Processed=2,
        /// <summary>
        /// 已逾期
        /// </summary>
        Expected=3,
        /// <summary>
        /// 过期
        /// </summary>
        Close=4,
    }
}
