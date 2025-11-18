
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_Message_NotificationService
    {
        WebResponseContent CheckEventTask();
        WebResponseContent GetList(string user_name);
        WebResponseContent UpdateStatus(Guid id, int status);
        Task<WebResponseContent> PushMessageToUser();
        Task<bool> UserOffline(string userName);
        Task<WebResponseContent> AddMessage(MessageNotificationAddDto dto);
        WebResponseContent UpdateStatusNoSave(Guid id);
        //Task<WebResponseContent> SendEmail(string sRecipientEmail, string sSubject, string sMessage);
    }
 }
