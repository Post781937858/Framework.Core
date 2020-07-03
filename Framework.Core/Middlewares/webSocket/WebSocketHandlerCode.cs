using Framework.Core.Common;
using Framework.Core.IServices;
using Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Middlewares.webSocket
{
    public class WebSocketHandlerCore : WebSocketHandler
    {
        private readonly WebSocketConnectionManager webSocketConnectionManager;

        public WebSocketHandlerCore(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
            this.webSocketConnectionManager = webSocketConnectionManager;
        }
        public override async Task ReceiveAsync(string token, WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var resultdata = Encoding.Default.GetString(buffer, 0, result.Count);
            var Command = resultdata.ToObject<WebSocketRequestCommand>();
            if (Command != null)
            {
                switch (Command.Command)
                {
                    case 0:
                        webSocketConnectionManager.UpdataHeartbeat(socket);
                        break;
                }
            }
            await Task.CompletedTask;
        }

        
    }
}
