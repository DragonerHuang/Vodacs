
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
    
    public partial class Sys_User_Relation
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class SysUserRelationDto
    {
        public Guid? id { get; set; }
        public Guid? user_register_Id { get; set; }
        public Guid? relation_id { get; set; }
        public int relation_type { get; set; }
        public decimal? day_salary { get; set; }
        public decimal? night_salary { get; set; }
        public string remark { get; set; }
    }

    public class SysUserRelationSearchDto : SysUserRelationDto
    {
        public string user_name_eng { get; set; }
        public string user_true_name { get; set; }
        public string user_name { get; set; }
        public string type_name { get; set; }
    }
}