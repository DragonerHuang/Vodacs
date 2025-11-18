using Azure.Identity;
using Castle.Core.Configuration;
using Dm.util;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.Configuration;
using Vodace.Core.Enums;
using Vodace.Core.Utilities.Log4Net;


namespace Vodace.Core.Utilities
{
    public static class MailHelperOutLook
    {
        static MailHelperOutLook() 
        {
            IConfigurationSection section = AppSetting.GetSection("outlook");
            mailUserName = section["MailUserName"];
            mailUserPassword = section["MailUserPassword"];
            emailServer = section["EmailServer"];
            enableSsl = section["EnableSsl"];

            tenantId = section["TenantId"];
            clientId = section["ClientId"];
            clientSecret = section["ClientSecret"];
            fromUser = section["FromUser"];
            scopes = section["scopes"];
        }
        // <summary>
        ///  邮箱用户名
        /// </summary>
        private static  string mailUserName { get; set; }// = ConfigurationManager.AppSettings["MailUserName"];
        /// <summary>
        ///  邮箱密码
        /// </summary>
        private static  string mailUserPassword { get; set; }// = ConfigurationManager.AppSettings["MailUserPassword"];
        /// <summary>
        /// 邮件服务器
        /// </summary>
        private static  string emailServer { get; set; }// = ConfigurationManager.AppSettings["EmailServer"];
        /// <summary>
        /// 设置为true允许安全连接本地客户端发送邮件 ,  设置为false不允许允许安全连接本地客户端发送邮件 
        /// </summary>
        private static  string enableSsl  { get; set; }// ConfigurationManager.AppSettings["EnableSsl"];

        private static string tenantId { get; set; }
        private static string clientId { get; set; }
        private static string clientSecret { get; set; }
        private static string fromUser { get; set; }
        private static string scopes { get; set; }



        /// <summary>
        /// 发送EMAIL
        /// </summary>
        /// <param name="sRecipientEmail">收件人地址</param>
        /// <param name="sSubject">主题</param>
        /// <param name="sMessage">内容</param>
        /// <param name="sSendName">发件人名称</param>
        /// <returns>发送是否成功</returns>
        public static bool SendMail(string sRecipientEmail, string sSubject, string sMessage)
        {
            try
            {
                //邮件对象
                MailMessage emailMessage;
                //smtp客户端对象
                SmtpClient client;
                string sSenderEmail = mailUserName;
                emailMessage = new MailMessage(sSenderEmail, sRecipientEmail, sSubject, sMessage);
                emailMessage.IsBodyHtml = true;
                emailMessage.SubjectEncoding = System.Text.Encoding.Default;
                emailMessage.BodyEncoding = System.Text.Encoding.Default;
                //加入
                emailMessage.Headers.Add("X-Priority", "3");
                emailMessage.Headers.Add("X-MSMail-Priority", "Normal");
                emailMessage.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");
                emailMessage.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
                emailMessage.Headers.Add("ReturnReceipt", "1");

                //邮件发送客户端
                client = new SmtpClient();
                //邮件服务器及帐户信息
                client.Host = emailServer;
                //client.Host = "smtp.163.com";
                client.Port = 587;
                //client.EnableSsl = true;
                System.Net.NetworkCredential Credential = new System.Net.NetworkCredential();

                //web.config中读取邮件服务器用户名和密码信息
                Credential.UserName = mailUserName;
                Credential.Password = mailUserPassword;
                client.Credentials = Credential;
                client.EnableSsl = Convert.ToBoolean(enableSsl);
                //发送邮件
                client.Send(emailMessage);
            }
            catch (Exception ex)
            {
                //错误处理待定
                Log4NetHelper.Info($"发送邮件错误信息:{ex.Message},\r\n堆栈信息:{ex.StackTrace}");
                return false;
            }
            return true;
        }

