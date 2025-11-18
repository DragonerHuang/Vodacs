
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Configuration;
using Vodace.Core.DBManager;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using static Azure.Core.HttpHeader;

namespace Vodace.Sys.Services
{

    public partial class Sys_User_RegisterService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_User_RegisterRepository _repository;//访问数据库
        private readonly ISys_User_NewRepository _user_NewRepository;
        private readonly ISys_ContactRepository _ContactRepository;
        //private readonly ISys_UserRepository _userRepository;
        private readonly ISys_Worker_RegisterRepository _workerRepository;
        private readonly ISys_CompanyRepository _CompanyRepository;
        private readonly ISys_RoleRepository _roleRepository;
        private readonly ILocalizationService _localizationService;
        private readonly ISys_File_RecordsRepository _RecordsRepository;
        private readonly ISys_User_RelationRepository _relationRepository;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_User_RegisterService(
            ISys_User_RegisterRepository dbRepository,
            ISys_CompanyRepository companyRepository,
            //ISys_UserRepository userRepository,
            ISys_ContactRepository contactRepository,
            ISys_Worker_RegisterRepository sys_Worker_RegisterRepository,
            ILocalizationService localizationService,
            ISys_RoleRepository roleRepository,
            ISys_File_RecordsRepository recordsRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISys_User_NewRepository user_NewRepository,
            ISys_User_RelationRepository relationRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _CompanyRepository = companyRepository;
            //_userRepository = userRepository;
            _workerRepository = sys_Worker_RegisterRepository;
            _ContactRepository = contactRepository;
            _localizationService = localizationService;
            _roleRepository = roleRepository;
            _RecordsRepository = recordsRepository;
            _mapper = mapper;
            _user_NewRepository = user_NewRepository;
            _relationRepository = relationRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public WebResponseContent AddUserRegister(UserBaseDto register) 
        {
            try
            {
                if (register == null) return WebResponseContent.Instance.Error(_localizationService.GetString("user_register_null"));
                var idNo_isExist = _repository.Exists(d => d.id_no == register.id_no && d.register_status == (int)UserRegisterStatus.Complete);
                if (idNo_isExist) return WebResponseContent.Instance.Error(_localizationService.GetString("id_exist"));
                //var idPhone_isExist = _repository.Exists(d => d.Phone == register.Phone);
                //if (idPhone_isExist) return WebResponseContent.Instance.Error(_localizationService.GetString("phone_exist"));
                //var email_isExist = _repository.Exists(d => d.email == register.email);
                //if (email_isExist) return WebResponseContent.Instance.Error(_localizationService.GetString("email_exist"));
                var _register = _mapper.Map<Sys_User_Register>(register);
                _register.id = Guid.NewGuid();
                _register.register_status = (int)UserRegisterStatus.Registering;
                _register.create_id = UserContext.Current.UserId;
                _register.create_date = DateTime.Now;
                _register.create_name = UserContext.Current.UserName;
                _repository.Add(_register, true);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), _register.id);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"{this.GetType().Name}_AddUserRegister异常：{ex.Message}");
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent RegisterUserNew(UserRegisterNewDto register) 
        {
            var userId = Guid.NewGuid();
            try
            {
                if (register == null) return WebResponseContent.Instance.Error(_localizationService.GetString("user_register_null"));
                var _register = _mapper.Map<Sys_User_Register>(register.user);
                var Account_isExist = repository.Exists(d => (string.IsNullOrEmpty(_register.company_no) ? true : d.company_no == _register.company_no) && d.user_account == _register.user_account && d.delete_status ==0);
                if (Account_isExist) return WebResponseContent.Instance.Error(_localizationService.GetString("usern_exist"));
                WebResponseContent webResponse = repository.DbContextBeginTransaction(() =>
                {
                    #region 用户注册表
                    var registerId = Guid.NewGuid();
                    var entRegister = _repository.Find(d => d.id_no == _register.id_no && d.delete_status == 0).FirstOrDefault();
                    if (entRegister != null)
                    {
                        registerId = _register.id;
                        entRegister.id_no = _register.id_no;
                        entRegister.user_account = _register.user_account;
                        entRegister.user_password = _register.user_password.EncryptDES(AppSetting.Secret.User);
                        entRegister.user_phone = entRegister.user_phone;
                        entRegister.name_cht = entRegister.name_cht;
                        entRegister.name_eng = entRegister.name_eng;
                        entRegister.register_status = (int)UserRegisterStatus.Complete;
                        entRegister.company_no = entRegister.company_no;
                        entRegister.modify_name = UserContext.Current.UserName;
                        entRegister.modify_id = UserContext.Current.UserId;
                        entRegister.modify_date = DateTime.Now;
                        _repository.Update(entRegister);
                    }
                    else
                    {
                        _register.id = registerId;
                        _register.delete_status = 0;
                        _register.register_status = (int)UserRegisterStatus.Complete;
                        _register.create_id = UserContext.Current.UserId;
                        _register.create_date = DateTime.Now;
                        _register.create_name = UserContext.Current.UserName;
                        _repository.Add(_register);
                    }
          
                    #endregion

                    #region 工人表
                    var _worker = _mapper.Map<Sys_Worker_Register>(register.worker);
                    if (!string.IsNullOrEmpty(_worker.stc_no)) 
                    {
                        _worker.id = Guid.NewGuid();
                        _worker.user_register_Id = _register.id;
                        _worker.delete_status = 0;
                        _worker.create_date = DateTime.Now;
                        _worker.create_id = UserContext.Current.UserId;
                        _worker.create_name = UserContext.Current.UserName;
                        _workerRepository.Add(_worker);
                        #region 更新上传文件所属Id
                        if (register.FileIds.Length > 0)
                        {
                            var fileIds = register.FileIds.ToList();
                            var fileData = _RecordsRepository.Find(d => d.file_code == "wr_wrc" || d.file_code == "wr_stc").ToList();
                            foreach (var item in fileIds)
                            {
                                var upd_data = fileData.Where(d => d.id == item).FirstOrDefault();
                                if (upd_data != null) 
                                {
                                    upd_data.master_id = _worker.id;
                                    upd_data.upload_status = 1;
                                    upd_data.modify_name = UserContext.Current.UserName;
                                    upd_data.modify_id = UserContext.Current.UserId;
                                    upd_data.modify_date = DateTime.Now;
                                    _RecordsRepository.Update(upd_data, false);
                                }
                            }
                            _RecordsRepository.SaveChanges();
                        }
                    }

                    // 将工种添加到用户关系表中
                    var userWorkRelation = new Sys_User_Relation
                    {
                        id = Guid.NewGuid(),
                        create_date = DateTime.Now,
                        create_id = UserContext.Current.UserId,
                        create_name = UserContext.Current.UserName,
                        delete_status = 0,
                        relation_type = 0,
                        user_register_Id = _register.id,
                        relation_id = _worker.work_type
                    };
                    _relationRepository.Add(userWorkRelation);


                    #endregion
                    #endregion

                    #region 联系人表
                    var contactId = Guid.Empty;
                    var companyId = _CompanyRepository.Find(d=>d.company_no == _register.company_no && d.status == (int)AuditEnum.Passed).FirstOrDefault()?.id;
                    var entOldContact = _ContactRepository.Find(d => d.id_no == _register.id_no).FirstOrDefault();
                    if (entOldContact != null) 
                    {
                        contactId = entOldContact.id;
                        entOldContact.modify_date = DateTime.Now;
                        entOldContact.modify_id = UserContext.Current.UserId;
                        entOldContact.modify_name = UserContext.Current.UserName;
                        _ContactRepository.Update(entOldContact);
                    }
                    else
                    {
                        Sys_Contact entContact = new Sys_Contact
                        {
                            id = Guid.NewGuid(),
                            create_date = DateTime.Now,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            email = _register.user_email,
                            delete_status =0,
                            id_no = _register.id_no,
                            name_cht = _register.name_cht,
                            name_eng = _register.name_eng,
                            company_id = companyId,
                            user_phone = _register.user_phone,
                            user_account = _register.user_account,
                            user_gender = _register.user_gender,
                        };
                        contactId = entContact.id;
                        _ContactRepository.Add(entContact);
                    }
                   
                    #endregion

                    #region 角色
                    var roleName = !string.IsNullOrEmpty(_register.company_no)? "员工":"工人";
                    Sys_Role role = new Sys_Role();
                    var entRole = _roleRepository.Find(d => d.role_name.Equals(roleName) && d.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                    if (entRole == null)
                    {
                        role.role_name = roleName;
                        role.create_date = DateTime.Now;
                        role.create_name = UserContext.Current.UserName;
                        _roleRepository.Add(role);
                    }
                    else { role.role_id = entRole.role_id; }
                    #endregion

                    #region 登录用户表
                    Sys_User_New entUser_New = new Sys_User_New 
                    {
                        id = userId,
                        create_date = DateTime.Now,
                        create_id = UserContext.Current.UserId,
                        source = register.user.source,
                        user_register_id = registerId,
                        phone_no = _register.user_phone,
                        create_name = UserContext.Current.UserName,
                        company_id = companyId,
                        contact_id = contactId,
                        role_id = role.role_id,
                        enable = (int)UserStatus.enable,
                        delete_status =0,
                        user_pwd = _register.user_password.EncryptDES(AppSetting.Secret.User),
                        user_name = _register.user_account,
                        user_true_name = _register.name_cht,
                        user_name_eng = _register.name_eng,
                        email = _register.user_email,
                        gender = _register.user_gender,
                    };
                    _user_NewRepository.Add(entUser_New);
                    _user_NewRepository.SaveChanges();
                    SyncUserNo(userId);
                    #endregion
                    return new WebResponseContent().OK();
                });
                //判断事务是否执行成功
                if (!webResponse.status)
                {
                    //回滚
                    return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                else
                {
                    //SyncUserNo(userId);
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), _register.id);
                }
            }
            catch (Exception ex)
            {

                Log4NetHelper.Error($"{this.GetType().Name}_RegisterUserNew异常：{ex.Message}");
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_RegisterUserNew异常：{ex.Message}", ex);
                return new WebResponseContent().Error("注册异常！");
            }
        }

