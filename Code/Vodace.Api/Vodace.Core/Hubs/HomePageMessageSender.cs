using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.CacheManager;
using Vodace.Core.Utilities.Log4Net;

namespace Vodace.Core.Hubs
{
    public class HomePageMessageSender : IHomePageMessageSender
    {
        private readonly IHubContext<HomePageMessageHubNew> _hubContext;
        private readonly ICacheService _cacheService;

        public HomePageMessageSender(
            IHubContext<HomePageMessageHubNew> hubContext,
            ICacheService cacheService)
        {
            _hubContext = hubContext;
            _cacheService = cacheService;
        }

        public string GetConnectionId(string userId)
        {
            return _cacheService.Get<string>($"SignalR_Connection_{userId}");
        }

        public async Task SendMessageToUser(string userId,string title, string message)
        {
            Log4NetHelper.Info($"消息推送-站内信-推送标题{title},推送内容：{message}，接收人：{userId}");
            var connectionId = GetConnectionId(userId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveHomePageMessage", new
                {
                    title = title,
                    message = message,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")
                });
            }
        }

        /// <summary>
        /// 发送给指定的人
        /// </summary>
        /// <param name="username">sys_user表的登陆帐号</param>
        /// <param name="message">发送的消息</param>
        /// <returns></returns>
        public async Task<bool> SendHomeMessage(string username, string title, string message)
        {
            //if (_connectionIds[Context.ConnectionId]!="admin")
            //{
            //    return false;
            //}
            var userId = GetConnectionId(username);
            await _hubContext.Clients.Client(userId).SendAsync("ReceiveHomePageMessage", new
            {
                //   username,
                title,
                message,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")
            });
            return true;
        }

        public async Task SendSystemMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveHomePageMessage", new
            {
                title = "系统消息",
                content = message,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")
            });
        }

        public async Task NotifyUserOnline(string userId, string userName)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveHomePageMessage", 1, new
            {
                title = "系统消息",
                content = $"{userName} 上线"
            });
        }

        public async Task NotifyUserOffline(string userId, string userName)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveHomePageMessage", 4, new
            {
                title = "系统消息",
                content = $"{userName} 离线"
            });
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UserOffline(string userName)
        {
            _cacheService.Remove($"SignalR_Connection_{userName}");
            await Task.CompletedTask;
            return true;
        }
    }
}
