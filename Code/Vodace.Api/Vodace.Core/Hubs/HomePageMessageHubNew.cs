using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.CacheManager;
using Vodace.Core.Infrastructure;

namespace Vodace.Core.Hubs
{
    public class HomePageMessageHubNew : Hub<IHomePageMessageClient>
    {
        private readonly ICacheService _cacheService;
        public static ConcurrentDictionary<string, string> _connectionIds = new ConcurrentDictionary<string, string>();

        public HomePageMessageHubNew(ICacheService cacheService)
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

            //var userId1 = Context.UserIdentifier;
            var userId = Context.GetHttpContext().Request.Query["userName"].ToString();
            _connectionIds[Context.ConnectionId] = userId;
            _cacheService.Add($"SignalR_Connection_{userId}", Context.ConnectionId);
            //添加到一个组下
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            //发送上线消息
            //await Clients.All.SendAsync("ReceiveHomePageMessage", 1, new { title = "系统消息", content = $"{Context.ConnectionId} 上线" });
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
            //await UserOffline();
            //发送下线消息
            //await Clients.All.SendAsync("ReceiveHomePageMessage", 4, new { title = "系统消息", content = $"{Context.ConnectionId} 离线" });
           var userId = Context.GetHttpContext().Request.Query["userName"].ToString();

            var userId1 = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                _cacheService.Remove($"SignalR_Connection_{userId}");
            }
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
    }
}
