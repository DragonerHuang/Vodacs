
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
    
    public partial class Sys_Company
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class CompanyDto 
    {
        public Guid id { get; set; }
        /// <summary>
        ///公司名称
        /// </summary>
        public string company_name { get; set; }
        public string company_name_eng { get; set; }
        public string license_no { get; set; }
        /// <summary>
        ///联系人
        /// </summary>
        public string contact { get; set; }
        /// <summary>
        ///联系人电话
        /// </summary>
        public string contact_phone { get; set; }
        /// <summary>
        ///联系人邮箱
        /// </summary>
        public string contact_email { get; set; }
      
    }

    public class CompanyListDto 
    {
        public Guid id { get; set; }
        public string company_name { get; set; }
        public string company_no { get; set; }
        public string license_no { get; set; }
        public string contact { get; set; }
        public string contact_phone { get; set; }
        public string contact_email { get; set; }
        public int status { get; set; }
        public string status_str { get; set; }
        public string audit_user { get; set; }
        public DateTime? audit_date { get; set; }
        public string audit_remark { get; set; }
        public string remark { get; set; }
        public string create_name { get; set; }
        public DateTime? create_date { get; set; }
        public int company_type { get; set; }
        public string company_name_eng { get; set; }
    }

    public class CompanyQuery 
    {
        public int? status { get; set; }
        public string company_name { get; set; }
        public string company_no { get; set; }
        public string company_name_eng { get; set; }
    }
    public class CompanyAuditDto() 
    {
        public Guid id { get; set; }
        public int status { get; set; }
        public string audit_user { get; set; }
        public string audit_remark { get; set; }
    }
}