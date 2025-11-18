
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
    
    public partial class Sys_Attendance_Record
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class AttendanceRecordQuery
    {
        public int user_id { get; set; }

        /// <summary>
        /// 工人编号
        /// </summary>
        public string user_no { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
    }

    public class AtdRecordMaxMinTimeQuery
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? end_time { get; set; }
    }

    public class AtdDayRecordDetailsQuery:AtdRecordMaxMinTimeQuery
    {
        public string remark { get; set; }

        public string user_name { get; set; }
    }

    /// <summary>
    /// 返回实体
    /// </summary>
    public class AtdDayRecordDetailsWebDto
    {
        public string user_no { get; set; }

        public int user_id { get; set; }

        /// <summary>
        /// 当前日期
        /// </summary>
        public DateTime current_date { get; set; }

        /// <summary>
        /// 上班时间
        /// </summary>
        public DateTime? min_time { get; set; }
        
        /// <summary>
        /// 下班时间
        /// </summary>
        public DateTime? max_time { get; set; }
        
        /// <summary>
        /// 中文名
        /// </summary>
        public string user_true_name { get; set; }
        
        /// <summary>
        /// 英文名
        /// </summary>
        public string user_name_eng { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public decimal hours => (decimal)(!max_time.HasValue
            ? 0
            : Math.Round((max_time!.Value - min_time!.Value).Minutes / 60.00, 1));

        /// <summary>
        /// 上班地址
        /// </summary>
        public string on_work_address { get; set; }
        
        
        /// <summary>
        ///下班地址
        /// </summary>
        public string off_work_address { get; set; }
    }
    
    /// <summary>
    /// 当前上班打卡时间和下班打卡时间
    /// </summary>
    public class AtdRecordMaxMinTimeWebDto
    {
        public string user_no { get; set; }

        public int user_id { get; set; }

        public DateTime? min_time { get; set; }
        
        public DateTime? max_time { get; set; }
        
        public string user_true_name { get; set; }
        
        public string user_name_eng { get; set; }
    }
    
    public class AttendanceRecordWebDto 
    {
        public int type { get; set; }
        public string data { get; set; }
        public string extra { get; set; }
        public string time { get; set; }
        public string deviceSn { get; set; }
    }

    public class AttendanceRecordWebResponseDto
    {
        public int code { get; set; }
        public int cmd { get; set; }
        public string message { get; set; }
        public string voiceData { get; set; }
    }
    public class AttendanceRecordDto 
    {
        public int user_id { get; set; }

        /// <summary>
        /// 工人编号
        /// </summary>
        public string user_no { get; set; }

        /// <summary>
        ///打卡时间
        /// </summary>
        //public DateTime? punch_time { get; set; }

        /// <summary>
        ///设备来源（例如 打卡机：1，手机：2）
        /// </summary>
        public int device_source { get; set; }

        /// <summary>
        ///地点ID
        /// </summary>
        public Guid? location_id { get; set; }

        /// <summary>
        ///纬度
        /// </summary>
        public decimal? latitude { get; set; }

        /// <summary>
        ///经度
        /// </summary>
        public decimal? longitude { get; set; }
        /// <summary>
        ///打卡地点
        /// </summary>
        public string adderss { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }
    }

    public class AttendanceRecordDto1
    {
        public string cmd { get; set; }
        public string sn { get; set; }
        public Body body { get; set; }
        public string token { get; set; }
    }

    public class Body
    {
        public string name { get; set; }
        public int jobnumber { get; set; }
        public int time { get; set; }
        public int type { get; set; }
    }
}