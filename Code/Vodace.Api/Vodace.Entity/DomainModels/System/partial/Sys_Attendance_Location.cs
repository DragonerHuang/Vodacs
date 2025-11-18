
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
    
    public partial class Sys_Attendance_Location
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    /// <summary>
    /// 打开地点查询条件
    /// </summary>
    public class LocationSearchDto
    {
        public string location_name { get; set; }
    }

    /// <summary>
    /// 添加
    /// </summary>
    public class LocationInputDto
    {
        public string location_name { get; set; }
    }

    /// <summary>
    /// 编辑
    /// </summary>
    public class LocationEditDto: LocationInputDto
    {
        public Guid id { get; set; }
    }

    /// <summary>
    /// 地点数据
    /// </summary>
    public class LocationDataDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// 地点
        /// </summary>
        public string location_name { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public int? create_id { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string create_name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_date { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        public int? modify_id { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string modify_name { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modify_date { get; set; }
    }
}