
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
    
    public partial class Sys_Role
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class RoleDto
    {
        /// <summary>
        ///角色名称
        /// </summary>
        public string role_name { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public byte? enable { get; set; }
    }

    /// <summary>
    /// 修改角色
    /// </summary>
    public class EditRoleDto 
    {
        public int role_id { get; set; }
        /// <summary>
        ///角色名称
        /// </summary>
        public string role_name { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public byte? enable { get; set; }
    }


    public class RoleQuery 
    {
        public string role_name { get; set; }
        public byte? enable { get; set; }
    }

    public class RoleListDto 
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public string remark { get; set; }
        public byte? enable { get; set; }
        public DateTime? create_date { get; set; }
        public string create_name { get; set; }
        public string modify_name { get; set; }
        public DateTime? modify_date { get; set; }
    }
}