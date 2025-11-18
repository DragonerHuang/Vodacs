
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

    public partial class Sys_User_Register
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class UserRegisterDto 
    {
        public Sys_User_Register register { get; set; }
        public Sys_Company company { get; set; }
        public Sys_Worker_Register worker { get; set; }
        public Guid[] FileIds { get; set; }
    }

    public class UserRegister
    {
        public Guid Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Name_Cht { get; set; }
        public string Name_Eng { get; set; }
        public string ID_No { get; set; }
        /// <summary>
        ///电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        ///性别 0：女；1：男
        /// </summary>
        public int Gender { get; set; }

        public int? Enable { get; set; } = 1;

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int? CreateID { get; set; }
        public string Creator { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? ModifyID { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 0:公司；1：一般个人；2：施工工人
        /// </summary>
        public int UserType { get; set; }
        public Company company { get; set; }
        public Worker worker { get; set; }
    }

    public class Company 
    {
        /// <summary>
        ///公司名称
        /// </summary>
        public string Com_Name { get; set; }

        /// <summary>
        ///执照编号
        /// </summary>
        public string Com_No { get; set; }

        /// <summary>
        ///联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        ///联系人电话
        /// </summary>
        public string Contact_Phone { get; set; }

        /// <summary>
        ///联系人邮箱
        /// </summary>
        public string Contact_Email { get; set; }
    }

    public class Worker 
    {
        public string Stc_No { get; set; }

        /// <summary>
        ///签发日期
        /// </summary>
        public DateTime Stc_Issued_Start_Date { get; set; }

        /// <summary>
        ///有效日期
        /// </summary>
        public DateTime Stc_Issued_End_Date { get; set; }

        /// <summary>
        ///工人注册证
        /// </summary>
        public string Wrc_No { get; set; }

        /// <summary>
        ///签发日期
        /// </summary>
        public DateTime Wrc_Issued_Start_Date { get; set; }

        /// <summary>
        ///有效日期
        /// </summary>
        public DateTime Wrc_Issued_End_Date { get; set; }

        /// <summary>
        ///专业工种
        /// </summary>
        public Guid Work_Type { get; set; }
        public Guid[] FileIds { get; set; }
    }

    #region 9.9变更需求后
    /// <summary>
    /// 用户基本信息
    /// </summary>
    public class UserBaseDto
    {
        public string name_cht { get; set; }
        public string name_eng { get; set; }
        public string user_phone { get; set; }
        public string id_no { get; set; }
        public string user_email { get; set; }
        public int user_gender { get; set; }
    }
    public class UserRegisterNewDto 
    {
        public UserAllDto user { get; set; }
        public WorkerDto worker { get; set; }
        public Guid[] FileIds { get; set; }
    }
    /// <summary>
    /// 用户所有信息
    /// </summary>
    public class UserAllDto
    {
        public string user_account { get; set; }
        public string user_password { get; set; }
        public string name_cht { get; set; }
        public string name_eng { get; set; }
        public string user_phone { get; set; }
        public string id_no { get; set; }
        public string user_email { get; set; }
        public int user_gender { get; set; }
        public string company_no { get; set; }
        /// <summary>
        /// 来源（0：平台；1：APP）
        /// </summary>
        public int source { get; set; }
    }

    public partial class WorkerDto
    {
        /// <summary>
        ///建造行业安全训练证明书
        /// </summary>
        public string stc_no { get; set; }

        /// <summary>
        ///签发日期
        /// </summary>
        public DateTime? stc_issued_start_date { get; set; }

        /// <summary>
        ///有效日期
        /// </summary>
        public DateTime? stc_issued_end_date { get; set; }

        /// <summary>
        ///工人注册证
        /// </summary>
        public string wrc_no { get; set; }

        /// <summary>
        ///签发日期
        /// </summary>
        public DateTime? wrc_issued_start_date { get; set; }

        /// <summary>
        ///有效日期
        /// </summary>
        public DateTime? wrc_issued_end_date { get; set; }

        /// <summary>
        ///专业工种
        /// </summary>
        public Guid? work_type { get; set; }
    }
    #endregion

}