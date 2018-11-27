using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebSocketsTest.Models.Protocol
{
    public class Subscribe : IMessage
    {
        public MessageType Type {
            get { return MessageType.Subscribe; }
        }
        public string Nonce { get; set; }
        public string View { get; set; }
        public NameValueCollection Filters { get; set; }

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
