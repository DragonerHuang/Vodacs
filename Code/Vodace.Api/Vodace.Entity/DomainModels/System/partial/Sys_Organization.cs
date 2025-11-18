
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
    
    public partial class Sys_Organization
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
        /// <summary>
        /// 合约id
        /// </summary>
        //public Guid ContractId { get; set; }
    }

    public class OrganizationDto
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

    public class OrganizationEditDto : OrganizationDto
    {
        /// <summary>
        /// 是否启用（1：启用；0：禁用）
        /// </summary>
        public int enable { get; set; }
        public Guid? id { get; set; }
    }

    public class OrganizationEnableDto
    {
        /// <summary>
        /// 是否启用（1：启用；0：禁用）
        /// </summary>
        public int enable { get; set; }
        public Guid id { get; set; }
    }

    public class OrganizationListDto : Sys_Organization
    {
        /// <summary>
        /// 上级组织英文名称
        /// </summary>
        public string master_name_eng { get; set; }
        /// <summary>
        /// 上级组织中文名称
        /// </summary>
        public string master_name_cht { get; set; }
        /// <summary>
        /// 上级组织简称
        /// </summary>
        public string master_name_sho { get; set; }
        /// <summary>
        /// 上级组织别名
        /// </summary>
        public string master_name_ali { get; set; }
    }

}