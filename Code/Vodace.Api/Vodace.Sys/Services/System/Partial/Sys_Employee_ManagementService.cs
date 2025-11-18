using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Vodace.Core.BaseProvider;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Newtonsoft.Json;
using Vodace.Core.Configuration;

namespace Vodace.Sys.Services
{
    public partial class Sys_Employee_ManagementService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Employee_ManagementRepository _repository; //访问数据库
        private readonly ISys_DepartmentRepository _departmentRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly ISys_File_RecordsRepository _sysFileRecordsRepository;
        private readonly ISys_File_RecordsService _fileRecordsService;
        private readonly ISys_RoleRepository _roleRepository;
        private readonly ISys_User_RegisterRepository _userRegisterRepository;//访问数据库
        private readonly ISys_ContactRepository _contactRepository;
        private readonly ISys_User_NewRepository _userNewRepository;
        private readonly ISys_OrganizationRepository _organizationRepository;

        [ActivatorUtilitiesConstructor]
        public Sys_Employee_ManagementService(IHttpContextAccessor httpContextAccessor,
            ISys_Employee_ManagementRepository repository,
            ISys_DepartmentRepository departmentRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            ISys_File_RecordsRepository sysFileRecordsRepository,
            ISys_File_RecordsService fileRecordsService,
            ISys_Work_TypeRepository workTypeRepository,
            ISys_RoleRepository roleRepository,
            ISys_User_RegisterRepository userRegisterRepository,
            ISys_ContactRepository contactRepository,
            ISys_User_NewRepository userNewRepository,
            ISys_CompanyRepository companyRepository,
            ISys_OrganizationRepository organizationRepository) : base(repository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _departmentRepository = departmentRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _sysFileRecordsRepository = sysFileRecordsRepository;
            _fileRecordsService = fileRecordsService;
            _roleRepository = roleRepository;
            _userRegisterRepository = userRegisterRepository;
            _contactRepository = contactRepository;
            _userNewRepository = userNewRepository;
            _organizationRepository = organizationRepository;
        }

