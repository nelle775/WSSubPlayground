using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketsTest
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, SocketClient> _sockets = new ConcurrentDictionary<string, SocketClient>();

        public SocketClient GetSocketById(string id)
        {
            SocketClient socket = null;
            _sockets.TryGetValue(id, out socket);
            return socket;
        }

        public ICollection<SocketClient> GetAll()
        {
            return _sockets.Values;
        }

        public void AddSocket(SocketClient socket)
        {
            _sockets.TryAdd(socket.Id, socket);
        }

        public async Task RemoveSocket(string id)
        {
            if (id == null) return;

            SocketClient socket;
            _sockets.TryRemove(id, out socket);

            if (socket.Socket.State != WebSocketState.Open) return;

            await socket.Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketManager", CancellationToken.None);
        }

    }
}
