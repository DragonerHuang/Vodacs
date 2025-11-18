
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Entity.SystemModels;

namespace Vodace.Entity.DomainModels
{
    
    public partial class Biz_Site_Work_Record_Item_Check
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    /// <summary>
    /// BRF：两层结构（Level1/Level2），以第二层为准，承载选择框与文本值。
    /// 值来源 Biz_Site_Work_Record_Item_Check：check_type=0，check_code=Level2.item_code。
    /// </summary>
    public class BrfItemDto
    {
        /// <summary>
        /// 第一层编码（父项 item_code）
        /// </summary>
        public int level1_code { get; set; }

        /// <summary>
        /// 第一层中文名
        /// </summary>
        public string level1_name_cht { get; set; }

        /// <summary>
        /// 第一层英文名
        /// </summary>
        public string level1_name_eng { get; set; }

        /// <summary>
        /// 第一层排序列
        /// </summary>
        public int level1_index { get; set; }

        /// <summary>
        /// 第二层编码（子项 item_code）
        /// </summary>
        public int level2_code { get; set; }

        /// <summary>
        /// 第二层中文名
        /// </summary>
        public string level2_name_cht { get; set; }

        /// <summary>
        /// 第二层英文名
        /// </summary>
        public string level2_name_eng { get; set; }

        /// <summary>
        /// 第二层排序列
        /// </summary>
        public int level2_index { get; set; }

        /// <summary>
        /// 全局编码（global_code，用于分组与前缀判断）
        /// </summary>
        public string global_code { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        public SiteWorkCheckValueDto value { get; set; }
    }

    /// <summary>
    /// Safety_Working_Cycle数据 DTO
    /// </summary>
    public class SafetyWorkingCycleDto
    {
        /// <summary>
        /// 安全早会
        /// </summary>
        public ScrSectionDto safety_morning_meeting { get; set; }

        /// <summary>
        /// 開工前檢查記錄
        /// </summary>
        public ScrSectionDto pre_work_check { get; set; }

        /// <summary>
        /// 工作中安全巡查記錄
        /// </summary>
        public ScrSectionDto safety_record { get; set; }

        /// <summary>
        /// 工作中指導及監督
        /// </summary>
        public ScrSectionDto guidance_supervision { get; set; }

        /// <summary>
        /// 收工前清掃
        /// </summary>
        public ScrSectionDto clean_up { get; set; }

        /// <summary>
        /// 最後檢查
        /// </summary>
        public ScrSectionDto last_check { get; set; }

        /// <summary>
        /// 安全檢討會議
        /// </summary>
        public ScrSectionDto safety_meeting { get; set; }

        /// <summary>
        /// 执行合资格人员签署列表
        /// </summary>
        public List<SiteWorkCheckSignDto> spq_sign { get; set; } = new List<SiteWorkCheckSignDto>();

        /// <summary>
        /// WPIC检查后签署列表
        /// </summary>
        public List<SiteWorkCheckSignDto> wpic_sign { get; set; } = new List<SiteWorkCheckSignDto>();

        /// <summary>
        /// 列表数据
        /// </summary>
        //public List<ScrSectionDto> scr_data { get; set; } = new List<ScrSectionDto>();
    }

    /// <summary>
    /// CPDAS 数据DTO
    /// </summary>
    public class CPDASDto
    {

        /// <summary>
        /// 开工前
        /// </summary>
        public OptionTableDto before_work { get; set; }

        /// <summary>
        /// 施工期间
        /// </summary>
        public OptionTableDto working { get; set; }

        /// <summary>
        /// 完工后
        /// </summary>
        public OptionTableDto after_work { get; set; }

        /// <summary>
        /// 列表数据
        /// </summary>
        //public List<OptionTableDto> cpd_data { get; set; } = new List<OptionTableDto>();

        /// <summary>
        /// 执行合资格人员签署列表
        /// </summary>
        public List<SiteWorkCheckSignDto> spq_sign { get; set; } = new List<SiteWorkCheckSignDto>();

        /// <summary>
        /// WPIC检查后签署列表
        /// </summary>
        public List<SiteWorkCheckSignDto> check_sign { get; set; } = new List<SiteWorkCheckSignDto>();
    }

    /// <summary>
    /// Quality Checklist 数据DTO
    /// </summary>
    public class QualityCheckDto
    {
        /// <summary>
        /// 工作人員資格及訓練
        /// </summary>
        public OptionTableDto qualifications_training { get; set; }