        private void SyncUserNo(Guid id) 
        {
            var data = _user_NewRepository.FindFirst(d => d.id == id);
            if (data != null)
            {
                var sql = $"update sys_user_new set user_no='{data.user_id}' where id='{data.id}'";
                var res = DBServerProvider.SqlDapper.ExcuteNonQueryAsync(sql, null);
            }
        }

        public async Task<WebResponseContent> CheckExist(string idNo) 
        {
            if (!string.IsNullOrEmpty(idNo)) 
            {
                var data = await _repository.FindAsyncFirst(d => d.id_no == idNo && d.register_status == (int)UserRegisterStatus.Complete);
                if (data != null) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), true);
                else return WebResponseContent.Instance.OK(_localizationService.GetString("no_data"),false);
            }
            return WebResponseContent.Instance.Error("No data");
        }

        public async Task<WebResponseContent> CheckCompanyNo(string com_no) 
        {
            var isExist = await _CompanyRepository.ExistsAsync(d=>d.company_no == com_no && d.status == (int)AuditEnum.Passed);
            return WebResponseContent.Instance.OK("Ok", isExist);

        }

        public async Task<WebResponseContent> CheckUserExist(string com_no, string userName)
        {
            var isExist = await repository.ExistsAsync(d => (string.IsNullOrEmpty(com_no) ? true : d.company_no == com_no) && d.user_account == userName);
            return WebResponseContent.Instance.OK(_localizationService.GetString("success"), isExist);
        }

    }
}
