/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
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
    
    public partial class Sys_Department
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class DepartmentDto
    {
        /// <summary>
        /// 上级数据id
        /// </summary>
        public Guid? master_id { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string name_eng { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string name_cht { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string name_sho { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string name_ali { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

    public class DepartmentEditDto: DepartmentDto
    {
        /// <summary>
        /// 是否启用（1：启用；0：禁用）
        /// </summary>
        public int enable { get; set; }
        public Guid? id { get; set; }
    }

    public class DepartmentEnableDto
    {
        /// <summary>
        /// 是否启用（1：启用；0：禁用）
        /// </summary>
        public int enable { get; set; }
        public Guid id { get; set; }
    }

    public class DepartmentListDto : Sys_Department
    {
        /// <summary>
        /// 上级部门英文名称
        /// </summary>
        public string master_name_eng { get; set; }
        /// <summary>
        /// 上级部门中文名称
        /// </summary>
        public string master_name_cht { get; set; }
        /// <summary>
        /// 上级部门简称
        /// </summary>
        public string master_name_sho { get; set; }
        /// <summary>
        /// 上级部门别名
        /// </summary>
        public string master_name_ali { get; set; }
    }




    public class Department_Old_Dto 
    {
        public Guid id { get; set; }

        /// <summary>
        ///部门名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///部门编号
        /// </summary>
        public string code { get; set; }

        /// <summary>
        ///上级部门
        /// </summary>
        public Guid? parent_id { get; set; }

        /// <summary>
        ///部门类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        ///是否启用
        /// </summary>
        public int? enable { get; set; }

        public string remark { get; set; }
    }
}