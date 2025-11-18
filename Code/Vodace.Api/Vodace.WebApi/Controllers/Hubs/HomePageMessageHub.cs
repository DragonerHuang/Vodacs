using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.CacheManager;
using Vodace.Core.Extensions;
using Vodace.Core.ManageUser;
using Vodace.Sys.IServices;

namespace Vodace.Api.Controllers.Hubs
{
    public class HomePageMessageHub : Hub
    {
        private readonly ICacheService _cacheService;


        public static ConcurrentDictionary<string, string> _connectionIds = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 构造 注入
        /// </summary>
        public HomePageMessageHub(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// 建立连接时异步触发
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            //Console.WriteLine($"建立连接{Context.ConnectionId}");
            _connectionIds[Context.ConnectionId] = Context.GetHttpContext().Request.Query["userName"].ToString();
            //添加到一个组下
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            //发送上线消息
            await Clients.All.SendAsync("ReceiveHomePageMessage", 1, new { title = "系统消息", content = $"{Context.ConnectionId} 上线" });
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 离开连接时异步触发
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            //Console.WriteLine($"断开连接{Context.ConnectionId}");
            //从组中删除
             //await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            //可自行调用下线业务处理方法...
            await UserOffline();
            //发送下线消息
             await Clients.All.SendAsync("ReceiveHomePageMessage", 4, new { title = "系统消息", content = $"{Context.ConnectionId} 离线" });
            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// 根据用户名获取所有的客户端
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public IEnumerable<string> GetCnnectionIds(string username)
        {
            foreach (var item in _connectionIds)
            {
                if (item.Value == username)
                {
                    yield return item.Key;
                }
            }
        }
        public async Task SendMessageToUser(string userAccount, string message)
        {
            // 使用 Clients.User 并传入用户标识
            await Clients.User(userAccount).SendAsync("ReceiveHomePageMessage", message);
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
            var userId = GetCnnectionIds(username).ToArray();
            await Clients.Clients(userId).SendAsync("ReceiveHomePageMessage", new
            {
                //   username,
                title,
                message,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")
            });
            return true;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UserOffline()
        {
            var cid = Context.ConnectionId;//也可以从缓存中获取ConnectionId
            await Clients.Client(cid).SendAsync("ReceiveHomePageMessage", 3, new { title = "系统消息", content = "离线成功" });
            //移除缓存
            if (_connectionIds.TryRemove(cid, out string value))
            {
            }
            await Task.CompletedTask;
            return true;
        }
    }
}
