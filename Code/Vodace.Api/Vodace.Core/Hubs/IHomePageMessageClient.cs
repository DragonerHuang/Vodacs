using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Infrastructure
{
    /// <summary>
    /// 定义客户端可以接收的消息方法
    /// </summary>
    public interface IHomePageMessageClient
    {
        /// <summary>
        /// 接收首页消息
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="message">消息内容</param>
        Task ReceiveHomePageMessage(object message);
    }
}
