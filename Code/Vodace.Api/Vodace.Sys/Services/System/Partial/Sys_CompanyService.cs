
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

namespace Vodace.Sys.Services
{
    public partial class Sys_CompanyService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_CompanyRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_CompanyService(
            ISys_CompanyRepository dbRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _localizationService = localizationService;
            _repository = dbRepository;
            _mapper = mapper;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public async Task<WebResponseContent> GetCompanyList()
        {
            var data = await _repository.FindAsIQueryable(d => d.delete_status == 0).Select(d => new { d.id, d.company_name }).ToListAsync();
            return WebResponseContent.Instance.OK("Ok", data);
        }
        public async Task<WebResponseContent> Add(CompanyDto company) 
        {
            try
            {
                if (company == null) return WebResponseContent.Instance.Error(_localizationService.GetString("com_name_exist！"));
                var checkComName =  _repository.Exists(d => d.company_name == company.company_name);
                if (checkComName) return WebResponseContent.Instance.Error(_localizationService.GetString("com_name_exist！"));
                var checklicense_no = _repository.Exists(d => d.license_no == company.license_no);
                if (checklicense_no) return WebResponseContent.Instance.Error(_localizationService.GetString("com_lic_exist！"));
                var isChinese = CommonHelper.IsChineseChar(company.company_name);

                Sys_Company sys_Company = _mapper.Map<Sys_Company>(company);
                sys_Company.status = (int)AuditEnum.WaitAudit;
                sys_Company.delete_status= (int)SystemDataStatus.Valid;
                sys_Company.id = Guid.NewGuid();
                sys_Company.company_no = CommonHelper.GetCompanyCode(company.company_name_eng);//待确定生成规则
                sys_Company.create_date = DateTime.Now;
                sys_Company.create_id = UserContext.Current.UserId;
                sys_Company.create_name = UserContext.Current.UserName;
                await _repository.AddAsync(sys_Company);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"),company);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> Update(CompanyDto company)
        {
            try
            {
                if (company.id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var oldData = _repository.Find(d => d.id == company.id).FirstOrDefault();
                if (oldData != null) 
                {
                    oldData.company_name = company.company_name;
                    oldData.company_name_eng = company.company_name_eng;
                    oldData.contact = company.contact;
                    oldData.contact_email = company.contact_email;
                    oldData.contact_phone = company.contact_phone;
                    oldData.license_no = company.license_no;
                    oldData.modify_date = DateTime.Now;
                    oldData.modify_id = UserContext.Current.UserId;
                    oldData.modify_name = UserContext.Current.UserName;
                    _repository.Update(oldData);
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> DelData(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var entModel = _repository.FindAsIQueryable(x => x.id == id).FirstOrDefault();
                if (UserContext.Current.IsSuperAdmin)
                {
                    entModel.delete_status = (int)SystemDataStatus.Invalid;
                    entModel.modify_name = UserContext.Current.UserName;
                    entModel.modify_date = DateTime.Now;
                    _repository.Update(entModel);
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), id);
                }
                else
                {
                    //权限不足
                    return WebResponseContent.Instance.Error(_localizationService.GetString("insufficient_permissions"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> Audit(CompanyAuditDto company) 
        {
            try
            {
                if (company.id == Guid.Empty) return WebResponseContent.Instance.Error("");
                var oldData = _repository.Find(d => d.id == company.id).FirstOrDefault();
                if (oldData != null)
                {
                    oldData.status = company.status;
                    oldData.audit_user = UserContext.Current.UserName;
                    oldData.audit_remark = company.audit_remark;
                    oldData.audit_date = DateTime.Now;
                    _repository.Update(oldData);
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public async Task<WebResponseContent> GetListByPage(PageInput<CompanyQuery> query)
        {
            var queryPam = query.search;
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 && 
            (queryPam.status == -1 || queryPam.status == null ? true : d.status == queryPam.status) &&
            (string.IsNullOrEmpty(queryPam.company_name) ? true : d.company_name.Contains(queryPam.company_name)) &&
            (string.IsNullOrEmpty(queryPam.company_no) ? true : d.company_no.Contains(queryPam.company_no))).Select(d => new CompanyListDto
            {
                id = d.id,
                company_name = d.company_name,
                company_no = d.company_no,
                company_type = d.company_type,
                license_no = d.license_no,
                status = d.status,
                status_str = CommonHelper.GetAuditStatusStr(d.status),
                contact = d.contact,
                contact_phone = d.contact_phone,
                contact_email = d.contact_email,
                audit_date = d.audit_date,
                audit_remark = d.audit_remark,
                audit_user = d.audit_user,
                create_date = d.create_date,
                create_name = d.create_name,
                remark = d.remark,
                company_name_eng = d.company_name_eng
            });
            query.sort_field = "create_date";
            query.sort_type = "desc";
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }
    }
}
