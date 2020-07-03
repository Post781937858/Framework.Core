using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Middlewares.webSocket
{
    public class WebSocketUpdateResponse<T>
    {
        public int type { get; set; }

        public int state { get; set; }

        public T data { get; set; }
    }

    public class WebSocketRequestCommand
    {
        public int Command { get; set; }

        public string megType { get; set; }

    }
}
