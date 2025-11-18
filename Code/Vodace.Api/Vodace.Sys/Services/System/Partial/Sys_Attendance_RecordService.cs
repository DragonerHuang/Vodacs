using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.Formula.Functions;
using System;
using System.Data.Entity.Core.Objects;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Vodace.Sys.Services
{
    public partial class Sys_Attendance_RecordService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Attendance_RecordRepository _repository; //访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_User_NewRepository _user_NewRepository;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_Attendance_RecordService(
            ISys_Attendance_RecordRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            ,
            ILocalizationService localizationService,
            IMapper mapper,
            ISys_User_NewRepository user_NewRepository)
            : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _user_NewRepository = user_NewRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public async Task<WebResponseContent> GetRecord(int userId, string userNo, string punchTime)
        {
            try
            {
                if (userId == 0) return WebResponseContent.Instance.Error("user_id_null");
                if (string.IsNullOrEmpty(userNo)) return WebResponseContent.Instance.Error("user_no_null");
                if (string.IsNullOrEmpty(punchTime)) return WebResponseContent.Instance.Error("punch_time_null");
                var date = DateTime.Parse(punchTime).ToString("yyyy-MM-dd");
                var startTime = DateTime.Parse(date + " 00:00:00");
                var endTime = DateTime.Parse(date + " 23:59:59");
                var list = _repository.FindAsIQueryable(x =>
                    x.user_id == userId && x.user_no == userNo && x.punch_time >= startTime && x.punch_time <= endTime);
                var records = await list.OrderByDescending(d => d.punch_time).ToListAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), records);
            }
            catch (Exception ex)
            {
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> GetRecordList(PageInput<AttendanceRecordQuery> query)
        {
            var queryPam = query.search;
            if (queryPam.user_id == 0) return WebResponseContent.Instance.Error("user_id_null");
            if (string.IsNullOrEmpty(queryPam.user_no)) return WebResponseContent.Instance.Error("user_no_null");
            if (string.IsNullOrEmpty(queryPam.startTime)) return WebResponseContent.Instance.Error("punch_time_null");
            if (string.IsNullOrEmpty(queryPam.endTime)) return WebResponseContent.Instance.Error("punch_time_null");
            var strart_Time = DateTime.Parse(queryPam.startTime + " 00:00:00");
            var end_Time = DateTime.Parse(queryPam.endTime + " 23:59:59");
            var list = _repository.FindAsIQueryable(x => x.user_id == queryPam.user_id
                                                         && x.user_no == queryPam.user_no &&
                                                         x.punch_time >= strart_Time && x.punch_time <= end_Time);

            query.sort_field = "punch_time";
            query.sort_type = "desc";
            query.page_rows = query.page_rows == 0 ? 20 : query.page_rows;
            var result = await list.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        public async Task<WebResponseContent> AddData(AttendanceRecordDto recordDto)
        {
            try
            {
                if (recordDto.user_id == 0) return WebResponseContent.Instance.Error("user_id_null");
                if (string.IsNullOrEmpty(recordDto.user_no)) return WebResponseContent.Instance.Error("user_no_null");
                //if (recordDto.punch_time == DateTime.MinValue || recordDto.punch_time == null) return WebResponseContent.Instance.Error("punch_time_null");
                if (recordDto.device_source == 0) return WebResponseContent.Instance.Error("device_id_null");
                Sys_Attendance_Record record = _mapper.Map<Sys_Attendance_Record>(recordDto);
                record.delete_status = (int)SystemDataStatus.Valid;
                record.punch_time = DateTime.Now;
                record.id = Guid.NewGuid();
                record.create_date = DateTime.Now;
                record.user_id = recordDto.user_id;
                record.create_id = recordDto.user_id; //UserContext.Current.UserId;
                record.user_no = recordDto.user_no; //UserContext.Current.UserName;

                await _repository.AddAsync(record);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), record);
            }
            catch (global::System.Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<AttendanceRecordWebResponseDto> ClockIn(AttendanceRecordWebDto webDto)
        {
            try
            {
                if (webDto == null) return new AttendanceRecordWebResponseDto { code = -1 };
                var user = _user_NewRepository.FindFirst(d => d.user_no == webDto.data);
                if (user != null)
                {
                    Sys_Attendance_Record record = new Sys_Attendance_Record();
                    record.delete_status = (int)SystemDataStatus.Valid;

                    record.punch_time = DateTime.Now;
                    record.user_id = user.user_id;
                    record.user_no = user.user_no;
                    //record.adderss = "";
                    record.adderss = "上水廣場1722-1723A";
                    record.latitude = Convert.ToDecimal(22.502812754592664);
                    record.longitude = Convert.ToDecimal(114.1279321539113);
                    record.location_id = Guid.Parse("60E2BA21-6873-4A84-A86A-37A187CE66AA");
                    record.device_source = 1; //打卡机
                    record.id = Guid.NewGuid();
                    record.create_date = DateTime.Now;
                    record.create_id = user.user_id;
                    record.create_name = user.user_name;

                    await _repository.AddAsync(record);
                    await _repository.SaveChangesAsync();
                    Log4NetHelper.Info($"人脸打卡机_ClockIn刷脸成功：用户：{user.user_name},工号：{user.user_no},返回：1");
                    return new AttendanceRecordWebResponseDto { code = 1, cmd = 1, message = "识别成功", voiceData = "eg" };
                }
                Log4NetHelper.Info($"人脸打卡机_ClockIn刷脸失败xxxx：用户：{user.user_name},工号：{user.user_no},返回：-1");
                return new AttendanceRecordWebResponseDto { code = -1 };
            }
            catch (Exception ex)
            {
                Log4NetHelper.Info($"人脸打卡机_ClockIn刷脸异常：{ex.Message}");
                return new AttendanceRecordWebResponseDto { code = -1 };
            }
        }


        public async Task<WebResponseContent> GetMaxAndMinTimeRecordListAsync(AtdRecordMaxMinTimeQuery input)
        {
            if (!input.start_time.HasValue && !input.end_time.HasValue)
            {
                return WebResponseContent.Instance.Error("punch_time_null");
            }

            input.start_time ??= input.end_time;
            input.end_time ??= input.start_time;

            // 确保时间范围包含完整的一天
            var startTime = input.start_time.Value.Date; // 当天00:00:00
            var endTime = input.end_time.Value.Date.AddDays(1).AddSeconds(-1); // 当天23:59:59


            var userQuery = _user_NewRepository.FindAsIQueryable(n => true);
            var attendRecordQuery = _repository.FindAsIQueryable(n =>
                n.punch_time.HasValue && n.punch_time.Value >= startTime && n.punch_time.Value <= endTime);

            var list = await (from attendItemRecord in attendRecordQuery
                    join
                        userItem in userQuery on
                        attendItemRecord.user_id equals userItem.user_id
                    group new
                        {
                            attendItemRecord, userItem
                        }
                        by new { attendItemRecord.user_id, attendItemRecord.punch_time.Value.Date }
                    into res
                    select new AtdRecordMaxMinTimeWebDto()
                    {
                        user_id = res.Key.user_id,
                        user_no = res.Max(n => n.attendItemRecord.user_no),
                        max_time = res.Select(n => n.attendItemRecord).Count() > 1
                            ? res.Max(n => n.attendItemRecord.punch_time)
                            : null,
                        min_time = res.Min(n => n.attendItemRecord.punch_time),
                        user_name_eng = res.Max(n => n.userItem.user_name_eng),
                        user_true_name = res.Max(n => n.userItem.user_true_name)
                    }
                ).ToListAsync();


            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), list);
        }


        /// <summary>
        /// 某天打卡最早和最晚时间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetDayRecordDetailsListAsync(AtdDayRecordDetailsQuery input)
        {
            DateTime? startTime = null, endTime = null;

            if (input.start_time.HasValue || !input.end_time.HasValue)
            {
                input.start_time ??= input.end_time;
                input.end_time ??= input.start_time;

                // 确保时间范围包含完整的一天
                startTime = input.start_time!.Value!.Date; // 当天00:00:00
                endTime = input.end_time!.Value.Date.AddDays(1).AddSeconds(-1); // 当天23:59:59
            }


            var userQuery = _user_NewRepository.FindAsIQueryable(n => true).AsNoTracking();

            var attendRecordQuery = _repository.WhereIF(startTime.HasValue, n =>
                    n.punch_time.HasValue && n.punch_time.Value >= startTime)
                .WhereIF(endTime.HasValue, n => n.punch_time.HasValue && n.punch_time.Value <= endTime)
                .WhereIF(!input.remark.IsNullOrEmpty(), n => n.remark.Contains(input.remark))
                .WhereIF(!input.user_name.IsNullOrEmpty(), n => n.user_no.Contains(input.user_name))
                .AsNoTracking();

            var list = await (from attendItemRecord in attendRecordQuery
                    join
                        userItem in userQuery on
                        attendItemRecord.user_id equals userItem.user_id
                    group new
                        {
                            attendItemRecord, userItem
                        }
                        by new { attendItemRecord.user_id, attendItemRecord.punch_time.Value.Date }
                    into res
                    select new AtdDayRecordDetailsWebDto()
                    {
                        user_id = res.Key.user_id,
                        current_date = res.Key.Date,
                        remark = res.Max(n => n.attendItemRecord.remark),
                        user_no = res.Max(n => n.attendItemRecord.user_no),
                        max_time = res.Select(n => n.attendItemRecord).Count() > 1
                            ? res.Max(n => n.attendItemRecord.punch_time)
                            : null,
                        min_time = res.Min(n => n.attendItemRecord.punch_time),
                        user_name_eng = res.Max(n => n.userItem.user_name_eng),
                        user_true_name = res.Max(n => n.userItem.user_true_name),
                        on_work_address = res.OrderBy(n => n.attendItemRecord.punch_time)
                            .Select(n => n.attendItemRecord.adderss).First(),
                        off_work_address = res.Select(n => n.attendItemRecord).Count() > 1
                            ? res.OrderByDescending(n => n.attendItemRecord.punch_time)
                                .Select(n => n.attendItemRecord.adderss).First()
                            : string.Empty
                    }
                ).ToListAsync();


            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), list);
        }
    }
}