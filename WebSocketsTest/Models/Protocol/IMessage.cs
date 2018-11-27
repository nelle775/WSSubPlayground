using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsTest.Models.Protocol
{
    public enum MessageType {
        Invalid = 0,
        Subscribe = 1,
        subscriptionAccepted = 2,
        Unsubscribe = 3,
        Unsubscribed = 4,
        Update = 5,
        Error = 6
    };

    public interface IMessage
    {
        MessageType Type { get; }

        string ToJson();

        void ParseMessage(JsonReader reader);

    }

}
