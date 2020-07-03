using Framework.Core.Common;
using Framework.Core.IServices;
using Framework.Core.Middlewares.webSocket;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Extensions.Quartz
{
    public class MessageToWebSocketJob : IJob
    {

        private readonly WebSocketHandlerCore socketHandlerCore;
        private readonly WebSocketConnectionManager connectionManager;
        private readonly ILogger<MessageToWebSocketJob> logger;

        public MessageToWebSocketJob(WebSocketHandlerCore socketHandlerCore, WebSocketConnectionManager connectionManager, ILogger<MessageToWebSocketJob> logger)
        {
            this.socketHandlerCore = socketHandlerCore;
            this.connectionManager = connectionManager;
            this.logger = logger;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (connectionManager.GetCount() > 0)
                {
                    foreach (var WebSocketitem in connectionManager.GetAll().Values)
                    {
                        long interval = UTC.ConvertDateTimeLong(DateTime.Now) - UTC.ConvertDateTimeLong(WebSocketitem.HeartbeatTime);
                        if (interval >= 60)
                        {
                            await connectionManager.RemoveSocket(WebSocketitem.Guid);
                        }
                        else if (interval >= 10)
                        {
                            WebSocketitem.connectstate = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }
    }
}
