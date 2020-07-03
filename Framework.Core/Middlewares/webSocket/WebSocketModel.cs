using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Framework.Core.Middlewares.webSocket
{

    public class WebSocketModelBase
    {
        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime connectTime { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string userName { get; set; }


        /// <summary>
        /// 连接状态
        /// </summary>
        public bool connectstate { get; set; }

        /// <summary>
        /// 心跳时间
        /// </summary>
        public DateTime HeartbeatTime { get; set; } = DateTime.Now;

        /// <summary>
        /// ip地址
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>

        public string Browser { get; set; }

        /// <summary>
        /// 系统
        /// </summary>
        public string OS { get; set; }


        public string token { get; set; }

    }

    public class WebSocketModel : WebSocketModelBase
    {
        public WebSocket socket { get; set; }
    }
}
