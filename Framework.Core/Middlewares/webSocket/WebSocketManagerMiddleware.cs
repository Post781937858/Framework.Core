using Framework.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Middlewares.webSocket
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUser user;
        private readonly ILogger<WebSocketManagerMiddleware> logger;
        private readonly WebSocketConnectionManager connectionManager;

        private WebSocketHandler _webSocketHandler { get; set; }

        public WebSocketManagerMiddleware(RequestDelegate next,
                                          WebSocketHandler webSocketHandler, IUser user, ILogger<WebSocketManagerMiddleware> logger, WebSocketConnectionManager connectionManager)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
            this.user = user;
            this.logger = logger;
            this.connectionManager = connectionManager;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            string token = context.Request.Query["token"];
            string userName = string.Empty;
            string UserId = string.Empty;
            if (!string.IsNullOrEmpty(token) && token!="null")
            {
                TokenModelJwt tokenModelJwt = JWTTokenService.SerializeJwt(token);
                userName = tokenModelJwt.Name;
                UserId = tokenModelJwt.Uid.ToString();
            }
            else
            {
                var Json = new WebSocketUpdateResponse<string>()
                {
                    state = 200,
                    type = 100
                }.ToJson();
                await _webSocketHandler.SendMessageAsync(socket, Json);
                await Task.Delay(1000);
                await connectionManager.CloseSocket(socket);
                return;
            }
            var websocketItem = connectionManager.GetSocketByModel(UserId);
            if (websocketItem != null)
            {
                var Json = new WebSocketUpdateResponse<string>()
                {
                    state = 200,
                    type = websocketItem.token != token ? 500 : 100
                }.ToJson();
                await _webSocketHandler.SendMessageAsync(websocketItem.socket, Json);
                await Task.Delay(1000);
                await connectionManager.RemoveSocket(UserId);
            }
            var ipaddress = context.Connection.RemoteIpAddress.ToIPv4String();
            var userAgent = context.Request.Headers["User-Agent"];
            var agent = new UserAgent(userAgent);
            var Browser = $"{agent.Browser?.Name} {agent.Browser?.Version}";
            var OS = $"{agent.OS?.Name} {agent.OS?.Version}";
            var webSocket = new WebSocketModel()
            {
                connectstate = true,
                connectTime = DateTime.Now,
                Guid = UserId,
                socket = socket,
                userName = userName,
                Browser = Browser,
                ip = ipaddress,
                OS = OS,
                token = token
            };
            _webSocketHandler.OnConnectedAsync(webSocket);
            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await _webSocketHandler.ReceiveAsync(token, socket, result, buffer);
                    return;
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocketHandler.OnDisconnected(socket);
                    return;
                }
            });
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            try
            {
                var buffer = new byte[1024 * 4];

                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                           cancellationToken: CancellationToken.None);
                    handleMessage(result, buffer);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }

        }
    }
}
