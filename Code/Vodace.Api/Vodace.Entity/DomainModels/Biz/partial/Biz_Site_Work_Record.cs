
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
    
    public partial class Biz_Site_Work_Record
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    /// <summary>
    /// 工地工作记录分页查询条件
    /// </summary>
    public class SiteWorkRecordSearchDto
    {
        /// <summary>
        /// 合同编码（模糊）
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 开始日期（work_date）
        /// </summary>
        public DateTime? start_date { get; set; }

        /// <summary>
        /// 截止日期（work_date）
        /// </summary>
        public DateTime? end_date { get; set; }

        /// <summary>
        /// 按合同筛选（contract_id）
        /// </summary>
        public Guid? contract_id { get; set; }

        /// <summary>
        /// 按站点筛选（可多个 site_id）
        /// </summary>
        public List<Guid> site_ids { get; set; }

        /// <summary>
        /// 当天日期（app）
        /// </summary>
        public DateTime? date { get; set; }

        /// <summary>
        /// 所属工程进度id
        /// </summary>
        public Guid? rolling_program_id { get; set; }

        /// <summary>
        /// 值更（0：TH日更，1：中更，2：NTN夜更）（app）
        /// </summary>
        public int? shift { get; set; }

       
    }

    /// <summary>
    /// 工地工作记录分页列表DTO
    /// </summary>
    public class SiteWorkRecordListDto
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 合同id
        /// </summary>
        public Guid? contract_id { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 所属工程进度id
        /// </summary>
        public Guid? rolling_program_id { get; set; }

        /// <summary>
        /// 工作性质
        /// </summary>
        public string rolling_program_content { get; set; }

        /// <summary>
        /// 位置（站点简称）
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public Guid? site_id { get; set; }

        /// <summary>
        /// 日期（work_date）
        /// </summary>
        public DateTime? date { get; set; }

        /// <summary>
        /// 入场时间（工人表最早的 time_in）
        /// </summary>
        public DateTime? time_in { get; set; }

        /// <summary>
        /// 离场时间（工人表最晚的 time_out）
        /// </summary>
        public DateTime? time_out { get; set; }

        /// <summary>
        /// 值更索引
        /// </summary>
        public int? shift_index { get; set; }

        /// <summary>
        /// 值更值
        /// </summary>
        public string shift { get; set; }

        /// <summary>
        /// CPT统计
        /// </summary>
        public int cpt_count { get; set; }

        /// <summary>
        /// CPNT统计
        /// </summary>
        public int cpnt_count { get; set; }

        /// <summary>
        /// FM统计
        /// </summary>
        public int fm_count { get; set; }

        /// <summary>
        /// 工人统计
        /// </summary>
        public int work_count { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid? company_id { get; set; }

        /// <summary>
        /// 管工id
        /// </summary>
        public Guid? duty_cp_id { get; set; }

        /// <summary>
        /// 当前值更管工id
        /// </summary>
        public Guid? current_duty_cp_id { get; set; }

        /// <summary>
        /// SIC复检人id
        /// </summary>
        public Guid? sic_id { get; set; }

    }

    /// <summary>
    /// 工地工作记录详情DTO
    /// </summary>
    public class SiteWorkRecordDetailsDto
    {
        /// <summary>
        /// 记录id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 所属合同id
        /// </summary>
        public Guid? contract_id { get; set; }

        /// <summary>
        /// 合同编码
        /// </summary>
        public string contract_no { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        public string contract_name { get; set; }

        /// <summary>
        /// 所属项目id
        /// </summary>
        public Guid? project_id { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string project_no { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string project_name { get; set; }

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
        /// 站点id
        /// </summary>
        public Guid? site_id { get; set; }

        /// <summary>
        /// 站点 缩写
        /// </summary>
        public string site_sho { get; set; }

        /// <summary>
        /// 子站点id
        /// </summary>
        public Guid? sub_site_id { get; set; }

        /// <summary>
        /// 子站点 缩写
        /// </summary>
        public string sub_site_sho { get; set; }

        /// <summary>
        /// 当前值更管工id
        /// </summary>
        public Guid? current_duty_id { get; set; }

        /// <summary>
        /// 当前值更管工名称（中文）
        /// </summary>
        public string current_duty_name_cht { get; set; }

        /// <summary>
        /// 当前值更管工名称（英文）
        /// </summary>
        public string current_duty_name_eng { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        public string job_duties { get; set; }

        /// <summary>
        /// 值更代码
        /// </summary>
        public string shift_code { get; set; }

        /// <summary>
        /// 是否进入轨道（0：否；1：是）
        /// </summary>
        public bool is_track { get; set; }

        /// <summary>
        /// 工作日期
        /// </summary>
        public DateTime? work_date { get; set; }

        /// <summary>
        /// 是否能操作（0：否，1：是）
        /// </summary>
        //public int is_operate { get; set; }
    }

    /// <summary>
    /// 工地工作记录图片DTO
    /// </summary>
    public class SiteWorkRecordPhotoDto
    {
        /// <summary>
        /// 开工前图片
        /// </summary>
        public List<PhotoDto> before_work { get; set; }

        /// <summary>
        /// 施工中图片
        /// </summary>
        public List<PhotoDto> working { get; set; }

        /// <summary>
        /// 完工后图片
        /// </summary>
        public List<PhotoDto> after_work { get; set; }
    }

    /// <summary>
    /// 图片dto
    /// </summary>
    public class PhotoDto
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int index { get; set; }

        /// <summary>
        /// 高清文件名
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        /// 高清文件路径
        /// </summary>
        //public string file_path { get; set; }

        /// <summary>
        /// 缩略图文件名
        /// </summary>
        public string file_thumb_name { get; set; }

        /// <summary>
        /// 缩略图路径
        /// </summary>
        //public string file_thumb_path { get; set; }
    }

    public class SiteWorkRecordDto
    {
        public Guid? id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 所属合同id
        /// </summary>
        public Guid? contract_id { get; set; }
        /// <summary>
        /// 所属工程进度id
        /// </summary>
        public Guid? rolling_program_id { get; set; }
        /// <summary>
        /// 站点id
        /// </summary>
        public Guid? site_id { get; set; }
        /// <summary>
        /// 子站点id（暂无）
        /// </summary>
        public Guid? sub_site_id { get; set; }
        /// <summary>
        /// 管工id
        /// </summary>
        public Guid? duty_cp_id { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string job_duties { get; set; }
        /// <summary>
        /// 值更
        /// </summary>
        public string shift { get; set; }
        /// <summary>
        /// 是否进入轨道（0：否；1：是）
        /// </summary>
        public int is_track { get; set; }
        /// <summary>
        /// 工作日期
        /// </summary>
        public DateTime? work_date { get; set; }
        /// <summary>
        /// 是否完成安全简介（0：否；1：是）
        /// </summary>
        public int finish_briefing { get; set; }
        /// <summary>
        /// 选择的配置（Sys_Site_Work_Chk_Item）
        /// </summary>
        public string check_config { get; set; }
        /// <summary>
        /// 当前值更管工id
        /// </summary>
        public Guid? current_duty_cp_id { get; set; }
    }

    /// <summary>
    /// 编辑工地工作记录信息
    /// </summary>
    public class EditSiteWorkRecordDto
    {
        /// <summary>
        /// 工地工作记录id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? work_date { get; set; }

        /// <summary>
        /// 值更
        /// </summary>
        public string shift { get; set; }

        /// <summary>
        /// 当值管工id
        /// </summary>
        public Guid? current_duty_id { get; set; }

        /// <summary>
        /// 工地id
        /// </summary>
        public Guid? site_id { get; set; }
    }

    /// <summary>
    /// 设置完成进度dto
    /// </summary>
    public class SetProgressDto
    {
        /// <summary>
        /// 工地工作记录id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 完成百分比
        /// </summary>
        public decimal percentage { get; set; }
    }

    public class CheckCodeSetting
    {
        /// <summary>
        /// Briefing Record
        /// </summary>
        public List<int> brf_setting { get; set; } = [];

        /// <summary>
        /// Safety Working Cycle
        /// </summary>
        public List<int> scr_setting { get; set; } = [];

        /// <summary>
        /// CPDAS
        /// </summary>
        public List<int> cpd_setting { get; set; } = [];

        /// <summary>
        /// Quality Checklist
        /// </summary>
        public List<int> qdc_setting { get; set; } = [];

        /// <summary>
        /// SIC CP(T)
        /// </summary>
        public List<int> cp_setting { get; set; } = [];

        /// <summary>
        /// SIC
        /// </summary>
        public List<int> sic_setting { get; set; } = [];



    }
}