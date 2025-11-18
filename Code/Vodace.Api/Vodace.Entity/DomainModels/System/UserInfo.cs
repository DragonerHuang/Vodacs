using Vodace.Entity.SystemModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Entity.DomainModels
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public int User_Id { get; set; }
        /// <summary>
        /// 多个角色ID
        /// </summary>
        public int Role_Id { get; set; }
        public Guid? Company_Id { get; set; }
        public Guid? Contact_Id { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string UserNameEng { get; set; }
        public string UserTrueName { get; set; }
        public int  Enable { get; set; }
        public int? Lang { get; set; }
        public int? Gender { get; set; }
       public bool IsSuperAdmin { get; set; }
        public string LoginIp { get; set; }
        public string Token { get; set; }
    }

    public class UserInfoNew
    {
        public Guid User_Id { get; set; }
        public int Role_Id { get; set; }
        public string Account { get; set; }
        public string UserName { get; set; }
        public int Enable { get; set; }
        public Guid Com_Id { get; set; }

        public string Token { get; set; }
    }
}