        public static void SendMailEx11(string sSubject, string content, string sRecipientEmail)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(mailUserName);
                mail.To.Add(sRecipientEmail);
                mail.Subject = sSubject;
                mail.Body = content;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(emailServer, 587))
                {
                    smtp.Credentials = new NetworkCredential(mailUserName, mailUserPassword);  //使用应用专用密码或授权码： 如果你的邮箱开启了两步验证，你需要生成一个应用专用密码或者使用授权码来代替登录密码。在代码中，NetworkCredential 的密码应该是应用专用密码或者授权码。
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;

                    try
                    {
                        smtp.Send(mail);
                        Console.WriteLine("Email sent successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }


        public static async Task SendMailOutLook(string sSubject, string sMessage, string sRecipientEmail)
        {
            try
            {
                var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret);
                var graphClient = new GraphServiceClient(clientSecretCredential,
                    new[] { scopes });

                var message = new Message
                {
                    Subject = sSubject,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = sMessage
                    },
                    ToRecipients = new List<Recipient>
                {
                    new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = sRecipientEmail
                        }
                    }
                }
                };

                // 發送郵件
                var sendMailRequest = new SendMailPostRequestBody
                {
                    Message = message,
                    SaveToSentItems = true // 对应之前的saveToSentItems参数
                };

                await graphClient.Users[fromUser].SendMail.PostAsync(sendMailRequest);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"发送邮件异常：{ex.Message}");
                Log4NetHelper.Error(ex.Message);
            }
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="htmlContent">邮件内容</param>
        /// <param name="toEmail">收件人</param>
        /// <param name="ccEmails">抄送人</param>
        /// <param name="attachmentFilePaths">附件</param>
        /// <returns></returns>
        public static async Task SendMailOutLookEx(string subject, string htmlContent, string toEmail,
        List<string> ccEmails = null, List<string> attachmentFilePaths = null)
        {
            try
            {
                var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                var graphClient = new GraphServiceClient(clientSecretCredential, new[] { scopes });
                var toRecipients = new List<Recipient>
                {
                    new Recipient
                    {
                        EmailAddress = new EmailAddress { Address = toEmail }
                    }
                };
                var ccRecipients = new List<Recipient>();
                if (ccEmails != null)
                {
                    foreach (var cc in ccEmails)
                    {
                        ccRecipients.Add(new Recipient
                        {
                            EmailAddress = new EmailAddress { Address = cc }
                        });
                    }
                }
                var attachments = new List<Microsoft.Graph.Models.Attachment>();
                if (attachmentFilePaths != null)
                {
                    foreach (var filePath in attachmentFilePaths)
                    {
                        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                            continue;

                        byte[] contentBytes = File.ReadAllBytes(filePath);
                        const int simpleAttachmentLimit = 3 * 1024 * 1024; 
                        if (contentBytes.Length > simpleAttachmentLimit)
                        {
                            Log4NetHelper.Error($"Attachment '{filePath}' is larger than {simpleAttachmentLimit} bytes. Use a file upload session for large attachments.");
                            continue; 
                        }
                        var fileAttachment = new FileAttachment
                        {
                            Name = Path.GetFileName(filePath),
                            ContentBytes = contentBytes,
                            ContentType = "application/octet-stream"
                        };
                        attachments.Add(fileAttachment);
                    }
                }
                var message = new Message
                {
                    Subject = subject,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Html,
                        Content = htmlContent
                    },
                    CcRecipients = ccRecipients.Count > 0 ? ccRecipients : null,
                    Attachments = attachments.Count > 0 ? attachments : null
                };
                var sendMailRequest = new SendMailPostRequestBody
                {
                    Message = message,
                    SaveToSentItems = true
                };

                await graphClient.Users[fromUser].SendMail.PostAsync(sendMailRequest);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"发送邮件异常：{ex.Message}");
            }
        }


        #region 邮件模板相关
        public static string GetMailContent(string user, int? overdueCount, int? dueSoonCount, int? diffDays, string taskRows, string systemUrl, string date)
        {
            string emailTemplate = @"
            <!DOCTYPE html>
            <html>
            <head><meta charset='utf-8'></head>
            <body style='font-family:Microsoft YaHei,Arial,sans-serif;font-size:14px;color:#333;line-height:1.6;margin:0;padding:20px;background-color:#fafafa;'>
            <div style='background-color:#fff;border:1px solid #ddd;border-radius:6px;padding:20px;max-width:800px;margin:auto;box-shadow:0 2px 8px rgba(0,0,0,0.05);'>

                <p>尊敬的 <b>{user}</b> 您好：</p>

                <p>這是來自 <b> 建設工程項目管控系統</b> 的自動提醒。</p>
                <p>
                    您目前有 <span style='color:red;font-weight:bold;'>{overdueCount}</span> 項任務已逾期，
                    以及 <span style='color:orange;font-weight:bold;'>{dueSoonCount}</span> 項任務即將到截止日期，
                    請您及時處理。
                </p>

                <p>待辦任務清單如下：</p>

                <table style='width:100%;border-collapse:collapse;border:1px solid #666;margin-top:10px;'>
                    <thead>
                        <tr style='background-color:#f3f3f3;'>
                            <th style='border:1px solid #666;padding:8px 10px;width:8%;'>序號</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:16%;'>合約編號</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:18%;'>任務階段</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:25%;'>截止日期</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:15%;'>狀態</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:18%;'>天數提醒</th>
                        </tr>
                    </thead>
                    <tbody>
                        {taskRows}
                    </tbody>
                </table>

                <div style='text-align:center;margin-top:20px;'>
                    <a href='{systemUrl}' target='_blank'
                       style='display:inline-block;background-color:#0078d4;color:#fff;padding:10px 18px;border-radius:4px;text-decoration:none;'>
                       前往建設工程項目管控系統處理待辦事項
                    </a>
                </div>

                <p style='margin-top:20px;'>為確保項目順利進行，請優先處理已逾期及臨到期的任務。</p>
                <p>感謝您的辛勤工作！</p>

                <p style='margin-top:25px;color:#555;'>祝順心，<br>建設工程項目管控系統 啟<br>{date}</p>
            </div>
            </body>
            </html>";


            string emailBody = emailTemplate
            .Replace("{user}", user)
            .Replace("{overdueCount}", overdueCount.ToString())
            .Replace("{dueSoonCount}", dueSoonCount.ToString())
            .Replace("{diffDays}", diffDays.ToString())
            .Replace("{taskRows}", taskRows)
            .Replace("{systemUrl}", systemUrl)
            .Replace("{date}", date);
            return emailBody;
        }

        public static string GetMailContentBySingle(string user, string status, string contractNo, string taskStage, string deadline, string dayTips, string systemUrl, string date)
        {
            string body = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: 'Microsoft YaHei', Arial, sans-serif;
                        font-size: 14px;
                        color: #333;
                        line-height: 1.8;
                    }}
                    table {{
                        border-collapse: collapse;
                        width: 100%;
                        margin-top: 10px;
                    }}
                    th, td {{
                        border: 1px solid #ddd;
                        padding: 8px 10px;
                        text-align: center;
                        white-space: nowrap;
                    }}
                    th {{
                        background-color: #f2f2f2;
                    }}
                    .overdue {{
                        color: red;
                        font-weight: bold;
                    }}
                    .soon {{
                        color: orange;
                        font-weight: bold;
                    }}
                    .link {{
                        display: inline-block;
                        margin-top: 15px;
                        padding: 8px 14px;
                        background-color: #007bff;
                        color: #fff !important;
                        text-decoration: none;
                        border-radius: 4px;
                    }}
                    .footer {{
                        margin-top: 25px;
                        font-size: 13px;
                        color: #666;
                    }}
                </style>
            </head>
            <body>
                <p>尊敬的 <b>{user}</b> 您好：</p>

                <p>這是來自建設工程項目管控系統的自動提醒。</p>

                <p>
                    您有 1 項任務需要關注：
                    <span class='{(status == "已逾期" ? "overdue" : "soon")}'>{status}</span>，
                    請您及時處理。
                </p>

                <p>任務詳情如下：</p>

                <table>
                    <tr>
                        <th>序號</th>
                        <th>合約編號</th>
                        <th>任務階段</th>
                        <th>截止日期</th>
                        <th>狀態</th>
                        <th>天數提醒</th>
                    </tr>
                    <tr>
                        <td>1</td>
                        <td>{contractNo}</td>
                        <td>{taskStage}</td>
                        <td>{deadline}</td>
                        <td><span class='{(status == "已逾期" ? "overdue" : "soon")}'>{status}</span></td>
                        <td>{dayTips}</td>
                    </tr>
                </table>

                <p>
                    請點擊以下連結前往處理：
                    <br/>
                    <a class='link' href='{systemUrl}' target='_blank'>前往建設工程項目管控系統處理待辦事項</a>
                </p>

                <div class='footer'>
                    為確保項目順利進行，請優先處理已逾期及臨到期的任務。<br/>
                    感謝您的辛勤工作！<br/><br/>
                    祝順心，<br/>
                    建設工程項目管控系統 啟<br/>
                    {date}
                </div>
            </body>
            </html>";
            return body;
        }

        public static string BuildEmailBody(int? lang, string user, int? overdueCount, int? dueSoonCount, string taskRows, string systemUrl, string date)
        {
            string title = "", greeting = "", intro = "", summary = "", tableTitle = "",
                   btnText = "", footer = "", signOff = "", note = "";

            switch (lang)
            {
                case (int)LangType.zh_CN:
                    title = "建设工程项目管控系统";
                    greeting = $"尊敬的 <b>{user}</b> 您好：";
                    intro = $"这是来自 <b>{title}</b> 的自动提醒。";
                    summary = $"您目前有 <span style='color:red;font-weight:bold;'>{overdueCount}</span> 项任务已逾期，" +
                              $"以及 <span style='color:orange;font-weight:bold;'>{dueSoonCount}</span> 项任务即将到截止日期，请您及时处理。";
                    tableTitle = "待办任务清单如下：";
                    btnText = "前往建设工程项目管控系统处理待办事项";
                    note = "为确保项目顺利进行，请优先处理已逾期及临到期的任务。";
                    footer = "感谢您的辛勤工作！";
                    signOff = $"祝顺心，<br>{title} 启<br>{date}";
                    break;

                case (int)LangType.zh_TW:
                    title = "建設工程項目管控系統";
                    greeting = $"尊敬的 <b>{user}</b> 您好：";
                    intro = $"這是來自 <b>{title}</b> 的自動提醒。";
                    summary = $"您目前有 <span style='color:red;font-weight:bold;'>{overdueCount}</span> 項任務已逾期，" +
                              $"以及 <span style='color:orange;font-weight:bold;'>{dueSoonCount}</span> 項任務即將到截止日期，請您及時處理。";
                    tableTitle = "待辦任務清單如下：";
                    btnText = "前往建設工程項目管控系統處理待辦事項";
                    note = "為確保項目順利進行，請優先處理已逾期及臨到期的任務。";
                    footer = "感謝您的辛勤工作！";
                    signOff = $"祝順心，<br>{title} 啟<br>{date}";
                    break;

                case (int)LangType.en_US:
                    title = "Construction Project Management System";
                    greeting = $"Dear <b>{user}</b>,";
                    intro = $"This is an automatic reminder from the <b>{title}</b>.";
                    summary = $"You currently have <span style='color:red;font-weight:bold;'>{overdueCount}</span> overdue tasks, " +
                              $"and <span style='color:orange;font-weight:bold;'>{dueSoonCount}</span> tasks that are approaching their deadlines. Please handle them promptly.";
                    tableTitle = "Task List:";
                    btnText = "Go to the Construction Project Management System to handle tasks";
                    note = "To ensure smooth project progress, please prioritize overdue and nearly due tasks.";
                    footer = "Thank you for your hard work!";
                    signOff = $"Best regards,<br>{title}<br>{date}";
                    break;
            }

            // ======= 通用HTML模板 =======
            string emailTemplate = $@"
            <!DOCTYPE html>
            <html>
            <head><meta charset='utf-8'></head>
            <body style='font-family:Microsoft YaHei,Arial,sans-serif;font-size:14px;color:#333;line-height:1.6;margin:0;padding:20px;background-color:#fafafa;'>
            <div style='background-color:#fff;border:1px solid #ddd;border-radius:6px;padding:20px;max-width:800px;margin:auto;box-shadow:0 2px 8px rgba(0,0,0,0.05);'>

                <p>{greeting}</p>
                <p>{intro}</p>
                <p>{summary}</p>

                <p>{tableTitle}</p>

                <table style='width:100%;border-collapse:collapse;border:1px solid #666;margin-top:10px;'>
                    <thead>
                        <tr style='background-color:#f3f3f3;'>
                            <th style='border:1px solid #666;padding:8px 10px;width:8%;'>{(lang == (int)LangType.en_US ? "No." : "序号")}</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:16%;'>{(lang == (int)LangType.en_US ? "Contract No." : "合约编号")}</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:18%;'>{(lang == (int)LangType.en_US ? "Task Stage" : "任务阶段")}</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:25%;'>{(lang == (int)LangType.en_US ? "Deadline" : "截止日期")}</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:15%;'>{(lang == (int)LangType.en_US ? "Status" : "状态")}</th>
                            <th style='border:1px solid #666;padding:8px 10px;width:18%;'>{(lang == (int)LangType.en_US ? "Days Reminder" : "天数提醒")}</th>
                        </tr>
                    </thead>
                    <tbody>
                        {taskRows}
                    </tbody>
                </table>

                <div style='text-align:center;margin-top:20px;'>
                    <a href='{systemUrl}' target='_blank'
                       style='display:inline-block;background-color:#0078d4;color:#fff;padding:10px 18px;border-radius:4px;text-decoration:none;'>
                       {btnText}
                    </a>
                </div>

                <p style='margin-top:20px;'>{note}</p>
                <p>{footer}</p>

                    <p style='margin-top:25px;color:#555;'>{signOff}</p>
                </div>
                </body>
                </html>";

            return emailTemplate;
        }
        public static string BuildSingleTaskEmail(int? lang, string user, string contractNo, string taskStage, string deadline, string status, string dayTips, string systemUrl, string date)
        {
            string greeting = "", intro = "", summary = "", tableTitle = "", btnText = "",
                   note = "", footer = "", signOff = "", title = "";

            // === 多语言文案 ===
            switch (lang)
            {
                case (int)LangType.zh_CN:
                    title = "建设工程项目管控系统";
                    greeting = $"尊敬的 <b>{user}</b> 您好：";
                    intro = $"这是来自 <b>{title}</b> 的自动提醒。";
                    summary = $"您有 1 项任务需要关注：<span class='{(status == "已逾期" ? "overdue" : "soon")}'>{status}</span>，请您及时处理。";
                    tableTitle = "任务详情如下：";
                    btnText = "前往建设工程项目管控系统处理待办事项";
                    note = "为确保项目顺利进行，请优先处理已逾期及临到期的任务。";
                    footer = "感谢您的辛勤工作！";
                    signOff = $"祝顺心，<br>{title} 启<br>{date}";
                    break;

                case (int)LangType.zh_TW:
                    title = "建設工程項目管控系統";
                    greeting = $"尊敬的 <b>{user}</b> 您好：";
                    intro = $"這是來自 <b>{title}</b> 的自動提醒。";
                    summary = $"您有 1 項任務需要關注：<span class='{(status == "已逾期" ? "overdue" : "soon")}'>{status}</span>，請您及時處理。";
                    tableTitle = "任務詳情如下：";
                    btnText = "前往建設工程項目管控系統處理待辦事項";
                    note = "為確保項目順利進行，請優先處理已逾期及臨到期的任務。";
                    footer = "感謝您的辛勤工作！";
                    signOff = $"祝順心，<br>{title} 啟<br>{date}";
                    break;

                case (int)LangType.en_US:
                    title = "Construction Project Management System";
                    greeting = $"Dear <b>{user}</b>,";
                    intro = $"This is an automatic reminder from the <b>{title}</b>.";
                    summary = $"You have 1 task that requires your attention: " +
                              $"<span class='{(status == "Overdue" ? "overdue" : "soon")}'>{status}</span>. Please handle it promptly.";
                    tableTitle = "Task Details:";
                    btnText = "Go to Construction Project Management System to handle the task";
                    note = "To ensure the project proceeds smoothly, please prioritize overdue or nearly due tasks.";
                    footer = "Thank you for your hard work!";
                    signOff = $"Best regards,<br>{title}<br>{date}";
                    break;
            }

            // === 邮件 HTML 模板 ===
            string body = $@"
            <html>
            <head>
                <meta charset='utf-8'>
                <style>
                    body {{
                        font-family: 'Microsoft YaHei', Arial, sans-serif;
                        font-size: 14px;
                        color: #333;
                        line-height: 1.8;
                    }}
                    table {{
                        border-collapse: collapse;
                        width: 100%;
                        margin-top: 10px;
                    }}
                    th, td {{
                        border: 1px solid #ddd;
                        padding: 8px 10px;
                        text-align: center;
                        white-space: nowrap;
                    }}
                    th {{
                        background-color: #f2f2f2;
                    }}
                    .overdue {{
                        color: red;
                        font-weight: bold;
                    }}
                    .soon {{
                        color: orange;
                        font-weight: bold;
                    }}
                    .link {{
                        display: inline-block;
                        margin-top: 15px;
                        padding: 8px 14px;
                        background-color: #007bff;
                        color: #fff !important;
                        text-decoration: none;
                        border-radius: 4px;
                    }}
                    .footer {{
                        margin-top: 25px;
                        font-size: 13px;
                        color: #666;
                    }}
                </style>
            </head>
            <body>
                <p>{greeting}</p>

                <p>{intro}</p>

                <p>{summary}</p>

                <p>{tableTitle}</p>

                <table>
                    <tr>
                        <th>{(lang == (int)LangType.en_US ? "No." : "序號")}</th>
                        <th>{(lang == (int)LangType.en_US ? "Contract No." : "合約編號")}</th>
                        <th>{(lang == (int)LangType.en_US ? "Task Stage" : "任務階段")}</th>
                        <th>{(lang == (int)LangType.en_US ? "Deadline" : "截止日期")}</th>
                        <th>{(lang == (int)LangType.en_US ? "Status" : "狀態")}</th>
                        <th>{(lang == (int)LangType.en_US ? "Days Reminder" : "天數提醒")}</th>
                    </tr>
                    <tr>
                        <td>1</td>
                        <td>{contractNo}</td>
                        <td>{taskStage}</td>
                        <td>{deadline}</td>
                        <td><span class='{(status == "已逾期" || status == "Overdue" ? "overdue" : "soon")}'>{status}</span></td>
                        <td>{dayTips}</td>
                    </tr>
                </table>

                <p>
                    {(lang == (int)LangType.en_US ? "Please click the link below to handle it:" : "請點擊以下連結前往處理：")}
                    <br/>
                    <a class='link' href='{systemUrl}' target='_blank'>{btnText}</a>
                </p>

                <div class='footer'>
                    {note}<br/>
                    {footer}<br/><br/>
                    {signOff}
                </div>
            </body>
            </html>";
            return body;
        }

        public static string GetMailTemplate(string recipientName,string documentName,string projectInfo,string senderContact)
        {
            string template = @"
            Dear {RecipientName},<br/><br/>

            Please find attached the {DocumentName} for the {ProjectInfo}.<br/><br/>

            This is an automated message to confirm that the document has been sent successfully.<br/>
            Kindly review the attached file at your convenience.<br/><br/>

            If you have any questions or require further information, please contact {SenderContact} directly.<br/><br/>

            Thank you for your attention.<br/><br/>

            Best regards,<br/>
            {title}<br/>
            System Notification Service";
            var title = "Construction Project Management System";
            return template
                .Replace("{RecipientName}", recipientName)
                .Replace("{DocumentName}", documentName)
                .Replace("{ProjectInfo}", projectInfo)
                .Replace("{SenderContact}", senderContact)
                .Replace("{title}", title);
        }
        #endregion
    }
}
