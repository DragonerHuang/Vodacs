using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Hubs
{
    /// <summary>
    /// 定义发送消息的接口
    /// </summary>
    public interface IHomePageMessageSender
    {
        Task SendMessageToUser(string userName, string title, string message);
        Task SendSystemMessage(string message);
        Task NotifyUserOnline(string userId, string userName);
        Task NotifyUserOffline(string userId, string userName);
        string GetConnectionId(string userId);
        Task<bool> UserOffline(string userName);
    }
}
