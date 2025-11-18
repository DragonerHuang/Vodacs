
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
    
    public partial class Biz_Site_Relationship
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }


    public class SiteRelationshipDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///关系id（0：contract_id，1：施工地点线路（1号线等））
        /// </summary>
        public Guid? relation_id { get; set; }

        /// <summary>
        ///现场位置id
        /// </summary>
        public Guid? site_id { get; set; }

        /// <summary>
        ///是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; } = 1;

        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }

        public DateTime? create_date { get; set; } = DateTime.Now;

        /// <summary>
        ///地点位置：0：合同资料施工工作地点，1：施工地点线路（1号线等）
        /// </summary>
        public int? relation_type { get; set; }
    }
}