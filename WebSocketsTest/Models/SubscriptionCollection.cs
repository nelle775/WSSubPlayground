using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsTest.Models
{
    public class SubscriptionCollection
    {

        public ICollection<Subscription> ListSubscriptionsForView(string viewName) {
            throw new NotImplementedException();
        }
        public ICollection<Subscription> ListSubscriptionsForClient(string clientId) {
            throw new NotImplementedException();
        }
        public Subscription GetSubscriptionById(string subscriptionId) {
            throw new NotImplementedException();
        }

        public bool Add(Subscription subscription) {
            throw new NotImplementedException();
        }

        public bool RemoveSubscriptionById(string subscriptionId) {
            throw new NotImplementedException();
        }

    }
}
