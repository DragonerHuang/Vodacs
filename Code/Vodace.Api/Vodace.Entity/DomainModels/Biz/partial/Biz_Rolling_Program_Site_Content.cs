
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
    
    public partial class Biz_Rolling_Program_Site_Content
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class RollingProgramSiteContentDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid? id { get; set; }
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
        /// 工地id
        /// </summary>
        public Guid? site_id { get; set; }
        /// <summary>
        /// Sys_Construction_Content_Init.id
        /// </summary>
        public Guid? cc_id { get; set; }
        /// <summary>
        /// 内容编码
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
        /// 数量
        /// </summary>
        public int number { get; set; }
        /// <summary>
        /// 是否生成任务 1-是 0-否
        /// </summary>
        public int is_generate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

    public class RollingProgramSiteContentAddDto
    {
        public RollingProgramTaskDto rollingProgramTaskDto { get; set; }
        public List<RollingProgramSiteContentDto> rollingProgramSiteContentDtos { get; set; }
    }
    public class RollingProgramSiteContentSearchDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid? id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid? project_id { get; set; }
        /// <summary>
        /// 合约id
        /// </summary>
        public Guid? contract_id { get; set; }
        /// <summary>
        /// 工地id
        /// </summary>
        public Guid? site_id { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public string customer { get; set; }
        /// <summary>
        /// 工程类型
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 滚动计划任务名称
        /// </summary>
        public string task_name { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }
    }
}