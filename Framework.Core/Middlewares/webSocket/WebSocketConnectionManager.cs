using Framework.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Middlewares.webSocket
{
    public class WebSocketConnectionManager
    {
        public WebSocketConnectionManager(ILogger<WebSocketConnectionManager> logger)
        {
            this.logger = logger;
        }

        private ConcurrentDictionary<string, WebSocketModel> _sockets = new ConcurrentDictionary<string, WebSocketModel>();
        private readonly ILogger<WebSocketConnectionManager> logger;

        public int GetCount()
        {
            return _sockets.Count;
        }

        public WebSocket GetSocketById(string id)
        {
            if (!_sockets.ContainsKey(id))
            {
                return null;
            }
            return _sockets[id].socket;
        }

        public WebSocketModel GetSocketByModel(string id)
        {
            if (!_sockets.ContainsKey(id))
            {
                return null;
            }
            return _sockets[id];
        }


        public void UpdataHeartbeat(WebSocket socket)
        {
            var key = GetId(socket);
            if (!string.IsNullOrEmpty(key))
            {
                var item = _sockets[key];
                item.HeartbeatTime = DateTime.Now;
            }
        }


        public ConcurrentDictionary<string, WebSocketModel> GetAll()
        {
            return _sockets;
        }
        public WebSocket GetWebSocket(string key)
        {
            WebSocket _socket = default(WebSocket);
            if (_sockets.ContainsKey(key))
            {
                return _sockets[key].socket;
            }
            return _socket;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.Values.FirstOrDefault(p => p.socket == socket)?.Guid;
        }
        public void AddSocket(WebSocketModel webSocket)
        {
            _sockets.GetOrAdd(webSocket.Guid, webSocket);
        }

        public async Task RemoveSocket(string key)
        {
            try
            {
                if (_sockets.ContainsKey(key))
                {
                    var sockets = _sockets[key];
                    _sockets.Remove(key, out sockets);
                    await CloseSocket(sockets.socket);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

        public async Task CloseSocket(WebSocket socket)
        {
            await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