        /// <summary>
        /// 创建或修改
        /// </summary>
        /// <param name="dtoContactDetail"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> CreateOrUpdateAsync(SysEmpMentCreateOrUpdateDto dtoContactDetail)
        {
            var inputDto = dtoContactDetail.SysEmpMentInput;

            if (inputDto == null) return WebResponseContent.Instance.Error(_localizationService.GetString("user_register_null"));

            bool userExists =
                await _userNewRepository.ExistsAsync(n => n.user_no == inputDto.sys_employee_management_number && n.id != inputDto.user_id);
            if (userExists)
            {
                return WebResponseContent.Instance.Error(_localizationService.GetString("employee_exists"));
            }

            WebResponseContent webResponse = new WebResponseContent();
            UserRegisterNewDto register = new UserRegisterNewDto();
            register.user = new UserAllDto()
            {
                name_cht = inputDto.chinese_name,
                name_eng = inputDto.english_name,
                user_phone = inputDto.phone,
                user_email = inputDto.email,
                source = 0,
                user_gender = inputDto.user_gender ?? 0,
                user_account = inputDto.english_name,
                id_no = inputDto.card_no
            };

            List<FileInfoDto> addUpLoadFileList = new List<FileInfoDto>();

            bool isCreate = !inputDto.id.HasValue;

            if (isCreate)
            {
                inputDto.id = _repository.GetNewGuid();
            }

            var oldFileList = _sysFileRecordsRepository
                .FindAsIQueryable(n => n.master_id == inputDto.id).ToList();

            var userId = Guid.NewGuid();
            inputDto.user_id = userId;
            try
            {
                if (inputDto.emp_photo.form_file is not null)
                {
                    addUpLoadFileList = (_fileRecordsService.SaveFiles(new List<IFormFile>() { inputDto.emp_photo.form_file },
                        nameof(UploadFileCode.Employee_Management), masterId: inputDto.id)).data as List<FileInfoDto>;
                }

                if (inputDto.bank_info_list is not null && inputDto.bank_info_list.Any())
                {
                    inputDto.bank_info = JsonConvert.SerializeObject(inputDto.bank_info_list);
                }

                var _register = _mapper.Map<Sys_User_Register>(register.user);
                webResponse = _repository.DbContextBeginTransaction(() =>
               {
                   #region 用户注册表

                   var registerId = Guid.NewGuid();
                   var entRegister = _userRegisterRepository.Find(d => d.id_no == _register.id_no && d.delete_status == 0).FirstOrDefault();
                   if (entRegister != null)
                   {
                       registerId = _register.id;
                       entRegister.id_no = _register.id_no;
                       entRegister.user_account = _register.user_account;
                       entRegister.user_password = string.Empty;
                       entRegister.user_phone = entRegister.user_phone;
                       entRegister.name_cht = entRegister.name_cht;
                       entRegister.name_eng = entRegister.name_eng;
                       entRegister.register_status = (int)UserRegisterStatus.Complete;
                       entRegister.company_no = entRegister.company_no;
                       entRegister.modify_name = UserContext.Current.UserName;
                       entRegister.modify_id = UserContext.Current.UserId;
                       entRegister.modify_date = DateTime.Now;
                       _userRegisterRepository.Update(entRegister);
                   }
                   else
                   {
                       _register.id = registerId;
                       _register.delete_status = 0;
                       _register.register_status = (int)UserRegisterStatus.Complete;
                       _register.create_id = UserContext.Current.UserId;
                       _register.create_date = DateTime.Now;
                       _register.create_name = UserContext.Current.UserName;
                       _userRegisterRepository.Add(_register);
                   }

                   #endregion 用户注册表

                   #region 联系人表

                   var contactId = Guid.Empty;
                   var companyId = UserContext.Current.UserInfo.Company_Id;
                   var entOldContact = _contactRepository.Find(d => d.id_no == _register.id_no).FirstOrDefault();
                   if (entOldContact != null)
                   {
                       contactId = entOldContact.id;
                       entOldContact.modify_date = DateTime.Now;
                       entOldContact.modify_id = UserContext.Current.UserId;
                       entOldContact.modify_name = UserContext.Current.UserName;
                       _contactRepository.Update(entOldContact);
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
                           delete_status = 0,
                           id_no = _register.id_no,
                           name_cht = _register.name_cht,
                           name_eng = _register.name_eng,
                           company_id = companyId,
                           user_phone = _register.user_phone,
                           user_account = _register.user_account,
                           user_gender = _register.user_gender,
                       };
                       contactId = entContact.id;
                       _contactRepository.Add(entContact);
                   }

                   #endregion 联系人表

                   #region 角色

                   var roleName = !string.IsNullOrEmpty(_register.company_no) ? "员工" : "工人";
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

                   #endregion 角色

                   #region 登录用户表

                   var userNewOld = _userNewRepository.FindFirst(n =>
                       n.user_no == inputDto.sys_employee_management_number && n.id != inputDto.user_id);
                   if (userNewOld is null)
                   {
                       Sys_User_New entUserNew = new Sys_User_New
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
                           enable = (int)UserStatus.disable,
                           delete_status = 0,
                           user_pwd = string.Empty,
                           user_name = _register.user_account,
                           user_true_name = _register.name_cht,
                           user_name_eng = _register.name_eng,
                           email = _register.user_email,
                           gender = _register.user_gender,
                       };
                       _userNewRepository.Add(entUserNew);
                   }
                   else
                   {
                       userNewOld.create_date = DateTime.Now;
                       userNewOld.create_id = UserContext.Current.UserId;
                       userNewOld.source = register.user.source;
                       userNewOld.phone_no = _register.user_phone;
                       userNewOld.company_id = companyId;
                       userNewOld.role_id = role.role_id;
                       userNewOld.user_name = _register.user_account;
                       userNewOld.user_true_name = _register.name_cht;
                       userNewOld.user_name_eng = _register.name_eng;
                       userNewOld.email = _register.user_email;
                       userNewOld.gender = _register.user_gender;
                       _userNewRepository.Update(userNewOld);
                   }

                   #endregion 登录用户表

                   #region 员工表

                   //文件处理
                   if (oldFileList.Any())
                   {
                       oldFileList.ForEach(n =>
                       {
                           n.modify_id = UserContext.Current.UserId;
                           n.modify_name = UserContext.Current.UserName;
                           n.modify_date = DateTime.Now;
                           n.delete_status = (int)SystemDataStatus.Invalid;
                       });
                       _sysFileRecordsRepository.UpdateRange(oldFileList);
                   }
                   if (addUpLoadFileList != null && addUpLoadFileList.Any())
                   {
                       _sysFileRecordsRepository.AddRange(addUpLoadFileList);
                   }

                   if (!isCreate)
                   {
                       var oldData = _repository.FindFirst(d => d.id == inputDto.id);
                       if (oldData != null)
                       {
                           oldData = _mapper.Map<Sys_Employee_Management>(inputDto);
                           UpdateEntityInit(oldData);
                           _repository.Update(oldData, true);
                           return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                       }
                       webResponse =
                            WebResponseContent.Instance.Error(_localizationService.GetString("data_not_exists"));
                   }
                   else
                   {
                       var entity = _mapper.Map<Sys_Employee_Management>(inputDto);
                       AddEntityInit(entity);
                       _repository.Add(entity, true);
                       webResponse =
                            WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                   }
                   _repository.SaveChanges();

                   #endregion 员工表

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

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DelAsync(List<Guid> ids)
        {
            try
            {
                var oldData = await _repository.FindAsIQueryable(d => ids.Contains(d.id)).ToListAsync();
                if (oldData != null && oldData.Any())
                {
                    oldData.ForEach(n =>
                    {
                        DeleteEntityInit(n);
                    });

                    _repository.Update(oldData, true);
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }

                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_Employee_ManagementService.DelData", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetForEditAsync(Guid guid)
        {
            var entity = await _repository.FindFirstAsync(n => n.id == guid);

            if (entity is null)
                return WebResponseContent.Instance.Error(_localizationService.GetString("data_not_exists"));

            var sysFilesList = await _sysFileRecordsRepository.FindAsIQueryable(n => n.master_id == guid).ToListAsync();

            var output = _mapper.Map<SysEmpMentOutputDto>(entity);

            if (sysFilesList.Any())
            {
                output.file_id =
                    (sysFilesList.FirstOrDefault(n => n.file_code == nameof(SubmissionFileEnum.EmpPhoto)))
                    ?.id;
            }

            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), output);
        }

        /// <summary>
        /// 查询带分页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WebResponseContent> GetListByPageAsync(PageInput<SysEmpMentQueryDto> query)
        {
            var queryDto = query.search;

            DateTime? startTime = null, endTime = null;

            if (queryDto.start_time.HasValue)
            {
                queryDto.start_time ??= queryDto.end_time;
                startTime = queryDto.start_time!.Value!.Date; // 当天00:00:00
            }

            if (queryDto.end_time.HasValue)
            {
                endTime = queryDto.end_time!.Value.Date.AddDays(1).AddSeconds(-1); // 当天23:59:59
            }

            // 执行分页查询
            if (string.IsNullOrEmpty(query.sort_field))
            {
                query.sort_field = "create_date";
                query.sort_type = "desc";
            }

            var userQuery = await _userNewRepository.WhereIF(!queryDto.user_name.IsNullOrEmpty(),
                    n => n.user_true_name.Contains(queryDto.user_name))
                .Select(n => n.id).ToListAsync();

            var resQuery = _repository.FindAsIQueryable(n => true)
                .WhereIF(!queryDto.user_name.IsNullOrEmpty(),
                    n => n.user_id.HasValue && userQuery.Contains(n.user_id.Value))
                .WhereIF(startTime.HasValue, n =>
                    n.record_date.HasValue && n.record_date.Value >= startTime)
                .WhereIF(endTime.HasValue, n => n.record_date.HasValue && n.record_date.Value <= endTime)
                .AsNoTracking();

            var list = await resQuery.GetPageResultAsync(query);

            List<SysEmpMentWebDto> res = new();

            if (list is not null && list.data is not null)
            {
                res = _mapper.Map<List<SysEmpMentWebDto>>(list.data);
            }

            PageData<SysEmpMentWebDto> pageData = new PageData<SysEmpMentWebDto>()
            {
                total = list?.total ?? 0,
                data = res
            };

            var depIds = res.Select(n => n.department_id).Distinct().ToList();
            var organizationIds = res.Select(n => n.organization_id).Distinct().ToList();
            var userIds = res.Select(n => n.user_id).Distinct().ToList();

            var depList = await _departmentRepository.FindAsIQueryable(n => depIds.Contains(n.id)).ToListAsync();
            var organizationList = await _organizationRepository.FindAsIQueryable(n => organizationIds.Contains(n.id)).ToListAsync();
            var newUserList = await _userNewRepository.FindAsIQueryable(n => userIds.Contains(n.id)).ToListAsync();

            foreach (var itemRes in res)
            {
                itemRes.department_name = depList.FirstOrDefault(n => n.id == itemRes.department_id)?.name_cht;
                itemRes.organization_name = organizationList.FirstOrDefault(n => n.id == itemRes.organization_id)?.name_cht;

                var currentUser = newUserList.FirstOrDefault(n => n.id == itemRes.user_id);
                itemRes.sys_employee_management_number = currentUser?.user_no;
                itemRes.chinese_name = currentUser?.user_true_name;
                itemRes.english_name = currentUser?.user_name_eng;
                itemRes.phone = currentUser?.phone_no;
                itemRes.email = currentUser?.email;
                itemRes.user_gender = currentUser?.gender;
            }

            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), pageData);
        }

        public WebResponseContent RegisterUserNew(UserRegisterNewDto register)
        {
            return new WebResponseContent();
        }

        #region 公用封装

        private void AddEntityInit(Sys_Employee_Management sysEmployee)
        {
            sysEmployee.create_id = UserContext.Current.UserId;
            sysEmployee.create_name = UserContext.Current.UserName;
            sysEmployee.create_date = DateTime.Now;
            sysEmployee.delete_status = 0;
        }

        private void UpdateEntityInit(Sys_Employee_Management sysEmployee)
        {
            sysEmployee.modify_id = UserContext.Current.UserId;
            sysEmployee.modify_name = UserContext.Current.UserName;
            sysEmployee.modify_date = DateTime.Now;
        }

        private void DeleteEntityInit(Sys_Employee_Management sysEmployee)
        {
            sysEmployee.modify_id = UserContext.Current.UserId;
            sysEmployee.modify_name = UserContext.Current.UserName;
            sysEmployee.modify_date = DateTime.Now;
            sysEmployee.delete_status = (int)SystemDataStatus.Invalid;
        }

        #endregion 公用封装
    }
}