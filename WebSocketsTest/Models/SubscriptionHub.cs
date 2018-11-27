using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using WebSocketsTest.Models.Protocol;

namespace WebSocketsTest.Models
{
    public enum ModelChangeType { None = 0, Insert = 1, Update = 2, Delete = 3 };

    //TODO: Implement abstract WebSocketHandler
    //Loop Incoming messages
    //In abstract handler implementation parse Incoming protocol messages
    //Bind protocol messages to the Subscribe/Unsubscribe methods
    //Create outgoing protocol responses from incoming message, dispatch async
    //Create outgoing protocol messages to subscribing clients when signals are recieved, dispatch async

    //References:
    //https://blog.marcinbudny.com/2011/10/websockets-in-net-45-simple-game.html                    //pong :-)
    //https://www.codetinkerer.com/2018/06/05/aspnet-core-websockets.html                           //Valuable notes..
    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/websockets?view=aspnetcore-2.1      //.NET Core WS Documentation
    //https://gunnarpeipman.com/aspnet/aspnet-core-websocket-chat/                                  //Basic chat
    //https://radu-matei.com/blog/real-time-aspnet-core/                                            //Dude RePooped signalR
    //https://github.com/radu-matei/websocket-manager/blob/master/src/WebSocketManager/WebSocketConnectionManager.cs //RePooping SignalR Dudes repo

    public class SubscriptionHub
    {
        private SubscriptionCollection _subscriptions = null;
        private RealtimeViewCollection _views = null;

        public async Task SignalChangeAsync(string viewName, ModelChangeType changeType, object model) {
            if (changeType == ModelChangeType.None) return;
            Type modelType = model.GetType();
            //Get a collection of Views containing the model that has changed
            ICollection<RealtimeView> views = _views.GetViewsContainingModel(modelType);

            List<Task> filteredSubscriptions = new List<Task>();
            foreach (RealtimeView view in views) {
                ICollection<Subscription> viewSubs = _subscriptions.ListSubscriptionsForView(view.ViewName);
                if (viewSubs.Count > 0) {
                    RealtimeView.FilterContext filter = view.GetFilterContext(modelType, model);
                    foreach (Subscription viewSub in viewSubs) {
                        if (filter.IsWithinFilter(viewSub.Parameters)) {
                            //filteredSubscriptions.Add(SendMessageAsync(viewSub, modelType, model));
                        }
                    }
                }
            }

            if (filteredSubscriptions.Count > 0) {
                await Task.WhenAll(filteredSubscriptions);
            }
        }

        public string Subscribe(SocketClient client, string viewName, NameValueCollection parameters) {
            Subscription subscription = new Subscription(client, viewName, parameters);
            if (!_subscriptions.Add(subscription)) {

            }
            throw new NotImplementedException();
        }

        public string Unsubscribe(SocketClient client, string subscriptionId) {
            throw new NotImplementedException();
        }

        private async Task SendMessageAsync(string clientId, IMessage msg) {
            throw new NotImplementedException();
        }

    }
}
