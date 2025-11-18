
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using StackExchange.Redis;
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
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;

namespace Vodace.Sys.Services
{
    public partial class Sys_Leave_BalanceService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Leave_BalanceRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_User_NewRepository _userNewRepository;
        private readonly ISys_Leave_TypeRepository _leaveTypeRepository;
        private readonly ISys_Leave_RecordRepository _leaveRecordRepository;
        private readonly ISys_Leave_Balance_RecordRepository _leaveBalanceRecordRepository;

        [ActivatorUtilitiesConstructor]
        public Sys_Leave_BalanceService(
            ISys_Leave_BalanceRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            ILocalizationService localizationService,
            ISys_User_NewRepository userNewRepository,
            ISys_Leave_TypeRepository leaveTypeRepository,
            ISys_Leave_RecordRepository leaveRecordRepository,
            ISys_Leave_Balance_RecordRepository leaveBalanceRecordRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _userNewRepository = userNewRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _leaveRecordRepository = leaveRecordRepository;
            _leaveBalanceRecordRepository = leaveBalanceRecordRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public WebResponseContent Add(LeaveBalanceDto dto)
        {
            try
            {
                if (dto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");
                if (dto.user_no.IsNullOrEmpty() && dto.user_id <= 0) return WebResponseContent.Instance.Error(_localizationService.GetString("taking_leave"));

                var user = _userNewRepository.Find(a => a.user_id == dto.user_id || a.user_no == dto.user_no).FirstOrDefault();
                if (user == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("taking_leave"));

                if(!dto.leave_type_id.HasValue)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("leave_type_null"));

                //判断当前用户是否已存在相同请假记录
                var exists = _repository.Find(p => p.delete_status == (int)SystemDataStatus.Valid
                    && p.user_id == user.user_id && p.year == dto.year && p.leave_type_id == dto.leave_type_id).ToList();
                if (exists.Count > 0)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("leave_period_fall"));

                Sys_Leave_Balance model = new Sys_Leave_Balance();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                model.year = dto.year;
                model.remaing_leave = dto.remaing_leave;
                model.user_id = user.user_id;
                model.user_no = user.user_no;
                if(dto.leave_type_id.HasValue)
                    model.leave_type_id = dto.leave_type_id.Value;

                _repository.Add(model, true);
                return WebResponseContent.Instance.OK("Ok", model);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_BalanceService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                var model = repository.Find(p => p.id == guid).FirstOrDefault();
                if (model != null)
                {
                    model.delete_status = (int)SystemDataStatus.Invalid;
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;
                    var res = _repository.Update(model, true);

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
                Log4NetHelper.Error("Sys_Leave_BalanceService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public WebResponseContent Edit(LeaveBalanceEditDto dto)
        {
            try
            {
                if (dto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("edit")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                var user = _userNewRepository.Find(a => a.user_id == dto.user_id || a.user_no == dto.user_no).FirstOrDefault();
                if (user == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("taking_leave"));

                if (!dto.leave_type_id.HasValue)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("leave_type_null"));

                var model = _repository.Find(p => p.id == dto.id).FirstOrDefault();
                if(model == null)
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");

                if (!dto.leave_type_id.HasValue)
                    model.leave_type_id = dto.leave_type_id.Value;

                model.year = dto.year;
                model.remaing_leave = dto.remaing_leave;
                model.user_id = model.user_id;    //不允许修改员工ID
                model.user_no = model.user_no;    //不允许修改员工编码
                model.modify_id = UserContext.Current.UserId;
                model.modify_name = UserContext.Current.UserName;
                model.modify_date = DateTime.Now;
                _repository.Update(model, true);

                return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_BalanceService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetLeaveBalanceList(PageInput<LeaveBalanceEditDto> dto)
        {
            try
            {
                var userInfo = _userNewRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);
                // 构建查询，实现三表左连接
                var query = from r in _repository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                            join t in _leaveTypeRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                            on r.leave_type_id equals t.id into leaveTypeJoin
                            from lt in leaveTypeJoin.DefaultIfEmpty()
                            join n in userInfo
                            on r.user_id equals n.user_id into userNewJoin
                            from un in userNewJoin.DefaultIfEmpty()
                            select new LeaveBalanceListDto()
                            {
                                // 获取请假记录所有字段
                                id = r.id,
                                user_id = r.user_id,
                                user_no = r.user_no,
                                leave_type_id = r.leave_type_id,
                                year = r.year,
                                remaing_leave = r.remaing_leave,
                                //used_leave = r.used_leave,
                                delete_status = r.delete_status,
                                create_id = r.create_id,
                                create_name = r.create_name,
                                create_date = r.create_date,
                                modify_id = r.modify_id,
                                modify_name = r.modify_name,
                                modify_date = r.modify_date,
                                // 获取假期类型字段
                                leave_type_code = lt.leave_type_code,
                                leave_type_name = lt.leave_type_name,
                                // 获取用户表字段
                                user_name = un.user_name,
                                user_true_name = un.user_true_name,
                                user_name_eng = un.user_name_eng,
                                company_id = un.company_id
                            };

                if (!UserContext.Current.UserInfo.IsSuperAdmin)
                    query = query.WhereIF(false, p => p.company_id == UserContext.Current.UserInfo.Company_Id);

                // 获取查询条件
                var searchDto = dto.search;

                // 添加查询条件过滤
                if (searchDto != null)
                {
                    query = query.WhereIF(searchDto.id.HasValue, p => p.id == searchDto.id);
                    query = query.WhereIF(searchDto.leave_type_id.HasValue, p => p.leave_type_id == searchDto.leave_type_id);
                    query = query.WhereIF(searchDto.user_id > 0, p => p.user_id == searchDto.user_id);
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.user_no), p => p.user_no.Contains(searchDto.user_no));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.year), p => p.year.Contains(searchDto.year));
                    query = query.WhereIF(searchDto.remaing_leave > 0, p => p.remaing_leave == searchDto.remaing_leave);
                }

                if (!string.IsNullOrEmpty(dto.sort_field))
                    query = query.OrderByDynamic(dto.sort_field, dto.sort_type);
                else
                    query = query.OrderByDescending(x => x.create_date);

                var sql = query.ToQueryString();

                // 执行分页查询（使用项目提供的扩展方法）
                var result = await query.GetPageResultAsync(dto);

                if (result.data != null || result.data.Count > 0)
                {
                    var userIds = result.data.Select(item => item.user_id).Distinct().ToList();
                    var leaveRecords = _leaveRecordRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid && p.status == 2 && userIds.Contains(p.user_id)).ToList();
                    foreach (var item in result.data)
                    {
                        item.used_leave = GetUsedLeave(leaveRecords, item.user_id, (Guid)item.leave_type_id, item.year);
                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_BalanceService.GetLeaveRecordList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 员工已使用假期天数
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userId"></param>
        /// <param name="leaveTypeId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        private decimal GetUsedLeave(List<Sys_Leave_Record> list, int userId, Guid leaveTypeId, string year)
        {
            decimal res = 0m;
            try
            {
                // 查询请假记录表，获取符合条件的请假记录
                list = list.Where(p => p.user_id == userId && p.leave_type_id == leaveTypeId && p.start_date >= DateTime.Parse(year + "-01-01") && p.end_date <= DateTime.Parse(year + "-12-31")).ToList();

                // 遍历记录，计算已用假期总额
                foreach (var record in list)
                {
                    // 计算请假天数
                    decimal days = 0m;

                    // 根据period字段判断假期类型
                    if (record.period == 0) // 全天假
                    {
                        // 处理全天假，计算从start_date到end_date的天数
                        if (record.start_date.Date == record.end_date.Date)
                        {
                            days = 1m; // 同一天的全天假
                        }
                        else
                        {
                            // 跨天的全天假，计算天数差
                            days = (record.end_date.Date - record.start_date.Date).Days + 1;
                        }
                    }
                    else if (record.period == 1 || record.period == 2) // 上午或下午的半天假
                    {
                        // 半天假无论是否跨天，都按0.5天计算
                        // 注意：这里假设一条请假记录只包含一个半天时段
                        days = 0.5m;
                    }

                    // 累加已用假期
                    res += days;
                }
            }
            catch (Exception e)
            {

            }

            return res; // 返回计算结果
        }

        #region  -- 定时任务 --

        /// <summary>
        /// 定时任务
        /// </summary>
        /// <remarks>
        /// 1、每月更新假期天数：
        ///     病假每月刷新2天、
        ///     疾病假（超过4天假期）每月叠加两天，最多120天
        /// 2、更新年假：（年假可以负数，提前预支）
        ///     入职满365天开始叠加，最初10天（第一，第二年），之后每年递加1天，最高14天
        ///     未满365天没有年假
        /// </remarks>
        public void ScheduledTasks()
        {
            try
            {
                // 获取当前年份
                string currentYear = DateTime.Now.Year.ToString();
                // 获取所有在职员工
                var activeUsers = _userNewRepository.Find(a => a.is_current == 1 && a.delete_status == (int)SystemDataStatus.Valid).ToList();
                // 获取假期类型
                var leaveTypes = _leaveTypeRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid).ToList();

                // 循环处理每位员工
                foreach (var user in activeUsers)
                {
                    // 检查是否处于试用期（试用期为三个月）
                    bool isProbation = IsInProbationPeriod(user);

                    if (isProbation)
                    {
                        AddProbationLeaveBalance(user, leaveTypes); // 试用期员工无有薪假期
                    }
                    else
                    {
                        // 获取员工当前年份的假期余额
                        var currentYearBalances = _repository.Find(a =>
                            a.user_id == user.user_id &&
                            a.year == currentYear &&
                            a.delete_status == 0).ToList();

                        // 处理病假刷新逻辑（改为刷新，不递加）
                        RefreshSickLeave(user, currentYear, currentYearBalances, leaveTypes);

                        // 处理疾病假刷新逻辑（保持递加）
                        RefreshDiseaseLeave(user, currentYear, currentYearBalances, leaveTypes);

                        // 处理年假刷新逻辑（使用doj作为入职时间）
                        RefreshAnnualLeave(user, currentYear, currentYearBalances, leaveTypes);
                    }
                }

                // 保存所有更改
                _repository.SaveChanges();
            }
            catch(Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_BalanceService.ScheduledTasks", e);
            }
        }

        /// <summary>
        /// 检查员工是否处于试用期（按自然月份计算三个月）
        /// </summary>
        private bool IsInProbationPeriod(Sys_User_New user)
        {
            if (!user.doj.HasValue || user.is_current != 1)
            {
                return false;
            }

            DateTime entryDate = user.doj.Value;
            DateTime today = DateTime.Now;

            // 计算试用期结束日期（按自然月份计算三个月）
            DateTime probationEndDate;

            try
            {
                // 尝试直接添加三个月
                probationEndDate = entryDate.AddMonths(3);
            }
            catch (ArgumentOutOfRangeException)
            {
                // 处理月末日期边界情况（如2月28日+1个月应到3月31日）
                int year = entryDate.Year;
                int month = entryDate.Month + 3;
                int day = entryDate.Day;

                // 调整年份和月份
                while (month > 12)
                {
                    month -= 12;
                    year++;
                }

                // 获取调整后月份的天数
                int daysInMonth = DateTime.DaysInMonth(year, month);
                // 如果原日期是月末，调整后的日期也应该是月末
                day = Math.Min(day, daysInMonth);

                probationEndDate = new DateTime(year, month, day);
            }

            // 如果今天小于试用期结束日期，则处于试用期内
            return today < probationEndDate;
        }

        /// <summary>
        /// 为试用期员工创建无薪假期余额
        /// </summary>
        /// <param name="dto">无薪假期余额DTO</param>
        /// <returns>操作结果</returns>
        public void AddProbationLeaveBalance(Sys_User_New user, List<Sys_Leave_Type> leaveTypes)
        {
            try
            {
                // 获取病假类型
                var sickLeaveType = leaveTypes.FirstOrDefault(t => t.leave_type_name.Contains("病假") && t.is_leave == 1);
                if (sickLeaveType == null) return;

                Sys_Leave_Balance model = new Sys_Leave_Balance();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                model.year = DateTime.Now.Year.ToString();
                model.remaing_leave = 0;
                model.user_id = user.user_id;
                model.user_no = user.user_no;
                model.leave_type_id = sickLeaveType.id;

                _repository.Add(model);

                // 添加无薪假期余额变动记录
                AddBalanceRecord(user, sickLeaveType, model.id, 0, 0, "为试用期员工创建无薪假期余额");
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_BalanceService.AddProbationLeaveBalance", e);
            }
        }

        /// <summary>
        /// 刷新病假：每月刷新2天
        /// </summary>
        private void RefreshSickLeave(Sys_User_New user, string currentYear, List<Sys_Leave_Balance> currentBalances, List<Sys_Leave_Type> leaveTypes)
        {
            // 获取病假类型
            var sickLeaveType = leaveTypes.FirstOrDefault(t => t.leave_type_name.Contains("病假") && t.is_leave == 1);
            if (sickLeaveType == null) return;

            // 获取或创建该员工的病假余额记录
            var sickLeaveBalance = currentBalances.FirstOrDefault(b => b.leave_type_id == sickLeaveType.id);

            if (sickLeaveBalance == null)
            {
                // 创建新的病假余额记录
                sickLeaveBalance = new Sys_Leave_Balance
                {
                    id = Guid.NewGuid(),
                    user_id = user.user_id,
                    user_no = user.user_no,
                    year = currentYear,
                    leave_type_id = sickLeaveType.id,
                    remaing_leave = 2, // 初始为2天
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now
                };
                _repository.Add(sickLeaveBalance);

                // 添加余额变动记录
                AddBalanceRecord(user, sickLeaveType, sickLeaveBalance.id, 2, 2, "系统自动初始化病假余额");
            }
            else
            {
                // 已存在记录，每月固定刷新2天（不递加）
                sickLeaveBalance.remaing_leave = 2;
                sickLeaveBalance.modify_id = UserContext.Current.UserId;
                sickLeaveBalance.modify_name = UserContext.Current.UserName;
                sickLeaveBalance.modify_date = DateTime.Now;

                _repository.Update(sickLeaveBalance);
                // 添加余额变动记录
                AddBalanceRecord(user, sickLeaveType, sickLeaveBalance.id, 2, 2, "系统自动刷新病假余额");
            }
        }

        /// <summary>
        /// 刷新疾病假：每月叠加两天，最多120天
        /// </summary>
        private void RefreshDiseaseLeave(Sys_User_New user, string currentYear, List<Sys_Leave_Balance> currentBalances, List<Sys_Leave_Type> leaveTypes)
        {
            // 获取疾病假类型
            var diseaseLeaveType = leaveTypes.FirstOrDefault(t => t.leave_type_name.Contains("疾病假") && t.is_leave == 1);
            if (diseaseLeaveType == null) return;

            // 获取或创建该员工的疾病假余额记录
            var diseaseLeaveBalance = currentBalances.FirstOrDefault(b => b.leave_type_id == diseaseLeaveType.id);

            if (diseaseLeaveBalance == null)
            {
                // 创建新的疾病假余额记录
                diseaseLeaveBalance = new Sys_Leave_Balance
                {
                    id = Guid.NewGuid(),
                    user_id = user.user_id,
                    user_no = user.user_no,
                    year = currentYear,
                    leave_type_id = diseaseLeaveType.id,
                    remaing_leave = 2, // 初始为2天
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now
                };
                _repository.Add(diseaseLeaveBalance);

                // 添加余额变动记录
                AddBalanceRecord(user, diseaseLeaveType, diseaseLeaveBalance.id, 2, 2, "系统自动初始化疾病假余额");
            }
            else
            {
                // 已存在记录，每月增加2天，但不超过120天上限
                decimal previousBalance = diseaseLeaveBalance.remaing_leave ?? 0;
                decimal remaing_leval = Math.Min(previousBalance + 2, 120);
                decimal spend = remaing_leval - previousBalance;

                if (previousBalance < 120)
                {
                    diseaseLeaveBalance.remaing_leave = remaing_leval;
                    diseaseLeaveBalance.modify_id = UserContext.Current.UserId;
                    diseaseLeaveBalance.modify_name = UserContext.Current.UserName;
                    diseaseLeaveBalance.modify_date = DateTime.Now;

                    _repository.Update(diseaseLeaveBalance);

                    AddBalanceRecord(user, diseaseLeaveType, diseaseLeaveBalance.id, spend, remaing_leval, "系统自动增加疾病假余额");
                }
            }

            // 非年假不允许负数余额
            EnsureNonNegativeBalance(diseaseLeaveBalance, false);
        }

        /// <summary>
        /// 刷新年假：根据入职时间计算年假天数
        /// </summary>
        private void RefreshAnnualLeave(Sys_User_New user, string currentYear, List<Sys_Leave_Balance> currentBalances, List<Sys_Leave_Type> leaveTypes)
        {
            // 获取年假类型
            var annualLeaveType = leaveTypes.FirstOrDefault(t => t.leave_type_name.Contains("年假") && t.is_leave == 1);
            if (annualLeaveType == null) return;

            // 检查入职日期，计算工作年限
            if (!user.doj.HasValue)
            {
                return; // 没有入职日期，无法计算年假
            }

            DateTime entryDate = user.doj.Value;
            DateTime today = DateTime.Now;

            // 计算是否满一年：从去年的同月同日到今年的同月同日为满一年
            DateTime anniversaryDate = new DateTime(today.Year, entryDate.Month, entryDate.Day);

            // 如果今年的周年日还没到，则取去年的周年日
            if (anniversaryDate > today)
            {
                anniversaryDate = anniversaryDate.AddYears(-1);
            }

            // 计算工作年限（已满的整年数）
            int fullYears = anniversaryDate.Year - entryDate.Year;

            // 未满1年没有年假
            if (fullYears < 1)
            {
                return;
            }

            // 计算年假天数：最初10天（第一、第二年），之后每年递加1天，最高14天
            int remaing_leval = 10;
            if (fullYears > 2)
            {
                remaing_leval = Math.Min(10 + (fullYears - 2), 14);
            }

            // 获取该员工的年假余额记录
            var annualLeaveBalance = currentBalances.FirstOrDefault(b => b.leave_type_id == annualLeaveType.id);

            if (annualLeaveBalance == null)
            {
                // 创建新的年假余额记录
                annualLeaveBalance = new Sys_Leave_Balance
                {
                    id = Guid.NewGuid(),
                    user_id = user.user_id,
                    user_no = user.user_no,
                    year = currentYear,
                    leave_type_id = annualLeaveType.id,
                    remaing_leave = remaing_leval,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now
                };
                _repository.Add(annualLeaveBalance);

                // 添加余额变动记录
                AddBalanceRecord(user, annualLeaveType, annualLeaveBalance.id, remaing_leval, remaing_leval, "系统自动初始化年假余额");
            }
            else
            {
                // 计算余额变动
                decimal previousBalance = annualLeaveBalance.remaing_leave ?? 0;
                decimal spend = remaing_leval - previousBalance;

                // 更新年假余额
                annualLeaveBalance.remaing_leave = remaing_leval;
                annualLeaveBalance.modify_id = UserContext.Current.UserId;
                annualLeaveBalance.modify_name = UserContext.Current.UserName;
                annualLeaveBalance.modify_date = DateTime.Now;

                _repository.Update(annualLeaveBalance);
                // 年假允许负数余额，所以不需要额外处理
                AddBalanceRecord(user, annualLeaveType, annualLeaveBalance.id, spend, remaing_leval, "系统自动更新年假余额");
            }
        }

        /// <summary>
        /// 确保非年假类型的余额不为负数
        /// </summary>
        private void EnsureNonNegativeBalance(Sys_Leave_Balance balance, bool allowNegative)
        {
            if (!allowNegative && balance.remaing_leave < 0)
            {
                balance.remaing_leave = 0;
            }
        }

        /// <summary>
        /// 添加假期余额变动记录
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="leaveType">请假类型</param>
        /// <param name="balanceId">假期余额ID</param>
        /// <param name="spend">消费额度</param>
        /// <param name="remaing_leval">剩余额度</param>
        private void AddBalanceRecord(Sys_User_New user, Sys_Leave_Type leaveType, Guid balanceId, decimal spend, decimal remaing_leval, string remark = "系统每月自动刷新假期余额")
        {
            try
            {
                var balanceRecord = new Sys_Leave_Balance_Record
                {
                    id = Guid.NewGuid(),
                    user_id = user.user_id,
                    user_no = user.user_no,
                    year = int.Parse(DateTime.Now.Year.ToString()),
                    leave_type_code = leaveType.leave_type_code,
                    leave_type_name = leaveType.leave_type_name,
                    is_leave = leaveType.is_leave,
                    pay_type = leaveType.pay_type,
                    spend = spend,
                    remaing_leave = remaing_leval,
                    leave_balance_id = balanceId,
                    remark = remark,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now
                };

                _leaveBalanceRecordRepository.Add(balanceRecord);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_BalanceService.AddBalanceRecord", e);
            }
        }

        #endregion
    }
}
