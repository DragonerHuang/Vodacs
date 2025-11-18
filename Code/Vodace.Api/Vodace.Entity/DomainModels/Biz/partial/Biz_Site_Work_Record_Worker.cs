
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

    public partial class Biz_Site_Work_Record_Worker
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    /// <summary>
    /// 工地工人分页查询条件
    /// </summary>
    public class SiteWorkerSearchDto
    {
        /// <summary>
        /// 工地记录id
        /// </summary>
        public Guid? record_id { get; set; }

        /// <summary>
        /// 是否展示图片
        /// </summary>
        public bool is_show_image { get; set; }
    }

    /// <summary>
    /// 工人dto
    /// </summary>
    public class SiteWorkerDto
    {
        /// <summary>
        /// 工人id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 所属工地记录id
        /// </summary>
        public Guid? record_id { get; set; }

        /// <summary>
        /// 联系人id
        /// </summary>
        public Guid? contact_id { get; set; }

        /// <summary>
        /// 联系人中文名
        /// </summary>
        public string contact_name_cht { get; set; }

        /// <summary>
        /// 联系人英文名
        /// </summary>
        public string contact_name_eng { get; set; }

        /// <summary>
        /// 绿卡编号
        /// </summary>
        public string green_card_no { get; set; }

        /// <summary>
        /// 绿卡过期时间
        /// </summary>
        public DateTime? green_card_exp { get; set; }

        /// <summary>
        /// valid（0：否；1：是）
        /// </summary>
        public bool is_valid { get; set; }

        /// <summary>
        /// 所属工种id
        /// </summary>
        public Guid? work_type_id { get; set; }

        /// <summary>
        /// 所属工种
        /// </summary>
        public string work_type { get; set; }

        /// <summary>
        /// wpic（0：否；1：是）
        /// </summary>
        public bool is_wpic { get; set; }

        /// <summary>
        /// is_cp（0：否；1：是）
        /// </summary>
        public bool is_cp { get; set; }

        /// <summary>
        /// NT/T（0：否；1：是）
        /// </summary>
        public bool is_nt { get; set; }

        /// <summary>
        /// 工作备注
        /// </summary>
        public string work_remark { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public Guid? company_id { get; set; }

        /// <summary>
        /// 公司中文
        /// </summary>
        public string company_name_cht { get; set; }

        /// <summary>
        /// 公司英文
        /// </summary>
        public string company_name_eng { get; set; }

        /// <summary>
        /// 是否生病（0：否；1：是）
        /// </summary>
        public bool is_sick { get; set; }

        /// <summary>
        /// 是否签名（0：否；1：是）
        /// </summary>
        public bool is_sign { get; set; }

        /// <summary>
        /// 签名图片
        /// </summary>
        public byte[] sign_image { get; set; }
    }

    /// <summary>
    /// 出勤记录
    /// </summary>
    public class CICAttendanceDto
    {
        /// <summary>
        /// 工人id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 所属工地记录id
        /// </summary>
        public Guid? record_id { get; set; }

        /// <summary>
        /// 联系人id
        /// </summary>
        public Guid? contact_id { get; set; }

        /// <summary>
        /// 联系人中文名
        /// </summary>
        public string contact_name_cht { get; set; }

        /// <summary>
        /// 联系人英文名
        /// </summary>
        public string contact_name_eng { get; set; }

        /// <summary>
        /// cic编号
        /// </summary>
        public string cic_no { get; set; }

        /// <summary>
        /// cic序列号
        /// </summary>
        public string cic_card_no { get; set; }

        /// <summary>
        /// 上班时间
        /// </summary>
        public DateTime? time_in { get; set; }

        /// <summary>
        /// 下班时间
        /// </summary>
        public DateTime? time_out { get; set; }
    }


    /// <summary>
    /// 添加工人DTO
    /// </summary>
    public class AddWorkerDto
    {
        /// <summary>
        /// 工地工作记录id
        /// </summary>
        public Guid record_id { get; set; }

        /// <summary>
        /// 工人id（联系人id）
        /// </summary>
        public Guid contact_id { get; set; }

        /// <summary>
        /// 工种id
        /// </summary>
        public Guid? work_type_id { get; set; }

        /// <summary>
        /// valid（0：否；1：是）
        /// </summary>
        public int? is_valid { get; set; }

        /// <summary>
        /// wpic（0：否；1：是）
        /// </summary>
        public int? is_wpic { get; set; }

        /// <summary>
        /// is_cp（0：否；1：是）
        /// </summary>
        public int? is_cp { get; set; }

        /// <summary>
        /// NT/T（0：否；1：是）
        /// </summary>
        public int? is_nt { get; set; }

        /// <summary>
        /// 是否生病（0：否；1：是）
        /// </summary>
        public int? is_sick { get; set; }

        /// <summary>
        /// 工作备注
        /// </summary>
        public string work_remark { get; set; }

        /// <summary>
        /// 上班时间
        /// </summary>
        public DateTime? time_in { get; set; }

        /// <summary>
        /// 下班时间
        /// </summary>
        public DateTime? time_out { get; set; }
    }

    /// <summary>
    /// 编辑工人DTO（app端）
    /// </summary>
    public class EditWorkerDto
    {
        /// <summary>
        /// 工人id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 工种id
        /// </summary>
        public Guid? work_type_id { get; set; }

        /// <summary>
        /// wpic（0：否；1：是）
        /// </summary>
        public int? is_wpic { get; set; }

        /// <summary>
        /// 是否生病（0：否；1：是）
        /// </summary>
        public int? is_sick { get; set; }

        /// <summary>
        /// 工作备注
        /// </summary>
        public string work_remark { get; set; }
    }

    /// <summary>
    /// 编辑工人DTO（web端）
    /// </summary>
    public class EditWorkByWebDto
    {
        /// <summary>
        /// 工人id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 绿卡号
        /// </summary>
        public string green_card_no { get; set; }

        /// <summary>
        /// 绿卡过期时间
        /// </summary>
        public DateTime? green_card_exp { get; set; }

        /// <summary>
        /// wpic（0：否；1：是）
        /// </summary>
        public int? is_wpic { get; set; }

        /// <summary>
        /// 工种id
        /// </summary>
        public Guid? work_type_id { get; set; }

        /// <summary>
        /// is_cp（0：否；1：是）
        /// </summary>
        public int? is_cp { get; set; }

        /// <summary>
        /// NT/T（0：否；1：是）
        /// </summary>
        public int? is_nt { get; set; }

        /// <summary>
        /// 工作备注
        /// </summary>
        public string work_remark { get; set; }

        /// <summary>
        /// 是否生病（0：否；1：是）
        /// </summary>
        public int? is_sick { get; set; }
    }

    /// <summary>
    /// 添加工人dto
    /// </summary>
    public class AddWorkerByContactDto()
    {
        /// <summary>
        /// 工地工作记录id
        /// </summary>
        public Guid record_id { get; set; }

        /// <summary>
        /// 工人id（联系人id）
        /// </summary>
        public List<Guid> contact_ids { get; set; } = new List<Guid>();
    }

    /// <summary>
    /// 工人上下班时间调整
    /// </summary>
    public class WorkerTimeChangeDto
    {
        /// <summary>
        /// 工人id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 时间类型（0：上班；1：下班）
        /// </summary>
        public int time_type { get; set; }

        /// <summary>
        /// 调整的时间
        /// </summary>
        public DateTime? time_change { get; set; }
    }

    /// <summary>
    /// 工人薪资手动调整
    /// </summary>
    public class WorkerSalaryChangeDto
    {
        /// <summary>
        /// 工人id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 调整类型（0：手动赋值；1：重新根据工种薪资计算）
        /// </summary>
        public int change_type { get; set; }

        /// <summary>
        /// 薪资
        /// </summary>
        public decimal? salary { get; set; }
    }

    /// <summary>
    /// 工人联系人DTO
    /// </summary>
    public class WorkerContactDto
    {
        /// <summary>
        /// 工人id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 联系人id
        /// </summary>
        public Guid contact_id { get; set; }

        /// <summary>
        /// 工人中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        /// 工人英文名
        /// </summary>
        public string name_eng { get; set; }
    }

    /// <summary>
    /// 编辑工人考勤记录DTO
    /// </summary>
    public class EditWorkerAttendanceDto
    {
        /// <summary>
        /// 工人id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// cic编号
        /// </summary>
        public string cic_no { get; set; }

        /// <summary>
        /// cic卡序列号
        /// </summary>
        public string cic_card_no { get; set; }

        /// <summary>
        /// 上班时间
        /// </summary>
        public DateTime? time_in { get; set; }

        /// <summary>
        /// 下班时间
        /// </summary>
        public DateTime time_out { get; set; }
    }

    /// <summary>
    /// 设置联系人所属工种薪资
    /// </summary>
    public class SetContactWorkTypeSalaryDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 白班薪资
        /// </summary>
        public decimal day_salary { get; set; }

        /// <summary>
        /// 晚班薪资
        /// </summary>
        public decimal night_salary { get; set; }
    }

    /// <summary>
    /// 联系人工种查询DTO
    /// </summary>
    public class ContactWorkTypeSearchDto
    {
        /// <summary>
        /// 联系人id
        /// </summary>
        public Guid contact_id { get; set; }

        /// <summary>
        /// 工种名称
        /// </summary>
        public string work_type_name { get; set; }
        /// <summary>
        /// 开工日期（开始）
        /// </summary>
        public DateTime? work_date_start { get; set; }
        /// <summary>
        /// 开工日期（结束）
        /// </summary>
        public DateTime? work_date_end { get; set; }
    }

    public class SiteAttendanceRecordDto
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public Guid? site_id { get; set; }
        /// <summary>
        /// 合约id
        /// </summary>
        public Guid? contract_id { get; set; }
        /// <summary>
        /// 联系人id
        /// </summary>
        public Guid? contact_id { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string contact_name { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string company_name { get; set; }
        /// <summary>
        /// 开工日期（开始）
        /// </summary>
        public DateTime? work_date_start { get; set; }
        /// <summary>
        /// 开工日期（结束）
        /// </summary>
        public DateTime? work_date_end { get; set; }
        /// <summary>
        /// 地点名称
        /// </summary>
        public string site_name { get; set; }
        /// <summary>
        /// 按站点筛选
        /// </summary>
        public List<Guid> site_ids { get; set; }
    }
}