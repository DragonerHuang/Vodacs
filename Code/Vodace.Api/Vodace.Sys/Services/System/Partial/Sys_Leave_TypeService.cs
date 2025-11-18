
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Vodace.Sys.Services
{
    public partial class Sys_Leave_TypeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Leave_TypeRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_Leave_TypeService(
            ISys_Leave_TypeRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            ILocalizationService localizationService,
            IMapper mapper)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public async Task<WebResponseContent> GetListByPage(PageInput<LeaveTypeQuery> query)
        {
            var queryPam = query.search;
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
            (queryPam.is_leave == -1 || queryPam.is_leave ==null? true : d.is_leave == queryPam.is_leave) &&
            (string.IsNullOrEmpty(queryPam.leave_type_code) ? true : d.leave_type_code.Contains(queryPam.leave_type_code)) &&
            (string.IsNullOrEmpty(queryPam.leave_type_name)?true:d.leave_type_name.Contains(queryPam.leave_type_name))).Select(d => new LeaveTypeListDto
            {
                id = d.id,
                leave_type_name = d.leave_type_name,
                leave_type_code = d.leave_type_code,
                is_leave = d.is_leave,
                pay_type = d.pay_type,
                create_name = d.create_name,
                create_date = d.create_date,
            });
            query.sort_field = "leave_type_code";
            query.sort_type = "desc";
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }
        public async Task<WebResponseContent> GetList()
        {
            var query = _repository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid).Select(d=> new 
            { 
                d.id,
                d.leave_type_code,
                d.create_name,
                d.leave_type_name,
                d.is_leave,
                d.pay_type,
            });
            var list  = await query.OrderByDescending(d=>d.leave_type_code).ToListAsync();
            return WebResponseContent.Instance.OK("Ok", list);
        }

        public async Task<WebResponseContent> Add(LeaveTypeAddDto dto)
        {
            try
            {
                if (dto == null) return WebResponseContent.Instance.Error(_localizationService.GetString("connot_be_empty！"));
                var checkTypeCode = _repository.Exists(d => d.leave_type_code == dto.leave_type_code && d.delete_status == (int)SystemDataStatus.Valid);
                if (checkTypeCode) return WebResponseContent.Instance.Error(_localizationService.GetString("leave_type_code_exist！"));
                var checkTypeName = _repository.Exists(d => d.leave_type_name == dto.leave_type_name && d.delete_status == (int)SystemDataStatus.Valid);
                if (checkTypeName) return WebResponseContent.Instance.Error(_localizationService.GetString("leave_type_name_exist！"));

                Sys_Leave_Type model = _mapper.Map<Sys_Leave_Type>(dto);
                model.id = Guid.NewGuid();
                model.create_date = DateTime.Now;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.delete_status = (int)SystemDataStatus.Valid;
                await _repository.AddAsync(model);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), model);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_Leave_TypeService.Add", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            } 
        }

        public async Task<WebResponseContent> Update(LeaveTypeEditDto dto)
        {
            try
            {
                if (dto.id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var oldData = _repository.Find(d => d.id == dto.id).FirstOrDefault();
                if (oldData != null) 
                {
                    var checkTypeCode = _repository.Exists(d => d.leave_type_code == dto.leave_type_code && d.delete_status == (int)SystemDataStatus.Valid && d.id != dto.id);
                    if (checkTypeCode) return WebResponseContent.Instance.Error(_localizationService.GetString("leave_type_code_exist！"));
                    var checkTypeName = _repository.Exists(d => d.leave_type_name == dto.leave_type_name && d.delete_status == (int)SystemDataStatus.Valid && d.id != dto.id);
                    if (checkTypeName) return WebResponseContent.Instance.Error(_localizationService.GetString("leave_type_name_exist！"));

                    oldData.leave_type_code = dto.leave_type_code;
                    oldData.leave_type_name = dto.leave_type_name;
                    oldData.is_leave = dto.is_leave;
                    oldData.pay_type = dto.pay_type;
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
                Log4NetHelper.Error("Sys_Leave_TypeService.Update", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> DelData(Guid id)
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var oldData = _repository.Find(d => d.id == id).FirstOrDefault();
                if (oldData != null)
                {
                    oldData.delete_status = (int)SystemDataStatus.Invalid;
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
                Log4NetHelper.Error("Sys_Leave_TypeService.DelData", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
  }
}
