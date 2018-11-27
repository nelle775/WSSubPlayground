using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebSocketsTest.Models.Protocol
{
    public class Update : IMessage
    {
        public MessageType Type { get { return MessageType.Update; } }
        public string SubscriptionId { get; set; }
        public string MessagePrimaryKey {
            get {
                if (Model == null) return null;
                return Model.PrimaryKey;
            }
        }
        public IRealtimeModel Model { get; set; }

        public void ParseMessage(JsonReader reader)
        {
            throw new NotImplementedException();
        }

        public string ToJson()
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, new UTF8Encoding(false));
            JsonTextWriter writer = new JsonTextWriter(sw);

            writer.WriteStartObject();
            writer.WritePropertyName("msg");
            writer.WriteValue(Type.ToString());
            writer.WritePropertyName("subscriptionId");
            writer.WriteValue(SubscriptionId);
            writer.WritePropertyName("modelPrimaryKey");
            writer.WriteValue(MessagePrimaryKey);
            writer.WritePropertyName("model");
            

            JObject jObject = JObject.FromObject(Model);
            jObject.WriteTo(writer);

            writer.WriteEndObject();

            writer.Close();

            string json = Encoding.UTF8.GetString(ms.ToArray());
            ms.Dispose();
            return json;
        }
    }
}
