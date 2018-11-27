using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsTest.Models
{
    public class Subscription
    {
        private DateTime _created = DateTime.MinValue;
        public DateTime Created {
            get { return _created; }
        }
        private string _subscriptionId = null;
        public string SubscriptionId {
            get { return _subscriptionId; }
        }
        private string _clientSocketId = null;
        public string ClientSocketId {
            get { return _clientSocketId; }
        }
        private string _viewName = null;
        public string ViewName {
            get { return _viewName; }
        }
        //TODO: Refactor this...
        public NameValueCollection Parameters { get; set; }

        public Subscription(SocketClient client, string viewName, NameValueCollection parameters) {
            _subscriptionId = Guid.NewGuid().ToString();
            _clientSocketId = client.Id;
            _viewName = viewName;
            Parameters = parameters;
            _created = DateTime.Now;
        }

    }
}