        /// <summary>
        /// 現場施工文件情況
        /// </summary>
        public OptionTableDto site_work_file { get; set; }

        /// <summary>
        /// 物料品質管理
        /// </summary>
        public OptionTableDto quality_management { get; set; }

        /// <summary>
        /// 施工過程及品質控制
        /// </summary>
        public OptionTableDto process_quality_control { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public OptionTableDto others { get; set; }

        /// <summary>
        /// 列表数据
        /// </summary>
        //public List<OptionTableDto> qdc_data { get; set; } = new List<OptionTableDto>();

        /// <summary>
        /// WPIC检查后签署列表
        /// </summary>
        public List<SiteWorkCheckSignDto> check_sign { get; set; } = new List<SiteWorkCheckSignDto>();
    }

    /// <summary>
    /// SIC CP(T)数据DTO
    /// </summary>
    public class SICCPDto
    {
        /// <summary>
        /// 轨道清单数据
        /// </summary>
        public CheckSicDataDto sic_data { get; set; }

        /// <summary>
        /// 進入軌道前
        /// </summary>
        public GroupedSectionDto prior_to_track_access { get; set; }

        /// <summary>
        /// 取得授權進入軌道後
        /// </summary>
        public GroupedSectionDto after_authorized { get; set; }

        /// <summary>
        /// 撤離軌道前
        /// </summary>
        public GroupedSectionDto before_track_clear { get; set; }

        /// <summary>
        /// 撤離軌道後
        /// </summary>
        public GroupedSectionDto after_track_clear { get; set; }

        /// <summary>
        /// 列表数据
        /// </summary>
        //public List<GroupedSectionDto> cp_data { get; set; } = new List<GroupedSectionDto>();

        /// <summary>
        /// SIC CP(T)签署列表
        /// </summary>
        public List<SiteWorkCheckSignDto> sign { get; set; } = new List<SiteWorkCheckSignDto>();

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

    /// <summary>
    /// SCR：第三层选择项（Level3）。值来源：check_type=1，check_code=Level2.item_code，check_sub_code=Level3.item_code。
    /// </summary>
    public class ScrLevel3Dto
    {
        /// <summary>
        /// 第三层编码（子项 item_code）
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 第三层中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        /// 第三层英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        /// 全局编码（global_code）
        /// </summary>
        public string global_code { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        public SiteWorkCheckValueDto value { get; set; }
    }

    /// <summary>
    /// SCR：第二层分组行（Level2），承载第三层的选择项集合。
    /// </summary>
    public class ScrLevel2Dto
    {
        /// <summary>
        /// 第二层编码（父项 item_code）
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 第二层中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        /// 第二层英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        /// 全局编码（global_code）
        /// </summary>
        public string global_code { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        public SiteWorkCheckValueDto value { get; set; }

        /// <summary>
        /// 第三层选择项集合
        /// </summary>
        public List<ScrLevel3Dto> children { get; set; } = new List<ScrLevel3Dto>();
    }

    /// <summary>
    /// SCR：第一层标题（Level1），承载第二层分组项集合。
    /// </summary>
    public class ScrSectionDto
    {
        /// <summary>
        /// 标题编码（Level1.item_code）
        /// </summary>
        public int title_code { get; set; }

        /// <summary>
        /// 标题中文名
        /// </summary>
        public string title_cht { get; set; }

        /// <summary>
        /// 标题英文名
        /// </summary>
        public string title_eng { get; set; }

        /// <summary>
        /// 第二层分组项集合
        /// </summary>
        public List<ScrLevel2Dto> items { get; set; } = new List<ScrLevel2Dto>();

        /// <summary>
        /// 全局编码（global_code）
        /// </summary>
        public string global_code { get; set; }
    }

    /// <summary>
    /// CPD/QDC：两层结构（列头/数据行），承载单选结果（radio_result）。
    /// 值来源：CPD check_type=2；QDC check_type=3；check_code=Level2.item_code。
    /// </summary>
    public class OptionRowDto
    {
        /// <summary>
        /// 数据行编码（Level2.item_code）
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        /// 全局编码（global_code）
        /// </summary>
        public string global_code { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        public SiteWorkCheckValueDto value { get; set; }
    }

