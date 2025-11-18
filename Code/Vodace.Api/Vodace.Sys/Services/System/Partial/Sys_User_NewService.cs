
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Configuration;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Hubs;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Services;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;
using Vodace.Sys.Repositories;

namespace Vodace.Sys.Services
{
    public partial class Sys_User_NewService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_User_NewRepository _repository;//访问数据库
        private readonly ISys_RoleRepository _RoleRepository;
        private readonly ISys_CompanyRepository _CompanyRepository;
        private readonly ISys_ContactRepository _ContactRepository;
        private readonly IBiz_Contact_RelationshipRepository _ContactRelationshipRepository;
        private readonly IBiz_Contract_DetailsRepository _Contract_DetailsRepository;
        private readonly IBiz_Confirmed_OrderRepository _Confirmed_OrderRepository;
        private readonly IBiz_QuotationRepository _QuotationRepository;

        private Microsoft.AspNetCore.Http.HttpContext _context;
        private ISys_MenuRepository _menuRepository;
        private ISys_CompanyRepository _companyRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly IHomePageMessageSender _messageSender;
        [ActivatorUtilitiesConstructor]
        public Sys_User_NewService(IHttpContextAccessor httpContextAccessor, Sys_User_newRepository repository, ISys_CompanyRepository sys_Company, ISys_MenuRepository menuRepository, ILocalizationService localizationService, ISys_RoleRepository roleRepository, ISys_CompanyRepository companyRepository, IMapper mapper, ISys_ContactRepository contactRepository, IHomePageMessageSender messageSender, IBiz_Contact_RelationshipRepository contactRelationshipRepository, IBiz_Contract_DetailsRepository contract_DetailsRepository, IBiz_Confirmed_OrderRepository confirmed_OrderRepository, IBiz_QuotationRepository quotationRepository)
            : base(repository)
        {
            _context = httpContextAccessor.HttpContext;
            _repository = repository;
            _companyRepository = sys_Company;
            _localizationService = localizationService;
            _menuRepository = menuRepository;
            _RoleRepository = roleRepository;
            _CompanyRepository = companyRepository;
            _mapper = mapper;
            _ContactRepository = contactRepository;
            _messageSender = messageSender;
            _ContactRelationshipRepository = contactRelationshipRepository;
            _Contract_DetailsRepository = contract_DetailsRepository;
            _Confirmed_OrderRepository = confirmed_OrderRepository;
            _QuotationRepository = quotationRepository;
        }
        WebResponseContent webResponse = new WebResponseContent();
        /// <summary>
        /// WebApi登陆
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> Login(LoginInfo loginInfo, bool verificationCode = true)
        {
            string msg = string.Empty;
            try
            {
                if (loginInfo.source == 0) 
                {
                    IMemoryCache memoryCache = _context.GetService<IMemoryCache>();
                    string cacheCode = (memoryCache.Get(loginInfo.UUID) ?? "").ToString();
                    if (string.IsNullOrEmpty(cacheCode))
                    {
                        return webResponse.Error(_localizationService.GetString("veri_code_exp"));
                    }
                    if (cacheCode.ToLower() != loginInfo.VerificationCode.ToLower())
                    {
                        memoryCache.Remove(loginInfo.UUID);
                        return webResponse.Error(_localizationService.GetString("veri_code_err"));
                    }
                }
                #region 历史登录逻辑
                //Guid? companyId = Guid.Empty;
                //if (!string.IsNullOrEmpty(loginInfo.Com_No))
                //{
                //    companyId = _companyRepository.Find(d => d.company_no == loginInfo.Com_No && d.status == (int)AuditEnum.Passed).FirstOrDefault()?.id;
                //    if (companyId == null) return webResponse.Error(_localizationService.GetString("company_info_noexist"));
                //}
                //var user = await repository.FindAsIQueryable(x => (companyId == Guid.Empty || loginInfo.UserName.Equals("admin") ? true : x.company_id == companyId) && x.user_name == loginInfo.UserName)
                //    .FirstOrDefaultAsync();
                #endregion

                Guid? companyId = Guid.Empty;
                var user = new Sys_User_New();
                //var contact1 = _ContactRepository.Find(x => x.user_account == loginInfo.UserName).FirstOrDefault();
                var contact = _ContactRepository.Find(x => x.user_account == loginInfo.UserName && x.delete_status ==(int)SystemDataStatus.Valid).FirstOrDefault();
                if (contact != null)
                {
                    if (contact.company_id.HasValue && !string.IsNullOrEmpty(loginInfo.Com_No))
                    {
                        //有公司且输入了公司编号
                        companyId = _companyRepository.Find(d => d.company_no == loginInfo.Com_No && d.status == (int)AuditEnum.Passed).FirstOrDefault()?.id;
                        if (!companyId.HasValue) return webResponse.Error(_localizationService.GetString("company_info_noexist"));
                        else
                        {
                            //有公司的账户且当前已经绑定了公司
                            user = await repository.FindAsIQueryable(x =>  x.company_id == companyId && x.user_name == loginInfo.UserName).FirstOrDefaultAsync();
                        }
                    }
                    else if (contact.company_id.HasValue && string.IsNullOrEmpty(loginInfo.Com_No)) 
                    {
                        //4.有公司的账户未输入公司编号
                        if (companyId.HasValue) return webResponse.Error(_localizationService.GetString("company_no_required"));
                    }
                    else
                    {
                        //1.无公司的账户
                        user = await repository.FindAsIQueryable(x => x.user_name == loginInfo.UserName).FirstOrDefaultAsync();
                    }
                }

                else if(loginInfo.UserName.Equals("admin"))
                { 
                    user = await repository.FindAsIQueryable(x => x.user_name == loginInfo.UserName).FirstOrDefaultAsync();
                }

                //var user = await repository.FindAsIQueryable(x =>  (companyId == Guid.Empty || loginInfo.UserName.Equals("admin") ? true : x.company_id == companyId) && x.user_name == loginInfo.UserName).FirstOrDefaultAsync();
                if (user == null || string.IsNullOrEmpty(user.user_name))
                {
                    Log4NetHelper.Error("用户名不存在");
                    return webResponse.Error(_localizationService.GetString("user_no_exist"));
                }
                if (user.enable == (int)UserStatus.disable) 
                {
                    Log4NetHelper.Error("账号被禁用");
                    return webResponse.Error(_localizationService.GetString("account_disabled"));
                }
                if (loginInfo.Password.Trim().EncryptDES(AppSetting.Secret.User) != (user.user_pwd ?? ""))
                {
                    Log4NetHelper.Error("密码错误");
                    return webResponse.Error(_localizationService.GetString("incorrect_password"));
                }

                string UserIP = _context.GetUserIp()?.Replace("::ffff:", "");
                string ServiceIP = _context.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + _context.Connection.LocalPort;
                var res_data = new UserInfo()
                {
                    Id = user.id,
                    User_Id = user.user_id,
                    UserName = user.user_name,
                    Company_Id = user.company_id,
                    Role_Id = user.role_id,
                    Enable = user.enable,
                    UserTrueName = user.user_true_name,
                    UserNameEng = user.user_name_eng,
                    IsSuperAdmin = user.role_id ==1,
                    Contact_Id = user.contact_id,
                    Gender = user.gender,
                    Lang = user.lang,
                    LoginIp = UserIP,
                };
                string token = JwtHelper.IssueJwt(res_data);
                user.source = loginInfo.source;
                user.token = token;
                user.login_ip = UserIP;
                user.last_login_date = DateTime.Now;
                webResponse.data = new { token, userName = user.user_true_name, img = "", res_data,AppSetting.ExpMinutes };
                repository.Update(user, x => new { x.token,x.last_login_date,x.login_ip, x.source },  true);
                UserContext.Current.LogOut(user.user_id);
                loginInfo.Password = string.Empty;
                Log4NetHelper.Info($"{user.user_name}{_localizationService.GetString("login_success")}！");
                await _messageSender.SendMessageToUser(user.user_name,"系统消息", $"{user.user_name}登录成功");
                return webResponse.OK(_localizationService.GetString("login_success"), webResponse.data);
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                if (_context.GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>().IsDevelopment())
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                return webResponse.Error(_localizationService.GetString("login_failes"));
            }
            finally
            {
                //memoryCache.Remove(loginInfo.UUID);
                Logger.Info(LoggerType.Login, loginInfo.Serialize(), webResponse.message, msg);
            }
        }

