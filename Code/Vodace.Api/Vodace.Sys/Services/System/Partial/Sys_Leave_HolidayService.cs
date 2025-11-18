using AutoMapper;

using Dm;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
    public partial class Sys_Leave_HolidayService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Leave_HolidayRepository _repository;//访问数据库
        private readonly ISys_Leave_TypeRepository _typeRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_Leave_HolidayService(
            ISys_Leave_HolidayRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            ILocalizationService localizationService,
            IMapper mapper,
            ISys_Leave_TypeRepository typeRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _typeRepository = typeRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public async Task<WebResponseContent> GetListByPage(PageInput<LeaveHolidayQuery> query)
        {
            var queryPam = query.search;
            var typeData = _typeRepository.FindAsIQueryable(d => d.id == queryPam.leave_type_id);
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
            (queryPam.leave_type_id == Guid.Empty ? true : d.leave_type_id == queryPam.leave_type_id) &&
            (string.IsNullOrEmpty(queryPam.holiday_name) ? true : d.holiday_name.Contains(queryPam.holiday_name))).Select(d => new LeaveHolidayListDto
            {
                id = d.id,
                leave_type_id = d.leave_type_id,
                leave_type_name = typeData.Where(x => x.id == d.leave_type_id).FirstOrDefault().leave_type_name,
                date = d.date,
                holiday_name = d.holiday_name,
                create_name = d.create_name,
                create_date = d.create_date,
            });
            query.sort_field = "date";
            query.sort_type = "desc";
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        public async Task<WebResponseContent> GetList()
        {
            var query = _repository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid).Select(d => new
            {
                d.id,
                d.leave_type_id,
                d.holiday_name,
                d.date,
            });
            var list = await query.OrderByDescending(d => d.date).ToListAsync();
            return WebResponseContent.Instance.OK("Ok", list);
        }

        public async Task<WebResponseContent> AddData(LeaveHolidayAddDto dto)
        {
            try
            {
                if (dto == null) return WebResponseContent.Instance.Error(_localizationService.GetString("connot_be_empty！"));
                var checkName = _repository.Exists(d => d.holiday_name == dto.holiday_name && d.delete_status == (int)SystemDataStatus.Valid && d.leave_type_id == dto.leave_type_id);
                if (checkName) return WebResponseContent.Instance.Error(_localizationService.GetString("holiday_name_exist！"));
                var checkData = _repository.Exists(d => d.date == dto.date && d.delete_status == (int)SystemDataStatus.Valid && d.leave_type_id == dto.leave_type_id);
                if (checkData) return WebResponseContent.Instance.Error(_localizationService.GetString("holiday_date_exist！"));

                Sys_Leave_Holiday model = _mapper.Map<Sys_Leave_Holiday>(dto);
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
                Log4NetHelper.Error("Sys_Leave_HolidayService.Add", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> EditData(LeaveHolidayEditDto dto)
        {
            try
            {
                if (dto.id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var oldData = _repository.Find(d => d.id == dto.id).FirstOrDefault();
                if (oldData == null) return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                var checkName = _repository.Exists(d => d.holiday_name == dto.holiday_name && d.delete_status == (int)SystemDataStatus.Valid && d.id != dto.id && d.leave_type_id == dto.leave_type_id);
                if (checkName) return WebResponseContent.Instance.Error(_localizationService.GetString("holiday_name_exist！"));
                var checkData = _repository.Exists(d => d.date == dto.date && d.delete_status == (int)SystemDataStatus.Valid && d.id != dto.id && d.leave_type_id == dto.leave_type_id);
                if (checkData) return WebResponseContent.Instance.Error(_localizationService.GetString("holiday_date_exist！"));

                oldData.date = dto.date;
                oldData.holiday_name = dto.holiday_name;
                oldData.leave_type_id = dto.leave_type_id;
                oldData.modify_date = DateTime.Now;
                oldData.modify_id = UserContext.Current.UserId;
                oldData.modify_name = UserContext.Current.UserName;
                _repository.Update(oldData);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_Leave_HolidayService.EditData", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> DelData(List<Guid> ids)
        {
            try
            {
                if (ids is null || ids.Any()) return WebResponseContent.Instance.Error("id_null");
                var oldDatas = await _repository.FindAsIQueryable(d => ids.Contains(d.id)).ToListAsync();
                if (oldDatas.Any())
                {
                    oldDatas.ForEach(n =>
                    {
                        n.delete_status = (int)SystemDataStatus.Invalid;
                        n.modify_date = DateTime.Now;
                        n.modify_id = UserContext.Current.UserId;
                        n.modify_name = UserContext.Current.UserName;
                    });
                    _repository.Update(oldDatas);
                    await _repository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_Leave_HolidayService.DelData", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> ImportDataAsync(LeaveHolidayImportDto importDto)
        {
            var jsonToHolidayList = importDto.import_json.FromJson<RootObject>();

            if (!(jsonToHolidayList?.vcalendar?.Any()).HasValue || !(jsonToHolidayList.vcalendar?.Select(n=>n.vevent)?.Any()).HasValue)
            {
                return WebResponseContent.Instance.Error(_localizationService.GetString("holiday_parse_failure"));
            }
            
            try
            {
                var addHolidayList = jsonToHolidayList.vcalendar
                    .SelectMany(n=>n.vevent)
                    .Where(n=>n.StartDate.ToDateTime().HasValue).Select(n=>new Sys_Leave_Holiday()
                {
                    date =  (n.StartDate.ToDateTime()).Value,
                    holiday_name = n.summary,
                    leave_type_id = importDto.leave_type_id,
                    create_date = DateTime.Now,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    delete_status = (int)SystemDataStatus.Valid,
                    id =  Guid.NewGuid()
                }).ToList();
                await _repository.AddRangeAsync(addHolidayList);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Sys_Leave_HolidayService.ImportData", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}