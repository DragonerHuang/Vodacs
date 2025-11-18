using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Entity.DomainModels
{
    public partial class Biz_Contact_Relationship
    {
    }

    public class ContactRelationshipAddDto
    {
        /// <summary>
        ///联系人关系ID
        /// </summary>
        public Guid? id { get; set; }
        /// <summary>
        ///联系人ID
        /// </summary>
        public Guid? contact_id { get; set; }

        /// <summary>
        ///角色id
        /// </summary>
        public int? role_id { get; set; }

        /// <summary>
        ///联络类型：0：qn联系人、1：公司联系人、2：组织架构，3：合约组织架构人员
        /// </summary>
        public int? relation_type { get; set; }

        /// <summary>
        ///对应的表ID
        /// </summary>
        public Guid? relation_id { get; set; }

        /// <summary>
        ///发送邮件类型，Gen，Pay ，Safe ，PD， ABWF ，EL， FS（多选，用,号隔开）
        /// </summary>
        public string mail_to { get; set; }

        /// <summary>
        ///公司ID
        /// </summary>
        public Guid? company_id { get; set; }

        public string name { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string title { get; set; }
        public Guid? department_id { get; set; }
        public string department_name { get; set; }
        public string role_name { get; set; }
        public Guid? org_id { get; set; }
    }

    public class ContactRelationshipListDto : ContactRelationshipAddDto
    {
        public string id_no { get; set; }
        public string name_sho { get; set; }
        public string name_eng { get; set; }
        public string name_cht { get; set; }
        public string name_ali { get; set; }
        public string address { get; set; }
        public string daily_salary { get; set; }
    }

    public class ContactRelationshipDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid? id { get; set; }

        /// <summary>
        ///公司ID
        /// </summary>
        public Guid? company_id { get; set; }

        /// <summary>
        ///联系人ID
        /// </summary>
        public Guid? contact_id { get; set; }

        /// <summary>
        ///角色id
        /// </summary>
        public int? role_id { get; set; }

        /// <summary>
        ///联络类型：0：qn联系人、1：公司联系人、2：组织架构，3：合约组织架构人员
        /// </summary>
        public int? relation_type { get; set; }

        /// <summary>
        ///发送邮件类型，Gen，Pay ，Safe ，PD， ABWF ，EL， FS（多选，用,号隔开）
        /// </summary>
        public string mail_to { get; set; }

        /// <summary>
        ///对应的表ID
        /// </summary>
        public Guid? relation_id { get; set; }
    }

    public class ContactInfoDto
    {
        /// <summary>
        /// 联系人Id
        /// </summary>
        public Guid? id { get; set; }

        /// <summary>
        ///联络类型：0：qn联系人、1：公司联系人、2：组织架构，3：合约组织架构人员
        /// </summary>
        public int? relation_type { get; set; }

        /// <summary>
        ///对应的表ID
        /// </summary>
        public Guid? relation_id { get; set; }
    }

    public class ContactRelationSearchAllDto : ContactInfoDto
    {
        /// <summary>
        /// 合约ID
        /// </summary>
        public Guid? contract_id { get; set; }
    }
}
