
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Entity;

namespace Vodace.Entity.DomainModels
{
    
    public partial class Biz_Upcoming_Events
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class UpcomingEventsOut 
    {
        public Guid id { get; set; }

        /// <summary>
        ///活动名称
        /// </summary>
        public string event_name { get; set; }
        public string event_name_eng { get; set; }
        public string event_name_chw { get; set; }
        /// <summary>
        ///截至日期
        /// </summary>
        public DateTime closing_date { get; set; }

        /// <summary>
        ///剩余天数
        /// </summary>
        public int days_left_to_close { get; set; }

        /// <summary>
        ///活动类型（0：报价；1：项目；2：滚动计划）
        /// </summary>
        public int event_type { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }
        public string event_no { get; set; }

        /// <summary>
        ///活动关联id
        /// </summary>
        public Guid? relation_id { get; set; }
    }

    public class Upcoming_Events_RemarkDto
    { 
        public string qn_no { get; set; }
        public int qn_type { get; set; }
        public string closing_date { get; set; }
    }

    public class Upcoming_EventsDto: Upcoming_Events_OptionDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; } = 0;
        /// <summary>
        ///
        /// </summary>
        public DateTime? create_date { get; set; } = DateTime.Now;
    }

    public class Upcoming_Events_OptionDto
    {
        /// <summary>
        ///活动名称
        /// </summary>
        public string event_name { get; set; }

        /// <summary>
        /// 活动英文名
        /// </summary>
        public string event_name_eng { get; set; }

        /// <summary>
        ///截至日期
        /// </summary>
        public DateTime closing_date { get; set; }

        /// <summary>
        ///活动类型（0：报价；1：项目；2：滚动计划）
        /// </summary>
        public int event_type { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///活动编号
        /// </summary>
        public string event_no { get; set; }

        /// <summary>
        ///活动关联id
        /// </summary>
        public Guid? relation_id { get; set; }

        /// <summary>
        /// 消息推送接收人Id
        /// </summary>
        public Guid? recipient_user_id { get; set; }

    }
}