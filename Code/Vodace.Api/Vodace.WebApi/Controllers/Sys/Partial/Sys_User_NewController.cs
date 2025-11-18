
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.CacheManager;
using Vodace.Core.Configuration;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Filters;
using Vodace.Core.ManageUser;
using Vodace.Core.ObjectActionValidator;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Entity.DomainModels.Common;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_User_NewController
    {
        private readonly ISys_User_NewService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ICacheService _cache;
        private ISys_User_NewRepository _userNewRepository;
        private readonly ApiVersionConfig _versionConfig;


        [ActivatorUtilitiesConstructor]
        public Sys_User_NewController(
            ISys_User_NewService service,
            IHttpContextAccessor httpContextAccessor,
            ICacheService cache,
            ISys_User_NewRepository userNewRepository,
            IOptionsMonitor<ApiVersionConfig> versionConfig)
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _userNewRepository = userNewRepository;
            _versionConfig = versionConfig.CurrentValue; 
        }

        [HttpPost, HttpGet, Route("login"), AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [ObjectModelValidatorFilter(ValidatorModel.Login)]
        public async Task<IActionResult> Login([FromBody] LoginInfo loginInfo)
        {
            return Json(await _service.Login(loginInfo));
        }

        private readonly ConcurrentDictionary<int, object> _lockCurrent = new ConcurrentDictionary<int, object>();
        [HttpPost, Route("replaceToken")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ReplaceToken()
        {
            WebResponseContent responseContent = new WebResponseContent();
            string error = "";
            string key = $"rp:token:{UserContext.Current.UserId}";
            UserInfo userInfo = null;
            try
            {
                //如果5秒内替换过token,直接使用最新的token(防止一个页面多个并发请求同时替换token导致token错位)
                if (_cache.Exists(key))
                {
                    return Json(responseContent.OK(null, _cache.Get(key)));
                }
                var _obj = _lockCurrent.GetOrAdd(UserContext.Current.UserId, new object() { });
                lock (_obj)
                {
                    if (_cache.Exists(key))
                    {
                        return Json(responseContent.OK(null, _cache.Get(key)));
                    }
                    string requestToken = HttpContext.Request.Headers[AppSetting.TokenHeaderName];
                    requestToken = requestToken?.Replace("Bearer ", "");

                    if (JwtHelper.IsExp(requestToken)) return Json(responseContent.Error("Token已过期!"));

                    int userId = UserContext.Current.UserId;

                    userInfo = _userNewRepository.FindAsIQueryable(x => x.user_id == userId).Select(
                             s => new UserInfo()
                             {
                                 User_Id = userId,
                                 UserName = s.user_name,
                                 UserTrueName = s.user_true_name,
                                 UserNameEng = s.user_name_eng,
                                 Role_Id = s.role_id,
                             }).FirstOrDefault();

                    if (userInfo == null) return Json(responseContent.Error("未查到用户信息!"));

                    string token = JwtHelper.IssueJwt(userInfo);
                    var data = new { token, expMinutes = AppSetting.ExpMinutes };
                    //移除当前缓存
                    _cache.Remove(userId.GetUserIdKey());
                    //只更新的token字段
                    _userNewRepository.Update(new Sys_User() { User_Id = userId, Token = token }, x => x.Token, true);
                    //添加一个5秒缓存
                    _cache.Add(key, token, 5);
                    responseContent.OK(null, data);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message + ex.StackTrace;
                responseContent.Error("token替换异常");
            }
            finally
            {
                _lockCurrent.TryRemove(UserContext.Current.UserId, out object val);
                string _message = $"用户{userInfo?.User_Id}_{userInfo?.UserTrueName},({(responseContent.status ? "token替换成功" : "token替换失败")})";
                Log4NetHelper.Info(LoggerType.ReplaceToeken.ToString());
            }
            return Json(responseContent);
        }

        [HttpPost, Route("ModifyPwd"), ApiActionPermission]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ModifyPwd([FromBody] PwdInfo info)
        {
            return Json(await _service.ModifyPwd(info.old_pwd, info.new_pwd));
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost, Route("GetListByPage"), ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody] PageInput<UserQuery> query)
        {
            return Json(await _service.GetListByPage(query));
        }
        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("GetUserById"),ApiActionPermission]
        public IActionResult GetUserById(int id) 
        {
            return Json( _service.GetUserById(id));
        }

        [HttpPost, Route("getCurrentUserInfo")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            return Json(await _service.GetCurrentUserInfo());
        }

        //只能超级管理员才能修改密码
        //2020.08.01增加修改密码功能
        //[HttpPost, Route("modifyUserPwd"), ApiActionPermission(ActionRolePermission.SuperAdmin)]
        [HttpPost, Route("modifyUserPwd"), ApiActionPermission(ActionPermissionOptions.Add | ActionPermissionOptions.Update)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ModifyUserPwd([FromBody] LoginInfo loginInfo)
        {
            string userName = loginInfo?.UserName;
            string password = loginInfo?.Password;
            WebResponseContent webResponse = new WebResponseContent();
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(userName))
            {
                return Json(webResponse.Error("参数不完整"));
            }
            if (password.Length < 6) return Json(webResponse.Error("密码长度不能少于6位"));
            Sys_User_New user = _userNewRepository.FindFirst(x => x.user_name == userName);
            if (user == null)
            {
                return Json(webResponse.Error("用户不存在"));
            }
            user.user_pwd = password.EncryptDES(AppSetting.Secret.User);
            _userNewRepository.Update(user, x => new { x.user_pwd }, true);
            //如果用户在线，强制下线
            UserContext.Current.LogOut(user.user_id);
            return Json(webResponse.OK("密码修改成功"));
        }

        /// <summary>
        /// 增加登陆验证码
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet, Route("getVierificationCode"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetVierificationCode()
        {
            string code = VierificationCode.RandomText();
            var data = new
            {
                img = VierificationCode.CreateBase64Imgage(code),
                uuid = Guid.NewGuid()
            };
            HttpContext.GetService<IMemoryCache>().Set(data.uuid.ToString(), code, new TimeSpan(0, 5, 0));
            return Json(WebResponseContent.Instance.OK("Ok", data));
        }

        [ApiActionPermission()]
        public override IActionResult Upload(IEnumerable<IFormFile> fileInput)
        {
            return base.Upload(fileInput);
        }
        [HttpPost, Route("updateUserInfo")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdateUserInfo([FromBody] Sys_User user)
        {
            user.User_Id = UserContext.Current.UserId;
            _userNewRepository.Update(user, x => new { x.UserTrueName, x.Gender, x.Remark }, true);
            return Content("修改成功");
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut,Route("EditData"),ApiActionPermission]
        public IActionResult EditData([FromBody]UserDto user) 
        {
            return Json(_service.EditData(user));
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelData"), ApiActionPermission]
        public IActionResult EditData(int[] ids)
        {
            return Json(_service.DelData(ids));
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        [HttpPut,Route("ModifyUserPwd"),ApiActionPermission]
        public IActionResult ModifyUserPwd([FromBody] UserPwdInfo userPwd) 
        {
            return Json(_service.ModifyUserPwd(userPwd));
        }
        /// <summary>
        /// 用户切换语言(0：中文简体；1：中文繁体；2：英语)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut, Route("SwitchLang"), ApiActionPermission]
        public IActionResult  SwitchLang([FromBody]SwitchLangDto dto) 
        {
            return Json(_service.SwitchLang(dto));
        }
        /// <summary>
        /// 用户启用/禁用
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="enable">0:禁用，1：启用</param>
        /// <returns></returns>
        [HttpPut, Route("SwitchEnable"), ApiActionPermission]
        public IActionResult SwitchEnable(int id, int enable) 
        {
            return Json(_service.SwitchEnable(id, enable));
        }
        /// <summary>
        /// 打卡机测试
        /// </summary>
        /// <param name="ClockIn"></param>
        /// <returns></returns>
        [HttpPost, Route("ClockInTest"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ClockInTest([FromBody] ClockIn clockIn)
        {
            var json = JsonConvert.SerializeObject(clockIn);
            Log4NetHelper.Info($"打卡机测试:{json}");
            return Json("Test Success...");
        }

        [HttpPost, Route("upRecord"), AllowAnonymous]
        public IActionResult upRecord([FromBody] AttendanceRecordDto1 recordDto)
        {
            return Json("上传成功");
        }

        [HttpGet, Route("GetUserAllName"), AllowAnonymous]
        public async Task<IActionResult> GetUserAllName()
        {
            return Json(await _service.GetUserAllName());
        }
    }
}
