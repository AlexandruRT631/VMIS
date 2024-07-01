using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace user_backend.WebSockets;

public class WebSocketManager
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>> _sockets = new();

    public void AddSocket(string conversationId, WebSocket socket)
    {
        var conversationSockets = _sockets.GetOrAdd(conversationId, new ConcurrentDictionary<string, WebSocket>());
        var socketId = Guid.NewGuid().ToString();
        conversationSockets.TryAdd(socketId, socket);
    }

    public IEnumerable<WebSocket> GetSockets(string conversationId)
    {
        if (_sockets.TryGetValue(conversationId, out var conversationSockets))
        {
            return conversationSockets.Values;
        }

        return Enumerable.Empty<WebSocket>();
    }
}