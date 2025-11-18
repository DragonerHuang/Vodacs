
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
    
    public partial class Sys_User_New
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class ClockIn
    {
        public string cmd { get; set; }
        public string sn { get; set; }
        public string time { get; set; }
    }
    public class UserQuery
    {
        public string user_name { get; set; }
        public string phone_no { get; set; }
        public int? enable { get; set; }
        public int role_id { get; set; }
    }

    public class UserDto 
    {
        public Guid id { get; set; }
        public int user_id { get; set; }
        public Guid? company_id { get; set; }
        public string company_name { get; set; }
        public string company_name_eng { get; set; }
        public Guid? contact_id { get; set; }
        public string contact_name { get; set; }
        public string contact_name_eng { get; set; }
        public string contact_name_sho { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
        public string phone_no { get; set; }
        public string remark { get; set; }
        public string user_name { get; set; }
        public string user_true_name { get; set; }
        public string user_name_eng { get; set; }
        public int? lang { get; set; }
        public string email { get; set; }
        public byte enable { get; set; }
        public int? gender { get; set; }
    }

    public class UserListDto 
    {
        public Guid id { get; set; }
        public int user_id { get; set; }
        public Guid? company_id { get; set; }
        public string company_no { get; set; }
        public string company_name { get; set; }
        public Guid? contact_id { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
        public string phone_no { get; set; }
        public string remark { get; set; }
        public string user_name { get; set; }
        public string user_true_name { get; set; }
        public string user_name_eng { get; set; }
        public int? lang { get; set; }
        public string email { get; set; }
        public byte enable { get; set; }
        public int? gender { get; set; }
        public DateTime? last_login_date { get; set; }
        public string address { get; set; }
        //public string token { get; set; }
        //public int? delete_status { get; set; }
        public string create_name { get; set; }
        public DateTime? create_date { get; set; }
        //public string modify_name { get; set; }
        //public DateTime? modify_date { get; set; }
    }

}