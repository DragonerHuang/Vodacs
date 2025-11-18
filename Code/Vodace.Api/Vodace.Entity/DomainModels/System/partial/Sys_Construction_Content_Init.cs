
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
    
    public partial class Sys_Construction_Content_Init
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class ConstructionContentInitDto
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int? line_number { get; set; }
        /// <summary>
        /// 内容所属级别（根据内容编号判断）
        /// </summary>
        /// <remarks>
        /// 如：
        /// 1：一级目录
        /// 1.1：二级目录
        /// 1.1.1：三级目录
        /// </remarks>
        public int? level { get; set; }
        /// <summary>
        /// 内容所属上级id
        /// </summary>
        public Guid? master_id { get; set; }
        /// <summary>
        /// 内容外链id
        /// </summary>
        public Guid? external_link_id { get; set; }
        /// <summary>
        /// 内容编号
        /// </summary>
        public string item_code { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 所属工程类型
        /// </summary>
        /// <remarks>
        /// sheetname
        /// </remarks>
        public string work_type { get; set; }
        /// <summary>
        /// 检测类型
        /// </summary>
        /// <remarks>
        /// Check Point
        /// Whole Point
        /// Daily Point
        /// </remarks>
        public string point_type { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        /// <remarks>
        /// Site Work扩展属性：{"chief":"总监","manager":"管理","main_staff":"主要工作人员","security_deparment":"安全部","cp_wpic":"CP&WPIC","judge":"判头","judge_manage":"判头管理","auto":""}
        /// </remarks>
        public string extend_attr { get; set; }
    }

    public class ConstructionContentInitEditDto: ConstructionContentInitDto
    {
        public Guid? id { get; set; }
    }

    public class ConstructionContentInitListDto: ConstructionContentInitEditDto
    {
        public int? create_id { get; set; }
        public string create_name { get; set; }
        public DateTime? create_date { get; set; }
        public int? modify_id { get; set; }
        public string modify_name { get; set; }
        public DateTime? modify_date { get; set; }
    }
}