    /// <summary>
    /// CPD/QDC：列头（Level1），承载数据行集合。
    /// </summary>
    public class OptionTableDto
    {
        /// <summary>
        /// 列头编码（Level1.item_code）
        /// </summary>
        public int header_code { get; set; }

        /// <summary>
        /// 列头中文名
        /// </summary>
        public string header_cht { get; set; }

        /// <summary>
        /// 列头英文名
        /// </summary>
        public string header_eng { get; set; }

        /// <summary>
        /// 全局编码（global_code）
        /// </summary>
        public string global_code { get; set; }

        /// <summary>
        /// 数据行集合（Level2）
        /// </summary>
        public List<OptionRowDto> rows { get; set; } = new List<OptionRowDto>();
    }

    /// <summary>
    /// CP/SIC：三层结构（标题/行/名称聚合）。第三层同一 master_id 的名称聚合为单行字符串。
    /// </summary>
    public class GroupedRowDto
    {
        /// <summary>
        /// 第二层编码（行 item_code）
        /// </summary>
        public int level2_code { get; set; }

        /// <summary>
        /// 第二层中文名
        /// </summary>
        public string level2_name_cht { get; set; }

        /// <summary>
        /// 第二层英文名
        /// </summary>
        public string level2_name_eng { get; set; }

        /// <summary>
        /// 第三层中文名称聚合（按行分隔）
        /// </summary>
        public string level3_names_cht { get; set; }

        /// <summary>
        /// 第三层英文名称聚合（按行分隔）
        /// </summary>
        public string level3_names_eng { get; set; }

        /// <summary>
        /// 全局编码（global_code）
        /// </summary>
        public string global_code { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        public SiteWorkCheckValueDto value { get; set; }
    }

    /// <summary>
    /// CP/SIC：第一层标题（Level1），承载分组行集合。
    /// </summary>
    public class GroupedSectionDto
    {
        /// <summary>
        /// 标题编码（Level1.item_code）
        /// </summary>
        public int title_code { get; set; }

        /// <summary>
        /// 标题中文名
        /// </summary>
        public string title_cht { get; set; }

        /// <summary>
        /// 标题英文名
        /// </summary>
        public string title_eng { get; set; }

        /// <summary>
        /// 分组行集合（Level2）
        /// </summary>
        public List<GroupedRowDto> rows { get; set; } = new List<GroupedRowDto>();

        /// <summary>
        /// 全局编码（global_code）
        /// </summary>
        public string global_code { get; set; }
    }

    /// <summary>
    /// 检查签名
    /// </summary>
    public class SiteWorkCheckSignDto
    {
        /// <summary>
        /// 签名id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 签名人id
        /// </summary>
        public Guid? sign_id { get; set; }

        /// <summary>
        /// 签名人（中文）
        /// </summary>
        public string sign_name_cht { get; set; }

        /// <summary>
        /// 签名人（英文）
        /// </summary>
        public string sign_name_eng { get; set; }

        /// <summary>
        /// 签名类型
        /// 0：工人签名，
        /// 1：scr的执行合资格人员签署
        /// 2：scr的WPIC检查后签署;
        /// 3：cpd的执行合资格人员签署
        /// 4：cpd的工务督察人员签署；
        /// 5：qdc的WPIC检查后签署;
        /// 6：cp的SIC CP(T)签署；
        /// 7：sic的SIC签署
        /// </summary>
        public int sign_type { get; set; }

        /// <summary>
        /// 签名类型名称
        /// </summary>
        public string sign_type_name { get; set; }

        /// <summary>
        /// 签名图片
        /// </summary>
        public byte[] sign_image { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string sign_job { get; set; }
    }

    /// <summary>
    /// 选项值
    /// </summary>
    public class SiteWorkCheckValueDto
    {
        /// <summary>
        /// 单选结果（radio_result，通常为 1/2/3，没有的话就-1）
        /// </summary>
        public int? radio_result { get; set; } = -1;

        /// <summary>
        /// 选择结果（check_result==1 表示选中）
        /// </summary>
        public bool is_checked { get; set; } = false;

        /// <summary>
        /// 文本框值
        /// </summary>
        public string text_value { get; set; } = string.Empty;

        /// <summary>
        /// 检查时间
        /// </summary>
        public DateTime? time_result { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public string remark { get; set; }
    }

