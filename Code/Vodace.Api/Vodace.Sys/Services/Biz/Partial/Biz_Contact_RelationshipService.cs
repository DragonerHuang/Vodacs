
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.DBManager;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_Contact_RelationshipService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Contact_RelationshipRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        private readonly ISys_DepartmentRepository _repositoryDep;
        private readonly ISys_OrganizationRepository _repositoryOrg;
        private readonly ISys_RoleRepository _repositoryRole;
        private readonly ISys_ContactRepository _repositoryContact;
        private readonly IBiz_QuotationRepository _repositoryQn;
        private readonly IBiz_ContractRepository _repositoryContract;
        private readonly ISys_CompanyRepository _repositoryCompany;
        private readonly ISys_User_NewRepository _repositoryUser;

        [ActivatorUtilitiesConstructor]
        public Biz_Contact_RelationshipService(
            IBiz_Contact_RelationshipRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IMapper mapper,
            ISys_DepartmentRepository repositoryDep,
            ISys_OrganizationRepository repositoryOrg,
            ISys_RoleRepository repositoryRole,
            ISys_ContactRepository repositoryContact,
            IBiz_QuotationRepository repositoryQn,
            IBiz_ContractRepository repositoryContract,
            ISys_CompanyRepository repositoryCompany,
            ISys_User_NewRepository repositoryUser)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _repositoryDep = repositoryDep;
            _repositoryOrg = repositoryOrg;
            _repositoryRole = repositoryRole;
            _repositoryContact = repositoryContact;
            _repositoryQn = repositoryQn;
            _repositoryContract = repositoryContract;
            _repositoryCompany = repositoryCompany;
            _repositoryUser = repositoryUser;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        /// <summary>
        /// 新增联系人关系
        /// </summary>
        /// <param name="m_Contract"></param>
        /// <returns></returns>
        public WebResponseContent Add(ContactRelationshipAddDto dto)
        {
            try
            {
                if (dto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                Biz_Contact_Relationship model = new Biz_Contact_Relationship();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                if (!dto.contact_id.HasValue)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contact_id") + _localizationService.GetString("connot_be_empty"));

                if (!dto.relation_id.HasValue)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("relation_id") + _localizationService.GetString("connot_be_empty"));

                if (!dto.company_id.HasValue)
                {
                    if (dto.relation_type == 0)
                    {
                        var qn = _repositoryQn.Find(a => a.id == dto.relation_id).FirstOrDefault();
                        if (qn != null)
                        {
                            var contract = _repositoryContract.Find(a => a.id == qn.contract_id).FirstOrDefault();
                            if (contract != null)
                                dto.company_id = contract.company_id;
                        }
                    }
                }
                if (!dto.company_id.HasValue)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("company_id") + _localizationService.GetString("connot_be_empty"));

                //联系人不允许重复(更改于2025-11-04下午时分)
                var exist = _repository.Find(a => a.relation_id == dto.relation_id && a.relation_type == dto.relation_type && a.contact_id == dto.contact_id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                if (exist != null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_exist"));
                }

                model.company_id = dto.company_id;
                model.contact_id = dto.contact_id;  
                model.relation_id = dto.relation_id;
                model.role_id = dto.role_id;
                model.relation_type = dto.relation_type;
                model.mail_to = dto.mail_to;

                model.name = dto.name;
                model.tel = dto.tel;
                model.fax = dto.fax;
                model.email = dto.email;
                model.title = dto.title;
                model.department_id = dto.department_id;
                model.department_name = dto.department_name;
                model.role_name = dto.role_name;
                model.org_id = dto.org_id;

                _repository.Add(model, true);
                return WebResponseContent.Instance.OK("Ok", model);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Contact_RelationshipService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除联系人关系
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                Biz_Contact_Relationship m_Contract = _repository.Find(p => p.id == guid).FirstOrDefault();
                if (m_Contract != null)
                {
                    m_Contract.delete_status = (int)SystemDataStatus.Invalid;
                    m_Contract.modify_id = UserContext.Current.UserId;
                    m_Contract.modify_name = UserContext.Current.UserName;
                    m_Contract.modify_date = DateTime.Now;
                    var res = _repository.Update(m_Contract, true);

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
                Log4NetHelper.Error("Biz_Contact_RelationshipService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取合约列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContactRelationshipList(PageInput<ContactRelationshipDto> searchDto)
        {
            try
            {
                var search = searchDto.search;
                string strWhere = string.Empty;

                #region 查询条件

                strWhere = " where cr.delete_status = 0 ";

                //sys_contact只存在一条数据，一个联系人只允许存在一条数据，不支持存在多条数据，所有company_id从其它表里面关联获取    ------- 2025-10-17 15:50 By Island
                //if (UserContext.Current.UserInfo.Company_Id != null)
                //    strWhere += $" and cr.company_id='{UserContext.Current.UserInfo.Company_Id}'";
                //else
                //{
                //    if (!UserContext.Current.IsSuperAdmin)
                //    {
                //        strWhere += $" and cr.company_id='{Guid.Empty}'";
                //    }
                //}

                string relation_type_sql = string.Empty;
                string table_ali = string.Empty;
                if (search != null)
                {
                    if (search.relation_type > -1)
                    {
                        strWhere += $" and cr.relation_type='{search.relation_type}' ";
                        switch (search.relation_type)
                        {
                            //联络类型：0：qn联系人、1：公司联系人、2：组织架构，3：合约组织架构人员
                            case 0:
                                table_ali = "ct";
                                relation_type_sql = $@" left join Biz_Quotation qn on cr.relation_id=qn.id 
                                                        left join Biz_Contract {table_ali} on qn.contract_id=ct.id
                                                     ";
                                break;
                            //case 1:
                            //    relation_type_sql = " left join Biz_Quotation qn on cr.relation_id=qn.id ";
                            //    break;
                            //case 2:
                            //    relation_type_sql = " left join Biz_Quotation qn on cr.relation_id=qn.id ";
                            //    break;
                        }
                    }
                    else
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("relation_type_null"));
                    }

                    #region  -- where --

                    if (!string.IsNullOrEmpty(search.relation_id.ToString()))
                    {
                        strWhere += $" and cr.relation_id='{search.relation_id}' ";
                    }
                    else
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("relation_id") + " " + _localizationService.GetString("connot_be_empty"));
                    }


                    if (!string.IsNullOrEmpty(search.company_id.ToString()))
                    {
                        strWhere += $" and sc.company_id='{search.company_id}' ";
                    }
                    if (!string.IsNullOrEmpty(search.contact_id.ToString()))
                    {
                        strWhere += $" and sc.id='{search.contact_id}' ";
                    }
                    if (search.role_id > 0)
                    {
                        strWhere += $" and cr.role_id='{search.role_id}' ";
                    }
                    if (!string.IsNullOrEmpty(search.mail_to))
                    {
                        strWhere += $" and cr.mail_to like '%{search.mail_to}%' ";
                    }

                    #endregion
                }

                #endregion

                // 构建包含多表连接的SQL查询，优化查询性能
                var sql = $@"select cr.id,
                            cr.name,
                            cr.tel,
                            cr.fax,
                            cr.email,
                            cr.title,
                            cr.department_id,
                            cr.department_name,
                            cr.role_name,
                            cr.contact_id,
                            cr.role_id,
                            cr.relation_type,
                            cr.relation_id,
                            cr.mail_to,
                            cr.org_id,
                            {table_ali}.company_id,
                            sc.id_no,
                            sc.name_sho,
                            sc.name_eng,
                            sc.name_cht,
                            sc.name_ali,
                            sc.address,
                            sc.daily_salary
                            from Biz_Contact_Relationship cr
                            left join sys_contact sc on cr.contact_id=sc.id 
                            {relation_type_sql}
                            {strWhere}
                            order by cr.create_date desc";

                var list = DBServerProvider.SqlDapper.QueryQueryable<ContactRelationshipListDto>(sql, null);
                var result = list.GetPageResult(searchDto);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Contact_RelationshipService.GetContactList", e);
                return null;
            }
        }

        /// <summary>
        /// 获取其它关系列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 同时获取部门、组织、职位、联系人列表<br/>
        /// </remarks>
        public async Task<WebResponseContent> GetContactRelationshipOtherList(ContactInfoDto dto)
        {
            try
            {
                /* 目前不开放admin超级管理员账号给任何人操作，如果想要查看其他公司数据，则需要登录相应的账号去查看  --- 2025-10-17 14:36 By Island记录 */
                
                //默认提取当前请求用户所属公司，如果后续有需求可以传公司ID参数------2025-10-15 15:37 By Island
                var lstDeps = _repositoryDep.FindAsIQueryable(d => d.delete_status == 0).Select(a => new { name = a.name_cht, id = a.id}).ToList();
                var lstOrgs = _repositoryOrg.FindAsIQueryable(d => d.delete_status == 0).Select(a => new { name = (!string.IsNullOrEmpty(a.name_cht) ? a.name_cht : a.name_eng), id = a.id }).ToList();
                var lstRoles = _repositoryRole.FindAsIQueryable(d => d.delete_status == 0).Select(a => new { name = a.role_name, id = a.role_id }).ToList();
                var lstCompany = _repositoryCompany.FindAsIQueryable(d => d.delete_status == 0).Select(a => new { id = a.id, name = a.company_name }).ToList();
                //var lstContactsInfo = _repositoryContact.FindAsIQueryable(d => d.delete_status == 0).Select(a => new { name_eng = a.name_eng, name_cht = a.name_cht, id = a.id, company_id = a.company_id }).ToList();
                //默认所有联系人，都存在公司ID，如果不存在公司ID，则不予提取------2025-10-15 15:37 By Island
                //var company_id = UserContext.Current.UserInfo.Company_Id;

                string c = GetCompanyIdByQuotation((Guid)dto.relation_id);
                Guid company_id = !string.IsNullOrEmpty(c) ? Guid.Parse(c) : Guid.Empty;

                if (company_id == null || string.IsNullOrEmpty(company_id.ToString()) || company_id == Guid.Empty)
                {
                    if (!UserContext.Current.IsSuperAdmin)
                    {
                        company_id = Guid.Empty;
                    }
                }
                var lstContactsInfo = _repositoryContact.FindAsIQueryable(d => d.delete_status == 0 && d.company_id == company_id)
                        .Select(a => new { name_eng = a.name_eng, name_cht = a.name_cht, id = a.id, company_id = a.company_id }).ToList();

                var lstContacts = new List<lstContactInfo>();
                foreach (var item in lstContactsInfo)
                {
                    lstContactInfo model = new lstContactInfo();
                    var com = lstCompany.Find(a => a.id == item.company_id);
                    if (com != null)
                        model.company_name = com.name;

                    model.id = item.id;
                    model.company_id = item.company_id;
                    model.name_eng = item.name_eng;
                    model.name_cht = item.name_cht;
                    lstContacts.Add(model);
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), new {
                    department = lstDeps,
                    organization = lstOrgs,
                    role = lstRoles,
                    contact = lstContacts
                });
            }
            catch(Exception e)
            {
                Log4NetHelper.Error("Biz_Contact_RelationshipService.GetContactRelationshipOtherList", e);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), new
                {
                    department = new List<object>(),
                    organization = new List<object>(),
                    role = new List<object>(),
                    contact = new List<object>()
                });
            }
        }

        /// <summary>
        /// 获取联系人详情
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContactInfoByContactId(ContactInfoDto dto)
        {
            try
            {
                Guid company_id = Guid.Empty;
                if (dto.relation_type == 0)
                {
                    var s = GetCompanyIdByQuotation((Guid)dto.relation_id);
                    company_id = !string.IsNullOrEmpty(s) ? Guid.Parse(s) : Guid.Empty;
                }

                if (company_id != Guid.Empty)
                {
                    var model = _repositoryContact.Find(a => a.id == dto.id && a.company_id == company_id).FirstOrDefault();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), model);
                }
                return null;
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Contact_RelationshipService.GetContactInfoByContactId", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据报价ID获取公司ID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private string GetCompanyIdByQuotation(Guid guid)
        {
            string res = string.Empty;

            try
            {
                var qn = _repositoryQn.Find(a => a.id == guid).FirstOrDefault();
                if(qn != null)
                {
                    var contract = _repositoryContract.Find(a => a.id == qn.contract_id).FirstOrDefault();
                    if (contract != null)
                        res = contract.company_id.ToString();
                }
            }
            catch(Exception e)
            {

            }

            return res;
        }

        public WebResponseContent GetContactRelationUserList(ContactRelationSearchAllDto dto)
        {
            try
            {
                if (dto.relation_type == 0)
                {
                    //如果联系人的职位与当前的组织不一致，同步报价联系人信息
                    var quotationQuery = _repositoryQn.WhereIF(true, a => a.delete_status == (int)SystemDataStatus.Valid);
                    quotationQuery = quotationQuery.WhereIF(dto.contract_id.HasValue, a => a.contract_id == dto.contract_id);
                    quotationQuery = quotationQuery.WhereIF(dto.relation_id.HasValue, a => a.id == dto.relation_id);
                    var quotation = quotationQuery.FirstOrDefault();

                    if(quotation == null)
                        return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", (UserContext.Current.Lang == (int)LangType.en_US ? "Quotation" : "报价")));

                    var list = _repository.Find(a => a.delete_status == (int)SystemDataStatus.Valid && a.relation_type == 0 && a.relation_id == quotation.id).ToList();
                    if (list.Count > 0)
                    {
                        var user = _repositoryUser.Find(a => a.delete_status == (int)SystemDataStatus.Valid).ToList();
                        List<object> result = new List<object>();
                        foreach(var item in list)
                        {
                            result.Add(user.Where(a => a.contact_id == item.contact_id && a.company_id == item.company_id).Select(x => new { x.user_id, name_eng = x.user_name_eng, name_cht = x.user_true_name, x.contact_id }).FirstOrDefault());
                        }
                        return WebResponseContent.Instance.OK("OK", result);
                    }
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", (UserContext.Current.Lang == (int)LangType.en_US ? "Quotation contact person" : "报价联络人")));
                }
                else return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "relation_type"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Contact_RelationshipService.GetContactRelationUserList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }

    public class lstContactInfo
    {
        public Guid? id { get; set; }
        public Guid? company_id { get; set; }
        public string company_name { get; set; }
        public string name_eng { get; set; }
        public string name_cht { get; set; }
    }
}
