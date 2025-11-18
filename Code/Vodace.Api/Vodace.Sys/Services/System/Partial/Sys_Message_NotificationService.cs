
using AutoMapper;
using Dm.util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text.Json;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Configuration;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Hubs;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Sys_Message_NotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Message_NotificationRepository _repository;//访问数据库
        private readonly IBiz_Upcoming_EventsRepository _eventsRepository;
        private readonly IBiz_QuotationRepository _quotationRepository;
        private readonly ISys_ContactRepository _ContactRepository;
        private readonly ISys_User_NewRepository _User_NewRepository;
        private readonly IBiz_ProjectRepository _ProjectRepository;
        private readonly IHomePageMessageSender _messageSender;
        private readonly ISys_ConfigRepository _configRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_Message_NotificationService(
            ISys_Message_NotificationRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            IBiz_Upcoming_EventsRepository eventsRepository,
            IBiz_QuotationRepository quotationRepository,
            ISys_ContactRepository contactRepository,
            ISys_User_NewRepository user_NewRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            IHomePageMessageSender messageSender,
            ISys_ConfigRepository configRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _eventsRepository = eventsRepository;
            _quotationRepository = quotationRepository;
            _ContactRepository = contactRepository;
            _User_NewRepository = user_NewRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _messageSender = messageSender;
            _configRepository = configRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }


        public WebResponseContent CheckEventTask()
        {
            try
            {
                Log4NetHelper.Info($"检测是否有临近到期事件开始...");
                int leftDay = AppSetting.Notice.ExpiryDay;
                var all_lstEventData = _eventsRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid || d.delete_status == (int)SystemDataStatus.CompleteClose).ToList().Where(d => CommonHelper.DiffDays(d.closing_date) <= leftDay).ToList();
                var lstEventData = all_lstEventData.Where(d => d.delete_status == (int)SystemDataStatus.Valid).ToList();
                var lstCompleteData = all_lstEventData.Where(d => d.delete_status == (int)SystemDataStatus.CompleteClose).ToList();
                Log4NetHelper.Info($"共有{lstEventData.Count}条临近到期事件需要同步,{lstCompleteData.Count}条记录关闭");

                var lstUser = _User_NewRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid && d.contact_id != null && d.contact_id != Guid.Empty).ToList();
                var lstNotice = _repository.Find(d => d.status == (int)MessageStatus.Unread || d.status == (int)MessageStatus.Expected).ToList();
                var lstContact = _ContactRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid).ToList();

                List<Sys_Message_Notification> lstAdd = new List<Sys_Message_Notification>();
                List<Sys_Message_Notification> lstUpd = new List<Sys_Message_Notification>();
                List<Biz_Upcoming_Events> lstDel = new List<Biz_Upcoming_Events>();
                List<Sys_Message_Notification> lstMsgDel = new List<Sys_Message_Notification>();

                foreach (var item in lstCompleteData)
                {
                    var delMessage = lstNotice.Where(d => d.relation_id == item.relation_id).FirstOrDefault();
                    if (delMessage != null)
                    {
                        delMessage.status = (int)MessageStatus.Processed;
                        delMessage.modify_date = DateTime.Now;
                        delMessage.modify_name = UserContext.Current.UserName;
                        delMessage.modify_id = UserContext.Current.UserId;
                        lstMsgDel.Add(delMessage);
                    }
                }
                Log4NetHelper.Info($"共有{lstMsgDel.Count}条消息记录需要更新为【已处理】状态");
                if (lstMsgDel.Count > 0) _repository.UpdateRange(lstMsgDel);
                foreach (var item in lstEventData)
                {
                    var diffDays = CommonHelper.DiffDays(item.closing_date);
                    var isExpected = item.closing_date < DateTime.Now;
                    var title = diffDays > 0 ? $"【{item.event_name}】距离截至日期还剩{diffDays}天" : $"【{item.event_name}】已逾期{-diffDays}天";
                    Log4NetHelper.Info($"{title}");

                    var contact = lstContact.Where(d => d.id == item.recipient_user_id).FirstOrDefault();
                    var user = lstUser.Where(d => d.contact_id == contact?.id).FirstOrDefault();

                    var eventType = CommonHelper.GetUpcomingEventsStr(item.event_type);
                    var entOldData1 = lstNotice.Where(d => d.relation_id == item.relation_id).FirstOrDefault();
                    if (entOldData1 != null)
                    {
                        Log4NetHelper.Info($"事件类型：{eventType}_更新数据：{entOldData1.id}");
                        entOldData1.msg_title = title;
                        entOldData1.days_left_to_close = diffDays;
                        entOldData1.msg_content = item.remark;
                        entOldData1.status = isExpected ? (int)MessageStatus.Expected : (int)MessageStatus.Unread;
                        entOldData1.modify_id = UserContext.Current.UserId;
                        entOldData1.modify_date = DateTime.Now;
                        entOldData1.modify_name = UserContext.Current.UserName ?? "admin";
                        lstUpd.add(entOldData1);
                    }
                    else
                    {
                        Log4NetHelper.Info($"事件类型：{eventType}_新增数据");
                        lstAdd.add(new Sys_Message_Notification
                        {
                            id = Guid.NewGuid(),
                            msg_type = (int)MessageTypeEnum.Message,
                            msg_title = title,
                            days_left_to_close = diffDays,
                            relation_id = item.relation_id,
                            msg_content = item.remark,
                            status = isExpected ? (int)MessageStatus.Expected : (int)MessageStatus.Unread,
                            receive_user = user.user_name,
                            create_id = UserContext.Current.UserId,
                            create_date = DateTime.Now,
                            create_name = UserContext.Current.UserName ?? "admin",
                        });
                    }
                }
                if (lstAdd.Count > 0) _repository.AddRange(lstAdd);
                if (lstUpd.Count > 0) _repository.UpdateRange(lstUpd);
                if (lstMsgDel.Count > 0) _repository.UpdateRange(lstMsgDel);
                _repository.SaveChanges();
                Log4NetHelper.Info($"检测快过期事件结束");
                return WebResponseContent.Instance.OK("Ok");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Info($"检测快过期事件异常：" + ex.Message);
                return WebResponseContent.Instance.Error("CheckEventTask异常：" + ex.Message);
            }
        }

        public WebResponseContent GetList(string user_name)
        {
            if (string.IsNullOrEmpty(user_name)) return WebResponseContent.Instance.Error(_localizationService.GetString("connot_be_empty"));
            var list = _repository.Find(d => d.receive_user == user_name && d.status < (int)MessageStatus.Close).OrderBy(d => d.status).ThenBy(d => d.create_date).Select(d =>
            new MessageNotificationDto
            {
                id = d.id,
                msg_title = d.msg_title,
                msg_content = d.msg_content,
                msg_type = d.msg_type,
                msg_type_str = CommonHelper.GetMessageTypeStr(d.msg_type),
                status = d.status,
                status_str = CommonHelper.GetMessageStatusStr(d.status),
                modify_date = d.modify_date,
                receive_user = user_name,
                relation_id = d.relation_id,
                create_date = d.create_date
            }).ToList();
            return WebResponseContent.Instance.OK("Ok", list);
        }

        public WebResponseContent UpdateStatus(Guid id, int status)
        {
            try
            {
                var data = _repository.Find(d => d.id == id).FirstOrDefault();
                if (data != null)
                {
                    data.status = status;
                    data.modify_date = DateTime.Now;
                    data.modify_id = UserContext.Current.UserId;
                    data.modify_name = UserContext.Current.UserName;

                    var res = _repository.Update(data, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<bool> UserOffline(string userName)
        {
            return await _messageSender.UserOffline(userName);
        }

        public async Task<WebResponseContent> PushMessageToUser()
        {
            try
            {
                Log4NetHelper.Info("开始执行消息推送&邮件推送...");
                var lstContact = _ContactRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid).ToList();
                var lstUser = _User_NewRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid).ToList();
                var list = _repository.Find(d => d.status == (int)MessageStatus.Unread && d.days_left_to_close != null).ToList();
                var config = _configRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid && d.config_type == (int)ConfigEnum.Message).ToList();
                int day0 = 0, day3 = 3, day7 = 7;
                if (config.Count >= 3)
                {
                    day0 = Convert.ToInt32(config.Where(d => d.config_key == "day0").FirstOrDefault()?.config_value);
                    day3 = Convert.ToInt32(config.Where(d => d.config_key == "day3").FirstOrDefault()?.config_value);
                    day7 = Convert.ToInt32(config.Where(d => d.config_key == "day7").FirstOrDefault()?.config_value);
                }
                List<MailDto> lstMail = new List<MailDto>();
                var url = AppSetting.WebUrl;//"http://192.168.31.249:8080";
                var mailTo = string.Empty;
                var userName = string.Empty;
                var userNameEng = string.Empty;
                Log4NetHelper.Info($"消息推送-共有{list.Count}条记录！");
                foreach (var item in list)
                {
                    var userInfo = lstUser.Where(d => d.user_name == item.receive_user).FirstOrDefault();
                    mailTo = userInfo?.email;
                    userName = userInfo?.user_true_name;
                    userNameEng = userInfo?.user_name_eng;
                    if (string.IsNullOrEmpty(mailTo))
                    {
                        userName = lstContact.Where(d => d.name_eng == item.receive_user).FirstOrDefault()?.name_cht;
                        userNameEng = lstContact.Where(d => d.name_eng == item.receive_user).FirstOrDefault()?.name_eng;
                        mailTo = lstContact.Where(d => d.name_eng == item.receive_user).FirstOrDefault()?.email;
                    }
                    //Log4NetHelper.Info($"消息推送-推送频率7天、3天、当天、过期");
                    //消息推送频率7天、3天、当天
                    if (item.days_left_to_close == day7 || item.days_left_to_close == day3 || item.days_left_to_close == day0)
                    {
                        Log4NetHelper.Info($"消息推送-站内信-推送频率{item.days_left_to_close}天，接收人：{item.receive_user}，推送内容：{item.msg_title}");
                        await _messageSender.SendMessageToUser(item.receive_user, "任务提醒", item.msg_title);
                        if (!string.IsNullOrEmpty(mailTo))
                        {
                            var mail_info = new MailDto { Title = "工地管理系统待办任务提醒", DiffDays = item.days_left_to_close, Content = item.msg_content, MailTo = mailTo, IsExpected = false, UserName = userName };
                            Log4NetHelper.Info($"消息推送-邮件-推送频率{item.days_left_to_close}天，接收人：{mailTo}，推送内容：{item.msg_content}");
                            lstMail.Add(mail_info);
                        }
                    }
                    else
                    {
                        Log4NetHelper.Info($"消息推送-站内信-推送频率{item.days_left_to_close}天,已过期不用推送");
                        if (!string.IsNullOrEmpty(mailTo))
                        {
                            var mail_info = new MailDto { Title = "工地管理系统待办任务提醒", DiffDays = item.days_left_to_close, Content = item.msg_content, MailTo = mailTo, IsExpected = true, UserName = userName };
                            Log4NetHelper.Info($"消息推送-邮件-推送频率{item.days_left_to_close}天，接收人：{mailTo}，推送内容：{item.msg_content}");
                            lstMail.Add(mail_info);
                        }
                    }
                }
                if (lstMail.Count > 0)
                {
                    var group = (from d in lstMail
                                 group d by d.MailTo into g
                                 select new
                                 {
                                     key = g.Key,
                                     count = g.Count()
                                 }).ToList();
                    var tasks = new List<MailQnTasks>();
                    var user = string.Empty;
                    var toUser = string.Empty;
                    int? overdueCount = 0;
                    int? dueSoonCount = 0;
                    int? diffDays = 0;
                    var mantyCount = 0;
                    foreach (var item in group)
                    {
                        if (item.count > 1)
                        {
                            mantyCount = item.count;
                            var many_data = lstMail.Where(d => d.MailTo == item.key).ToList();
                            var content_data = many_data.Select(d => d.Content).ToList();
                            for (int i = 0; i < content_data.Count; i++)
                            {
                                var qn_data = JsonSerializer.Deserialize<EventQnDto>(content_data[i].ToString());
                                diffDays = CommonHelper.DiffDays(Convert.ToDateTime(qn_data.closing_date));
                                var status = diffDays >= 0 ? "<span style='color:orange;'>臨到期</span>" : "<span style='color:red;'>已逾期</span>";
                                var taskStage = CommonHelper.GetUpcomingEventsStr(qn_data.qn_type);
                                string statusLang = diffDays >= 0 ? "临到期" : "已逾期", dateStr = DateTime.Now.ToString("yyyy年MM月dd日"), reminder = "";
                                switch (UserContext.Current.UserInfo.Lang)
                                {
                                    case (int)LangType.zh_CN:
                                        statusLang = diffDays >= 0 ? "即将到期" : "已逾期";
                                        reminder = diffDays > 0 ? $"剩余{diffDays}天" : diffDays == 0 ? "今日截止" : $"逾期{-diffDays}天";
                                        break;
                                    case (int)LangType.zh_TW:
                                        statusLang = diffDays >= 0 ? "即將到期" : "已逾期";
                                        reminder = diffDays > 0 ? $"剩餘{diffDays}天" : diffDays == 0 ? "今日截止" : $"逾期{-diffDays}天";
                                        taskStage = ChineseConverter.Convert(taskStage, ChineseConversionDirection.SimplifiedToTraditional);
                                        break;
                                    case (int)LangType.en_US:
                                        statusLang = diffDays >= 0 ? "Expires Soon" : "Overdue";
                                        reminder = diffDays > 0 ? $"{diffDays} Days Remaining" : diffDays == 0 ? "Deadline Today" : $"{-diffDays} Days Overdue";
                                        taskStage = CommonHelper.GetUpcomingEventsStrEng(qn_data.qn_type);
                                        break;
                                    default:
                                        break;
                                }
                                tasks.Add(new MailQnTasks { ContractNo = qn_data.qn_no, Stage = taskStage, Deadline = qn_data.closing_date, Status = statusLang, Reminder = reminder, IsExpected = diffDays < 0, DiffDays = diffDays });
                            }
                            user = many_data.Select(d => d.UserName).FirstOrDefault();
                            toUser = item.key;
                        }
                        else
                        {
                            mantyCount = 1;
                            var mail_data = lstMail.Where(d => d.MailTo == item.key).FirstOrDefault();
                            user = mail_data.UserName;
                            toUser = item.key;
                            var qn_data = JsonSerializer.Deserialize<EventQnDto>(mail_data.Content);
                            diffDays = CommonHelper.DiffDays(Convert.ToDateTime(qn_data.closing_date));
                            Log4NetHelper.Info($"1个人单封邮件模式");
            
                            string statusLang= diffDays >= 0 ? "临到期" : "已逾期", dateStr= DateTime.Now.ToString("yyyy年MM月dd日"), reminder ="";
                            var title = $"【建設工程項目管控系統】待辦任務提醒 - 您有1項任務{statusLang}，請及時處理！";
                            var taskStage = CommonHelper.GetUpcomingEventsStr(qn_data.qn_type);
                            switch (UserContext.Current.UserInfo.Lang)
                            {
                                case (int)LangType.zh_CN:
                                    statusLang = diffDays >= 0 ? "即将到期" : "已逾期";
                                    reminder = diffDays > 0 ? $"剩余{diffDays}天" : diffDays == 0 ? "今日截止" : $"逾期{-diffDays}天";
                                    title = $"【建设工程项目管控系統】待办任务提醒 - 您有1项任务{statusLang}，请及时处理！";
                                    break;
                                    case (int)LangType.zh_TW:
                                    statusLang = diffDays >= 0 ? "即將到期" : "已逾期";
                                    reminder = diffDays > 0 ? $"剩餘{diffDays}天" : diffDays == 0 ? "今日截止" : $"逾期{-diffDays}天";
                                    user = ChineseConverter.Convert(user, ChineseConversionDirection.SimplifiedToTraditional);
                                    taskStage = ChineseConverter.Convert(taskStage, ChineseConversionDirection.SimplifiedToTraditional);
                                    break;
                                    case (int)LangType.en_US:
                                    statusLang = diffDays >= 0 ? "Expires Soon": "Overdue";
                                    reminder = diffDays > 0 ? $"{diffDays} Days Remaining" : diffDays == 0 ? "Deadline Today" : $"{-diffDays} Days Overdue";
                                    dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                                    title = $"【Construction Project Management System】To-do Task Reminder - You have 1 task {statusLang}, please process it in time";
                                    taskStage = CommonHelper.GetUpcomingEventsStrEng(qn_data.qn_type);
                                    user = userNameEng;
                                    break;
                                default:
                                    break;
                            }
                            //var reminder = diffDays > 0 ? $"剩餘{diffDays}天" : diffDays == 0 ? "今日截止" : $"逾期{-diffDays}天";
                            //var mail_content = GetMailContentBySingle(user, status, qn_data.qn_no, CommonHelper.GetUpcomingEventsStr(qn_data.qn_type), qn_data.closing_date, reminder, url, DateTime.Now.ToString("yyyy年MM月dd日"));
                            var mail_content1 = MailHelperOutLook.BuildSingleTaskEmail(UserContext.Current.UserInfo.Lang, user, qn_data.qn_no, taskStage, qn_data.closing_date, statusLang, reminder, url, dateStr);
                
                            await MailHelperOutLook.SendMailOutLook(title, mail_content1, toUser);
                        }
                    }
                    if (mantyCount > 1)
                    {
                        Log4NetHelper.Info($"1个人{mantyCount}封邮件模式");
                        overdueCount = tasks.Where(d => d.IsExpected == true).Count();
                        dueSoonCount = tasks.Where(d => d.IsExpected == false).Count();
                        int index = 1;
                        string taskRows = string.Join("", tasks.Select(t => $@"
                        <tr style='background-color:#fff;'>
                            <td style='border:1px solid #666;padding:8px 10px;text-align:center;'>{index++}</td>
                            <td style='border:1px solid #666;padding:8px 10px;text-align:center;'>{t.ContractNo}</td>
                            <td style='border:1px solid #666;padding:8px 10px;text-align:center;'>{t.Stage}</td>
                            <td style='border:1px solid #666;padding:8px 10px;text-align:center;'>{t.Deadline}</td>
                            <td style='border:1px solid #666;padding:8px 10px;text-align:center;{(t.Status.Contains("逾期") || t.Status.Contains("Overdue") ? "color:red;font-weight:bold;" : t.Status.Contains("Expires") || t.Status.Contains("即") ? "color:orange;font-weight:bold;" : "color:gray;")}'>{t.Status}</td>
                            <td style='border:1px solid #666;padding:8px 10px;text-align:center;'>{t.Reminder}</td>
                        </tr>"));
                        //var mail_content = GetMailContent(user, overdueCount, dueSoonCount, diffDays, taskRows, url, DateTime.Now.ToString("yyyy年MM月dd日"));
                       


                        var title = $"【建設工程項目管控系統】待辦任務提醒 - 您共有 [{overdueCount + dueSoonCount}] 項任務[已逾期：{overdueCount}項]/[即將到期：{dueSoonCount}項]";
                        string dateStr = DateTime.Now.ToString("yyyy年MM月dd日");
                        switch (UserContext.Current.UserInfo.Lang)
                        {
                            case (int)LangType.zh_CN:
                                title = $"【建设工程项目管控系統】待办任务提醒 - 您共有 [{overdueCount + dueSoonCount}] 项任务[已逾期：{overdueCount}项]/[即将到期：{dueSoonCount}项]";
                                break;
                            case (int)LangType.zh_TW:
                                user = ChineseConverter.Convert(user, ChineseConversionDirection.SimplifiedToTraditional);
                                break;
                            case (int)LangType.en_US:
                                title = $"【Construction Project Management System】To-do Task Reminder - You have a total of [10] tasks [Overdue: 5]/[About to expire: {5}]";
                                dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                                user = userNameEng;
                                break;
                            default:
                                break;
                        }
                        var mail_content1 = MailHelperOutLook.BuildEmailBody(UserContext.Current.UserInfo.Lang, user, overdueCount, dueSoonCount, taskRows, url, dateStr);
                        await MailHelperOutLook.SendMailOutLook(title, mail_content1, toUser);
                    }
                }
                return WebResponseContent.Instance.OK("推送成功！");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Info($"PushMessageToUser定时推送消息异常：" + ex.Message);
                return WebResponseContent.Instance.Error("定时推送消息异常异常：" + ex.Message);
            }
        }

        public async Task<WebResponseContent> AddMessage(MessageNotificationAddDto dto) 
        {
            try
            {
                Log4NetHelper.Info($"添加消息提醒并推送通知，接收人：{dto.receive_user}，消息：{dto.msg_title}");

                Sys_Message_Notification notification = _mapper.Map<Sys_Message_Notification>(dto);
                notification.id = Guid.NewGuid();
                notification.status = (int)MessageStatus.Unread;
                notification.create_date = DateTime.Now;
                notification.create_id = UserContext.Current.UserId;
                notification.create_name = UserContext.Current.UserName;
                notification.days_left_to_close = 30;
                await _repository.AddAsync(notification);
                //await _repository.SaveChangesAsync();
                await _messageSender.SendMessageToUser(dto.receive_user, "通知", dto.msg_title);
                return WebResponseContent.Instance.OK("添加成功！");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this,ex);
                return WebResponseContent.Instance.Error("CheckEventTask异常：" + ex.Message);
            }
        }

        public WebResponseContent UpdateStatusNoSave(Guid id)
        {
            try
            {
                var notification = _repository.FindFirst(d=>d.relation_id ==id);
                if (notification == null)
                {
                    return WebResponseContent.Instance.Error("消息不存在！");
                }
                notification.status = (int)MessageStatus.Processed;
                notification.modify_date = DateTime.Now;
                notification.modify_id = UserContext.Current.UserId;
                notification.modify_name = UserContext.Current.UserName;
                notification.days_left_to_close = 0;
                _repository.Update(notification,false);
                return WebResponseContent.Instance.OK("更新成功！");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this,ex);
                return WebResponseContent.Instance.Error("UpdateStatus异常：" + ex.Message);
            }
        }
    }
}