        /// <summary>
        ///当token将要过期时，提前置换一个新的token
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> ReplaceToken()
        {
            string error = "";
            UserInfo userInfo = null;
            try
            {
                string requestToken = _context.Request.Headers[AppSetting.TokenHeaderName];
                requestToken = requestToken?.Replace("Bearer ", "");
                if (UserContext.Current.Token != requestToken) return webResponse.Error("Token已失效!");

                if (JwtHelper.IsExp(requestToken)) return webResponse.Error("Token已过期!");

                int userId = UserContext.Current.UserId;
                userInfo = await repository.FindFirstAsync(x => x.user_id == userId,
                     s => new UserInfo()
                     {
                         User_Id = userId,
                         UserName = s.user_name,
                         UserTrueName = s.user_true_name,
                         Role_Id = s.role_id,
                     });

                if (userInfo == null) return webResponse.Error("未查到用户信息!");

                string token = JwtHelper.IssueJwt(userInfo);
                //移除当前缓存
                base.CacheContext.Remove(userId.GetUserIdKey());
                //只更新的token字段
                repository.Update(new Sys_User() { User_Id = userId, Token = token }, x => x.Token, true);
                webResponse.OK(null, token);
            }
            catch (Exception ex)
            {
                error = ex.Message + ex.StackTrace + ex.Source;
                webResponse.Error("token替换出错了..");
            }
            finally
            {
                Logger.Info(LoggerType.ReplaceToeken, ($"用户Id:{userInfo?.User_Id},用户{userInfo?.UserTrueName}")
                    + (webResponse.status ? "token替换成功" : "token替换失败"), null, error);
            }
            return webResponse;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ModifyPwd(string oldPwd, string newPwd)
        {
            oldPwd = oldPwd?.Trim();
            newPwd = newPwd?.Trim();
            string message = "";
            try
            {
                if (string.IsNullOrEmpty(oldPwd)) return webResponse.Error("旧密码不能为空");
                if (string.IsNullOrEmpty(newPwd)) return webResponse.Error("新密码不能为空");
                if (newPwd.Length < 6) return webResponse.Error("密码不能少于6位");

                int userId = UserContext.Current.UserId;
                string userCurrentPwd = await _repository.FindFirstAsync(x => x.user_id == userId, s => s.user_pwd);

                string _oldPwd = oldPwd.EncryptDES(AppSetting.Secret.User);
                if (_oldPwd != userCurrentPwd) return webResponse.Error("旧密码不正确");

                string _newPwd = newPwd.EncryptDES(AppSetting.Secret.User);
                if (userCurrentPwd == _newPwd) return webResponse.Error("新密码不能与旧密码相同");


                repository.Update(new Sys_User_New
                {
                    user_id = userId,
                    user_pwd = _newPwd,
                    modify_date = DateTime.Now,
                    modify_name = UserContext.Current.UserName,
                    modify_id = UserContext.Current.UserId,
                }, x => new { x.user_pwd, x.modify_date, x.modify_name , x.modify_id }, true);

                webResponse.OK("密码修改成功");
            }
            catch (Exception ex)
            {
                message = ex.Message;
                webResponse.Error("服务器了点问题,请稍后再试");
            }
            finally
            {
                if (message == "")
                {
                    Logger.OK(LoggerType.ApiModifyPwd, "密码修改成功");
                }
                else
                {
                    Logger.Error(LoggerType.ApiModifyPwd, message);
                }
            }
            return webResponse;
        }
        /// <summary>
        /// 个人中心获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentUserInfo()
        {
            var data = await base.repository
                .FindAsIQueryable(x => x.user_id == UserContext.Current.UserId)
                .Select(s => new
                {
                    s.user_name,
                    s.user_true_name,
                    s.address,
                    s.phone_no,
                    s.email,
                    s.remark,
                    s.gender,
                    s.create_date
                })
                .FirstOrDefaultAsync();
            return webResponse.OK(null, data);
        }

        /// <summary>
        /// 设置固定排序方式及显示用户过滤
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override PageGridData<Sys_User_New> GetPageData(PageDataOptions pageData)
        {
            int roleId = -1;
            //树形菜单传查询角色下所有用户
            if (pageData.Value != null)
            {
                roleId = pageData.Value.ToString().GetInt();
            }
            QueryRelativeExpression = (IQueryable<Sys_User_New> queryable) =>
            {
                if (roleId <= 0)
                {
                    if (UserContext.Current.IsSuperAdmin) return queryable;
                    roleId = UserContext.Current.RoleId;
                }
                //查看用户时，只能看下自己角色下的所有用户
                List<int> roleIds = Sys_RoleService.Instance.GetAllChildrenRoleId(roleId);
                roleIds.Add(roleId);
                //判断查询的角色是否越权
                if (roleId != UserContext.Current.RoleId && !roleIds.Contains(roleId))
                {
                    roleId = -999;
                }
                return queryable.Where(x => roleIds.Contains(x.role_id));
            };
            var gridData = base.GetPageData(pageData);

            gridData.data.ForEach(x =>
            {
                x.token = null;
            });
            return gridData;
        }

        public WebResponseContent GetUserById(int id)
        {
            var company_data = _CompanyRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid).ToList();
            var contact_data = _ContactRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid).ToList();
            var role_data = _RoleRepository.Find(d=>d.delete_status == (int)SystemDataStatus.Valid).ToList();
            var data = _repository.Find(d => d.delete_status == (int)SystemDataStatus.Valid && d.user_id == id).Select(d=> new UserDto
            {
                id = d.id,
                user_id = d.user_id,
                user_name = d.user_name,
                user_name_eng = d.user_name_eng,
                user_true_name = d.user_true_name,
                company_id = d.company_id,
                contact_id = d.contact_id,
                email = d.email,
                enable = d.enable,
                gender = d.gender,
                lang = d.lang,
                phone_no = d.phone_no,
                remark = d.remark,
                role_name = d.role_id ==0?"": role_data.Where(r => r.role_id == d.role_id).FirstOrDefault()?.role_name,
                company_name = d.company_id ==null || d.company_id == Guid.Empty? "": company_data.Where(c=>c.id == d.company_id).FirstOrDefault()?.company_name,
                company_name_eng = d.company_id == null || d.company_id == Guid.Empty ? "" : company_data.Where(c => c.id == d.company_id).FirstOrDefault()?.company_name_eng,
                contact_name = d.contact_id == null || d.contact_id == Guid.Empty ? "" : contact_data.Where(c=>c.id == d.contact_id ).FirstOrDefault()?.name_cht,
                contact_name_eng = d.contact_id == null || d.contact_id == Guid.Empty ? "" : contact_data.Where(c => c.id == d.contact_id).FirstOrDefault()?.name_eng,
                contact_name_sho = d.contact_id == null || d.contact_id == Guid.Empty ? "" : contact_data.Where(c => c.id == d.contact_id).FirstOrDefault()?.name_sho,
                role_id = d.role_id,
            }).FirstOrDefault();
            if (data == null) return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
            else return WebResponseContent.Instance.OK(_localizationService.GetString("success"), data);
        }

        public async Task<WebResponseContent> GetListByPage(PageInput<UserQuery> query)
        {
            PageGridData<UserListDto> pageGridData = new PageGridData<UserListDto>();
            var companyId = UserContext.Current.UserInfo.Company_Id;
            var lstRole = _RoleRepository.FindAsIQueryable(d => d.delete_status == 0);
            var lstCompany = _CompanyRepository.FindAsIQueryable(d => d.delete_status == 0);
            var queryPam = query.search;
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 && d.role_id != 1 &&
            (queryPam.enable == -1 || queryPam.enable ==null ? true: d.enable == queryPam.enable) &&
            (companyId == Guid.Empty || companyId == null || UserContext.Current.IsSuperAdmin? true : d.company_id == companyId) &&
            (string.IsNullOrEmpty(queryPam.user_name)?true:d.user_name.Contains(queryPam.user_name)) &&
            (string.IsNullOrEmpty(queryPam.phone_no) ?true :d.phone_no.Contains(queryPam.phone_no)) && 
            (queryPam.role_id ==0?true:d.role_id == queryPam.role_id)).Select(d => new UserListDto
            {
                id = d.id,
                user_id = d.user_id,
                company_id = d.company_id,
                company_no = lstCompany.Where(x => x.delete_status == 0 && x.id == d.company_id).FirstOrDefault().company_no,
                company_name = lstCompany.Where(x=>x.delete_status ==0 && x.id == d.company_id).FirstOrDefault().company_name,
                role_id = d.role_id,
                role_name = lstRole.Where(x=>x.delete_status ==0 && x.role_id == d.role_id).FirstOrDefault().role_name,
                address = d.address,
                contact_id = d.contact_id,
                create_date = d.create_date,
                create_name = d.create_name,
                email = d.email,
                enable = d.enable,
                gender = d.gender,
                lang = d.lang,
                last_login_date = d.last_login_date,
                phone_no = d.phone_no,
                user_name = d.user_name,
                remark = d.remark,
                user_name_eng = d.user_name_eng,
                user_true_name = d.user_true_name
            });

            var result =await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        public WebResponseContent EditData(UserDto user)
        {
            try
            {
                var model = _repository.FindAsIQueryable(x => x.delete_status == 0 && x.user_name == user.user_name && user.user_id != user.user_id).FirstOrDefault();
                if (model != null)
                {
                    //已存在
                    return WebResponseContent.Instance.Error(model.user_name + _localizationService.GetString("existent"));
                }
                //未存在
                Sys_User_New sys_user = _repository.FindAsIQueryable(d=>d.id == user.id).FirstOrDefault();
                if (sys_user != null) 
                {
                    sys_user.user_name = user.user_name;
                    sys_user.user_name_eng = user.user_name_eng;
                    sys_user.role_id = user.role_id;
                    sys_user.phone_no = user.phone_no;
                    sys_user.lang = user.lang;
                    sys_user.remark = user.remark;
                    sys_user.email = user.email;
                    sys_user.enable = user.enable;
                    sys_user.gender = user.gender;
                    sys_user.modify_date = DateTime.Now;
                    sys_user.modify_name = UserContext.Current.UserName;
                    var res = _repository.Update(sys_user, true);
                    if (res > 0) 
                    {
                        //同步更新联系人表中的联系人信息
                        var contact =_ContactRepository.Find(c => c.id == sys_user.contact_id).FirstOrDefault();
                        if (contact != null) 
                        {
                            contact.email = sys_user.email;
                            contact.user_account = sys_user.user_name;
                            contact.user_phone = sys_user.phone_no;
                            contact.user_gender = sys_user.gender;
                            contact.remark = sys_user.remark;
                            contact.modify_date = DateTime.Now;
                            contact.modify_name = UserContext.Current.UserName;
                            contact.modify_id = UserContext.Current.UserId;
                            res = _ContactRepository.Update(contact, true);
                        }
                        return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                    } 
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"{this.GetType().Name}_EditData异常：{ex.Message}");
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_EditData异常：{ex.Message}", ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent DelData(int[] keys)
        {
            try
            {
                if (!UserContext.Current.IsSuperAdmin)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("insufficient_permissions"));
                }
                #region 检查是否在用
                var contactIds = _repository.FindAsIQueryable(d => keys.Contains(d.user_id)).Select(d=>d.contact_id).ToList();
                var checkContactRelationship = _ContactRepository.Exists(x => x.delete_status == 0 && contactIds.Contains(x.id));
                if (checkContactRelationship) return WebResponseContent.Instance.Error(_localizationService.GetString("user_using"));
                var checkQuotation = _QuotationRepository.Exists(x => x.delete_status == 0 && contactIds.Contains(x.pe_handler_id));
                if (checkQuotation) return WebResponseContent.Instance.Error(_localizationService.GetString("user_using"));
                var checkQuotation1 = _QuotationRepository.Exists(x => x.delete_status == 0 && contactIds.Contains(x.pq_handler_id));
                if (checkQuotation1) return WebResponseContent.Instance.Error(_localizationService.GetString("user_using"));
                var checkQuotation2 = _QuotationRepository.Exists(x => x.delete_status == 0 && contactIds.Contains(x.tender_handler_id));
                if (checkQuotation2) return WebResponseContent.Instance.Error(_localizationService.GetString("user_using"));
                var checkConfirmedOrder = _Confirmed_OrderRepository.Exists(x => x.delete_status == 0 && contactIds.Contains(x.head_id));
                if (checkConfirmedOrder) return WebResponseContent.Instance.Error(_localizationService.GetString("user_using"));
                var checkContractDetails = _Contract_DetailsRepository.Exists(x => x.delete_status == 0 && contactIds.Contains(x.contract_id));
                if (checkContractDetails) return WebResponseContent.Instance.Error(_localizationService.GetString("user_using"));
                #endregion


                List<Sys_User_New> lstDel = new List<Sys_User_New>();
                var lstUser = _repository.Find(d => d.delete_status == (int)SystemDataStatus.Valid).ToList();
                foreach (var item in keys)
                {
                    var upd = lstUser.Where(d => d.user_id == item).FirstOrDefault();
                    upd.delete_status = (int)SystemDataStatus.Invalid;
                    upd.modify_date = DateTime.Now;
                    upd.modify_name = UserContext.Current.UserName;
                    lstDel.Add(upd);
                }
                var res = _repository.UpdateRange(lstDel, true);
                if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent ModifyUserPwd(UserPwdInfo userPwd) 
        {
            try
            {
                var user = _repository.Find(d => d.user_id == userPwd.user_id).FirstOrDefault();
                if (user != null) 
                {
                    user.user_pwd = userPwd.password.EncryptDES(AppSetting.Secret.User);
                    user.modify_date = DateTime.Now;
                    user.modify_id = UserContext.Current.UserId;
                    user.modify_name = UserContext.Current.UserName;
                    var res= _repository.Update(user,  true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent SwitchLang(SwitchLangDto dto) 
        {
            try
            {
                if(dto.user_id ==0)  return WebResponseContent.Instance.Error("id_null");
                var entData = _repository.Find(d => d.user_id == dto.user_id).FirstOrDefault();
                if (entData != null) 
                {
                    entData.lang = dto.lang;
                    entData.modify_date = DateTime.Now;
                    entData.modify_name = UserContext.Current.UserName;
                    entData.modify_id = UserContext.Current.UserId; 
                    
                    var res = _repository.Update(entData, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this,ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));;
            }
        }
        public WebResponseContent SwitchEnable(int id, int enable) 
        {
            try
            {
                if (id == 0) return WebResponseContent.Instance.Error("id_null");
                var entData = _repository.Find(d => d.user_id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.enable = (byte)enable;
                    entData.modify_date = DateTime.Now;
                    entData.modify_name = UserContext.Current.UserName;
                    entData.modify_id = UserContext.Current.UserId;

                    var res = _repository.Update(entData, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 获取用户ID及名称
        /// </summary>
        /// <returns></returns>
        /// <remarks>获取delete_status=0(未删除数据)</remarks>
        public async Task<WebResponseContent> GetUserAllName()
        {
            try
            {
                var list = await _repository
                    .WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid)
                    .WhereIF(!UserContext.Current.IsSuperAdmin, p => p.company_id == UserContext.Current.UserInfo.Company_Id)
                    .Select(x => new { x.user_id, name_eng = x.user_name_eng, name_cht = x.user_true_name, x.user_register_id }).ToListAsync();
                return WebResponseContent.Instance.OK("OK", list);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_User_NewService.GetUserAllName", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}
