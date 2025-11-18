
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Configuration;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_User_NewService
    {

        Task<WebResponseContent> Login(LoginInfo loginInfo, bool verificationCode = true);
        /// <summary>
        ///当token将要过期时，提前置换一个新的token
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> ReplaceToken();
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<WebResponseContent> ModifyPwd(string oldPwd, string newPwd);
        Task<WebResponseContent> GetListByPage(PageInput<UserQuery> query);
        WebResponseContent GetUserById(int id);
        /// <summary>
        /// 个人中心获取当前用户信息
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> GetCurrentUserInfo();
        WebResponseContent EditData(UserDto user);
        WebResponseContent DelData(int[] keys);
        WebResponseContent ModifyUserPwd(UserPwdInfo userPwd);
        WebResponseContent SwitchLang(SwitchLangDto dto);
        WebResponseContent SwitchEnable(int id, int enable);
        Task<WebResponseContent> GetUserAllName();
    }
 }
