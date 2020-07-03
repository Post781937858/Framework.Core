using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.Common;
using Framework.Core.Middlewares.webSocket;
using Framework.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSocketManagerController : ControllerBase
    {
        private readonly WebSocketConnectionManager connectionManager;
        private readonly WebSocketHandlerCore webSocketHandler;

        public WebSocketManagerController(WebSocketConnectionManager connectionManager, WebSocketHandlerCore webSocketHandler)
        {
            this.connectionManager = connectionManager;
            this.webSocketHandler = webSocketHandler;
        }

        [HttpGet]
        public async Task<MessageModel<List<WebSocketModel>>> Query()
        {
            await Task.CompletedTask;
            return new MessageModel<List<WebSocketModel>>(connectionManager.GetAll().Values.OrderByDescending(p=>p.connectTime).ToList());
        }

        [HttpPost]
        public async Task<MessageModel> Offline(WebSocketModelBase webSocket)
        {
            await Task.CompletedTask;
            var Json = new WebSocketUpdateResponse<string>()
            {
                state = 200,
                type = 300
            }.ToJson();
            await webSocketHandler.SendMessageAsync(webSocket.Guid, Json);
            return new MessageModel();
        }
    }
}