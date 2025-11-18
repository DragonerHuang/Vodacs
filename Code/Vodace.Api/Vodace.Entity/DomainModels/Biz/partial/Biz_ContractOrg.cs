
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
    
    public partial class Biz_ContractOrg
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class ContractOrgDto
    {
        public Guid? id { get; set; }
        /// <summary>
        /// 上级数据id
        /// </summary>
        public Guid? master_id { get; set; }
        /// <summary>
        /// 合约id
        /// </summary>
        public Guid? contract_id { get; set; }
        /// <summary>
        /// 组织id
        /// </summary>
        public Guid? org_id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// 提交文件编码
        /// </summary>
        public string submit_file_code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 特殊职位（一人之下万人之上）
        /// </summary>
        public int is_special { get; set; }
    }

    public class ContractOrgListDto : Biz_ContractOrg
    {
        /// <summary>
        /// 合约中文名称
        /// </summary>
        public string contract_name_cht { get; set; }
        /// <summary>
        /// 合约英文名称
        /// </summary>
        public string contract_name_eng { get; set; }
        /// <summary>
        /// 组织中文名称
        /// </summary>
        public string org_name_cht { get; set; }
        /// <summary>
        /// 组织英文名称
        /// </summary>
        public string org_name_eng { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 用户联系人中文名称
        /// </summary>
        public string user_contact_name_cht { get; set; }
        /// <summary>
        /// 用户联系人英文名称
        /// </summary>
        public string user_contact_name_eng { get; set; }
    }


    public class ContractOrgSearchDto
    {
        public Guid? id { get; set; }
        /// <summary>
        /// 上级数据id
        /// </summary>
        public Guid? master_id { get; set; }
        /// <summary>
        /// 合约id
        /// </summary>
        public Guid? contract_id { get; set; }
        /// <summary>
        /// 组织id
        /// </summary>
        public Guid? org_id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// 提交文件编码
        /// </summary>
        public string submit_file_code { get; set; }

        /// <summary>
        /// 返回数据结构是否为Tree类型 1-是 0-否
        /// </summary>
        public int is_tree { get; set; }
    }
}