    /// <summary>
    /// 设置选择值dto
    /// </summary>
    public class SetIemValueDto
    {
        /// <summary>
        /// 工地记录id
        /// </summary>
        public Guid record_id { get; set; }

        /// <summary>
        /// 选项类型（0：brf ；1：scr；2：cpd；3：qdc；4：cp；5：sic）
        /// 0:BRF（Briefing Record）, 
        /// 1:SCR（Safety Working Cycle）, 
        /// 2:CPD（CPDAS）, 
        /// 3:QDC（Quality Checklist）,
        /// 4:CP（SIC CP(T)）,
        /// 5:SIC（SIC）
        /// </summary>
        public int check_type { get; set; }

        /// <summary>
        /// 设置的选项值列表
        /// </summary>
        public List<SetItemValueDataDto> value_items { get; set; } = new List<SetItemValueDataDto>();
    }

    /// <summary>
    /// 选择值选项列表
    /// </summary>
    public class SetItemValueDataDto: SiteWorkCheckValueDto
    {
        /// <summary>
        /// 工地记录id
        /// </summary>
        public Guid record_id { get; set; }

        /// <summary>
        /// 选项类型（0：brf ；1：scr；2：cpd；3：qdc；4：cp；5：sic）
        /// 0:BRF（Briefing Record）, 
        /// 1:SCR（Safety Working Cycle）, 
        /// 2:CPD（CPDAS）, 
        /// 3:QDC（Quality Checklist）,
        /// 4:CP（SIC CP(T)）, 
        /// 5:SIC（SIC）
        /// </summary>
        public int item_type { get; set; }

        /// <summary>
        /// 选项代码
        /// </summary>
        public int? item_code { get; set; }
    }

    /// <summary>
    /// 选项值签名
    /// </summary>
    public class SetItemSignDto
    {
        /// <summary>
        /// 工地记录id
        /// </summary>
        public Guid record_id { get; set; }

        /// <summary>
        /// 如果是工人签名的话，这个不能为空
        /// </summary>
        //public Guid? worker_id { get; set; }

        /// <summary>
        /// 签名类型
        /// 0：工人签名，
        /// 1：scr的执行合资格人员签署，2：scr的WPIC检查后签署;
        /// 3：cpd的执行合资格人员签署，4：cpd的工务督察人员签署；
        /// 5：qdc的WPIC检查后签署;
        /// 6：cp的SIC CP(T)签署；
        /// 7：sic的SIC签署
        /// </summary>
        public int sign_type { get; set; }

        /// <summary>
        /// 签名人（没有的话，就默认当前用户）
        /// </summary>
        public Guid? sign_id { get; set; }

        /// <summary>
        /// 文件索引（传过来的文件流集合索引号）
        /// </summary>
        public int file_index { get; set; }
    }

    /// <summary>
    /// 轨道检查数据
    /// </summary>
    public class CheckSicDataDto
    {
        /// <summary>
        /// 工地记录
        /// </summary>
        public Guid record_id { get; set; }

        /// <summary>
        /// 参考编号
        /// </summary>
        public string sic_ref { get; set; }

        /// <summary>
        /// 工作地点id
        /// </summary>
        public Guid? site_id { get; set; }

        /// <summary>
        /// 工作地点
        /// </summary>
        public string site_sho { get; set; }

        /// <summary>
        /// sic工人id
        /// </summary>
        public Guid? sic_worker_id { get; set; }

        /// <summary>
        /// sic工人中文名
        /// </summary>
        public string sic_worker_cht { get; set; }

        /// <summary>
        /// sic工人英文名
        /// </summary>
        public string sic_worker_eng { get; set; }

        /// <summary>
        /// CP(T)工人id
        /// </summary>
        public Guid? cp_worker_id { get; set; }

        /// <summary>
        /// CP(T)工人中文名
        /// </summary>
        public string cp_worker_cht { get; set; }

        /// <summary>
        /// CP(T)工人英文名
        /// </summary>
        public string cp_worker_eng { get; set; }

        
    }

    /// <summary>
    /// 编辑轨道清单内容
    /// </summary>
    public class EditCheckSICdAT
    {
        /// <summary>
        /// 工地记录
        /// </summary>
        public Guid record_id { get; set; }

        /// <summary>
        /// 参考编号
        /// </summary>
        public string sic_ref { get; set; }

        /// <summary>
        /// sic工人id
        /// </summary>
        public Guid? sic_worker_id { get; set; }

    }
}