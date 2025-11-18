using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Vodace.Core.Configuration;
using Vodace.Core.Extensions;

namespace Vodace.Core.Utilities
{
    public static class MailHelper
    {
        private static string address { get; set; }
        private static string authPwd { get; set; }
        private static string name { get; set; }
        private static string host { get; set; }
        private static int port;
        private static bool enableSsl { get; set; }
        static MailHelper()
        {
            IConfigurationSection section = AppSetting.GetSection("Mail");
            address = section["address"];
            authPwd = section["AuthPwd"];
            name = section["Name"];
            host = section["Host"];
            port = section["Port"].GetInt();
            enableSsl = section["EnableSsl"].GetBool();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="list">收件人</param>
        public static void Send(string title, string content, params string[] list)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(address, name)//发送人邮箱
            };
            foreach (var item in list)
            {
                message.To.Add(item);//收件人地址
            }

            message.Subject = title;//发送邮件的标题

            message.Body = content;//发送邮件的内容
            //配置smtp服务地址
            SmtpClient client = new SmtpClient
            {
                Host = host,
                Port = port,//端口587
                EnableSsl = enableSsl,
                //发送人邮箱与授权密码
                Credentials = new NetworkCredential(address, authPwd)
            };
            client.Send(message);
        }

        public static WebResponseContent SendEmail(string title, string content, params string[] list)
        {
            try
            {
                foreach (var item in list)
                {
                    SmtpClient smtpClient = new SmtpClient(host, port);
                    smtpClient.EnableSsl = true; // 启用SSL/TLS加密
                    smtpClient.Credentials = new NetworkCredential(address, authPwd);
                    MailMessage mailMessage = new MailMessage(address, item);
                    mailMessage.Subject = title;
                    mailMessage.Body = content;
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully.");
                    return WebResponseContent.Instance.OK("Email sent successfully.");
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return WebResponseContent.Instance.OK("Failed to send email.");
        }

        public static WebResponseContent SendEmail(string title, string content, string item)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(host, port);
                smtpClient.EnableSsl = true; // 启用SSL/TLS加密
                smtpClient.Credentials = new NetworkCredential(address, authPwd);
                MailMessage mailMessage = new MailMessage(address, item);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = title;
                mailMessage.Body = content;
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
                return WebResponseContent.Instance.OK("Email sent successfully.");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return WebResponseContent.Instance.OK("Failed to send email.");
        }
    }
}
