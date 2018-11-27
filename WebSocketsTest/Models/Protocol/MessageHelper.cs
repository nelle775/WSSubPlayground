using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebSocketsTest.Models.Protocol
{
    public class MessageHelper
    {
        private string _json = null;
        private JsonTextReader _reader = null;
        private MessageType _type = MessageType.Invalid;
        public MessageHelper(string jsonMessage) {
            _json = jsonMessage;
        }

        public MessageType GetMsgType() {
            MessageType result = MessageType.Invalid;
            StringReader sr = new StringReader(_json);
            JsonTextReader _reader = new JsonTextReader(sr);

            string propertyName = null;
            while (_reader.Read()) {
                if (_reader.TokenType == JsonToken.PropertyName)
                {
                    propertyName = _reader.Value.ToString();
                }
                else if (_reader.TokenType == JsonToken.String) {
                    if (propertyName == "msg") {
                        string strMsgType = _reader.Value.ToString();

                        Enum.TryParse<MessageType>(strMsgType, out result);
                        break;
                    }
                }
            }
            return result;
        }

        public Subscribe GetMsgSubscribe() {
            if (_type != MessageType.Subscribe) throw new InvalidOperationException("Message type is not intialized or does not match type: \"Subscribe\"");
            Subscribe msg = new Subscribe();
            msg.ParseMessage(_reader);
            _reader.Close();
            _reader = null;
            return msg;
        }

        public Unsubscribe GetMsgUnsubscribe() {
            if(_type != MessageType.Unsubscribe) throw new InvalidOperationException("Message type is not intialized or does not match type: \"Unsubscribe\"");
            Unsubscribe msg = new Unsubscribe();
            msg.ParseMessage(_reader);
            _reader.Close();
            _reader = null;
            return msg;
        }


        public void Close() {
            if (_reader != null) {
                _reader.Close();
                _reader = null;
            }
        }


    }
}
