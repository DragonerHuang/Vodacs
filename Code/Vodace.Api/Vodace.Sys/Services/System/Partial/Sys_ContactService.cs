
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.DBManager;
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

namespace Vodace.Sys.Services
{
    public partial class Sys_ContactService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_ContactRepository _repository;
        private readonly IBiz_Site_RelationshipRepository _repositoryRelationship;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_CompanyRepository _CompanyRepository;
        private readonly ISys_DepartmentRepository _DepartmentRepository;
        private readonly ISys_User_NewRepository _UserRepository;
        private readonly ISys_Worker_RegisterRepository _WorkerRegisterRepository;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_ContactService(
            ISys_ContactRepository dbRepository,
            IMapper mapper,
            IBiz_Site_RelationshipRepository repositoryRelationship,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            ISys_CompanyRepository companyRepository,
            ISys_DepartmentRepository departmentRepository,
            ISys_User_NewRepository userRepository,
            ISys_Worker_RegisterRepository workerRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _repositoryRelationship = repositoryRelationship;
            _mapper = mapper;
            _localizationService = localizationService;
            _CompanyRepository = companyRepository;
            _DepartmentRepository = departmentRepository;
            _UserRepository = userRepository;
            _WorkerRegisterRepository = workerRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public async Task<WebResponseContent> GetListByPage(PageInput<ContactQuery> query)
        {
            PageGridData<MenuListDto> pageGridData = new PageGridData<MenuListDto>();
            var lstCompany = _CompanyRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var lstDepartment = _DepartmentRepository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid);
            var queryPam = query.search;
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
            (queryPam.company_id == Guid.Empty || queryPam.company_id==null ? true : d.company_id == queryPam.company_id) &&
            (string.IsNullOrEmpty(queryPam.name_cht) ? true : (d.name_cht.Contains(queryPam.name_cht) || (d.name_eng.Contains(queryPam.name_cht) || (d.name_ali.Contains(queryPam.name_cht))))))
                .Select(d => new ContactListDto
                {
                    id = d.id,
                    name_ali = d.name_ali,
                    name_cht = d.name_cht,
                    name_eng = d.name_eng,
                    email = d.email,
                    remark = d.remark,
                    create_date = d.create_date,
                    address = d.address,
                    //district_id = d.district_id,
                    //department_id = d.department_id,
                    strDepartmentName = d.department_id.HasValue ? lstDepartment.Where(p => p.id == (Guid)d.department_id).FirstOrDefault().name_cht : "",
                    company_id = d.company_id,
                    strCompanyName = d.company_id.HasValue ? lstCompany.Where(p => p.id == (Guid)d.company_id).FirstOrDefault().company_name : "",
                    id_no = d.id_no,
                    name_sho = d.name_sho,
                    fax = d.fax,
                    user_account = d.user_account,
                    user_gender = d.user_gender,
                    title = d.title,
                    user_phone = d.user_phone,
                    //organization_id = d.organization_id
                });
            query.sort_field = "create_date";
            query.sort_type = "asc";
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        /// <summary>
        /// 新增联系人
        /// </summary>
        /// <param name="sys_Contact"></param>
        /// <returns></returns>
        public WebResponseContent Add(ContactDetailDto dtoContactDetail)
        {
            try
            {
                if (dtoContactDetail == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                Sys_Contact sys_Contact = _mapper.Map<Sys_Contact>(dtoContactDetail);

                sys_Contact.id = Guid.NewGuid();
                sys_Contact.company_id = UserContext.Current.UserInfo.Company_Id;
                sys_Contact.delete_status = (int)SystemDataStatus.Valid;
                sys_Contact.create_id = UserContext.Current.UserId;
                sys_Contact.create_name = UserContext.Current.UserName;
                sys_Contact.create_date = DateTime.Now;
                _repository.Add(sys_Contact, true);
                return WebResponseContent.Instance.OK("Ok", sys_Contact);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_ContactService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="sys_Contact"></param>
        /// <returns></returns>
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                Sys_Contact sys_Contact =  repository.Find(p => p.id == guid).FirstOrDefault();
                if (sys_Contact != null)
                {
                    sys_Contact.delete_status = (int)SystemDataStatus.Invalid;
                    sys_Contact.modify_id = UserContext.Current.UserId;
                    sys_Contact.modify_name = UserContext.Current.UserName;
                    sys_Contact.modify_date = DateTime.Now;
                    var res = _repository.Update(sys_Contact, true);

                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("delete") + _localizationService.GetString("failes"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{guid}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_ContactService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改联系人
        /// </summary>
        /// <param name="sys_Contact"></param>
        /// <returns></returns>
        public WebResponseContent Edit(ContactEditlDto dtoContactDetail)
        {
            try
            {
                if (string.IsNullOrEmpty(dtoContactDetail.id.ToString()) || dtoContactDetail.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                Sys_Contact sys_Contact = repository.Find(p => p.id == dtoContactDetail.id).FirstOrDefault();
                var isChangeCompany = sys_Contact.company_id != dtoContactDetail.company_id;
                var old_companyId = sys_Contact.company_id;
                if (sys_Contact != null)
                {
                    sys_Contact.company_id = dtoContactDetail.company_id;
                    sys_Contact.id_no = dtoContactDetail.id_no;
                    sys_Contact.name_sho = dtoContactDetail.name_sho;
                    sys_Contact.name_eng = dtoContactDetail.name_eng;
                    sys_Contact.name_cht = dtoContactDetail.name_cht;
                    sys_Contact.name_ali = dtoContactDetail.name_ali;
                    //sys_Contact.department_id = dtoContactDetail.department_id;
                    sys_Contact.title = dtoContactDetail.title;
                    sys_Contact.address = dtoContactDetail.address;
                    //sys_Contact.district_id = dtoContactDetail.district_id;
                    sys_Contact.email = dtoContactDetail.email;
                    //sys_Contact.daily_salary = dtoContactDetail.daily_salary;
                    //sys_Contact.enable = sys_ContactDto.enable;   //默认这个不支持修改
                    sys_Contact.remark = dtoContactDetail.remark;
                    sys_Contact.user_phone = dtoContactDetail.user_phone;
                    sys_Contact.fax = dtoContactDetail.fax;

                    sys_Contact.modify_id = UserContext.Current.UserId;
                    sys_Contact.modify_name = UserContext.Current.UserName;
                    sys_Contact.modify_date = DateTime.Now;
                    var res = _repository.Update(sys_Contact, true);

                    if (res > 0) 
                    {
                        if (isChangeCompany) ChangeCompany(sys_Contact, old_companyId);
                        return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    } 
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));
                    
                }
                else return WebResponseContent.Instance.Error(dtoContactDetail.id + _localizationService.GetString("non_existent"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_ContactService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public void ChangeCompany(Sys_Contact contact,Guid? old_companyId)
        {
            try
            {
                Log4NetHelper.Info($"联系人：{contact.name_cht}更换了新公司：{contact.company_id},原公司：{old_companyId}");
                var user = _UserRepository.Find(d => d.contact_id == old_companyId).FirstOrDefault();
                if (user != null)
                {
                    var isExist = _UserRepository.Find(d => d.contact_id == contact.id && d.user_name == contact.user_account).FirstOrDefault();
                    if (isExist == null)
                    {
                        Log4NetHelper.Info($"新公司：{contact.name_cht}的账号：{contact.user_account}不存在，开始创建...");
                        Sys_User_New user_New = new Sys_User_New
                        {
                            id = Guid.NewGuid(),
                            user_name = user.user_name,
                            company_id = contact.company_id,
                            contact_id = contact.id,
                            phone_no = user.phone_no,
                            address = user.address,
                            email = user.email,
                            enable = (int)UserStatus.enable,
                            gender = user.gender,
                            lang = user.lang,
                            role_id = user.role_id,
                            user_pwd = user.user_pwd,
                            remark = user.remark,
                            user_name_eng = user.user_name_eng,
                            user_true_name = user.user_true_name,
                            create_date = DateTime.Now,
                            create_name = UserContext.Current.UserName,
                            create_id = UserContext.Current.UserId,
                            delete_status = (int)SystemDataStatus.Valid,
                        };
                        _UserRepository.Add(user_New, true);
                        Log4NetHelper.Info($"联系人：{contact.name_cht}更换新公司新增账号:成功");
                    }
                    else
                    {
                        Log4NetHelper.Info($"新公司：{contact.name_cht}的账号：{contact.user_account}已存在，不用创建");
                    } 
                }
                else
                {
                    Log4NetHelper.Info($"联系人：{contact.name_cht}更换新公司新增账号:失败，原用户信息不存在");
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"联系人：{contact.name_cht}更换了新公司：{contact.company_id}异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="dtoContactSearchInput"></param>
        /// <returns></returns>
        public async Task<PageGridData<ContactListDto>> GetContactList(PageInput<SearchContactDto> dtoContactSearchInput)
        {
            PageGridData<ContactListDto> pageGridData = new PageGridData<ContactListDto>();

            try
            {
                var search = dtoContactSearchInput.search;
                string strWhere = string.Empty;

                #region 查询条件

                strWhere = " where sc.delete_status = 0 ";

                if (UserContext.Current.UserInfo.Company_Id != null)
                    strWhere += $" and sc.company_id='{UserContext.Current.UserInfo.Company_Id}'";
                else
                {
                    if (!UserContext.Current.IsSuperAdmin)
                    {
                        strWhere += $" and sc.company_id='{Guid.Empty}'";
                    }
                }

                if (search != null)
                {
                    if (!string.IsNullOrEmpty(search.name_sho))
                    {
                        strWhere += $" and sc.name_sho like '%{search.name_sho.Replace("'", "''")}%' ";
                    }
                    if (!string.IsNullOrEmpty(search.name_eng))
                    {
                        strWhere += $" and sc.name_eng like '%{search.name_eng.Replace("'", "''")}%' ";
                    }
                    if (!string.IsNullOrEmpty(search.name_cht))
                    {
                        strWhere += $" and sc.name_cht like '%{search.name_cht.Replace("'", "''")}%' ";
                    }
                    if (!string.IsNullOrEmpty(search.name_ali))
                    {
                        strWhere += $" and sc.name_ali like '%{search.name_ali.Replace("'", "''")}%' ";
                    }
                    if (!string.IsNullOrEmpty(search.email))
                    {
                        strWhere += $" and sc.email like '%{search.email.Replace("'", "''")}%' ";
                    }
                    if (!string.IsNullOrEmpty(search.tel))
                    {
                        strWhere += $" and sc.tel like '%{search.tel.Replace("'", "''")}%' ";
                    }
                    if (!string.IsNullOrEmpty(search.fax))
                    {
                        strWhere += $" and sc.fax like '%{search.fax.Replace("'", "''")}%' ";
                    }
                }

                #endregion

                // 构建包含多表连接的SQL查询，优化查询性能
                var sql = $@"select *,com.company_name strCompanyName,org.title strOrganizationName,
                            dep.name strDepartmentName,dis.name_eng strDistrictNameEng,dis.name_cht strDistrictNameCht 
                            from sys_contact sc
                            left join Sys_Company com on sc.company_id=com.id
                            left join Sys_Organization org on sc.organization_id=org.id
                            left join Sys_Department dep on sc.department_id=dep.id
                            left join Sys_Country_District dis on sc.district_id=dis.id
                            {strWhere}
                            order by sc.create_date desc";

                // 执行SQL查询获取所有数据
                var list = DBServerProvider.SqlDapper.QueryList<ContactListDto>(sql, null);

                // 计算分页参数
                int page = dtoContactSearchInput.page_index > 0 ? dtoContactSearchInput.page_index : 1;
                int pageSize = dtoContactSearchInput.page_rows > 0 ? dtoContactSearchInput.page_rows : 20;
                int skip = (page - 1) * pageSize;

                // 执行内存分页
                pageGridData.data = list.Skip(skip).Take<ContactListDto>(pageSize).ToList();
                pageGridData.status = true;
                pageGridData.total = list.Count; // 设置总行数
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_ContactService.GetContactList", e);
                pageGridData.status = false;
                pageGridData.message = _localizationService.GetString("system_error");
            }

            return pageGridData;
        }

        /// <summary>
        /// 根据ID获取联系人信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContactById(Guid guid)
        {
            try
            {
                var detail = _repository.Find(p => p.id == guid).FirstOrDefault();
                return WebResponseContent.Instance.OK("OK", detail);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.GetProjectById", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取联系ID及名称
        /// </summary>
        /// <returns></returns>
        /// <remarks>获取delete_status=0(未删除数据)</remarks>
        public async Task<WebResponseContent> GetContactAllName()
        {
            try
            {
                var list = await _repository
                    .WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid)
                    .WhereIF(!UserContext.Current.IsSuperAdmin, p => p.company_id == UserContext.Current.UserInfo.Company_Id)
                    //.WhereIF(!UserContext.Current.IsSuperAdmin && !UserContext.Current.UserInfo.Company_Id.HasValue, p => p.company_id == Guid.Empty)
                    //.WhereIF(UserContext.Current.UserInfo.Company_Id != null, p => p.company_id == UserContext.Current.UserInfo.Company_Id)
                    .Select(x => new { x.id, x.name_eng, x.name_cht }).ToListAsync();
                return WebResponseContent.Instance.OK("OK", list);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.GetContactAllName", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取联系人下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContactWithCardDataAsync(PageInput<SearchContactWithCardDto> searchInput)
        {
            try
            {
                var search = searchInput.search ?? new SearchContactWithCardDto();

                var contacts = _repository
                    .FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                    .AsNoTracking();

                var users = _UserRepository
                    .FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                    .AsNoTracking();

                var workers = _WorkerRegisterRepository
                    .FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                    .AsNoTracking();

                var query = from c in contacts
                            join u in users on new { cid = (Guid?)c.id, comp = c.company_id } equals new { cid = u.contact_id, comp = u.company_id } into cu
                            from u in cu.DefaultIfEmpty()
                            join w in workers on u.user_register_id equals w.user_register_Id into uw
                            from w in uw.DefaultIfEmpty()
                            select new { c, w };

                if (!string.IsNullOrEmpty(search.name))
                {
                    query = query.Where(x => x.c.name_cht.Contains(search.name) || x.c.name_eng.Contains(search.name));
                }

                var lstData = query.Select(x => new ContactWithCardDto
                {
                    contact_id = x.c.id,
                    name_cht = x.c.name_cht,
                    name_eng = x.c.name_eng,
                    green_card_no = x.w != null ? x.w.stc_no : null,
                    green_card_start = x.w != null ? x.w.stc_issued_start_date.ToString() : null,
                    green_card_exp = x.w != null ? x.w.stc_issued_end_date.ToString() : null,
                    cic_card_no = x.w != null ? x.w.wrc_no : null,
                    cic_card_start = x.w != null ? x.w.wrc_issued_start_date.ToString() : null,
                    cic_card_exp = x.w != null ? x.w.wrc_issued_end_date.ToString() : null,
                    create_time = x.c.create_date
                });

                if (string.IsNullOrEmpty(searchInput.sort_field))
                {
                    searchInput.sort_field = "create_time";
                    searchInput.sort_type = "asc";
                }

                var result = await lstData.GetPageResultAsync(searchInput);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #region  -- 已弃用 --

        /// <summary>
        /// 新增联系人
        /// </summary>
        /// <param name="sys_Contact"></param>
        /// <returns></returns>
        private WebResponseContent Add_bak(ContactDto sys_ContactDto)
        {
            try
            {
                if (sys_ContactDto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                Sys_Contact sys_Contact = _mapper.Map<Sys_Contact>(sys_ContactDto);
                WebResponseContent webResponse = repository.DbContextBeginTransaction(() =>
                {
                    //如果想要回滚，返回new WebResponseContent().Error("返回消息")
                    DateTime nowTime = DateTime.Now;

                    sys_Contact.id = Guid.NewGuid();

                    if (sys_ContactDto.children.Count > 0)
                    {
                        foreach (var item in sys_ContactDto.children)
                        {
                            Biz_Site_Relationship relationship = new Biz_Site_Relationship();
                            relationship.id = Guid.NewGuid();
                            relationship.relation_id = sys_Contact.id;
                            relationship.site_id = item.id;
                            relationship.relation_type = 0;
                            relationship.create_id = UserContext.Current.UserId;
                            relationship.create_name = UserContext.Current.UserName;
                            relationship.create_date = nowTime;
                            _repositoryRelationship.Add(relationship);
                        }

                    }
                    sys_Contact.create_id = UserContext.Current.UserId;
                    sys_Contact.create_name = UserContext.Current.UserName;
                    sys_Contact.create_date = nowTime;
                    _repository.Add(sys_Contact);

                    _repository.SaveChanges();

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
                    return WebResponseContent.Instance.OK("OK", sys_Contact);
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_ContactService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion
    }
}
