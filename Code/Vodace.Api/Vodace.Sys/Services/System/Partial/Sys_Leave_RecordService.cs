
using Dm.util;
using EnumsNET;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
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
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Sys_Leave_RecordService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Leave_RecordRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_Leave_TypeRepository _leaveTypeRepository;
        private readonly ISys_Leave_BalanceRepository _leaveBalanceRepository;
        private readonly ISys_Leave_HolidayRepository _leaveHolidayRepository;
        private readonly ISys_User_NewRepository _userNewRepository;
        private readonly ISys_Leave_AttachmentRepository _attachmentRepository;
        private readonly ISys_File_RecordsService _fileRecordsService;
        private readonly ISys_Leave_Balance_RecordRepository _leaveBalanceRecordRepository;

        [ActivatorUtilitiesConstructor]
        public Sys_Leave_RecordService(
            ISys_Leave_RecordRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            ISys_Leave_TypeRepository leaveTypeRepository,
            ISys_User_NewRepository userNewRepository,
            ISys_Leave_AttachmentRepository attachmentRepository,
            ISys_File_RecordsService fileRecordsService,
            ISys_Leave_BalanceRepository leaveBalanceRepository,
            ISys_Leave_HolidayRepository leaveHolidayRepository,
            ISys_Leave_Balance_RecordRepository leaveBalanceRecordRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _leaveTypeRepository = leaveTypeRepository;
            _userNewRepository = userNewRepository;
            _attachmentRepository = attachmentRepository;
            _fileRecordsService = fileRecordsService;
            _leaveBalanceRepository = leaveBalanceRepository;
            _leaveHolidayRepository = leaveHolidayRepository;
            _leaveBalanceRecordRepository = leaveBalanceRecordRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public WebResponseContent Add(LeaveRecordDto dto)
        {
            try
            {
                if (dto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");
                if(dto.user_no.IsNullOrEmpty() && dto.user_id <= 0) return WebResponseContent.Instance.Error(_localizationService.GetString("taking_leave"));

                if(dto.start_date > dto.end_date)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("end_earlier_start"));
                }

                var user = _userNewRepository.Find(a => a.user_id == dto.user_id || a.user_no == dto.user_no).FirstOrDefault();
                if (user == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("taking_leave"));
                }

                //period 0：全日 1：上午 2：下午
                //status 1:申请中pending,2:已批准Approved,3:拒绝rejrcted
                //场景1：当天可以根据period的情况下，允许请两次半天假（上午+下午），但不允许两条数据都是上午或下午
                //场景2：请假的日期不允许出现重叠，不限制是否已审批通过或正在审批中，已拒绝的除外
                //场景3：在场景2的情况下，允许请多次假，只要日期不重叠即可
                var exists = _repository.Find(p => p.delete_status == (int)SystemDataStatus.Valid
                    && p.user_id == user.user_id && (p.status == 1 || p.status == 2)
                    && (   // 新记录的开始时间在现有记录的时间段内
                        (dto.start_date >= p.start_date && dto.start_date <= p.end_date)
                        // 新记录的结束时间在现有记录的时间段内
                        || (dto.end_date >= p.start_date && dto.end_date <= p.end_date)
                        // 新记录包含现有记录
                        || (dto.start_date <= p.start_date && dto.end_date >= p.end_date)
                        // 现有记录包含新记录
                        || (p.start_date >= dto.start_date && p.end_date <= dto.end_date)
                    )
                    // 如果是半天假，还需要判断时段是否冲突
                    && (dto.period == 0 || p.period == 0 || dto.period == p.period)
                ).ToList();
                if (exists.Count > 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("leave_period_fall"));
                }

                Sys_Leave_Record model = new Sys_Leave_Record();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                model.user_id = user.user_id;
                model.user_no = user.user_no;
                model.leave_type_id = dto.leave_type_id.Value;
                model.start_date = dto.start_date.Value;
                model.end_date = dto.end_date.Value;
                model.period = dto.period;
                model.reason = dto.reason;
                if(dto.approver_id.HasValue)
                    model.approver_id = dto.approver_id;
                model.status = dto.status <= 0 ? 1 : dto.status;    //默认申请中
                model.from_email = dto.from_email;
                model.to_email = dto.to_email;
                model.cc_email = dto.cc_email;
                model.bcc_email = dto.bcc_email;

                //假期额度校验
                string checkRemainLeave = CheckRemaingLeave(model);
                if (!string.IsNullOrEmpty(checkRemainLeave))
                    return WebResponseContent.Instance.Error(checkRemainLeave);

                List<Sys_Leave_Attachment> lstAttachment = new List<Sys_Leave_Attachment>();
                if (dto.file != null && dto.file.Count > 0)
                {
                    //读取文件配置地址
                    var strFolderPath = $"{AppSetting.FileSaveSettings.LeaveAttachment}{user.user_id}\\";

                    foreach (var att in dto.file)
                    {
                        //保存文件
                        var saveFileResult = _fileRecordsService.SaveFileByPath(new List<IFormFile> { att }, strFolderPath);
                        var fileInfo = saveFileResult.data as List<FileInfoDto>;
                        if (fileInfo == null)
                        {
                            return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                        }

                        Sys_Leave_Attachment attachment = new Sys_Leave_Attachment();
                        attachment.leave_id = model.id;
                        attachment.file_path = strFolderPath;
                        attachment.file_name = fileInfo[0].file_name;
                        attachment.file_type = fileInfo[0].file_ext;

                        attachment.id = Guid.NewGuid();
                        attachment.delete_status = (int)SystemDataStatus.Valid;
                        attachment.create_id = UserContext.Current.UserId;
                        attachment.create_name = UserContext.Current.UserName;
                        attachment.create_date = DateTime.Now;

                        _attachmentRepository.Add(attachment);
                        lstAttachment.add(attachment);
                    }
                }

                _repository.Add(model);

                _repository.SaveChanges();
                return WebResponseContent.Instance.OK("Ok", new { record = model, attachment = lstAttachment });
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_RecordService.Add", e);
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
                    if(model.status == 2)
                    {
                        //可以更改为 取消/拒绝 状态，之后再删除
                        return WebResponseContent.Instance.Error(_localizationService.GetString("holiday_reviewed", [GetLeaveStatusName(model.status)]));
                    }
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
                Log4NetHelper.Error("Sys_Leave_RecordService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public WebResponseContent Edit(LeaveRecordEditDto dto)
        {
            try
            {
                if (!dto.id.HasValue) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                if (dto.user_no.IsNullOrEmpty() && dto.user_id <= 0) return WebResponseContent.Instance.Error(_localizationService.GetString("taking_leave"));

                if (dto.start_date > dto.end_date)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("end_earlier_start"));
                }

                var user = _userNewRepository.Find(a => a.user_id == dto.user_id || a.user_no == dto.user_no).FirstOrDefault();
                if (user == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("taking_leave"));
                }

                var exists = _repository.Find(p => p.delete_status == (int)SystemDataStatus.Valid && p.id != dto.id
                    && p.user_id == user.user_id && (p.status == 1 || p.status == 2)
                    && (   // 新记录的开始时间在现有记录的时间段内
                        (dto.start_date >= p.start_date && dto.start_date <= p.end_date)
                        // 新记录的结束时间在现有记录的时间段内
                        || (dto.end_date >= p.start_date && dto.end_date <= p.end_date)
                        // 新记录包含现有记录
                        || (dto.start_date <= p.start_date && dto.end_date >= p.end_date)
                        // 现有记录包含新记录
                        || (p.start_date >= dto.start_date && p.end_date <= dto.end_date)
                    )
                    // 如果是半天假，还需要判断时段是否冲突
                    && (dto.period == 0 || p.period == 0 || dto.period == p.period)
                ).ToList();
                if (exists.Count > 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("leave_period_fall"));
                }

                var model = repository.Find(p => p.id == dto.id).FirstOrDefault();
                if (dto != null)
                {
                    //if(model.status != 1)
                    //{
                    //    //当前请假记录已审核，不允许修改
                    //    return WebResponseContent.Instance.Error(_localizationService.GetString("holiday_reviewed", [GetLeaveStatusName(model.status)]));
                    //}

                    bool isCancel = false;
                    if (model.status == 2 && dto.status != model.status) isCancel = true;

                    //假期额度校验
                    string checkRemainLeave = CheckRemaingLeave(model, true, isCancel);
                    if (!string.IsNullOrEmpty(checkRemainLeave))
                        return WebResponseContent.Instance.Error(checkRemainLeave);

                    model.user_id = model.user_id;    //不允许修改员工ID
                    model.user_no = model.user_no;    //不允许修改员工编码
                    model.leave_type_id = dto.leave_type_id.Value;
                    model.start_date = dto.start_date.Value;
                    model.end_date = dto.end_date.Value;
                    model.period = dto.period;
                    model.reason = dto.reason;
                    if (dto.approver_id.HasValue)
                        model.approver_id = dto.approver_id;
                    model.status = dto.status; 
                    model.from_email = dto.from_email;
                    model.to_email = dto.to_email;
                    model.cc_email = dto.cc_email;
                    model.bcc_email = dto.bcc_email;

                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;

                    if (dto.file != null && dto.file.Count > 0)
                    {
                        //读取文件配置地址
                        var strFolderPath = $"{AppSetting.FileSaveSettings.LeaveAttachment}{user.user_id}\\";

                        foreach (var att in dto.file)
                        {
                            //保存文件
                            var saveFileResult = _fileRecordsService.SaveFileByPath(new List<IFormFile> { att }, strFolderPath);
                            var fileInfo = saveFileResult.data as List<FileInfoDto>;
                            if (fileInfo == null)
                            {
                                return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                            }

                            Sys_Leave_Attachment attachment = new Sys_Leave_Attachment();
                            attachment.leave_id = model.id;
                            attachment.file_path = strFolderPath;
                            attachment.file_name = fileInfo[0].file_name;
                            attachment.file_type = fileInfo[0].file_ext;

                            attachment.id = Guid.NewGuid();
                            attachment.delete_status = (int)SystemDataStatus.Valid;
                            attachment.create_id = UserContext.Current.UserId;
                            attachment.create_name = UserContext.Current.UserName;
                            attachment.create_date = DateTime.Now;

                            _attachmentRepository.Add(attachment);
                        }
                    }
                    _repository.Update(model);
                    _repository.SaveChanges();

                    return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));

                }
                else return WebResponseContent.Instance.Error(_localizationService.GetString("leave_record_null"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_RecordService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        public WebResponseContent Review(LeaveRecordReviewDto dto)
        {
            try
            {
                if (!dto.id.HasValue) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                if (!dto.approver_id.HasValue) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                var model = repository.Find(p => p.id == dto.id && p.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                if (model != null)
                {
                    //if (model.status != 1)
                    //{
                    //    //当前请假记录已审核，不允许修改
                    //    return WebResponseContent.Instance.Error(_localizationService.GetString("holiday_reviewed", [GetLeaveStatusName(model.status)]));
                    //}

                    bool isCancel = false;
                    if (model.status == 2 && dto.status != model.status) isCancel = true;

                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;

                    model.status = dto.status;
                    model.approver_id = dto.approver_id;

                    //假期额度校验
                    string checkRemainLeave = CheckRemaingLeave(model, true, isCancel);
                    if (!string.IsNullOrEmpty(checkRemainLeave))
                        return WebResponseContent.Instance.Error(checkRemainLeave);

                    var res = _repository.Update(model);
                    _repository.SaveChanges();

                    return WebResponseContent.Instance.OK(_localizationService.GetString("holiday_approval", [GetLeaveStatusName(model.status)]) + " " + _localizationService.GetString("edit") + _localizationService.GetString("success"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("leave_record_null")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_RecordService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetLeaveRecordList(PageInput<SearchLeaveRecordDto> dto)
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
                            join na in userInfo
                            on r.approver_id equals na.id into userNewaJoin
                            from una in userNewaJoin.DefaultIfEmpty()
                            select new LeaveRecordListDto()
                            {
                                // 获取请假记录所有字段
                                id = r.id,
                                user_id = r.user_id,
                                user_no = r.user_no,
                                leave_type_id = r.leave_type_id,
                                start_date = r.start_date,
                                end_date = r.end_date,
                                period = r.period,
                                reason = r.reason,
                                approver_id = r.approver_id,
                                from_email = r.from_email,
                                to_email = r.to_email,
                                cc_email = r.cc_email,
                                bcc_email = r.bcc_email,
                                //attachment = 
                                status = r.status,
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
                                company_id = un.company_id,
                                // 审核人信息
                                approver_true_name = una.user_true_name,
                                approver_name_eng = una.user_name_eng,
                                approver_name = una.user_name,
                                approver_user_no = una.user_no
                            };

                if(!UserContext.Current.UserInfo.IsSuperAdmin)
                    query = query.WhereIF(false, p => p.company_id == UserContext.Current.UserInfo.Company_Id);

                // 获取查询条件
                var searchDto = dto.search;

                // 添加查询条件过滤
                if (searchDto != null)
                {
                    query = query.WhereIF(searchDto.id.HasValue, p => p.id == searchDto.id);
                    query = query.WhereIF(searchDto.user_id > 0, p => p.user_id == searchDto.user_id);
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.user_no), p => p.user_no.Contains(searchDto.user_no));
                    query = query.WhereIF(searchDto.leave_type_id.HasValue, p => p.leave_type_id == searchDto.leave_type_id);
                    query = query.WhereIF(searchDto.start_date.HasValue, p => p.start_date >= searchDto.start_date);
                    query = query.WhereIF(searchDto.end_date.HasValue, p => p.start_date <= searchDto.end_date);
                    query = query.WhereIF(searchDto.period > -1, p => p.period == searchDto.period);
                    query = query.WhereIF(searchDto.status > 0, p => p.status == searchDto.status);
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.reason), p => p.reason.Contains(searchDto.reason));
                    query = query.WhereIF(searchDto.approver_id.HasValue, p => p.approver_id == searchDto.approver_id);
                    query = query.WhereIF(searchDto.status > 0, p => p.status == searchDto.status);
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.from_email), p => p.from_email.Contains(searchDto.from_email));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.to_email), p => p.to_email.Contains(searchDto.to_email));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.cc_email), p => p.cc_email.Contains(searchDto.cc_email));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.bcc_email), p => p.bcc_email.Contains(searchDto.bcc_email));
                }

                if (!string.IsNullOrEmpty(dto.sort_field))
                    query = query.OrderByDynamic(dto.sort_field, dto.sort_type);
                else
                    query = query.OrderByDescending(x => x.create_date);

                var sql = query.ToQueryString();

                // 执行分页查询（使用项目提供的扩展方法）
                var result = await query.GetPageResultAsync(dto);

                if(result.data != null)
                {
                    var leave_ids = result.data.Select(item => item.id).Distinct().ToList();
                    var leaveAttac = _attachmentRepository
                        .FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid && leave_ids.Contains(p.leave_id))
                        .Select(a => new AttachmentDto() {
                            id = a.id,
                            leave_id = a.leave_id,
                            file_name = a.file_name,
                            file_path = a.file_path,
                            file_type = a.file_type
                        })
                        .ToList();
                    foreach (var item in result.data)
                    {
                        item.attachment = leaveAttac.Where(a => a.leave_id == item.id).ToList();
                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch(Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_RecordService.GetLeaveRecordList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 假期类型
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetLeaveRecordType()
        {
            try
            {
                var list = await _leaveTypeRepository
                    .WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid && p.is_leave == 0)
                    .Select(p => new
                    {
                        id = p.id,
                        name = p.leave_type_name,
                        code = p.leave_type_code
                    })
                    .ToListAsync();

                return WebResponseContent.Instance.OK("OK", list);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_RecordService.GetLeaveRecordType", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 假期批核状态
        /// </summary>
        /// <returns></returns>
        public WebResponseContent GetLeaveRecordStatus()
        {
            return WebResponseContent.Instance.OK("OK", GetLeaveStatus());
        }

        #region  -- 私有公共函数 --

        /// <summary>
        /// 校验员工假期额度
        /// </summary>
        /// <param name="model">请假记录</param>
        /// <param name="isUpdate">是否更新假期额度</param>
        /// <param name="isCancel">已审批后的假期，是否取消，默认：false，慎用</param>
        /// <returns></returns>
        /// <remarks>跨年的循环校验</remarks>
        private string CheckRemaingLeave(Sys_Leave_Record model, bool isUpdate = false, bool isCancel = false)
        {
            string res = string.Empty;

            try
            {
                // 获取用户的所有假期余额记录
                var lstBanlance = _leaveBalanceRepository.Find(a => a.user_id == model.user_id && a.leave_type_id == model.leave_type_id && a.delete_status == (int)SystemDataStatus.Valid).ToList();
                if (lstBanlance == null || lstBanlance.Count == 0)
                {
                    //无请假余额记录
                    return _localizationService.GetString("holiday_insufficient");
                }
                else
                {
                    // 判断是否为全天请假
                    if (model.period == 0)
                    {
                        // 计算请假总天数（扣除节假日）
                        var totalDiff = CalculateDaysBetweenDates((DateTime)model.start_date, (DateTime)model.end_date, (int)model.period) - (GetHolidayDays((DateTime)model.start_date, (DateTime)model.end_date));

                        if (totalDiff == 0)
                        {
                            //请假日期与法定假期等于0时，不需要请假
                            return _localizationService.GetString("holiday_public");
                        }

                        // 处理跨年度请假情况
                        var startYear = model.start_date.Year.ToString();
                        var endYear = model.end_date.Year.ToString();

                        if (startYear == endYear)
                        {
                            // 同一年度的请假
                            var balance = lstBanlance.FirstOrDefault(b => b.year == startYear);
                            if (balance == null || balance.remaing_leave < (decimal)totalDiff)
                            {
                                //判断是否为年假，只有年假允许有负数情况出现

                                //请假余额不足
                                return _localizationService.GetString("holiday_insufficient");
                            }

                            //减去相应请假余额
                            balance.remaing_leave -= (decimal)totalDiff;

                            if (isUpdate && model.status == 2)
                            {
                                //判断审批人是否存在
                                var approver = _userNewRepository.Find(a => a.id == model.approver_id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                                if (approver == null)
                                    return _localizationService.GetString("leave_approver_null");

                                balance.modify_id = UserContext.Current.UserId;
                                balance.modify_name = UserContext.Current.UserName;
                                balance.modify_date = DateTime.Now;
                                _leaveBalanceRepository.Update(balance);

                                res = AddBalanceRecord(balance, model, (decimal)totalDiff);
                                if (!string.IsNullOrEmpty(res)) return res;
                            }

                            if (isCancel)
                            {
                                //判断审批人是否存在
                                var approver = _userNewRepository.Find(a => a.id == model.approver_id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                                if (approver == null)
                                    return _localizationService.GetString("leave_approver_null");

                                balance.remaing_leave += (decimal)totalDiff;
                                balance.modify_id = UserContext.Current.UserId;
                                balance.modify_name = UserContext.Current.UserName;
                                balance.modify_date = DateTime.Now;
                                _leaveBalanceRepository.Update(balance);

                                res = AddBalanceRecord(balance, model, (decimal)totalDiff);
                                if (!string.IsNullOrEmpty(res)) return res;
                            }
                        }
                        else
                        {
                            // 跨年度请假，需要分别计算不同年份的请假天数
                            var yearDays = new Dictionary<string, double>();

                            // 计算起始年份的请假天数（从start_date到年底）
                            var startYearEndDate = new DateTime(model.start_date.Year, 12, 31);
                            double startYearDays = CalculateDaysBetweenDates((DateTime)model.start_date, startYearEndDate, (int)model.period) - GetHolidayDays((DateTime)model.start_date, startYearEndDate);
                            if (startYearDays > 0)
                            {
                                yearDays[startYear] = startYearDays;
                            }

                            // 计算中间年份的请假天数（全年）
                            for (int year = model.start_date.Year + 1; year < model.end_date.Year; year++)
                            {
                                var yearStart = new DateTime(year, 1, 1);
                                var yearEnd = new DateTime(year, 12, 31);
                                double days = CalculateDaysBetweenDates(yearStart, yearEnd, (int)model.period) - GetHolidayDays(yearStart, yearEnd);
                                if (days > 0)
                                {
                                    yearDays[year.ToString()] = days;
                                }
                            }

                            // 计算结束年份的请假天数（从年初到end_date）
                            var endYearStartDate = new DateTime(model.end_date.Year, 1, 1);
                            double endYearDays = CalculateDaysBetweenDates(endYearStartDate, (DateTime)model.end_date, (int)model.period) - GetHolidayDays(endYearStartDate, (DateTime)model.end_date);
                            if (endYearDays > 0)
                            {
                                yearDays[endYear] = endYearDays;
                            }

                            // 检查并扣除各年份的请假余额
                            foreach (var yearDay in yearDays)
                            {
                                var balance = lstBanlance.FirstOrDefault(b => b.year == yearDay.Key);
                                if (balance == null || balance.remaing_leave < (decimal)yearDay.Value)
                                {
                                    //判断是否为年假，只有年假允许有负数情况出现

                                    //某一年的请假余额不足
                                    return _localizationService.GetString("holiday_insufficient");
                                }
                            }

                            // 更新各年份的请假余额
                            if (isUpdate && model.status == 2)
                            {
                                //判断审批人是否存在
                                var approver = _userNewRepository.Find(a => a.id == model.approver_id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                                if (approver == null)
                                    return _localizationService.GetString("leave_approver_null");

                                foreach (var yearDay in yearDays)
                                {
                                    var balance = lstBanlance.FirstOrDefault(b => b.year == yearDay.Key);
                                    if (balance != null)
                                    {
                                        balance.remaing_leave -= (decimal)yearDay.Value;
                                        balance.modify_id = UserContext.Current.UserId;
                                        balance.modify_name = UserContext.Current.UserName;
                                        balance.modify_date = DateTime.Now;
                                        _leaveBalanceRepository.Update(balance);

                                        res = AddBalanceRecord(balance, model, (decimal)yearDay.Value);
                                        if (!string.IsNullOrEmpty(res)) return res;
                                    }
                                }
                            }


                            if (isCancel)
                            {
                                //判断审批人是否存在
                                var approver = _userNewRepository.Find(a => a.id == model.approver_id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                                if (approver == null)
                                    return _localizationService.GetString("leave_approver_null");

                                foreach (var yearDay in yearDays)
                                {
                                    var balance = lstBanlance.FirstOrDefault(b => b.year == yearDay.Key);
                                    if (balance != null)
                                    {
                                        balance.remaing_leave += (decimal)yearDay.Value;
                                        balance.modify_id = UserContext.Current.UserId;
                                        balance.modify_name = UserContext.Current.UserName;
                                        balance.modify_date = DateTime.Now;
                                        _leaveBalanceRepository.Update(balance);

                                        res = AddBalanceRecord(balance, model, (decimal)yearDay.Value);
                                        if (!string.IsNullOrEmpty(res)) return res;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //半天请假，不可能请超过1天（需求限制）
                        if (CalculateDaysBetweenDates(model.start_date, model.end_date, (int)model.period) > 0.5)
                        {
                            return _localizationService.GetString("half_day_leave_limit");
                        }

                        // 半天请假只涉及一个日期，获取该日期所在年份
                        var leaveYear = model.start_date.Year.ToString();
                        var balance = lstBanlance.FirstOrDefault(b => b.year == leaveYear);

                        if (balance == null || balance.remaing_leave < 0.5m)
                        {
                            //判断是否为年假，只有年假允许有负数情况出现

                            //请假余额不足
                            return _localizationService.GetString("holiday_insufficient");
                        }

                        //减去相应请假余额
                        balance.remaing_leave -= 0.5m;

                        if (isUpdate && model.status == 2)
                        {
                            //判断审批人是否存在
                            var approver = _userNewRepository.Find(a => a.id == model.approver_id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                            if (approver == null)
                                return _localizationService.GetString("leave_approver_null");

                            balance.modify_id = UserContext.Current.UserId;
                            balance.modify_name = UserContext.Current.UserName;
                            balance.modify_date = DateTime.Now;
                            _leaveBalanceRepository.Update(balance);

                            res = AddBalanceRecord(balance, model, 0.5m);
                            if (!string.IsNullOrEmpty(res)) return res;
                        }
                        if (isCancel)
                        {
                            //判断审批人是否存在
                            var approver = _userNewRepository.Find(a => a.id == model.approver_id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                            if (approver == null)
                                return _localizationService.GetString("leave_approver_null");

                            balance.remaing_leave += 0.5m;
                            balance.modify_id = UserContext.Current.UserId;
                            balance.modify_name = UserContext.Current.UserName;
                            balance.modify_date = DateTime.Now;
                            _leaveBalanceRepository.Update(balance);

                            res = AddBalanceRecord(balance, model, 0.5m);
                            if (!string.IsNullOrEmpty(res)) return res;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Leave_RecordService.CheckRemaingLeave", e);
                return _localizationService.GetString("system_error");
            }

            return res;
        }

        /// <summary>
        /// 获取假期状态名称
        /// </summary>
        /// <param name="statusValue"></param>
        /// <returns></returns>
        private string GetLeaveStatusName(int statusValue)
        {
            string res = "";
            try
            {
                var statusList = GetLeaveStatus();
                foreach (var status in statusList)
                {
                    var id = (int)status.GetType().GetProperty("id").GetValue(status);
                    if (id == statusValue)
                    {
                        //UserContext.Current.UserInfo.Lang //0-中文，1-繁体，2-英文
                        if (UserContext.Current.UserInfo.Lang == 0 || UserContext.Current.UserInfo.Lang == null)
                            res = (string)status.GetType().GetProperty("name_cht").GetValue(status);
                        else if (UserContext.Current.UserInfo.Lang == 1)
                            res = (string)status.GetType().GetProperty("name_cht").GetValue(status);
                        else
                            res = (string)status.GetType().GetProperty("name_eng").GetValue(status);
                    }
                }
            }
            catch(Exception e)
            {

            }
            return res;
        }

        /// <summary>
        /// 获取假期批核状态
        /// </summary>
        /// <returns></returns>
        private List<object> GetLeaveStatus()
        {
            //UserContext.Current.UserInfo.Lang //0-中文，1-繁体，2-英文
            var list = new List<object>();
            list.Add(new { id = 1, name_cht = "申请中", name_eng = "Pending", remark = "" });
            list.Add(new { id = 2, name_cht = "已批准", name_eng = "Approved", remark = "" });
            list.Add(new { id = 3, name_cht = "拒绝", name_eng = "Rejrcted", remark = "" });
            list.Add(new { id = 4, name_cht = "取消", name_eng = "Canceled", remark = "" });
            return list;
        }

        /// <summary>
        /// 获取请假期间的法定假日天数
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private int GetHolidayDays(DateTime startDate, DateTime endDate)
        {
            int res = 0;

            try
            {
                if (startDate > endDate) return 0;

                //未嵌入有薪假期的过滤（是否需要考虑无薪假的过滤？）
                var lstHolidayList = _leaveHolidayRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid)
                                                          .Select(a => a.date.Date) // 只选择日期部分并提前转换
                                                          .Distinct() // 去重
                                                          .ToList();
                if (lstHolidayList != null)
                {
                    var holidayDates = new HashSet<DateTime>(lstHolidayList);
                    //遍历开始到结束日期之间的所有日期
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        if (holidayDates.Contains(date.Date))
                        {
                            res++;
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }

            return res;
        }

        /// <summary>
        /// 获取指定日期范围内的公共假期天数（包含周六日统计）
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="holidays">公共假期列表</param>
        /// <returns>公共假期天数</returns>
        public static int GetHolidayDays(DateTime startDate, DateTime endDate, List<Sys_Leave_Holiday> holidays)
        {
            if (holidays == null || !holidays.Any())
            {
                return 0;
            }

            // 确保开始日期不大于结束日期
            if (startDate > endDate)
            {
                var temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            // 仅比较日期部分
            startDate = startDate.Date;
            endDate = endDate.Date;

            // 统计日期范围内的公共假期天数
            int holidayDays = holidays.Count(h =>
                h.date.Date >= startDate &&
                h.date.Date <= endDate &&
                h.date.DayOfWeek != DayOfWeek.Saturday &&
                h.date.DayOfWeek != DayOfWeek.Sunday); // 仅统计非周末的公共假期

            return holidayDays;
        }

        /// <summary>
        /// 计算两个日期之间的天数，忽略时分秒部分，根据日期顺序返回正数或负数
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="period">0：全日 1：上午 2：下午</param>
        /// <returns>两个日期之间的天数差（正数表示endDate在startDate之后，负数表示endDate在startDate之前）</returns>
        private double CalculateDaysBetweenDates(DateTime startDate, DateTime endDate, int period = 0)
        {
            // 检查参数有效性
            if (startDate > endDate)
            {
                throw new ArgumentException(_localizationService.GetString("end_earlier_start"));
            }

            // 仅比较日期部分，忽略时间
            DateTime start = startDate.Date;
            DateTime end = endDate.Date;

            // 计算天数差
            double days = (end - start).TotalDays + 1; // 包含起止日期

            // 根据假别类型调整天数
            switch (period)
            {
                case 0: // 全日
                    return days;
                case 1: // 上午
                case 2: // 下午
                        // 如果是同一天，算0.5天
                    if (start == end)
                    {
                        return 0.5;
                    }
                    // 跨天的半天假，首天算0.5天，中间算整天，末天算0.5天
                    return (days - 1) + 0.5;
                default:
                    return days;
            }
        }

        /// <summary>
        /// 计算两个日期之间的工作日天数（排除周末和公共假期）
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="holidays">公共假期列表</param>
        /// <param name="includeEndDate">是否包含结束日期</param>
        /// <returns>工作日天数</returns>
        public static decimal CalculateDaysBetweenDates(DateTime startDate, DateTime endDate, List<Sys_Leave_Holiday> holidays = null, bool includeEndDate = true)
        {
            // 确保开始日期不大于结束日期
            if (startDate > endDate)
            {
                var temp = startDate;
                startDate = endDate;
                endDate = temp;
            }

            // 仅比较日期部分
            startDate = startDate.Date;
            endDate = endDate.Date;

            // 如果开始日期和结束日期相同
            if (startDate == endDate)
            {
                // 检查是否为工作日且不是公共假期
                if (IsWorkDay(startDate, holidays))
                {
                    return includeEndDate ? 1 : 0;
                }
                return 0;
            }

            // 计算总天数（包含两端）
            int totalDays = (endDate - startDate).Days + (includeEndDate ? 1 : 0);
            int weekendDays = 0;
            int holidayDays = 0;

            // 计算周末天数
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekendDays++;
                }
            }

            // 计算公共假期天数
            if (holidays != null && holidays.Any())
            {
                holidayDays = GetHolidayDays(startDate, endDate, holidays);
            }

            // 工作日天数 = 总天数 - 周末天数 - 公共假期天数
            decimal workDays = totalDays - weekendDays - holidayDays;

            return Math.Max(0, workDays);
        }

        /// <summary>
        /// 判断指定日期是否为工作日（非周末且非公共假期）
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="holidays">公共假期列表</param>
        /// <returns>是否为工作日</returns>
        private static bool IsWorkDay(DateTime date, List<Sys_Leave_Holiday> holidays = null)
        {
            // 检查是否为周末
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // 检查是否为公共假期
            if (holidays != null && holidays.Any(h => h.date.Date == date.Date))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加假期余额变动记录
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="leaveType">请假类型</param>
        /// <param name="balanceId">假期余额ID</param>
        /// <param name="spend">消费额度</param>
        /// <param name="remaing_leval">剩余额度</param>
        private string AddBalanceRecord(Sys_Leave_Balance balance, Sys_Leave_Record record, decimal spend)
        {
            try
            {
                var leaveType = _leaveTypeRepository.Find(a => a.id == record.leave_type_id).FirstOrDefault();

                if (leaveType != null)
                {
                    _leaveBalanceRecordRepository.Add(new Sys_Leave_Balance_Record
                    {
                        id = Guid.NewGuid(),
                        user_id = record.user_id,
                        user_no = record.user_no,
                        year = int.Parse(DateTime.Now.Year.ToString()),
                        leave_type_code = leaveType.leave_type_code,
                        leave_type_name = leaveType.leave_type_name,
                        is_leave = leaveType.is_leave,
                        pay_type = leaveType.pay_type,
                        spend = spend,
                        remaing_leave = balance.remaing_leave,
                        leave_balance_id = balance.id,
                        remark = $"{record.user_no}{record.reason}, {GetLeaveStatusName(record.status)}",
                        delete_status = (int)SystemDataStatus.Valid,
                        create_id = UserContext.Current.UserId,
                        create_name = UserContext.Current.UserInfo.UserName,
                        create_date = DateTime.Now
                    });
                    return string.Empty;
                }
                return _localizationService.GetString("leave_type_null");
            }
            catch (Exception e)
            {
                return _localizationService.GetString("system_error");
            }
        }


        #endregion
    }
}
