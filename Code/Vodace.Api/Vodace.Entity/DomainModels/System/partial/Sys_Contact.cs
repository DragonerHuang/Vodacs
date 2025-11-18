
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
    
    public partial class Sys_Contact
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class ContactQuery
    {
        public Guid? company_id { get; set; }
        public string name_cht { get; set; }
    }
    public class ContactDto 
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///Sys_Company、所属公司\一般个人
        /// </summary>
        public Guid? company_id { get; set; }

        /// <summary>
        ///组织
        /// </summary>
        public Guid? organization_id { get; set; }

        /// <summary>
        ///身份证
        /// </summary>
        public string id_no { get; set; }

        /// <summary>
        ///简称
        /// </summary>
        public string name_sho { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        ///别名
        /// </summary>
        public string name_ali { get; set; }

        /// <summary>
        ///所属部门
        /// </summary>
        public Guid? department_id { get; set; }

        /// <summary>
        ///头衔、职称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        ///‌行政区id
        /// </summary>
        public Guid? district_id { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///工人日薪
        /// </summary>
        public decimal? daily_salary { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int? delete_status { get; set; } = 1;
        public DateTime? create_date { get; set; } = DateTime.Now;

        public string tel { get; set; }
        public string fax { get; set; }

        public List<Site> children { get; set; }
    }

    public class Site
    {
        public Guid id { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string name_cht { get; set; }
    }

    public class ContactByQnDto
    {
        /// <summary>
        /// 报价编码
        /// </summary>
        public string qn_no { get; set; }

        /// <summary>
        ///所属部门
        /// </summary>
        public Guid? department_id { get; set; }

        /// <summary>
        ///组织
        /// </summary>
        public Guid? organization_id { get; set; }

        /// <summary>
        ///头衔、职称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        ///电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        ///传真
        /// </summary>
        public string fax { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string email { get; set; }
    }

    public class ContactByQnEditDto : ContactByQnDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }
    }


    public class ContactEditlDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///Sys_Company、所属公司\一般个人
        /// </summary>
        public Guid? company_id { get; set; }

        /// <summary>
        ///组织
        /// </summary>
        //public Guid? organization_id { get; set; }

        /// <summary>
        ///身份证
        /// </summary>
        public string id_no { get; set; }

        /// <summary>
        ///简称
        /// </summary>
        public string name_sho { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        ///别名
        /// </summary>
        public string name_ali { get; set; }

        /// <summary>
        ///所属部门
        /// </summary>
        //public Guid? department_id { get; set; }

        /// <summary>
        ///头衔、职称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        ///‌行政区id
        /// </summary>
        //public Guid? district_id { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///工人日薪
        /// </summary>
        //public decimal? daily_salary { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        ///电话
        /// </summary>
        public string user_phone { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string user_account { get; set; }
        /// <summary>
        ///性别 0：女；1：男
        /// </summary>
        public int? user_gender { get; set; }

        /// <summary>
        ///传真
        /// </summary>
        public string fax { get; set; }
    }

    public class ContactDetailDto
    {
        /// <summary>
        ///
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        ///Sys_Company、所属公司\一般个人
        /// </summary>
        public Guid? company_id { get; set; }

        /// <summary>
        ///组织
        /// </summary>
        //public Guid? organization_id { get; set; }

        /// <summary>
        ///身份证
        /// </summary>
        public string id_no { get; set; }

        /// <summary>
        ///简称
        /// </summary>
        public string name_sho { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        ///别名
        /// </summary>
        public string name_ali { get; set; }

        /// <summary>
        ///所属部门
        /// </summary>
        //public Guid? department_id { get; set; }

        /// <summary>
        ///头衔、职称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        ///‌行政区id
        /// </summary>
        //public Guid? district_id { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///工人日薪
        /// </summary>
        //public decimal? daily_salary { get; set; }

        /// <summary>
        ///是否删除（0：是；1：否）
        /// </summary>
        //public int? delete_status { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime? create_date { get; set; }

        /// <summary>
        ///电话
        /// </summary>
        public string user_phone { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string user_account { get; set; }
        /// <summary>
        ///性别 0：女；1：男
        /// </summary>
        public int? user_gender { get; set; }

        /// <summary>
        ///传真
        /// </summary>
        public string fax { get; set; }
    }

    public class ContactListDto : ContactDetailDto
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string strCompanyName { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string strOrganizationName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string strDepartmentName { get; set; }
        /// <summary>
        /// 行政区名称(英文)
        /// </summary>
        public string strDistrictNameEng { get; set; }
        /// <summary>
        /// 行政区名称(中文)
        /// </summary>
        public string strDistrictNameCht { get; set; }
    }

    public class SearchContactDto
    {
        /// <summary>
        ///简称
        /// </summary>
        public string name_sho { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        ///别名
        /// </summary>
        public string name_ali { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        ///传真
        /// </summary>
        public string fax { get; set; }

        /// <summary>
        ///组织名称
        /// </summary>
        public string strOrganizationName { get; set; }

        /// <summary>
        ///所属部门名称
        /// </summary>
        public string strDepartmentName { get; set; }

    }

    public class ContactWithCardDto
    {
        /// <summary>
        /// 联系人id
        /// </summary>
        public Guid contact_id { get; set; }

        /// <summary>
        /// 联系人中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        /// 联系人英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        /// 绿卡号
        /// </summary>
        public string green_card_no { get; set; }

        /// <summary>
        /// 绿卡签发日期
        /// </summary>
        public string green_card_start { get; set; }

        /// <summary>
        /// 绿卡有效日期
        /// </summary>
        public string green_card_exp { get; set; }

        /// <summary>
        /// CIC号
        /// </summary>
        public string cic_card_no { get; set; }

        /// <summary>
        /// CIC签发日期
        /// </summary>
        public string cic_card_start { get; set; }

        /// <summary>
        /// CIC有效日期
        /// </summary>
        public string cic_card_exp { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? create_time { get; set; }
    }

    public class SearchContactWithCardDto
    {
        public string name { get; set; }
    }

}