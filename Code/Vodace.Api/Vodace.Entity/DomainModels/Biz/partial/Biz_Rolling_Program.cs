
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
    
    public partial class Biz_Rolling_Program
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class RollingProgramDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? project_id { get; set; }
        /// <summary>
        /// 合约id
        /// </summary>
        public Guid? contract_id { get; set; }
        /// <summary>
        /// Biz_Rolling_Program_Task.id
        /// </summary>
        public Guid? task_id { get; set; }
        /// <summary>
        /// Biz_Rolling_Program_Site_Content.id
        /// </summary>
        public Guid? sc_id { get; set; }
        /// <summary>
        /// Sys_Construction_Content_Init.id
        /// </summary>
        public Guid? cc_id { get; set; }
        /// <summary>
        /// 组织ids
        /// </summary>
        public string org_id { get; set; }
        /// <summary>
        /// 工地ids，多个之间以逗号之间分割
        /// </summary>
        public string site_id { get; set; }
        /// <summary>
        ///内容级别：1级、2级、3级，目前最高三级数据
        /// </summary>
        public int? level { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        public Guid? master_id { get; set; }
        /// <summary>
        /// 内容编号
        /// </summary>
        public string item_code { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 工作类型
        /// </summary>
        public string work_type { get; set; }
        /// <summary>
        /// 节点类型
        /// </summary>
        public string point_type { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? start_date { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? end_date { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public int? director { get; set; }
        /// <summary>
        ///相差报价细分金额
        /// </summary>
        public decimal? quotation { get; set; }
        /// <summary>
        ///值更：早班、中班、晚班
        /// </summary>
        public string duty { get; set; }
        /// <summary>
        ///色块
        /// </summary>
        public string color { get; set; }
        /// <summary>
        ///是否路轨范围工作，0-否 1-是
        /// </summary>
        public int? track_scope { get; set; }
        /// <summary>
        ///扩展类型：text,radio,checkbox,file....
        /// </summary>
        public string exp_type { get; set; }
        /// <summary>
        ///扩展类型值
        /// </summary>
        public string exp_value { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

    public class RollingProgramEditDto : RollingProgramDto
    {
        public Guid? id { get; set; }

        /// <summary>
        /// 为避免不会用的人添加的字段
        /// </summary>
        /// <remarks>避免过多的沟通成本</remarks>
        public List<int> lst_level { get; set; }

        /// <summary>
        /// 查找不存在的级别
        /// </summary>
        public List<int> lst_unlevel { get; set; }
    }
}