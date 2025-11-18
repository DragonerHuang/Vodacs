
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
    
    public partial class Sys_Site_Work_Check_Item
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    /// <summary>
    /// 检查项分页查询条件
    /// </summary>
    public class SiteWorkCheckItemSearchDto
    {
        /// <summary>
        /// 全局编码（前缀匹配）
        /// </summary>
        public string global_code { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int? level { get; set; }

        /// <summary>
        /// 主层级id
        /// </summary>
        public int? root_id { get; set; }

        /// <summary>
        /// 所属父层级（item_code）
        /// </summary>
        public int? master_id { get; set; }

        /// <summary>
        /// 名称（中英文模糊查询）
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    /// 检查项查询输出DTO
    /// </summary>
    public class SiteWorkCheckItemDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 行级代码
        /// </summary>
        public int item_code { get; set; }

        /// <summary>
        /// 主层级id
        /// </summary>
        public int? root_id { get; set; }

        /// <summary>
        /// 父层级（item_code）
        /// </summary>
        public int? master_id { get; set; }

        /// <summary>
        /// 业务全局编码
        /// </summary>
        public string global_code { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        /// 排序列
        /// </summary>
        public int? order_no { get; set; }

        /// <summary>
        /// 是否启用（0：否；1：是）
        /// </summary>
        public byte? enable { get; set; }
    }

    /// <summary>
    /// 检查项新增输入DTO
    /// </summary>
    public class SiteWorkCheckItemAddDto
    {
        /// <summary>
        /// 层级
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// 主层级id
        /// </summary>
        public int? root_id { get; set; }

        /// <summary>
        /// 父层级（item_code）
        /// </summary>
        public int? master_id { get; set; }

        /// <summary>
        /// 业务全局编码
        /// </summary>
        public string global_code { get; set; }

        /// <summary>
        /// 类型标识，不传默认0
        /// </summary>
        public int? type_id { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        /// 排序列
        /// </summary>
        public int? order_no { get; set; }

        /// <summary>
        /// 是否启用（0：否；1：是）
        /// </summary>
        public byte? enable { get; set; }
    }

    /// <summary>
    /// 检查项修改输入DTO
    /// </summary>
    public class SiteWorkCheckItemEditDto: SiteWorkCheckItemAddDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid id { get; set; }
    }
}
