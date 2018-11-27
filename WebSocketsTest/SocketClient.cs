using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace WebSocketsTest
{
    public class SocketClient
    {
        private string _id = null;
        public string Id {
            get { return _id; }
        }
        private WebSocket _socket = null;
        public WebSocket Socket {
            get { return _socket; }
        }

        public SocketClient(WebSocket socket) {
            if (socket == null) throw new ArgumentNullException("socket");
            _socket = socket;
            _id = Guid.NewGuid().ToString();
        }

    }
}
