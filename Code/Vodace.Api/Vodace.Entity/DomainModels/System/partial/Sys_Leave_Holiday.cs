using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Vodace.Entity.SystemModels;

namespace Vodace.Entity.DomainModels
{
    public partial class Sys_Leave_Holiday
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class LeaveHolidayQuery
    {
        /// <summary>
        ///假期类型id 来自Sys_Leave_Type
        /// </summary>
        public Guid leave_type_id { get; set; }

        /// <summary>
        ///假期名字
        /// </summary>
        public string holiday_name { get; set; }
    }

    public class LeaveHolidayAddDto
    {
        /// <summary>
        ///假期类型id 来自Sys_Leave_Type
        /// </summary>
        public Guid leave_type_id { get; set; }

        /// <summary>
        ///日期
        /// </summary>
        public DateTime date { get; set; }

        /// <summary>
        ///假期名字
        /// </summary>
        public string holiday_name { get; set; }
    }

    public class LeaveHolidayEditDto : LeaveHolidayAddDto
    {
        public Guid id { get; set; }
    }

    public class LeaveHolidayListDto
    {
        public Guid id { get; set; }

        /// <summary>
        ///假期类型id 来自Sys_Leave_Type
        /// </summary>
        public Guid leave_type_id { get; set; }

        public string leave_type_name { get; set; }

        /// <summary>
        ///日期
        /// </summary>
        public DateTime date { get; set; }

        /// <summary>
        ///假期名字
        /// </summary>
        public string holiday_name { get; set; }

        public string create_name { get; set; }

        public DateTime? create_date { get; set; }
    }

    public class LeaveHolidayImportDto
    {
        /// <summary>
        /// 假期类型
        /// </summary>
        public Guid leave_type_id { get; set; }

        /// <summary>
        /// 导入json
        /// </summary>
        public string import_json { get; set; }
    }

    public class VCalendar
    {
        [JsonProperty("vevent")]
        public List<VEventDetailed> vevent { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("vcalendar")]
        public List<VCalendar> vcalendar { get; set; }
    }

    // 如果需要处理dtstart和dtend中的复杂结构，可以创建专门的类
    public class DateValue
    {
        [JsonProperty("value")]
        public string value { get; set; }
    }

    // 或者使用更精确的VEvent类来处理日期字段
    public class VEventDetailed
    {
       
        [JsonProperty("dtstamp")]
        public string dtstamp { get; set; }

        [JsonProperty("transp")]
        public string transp { get; set; }

        [JsonProperty("uid")]
        public string uid { get; set; }

        [JsonProperty("summary")]
        public string summary { get; set; }
	
        [JsonProperty("dtstart")]
        public List<object> dtstart { get; set; }

        [JsonProperty("dtend")]
        public List<object> dtend { get; set; }

        // 添加便捷属性来获取日期
        public string StartDate => dtstart?[0] as string;
        public string EndDate => dtend?[0] as string;
    }

    public class DateArray
    {
        public string date_string { get; set; }
        public DateValue date_value { get; set; }
    }
}