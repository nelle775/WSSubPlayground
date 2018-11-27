using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebSocketsTest.Models.Protocol
{
    public class Unsubscribed : IMessage
    {
        public MessageType Type {
            get { return MessageType.Unsubscribed; }
        }
        public string Nonce { get; set; }

        public void ParseMessage(JsonReader reader)
        {
            throw new NotImplementedException();
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}
