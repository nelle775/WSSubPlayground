//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.WebSockets;
//using System.Threading;
//using System.Threading.Tasks;

//namespace WebSocketsTest
//{
//    public class WebSocketHandler
//    {
//        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }

//        //private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
//        //{
//        //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
//        //    TypeNameHandling = TypeNameHandling.All,
//        //    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
//        //    SerializationBinder = new JsonBinderWithoutAssembly()
//        //};

//        /// <summary>
//        /// The waiting remote invocations for Server to Client method calls.
//        /// </summary>
//        //private Dictionary<Guid, TaskCompletionSource<InvocationResult>> _waitingRemoteInvocations = new Dictionary<Guid, TaskCompletionSource<InvocationResult>>();

//        /// <summary>
//        /// Gets the method invocation strategy.
//        /// </summary>
//        /// <value>The method invocation strategy.</value>
//        //public MethodInvocationStrategy MethodInvocationStrategy { get; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WebSocketHandler"/> class.
//        /// </summary>
//        /// <param name="webSocketConnectionManager">The web socket connection manager.</param>
//        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
//        {
//            //_jsonSerializerSettings.Converters.Insert(0, new PrimitiveJsonConverter());
//            WebSocketConnectionManager = webSocketConnectionManager;
//            //MethodInvocationStrategy = methodInvocationStrategy;
//        }

//        /// <summary>
//        /// Called when a client has connected to the server.
//        /// </summary>
//        /// <param name="socket">The web-socket of the client.</param>
//        /// <returns>Awaitable Task.</returns>
//        public virtual async Task OnConnected(WebSocket socket)
//        {
//            WebSocketConnectionManager.AddSocket(new SocketClient(socket));

//            //await SendMessageAsync(socket, new Message()
//            //{
//            //    MessageType = MessageType.ConnectionEvent,
//            //    Data = WebSocketConnectionManager.GetId(socket)
//            //}).ConfigureAwait(false);
//        }

//        /// <summary>
//        /// Called when a client has disconnected from the server.
//        /// </summary>
//        /// <param name="socket">The web-socket of the client.</param>
//        /// <returns>Awaitable Task.</returns>
//        public virtual async Task OnDisconnected(WebSocket socket)
//        {
//            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket)).ConfigureAwait(false);
//        }

//        public virtual async Task SendBinaryAsync(string clientId, byte[] data) {
//            WebSocket socket = WebSocketConnectionManager.GetSocketById(clientId);
//            if (socket == null || socket.State != WebSocketState.Open)
//            {
//                return;
//            }

//            ArraySegment<byte> readOnlyData = new ArraySegment<byte>(data, 0, data.Length);

//            await socket.SendAsync(readOnlyData, WebSocketMessageType.Binary, true, CancellationToken.None);
//        }

//        public virtual async Task SendTextAsync(string clientId, string text) {
//            WebSocket socket = WebSocketConnectionManager.GetSocketById(clientId);
//            if (socket == null || socket.State != WebSocketState.Open) {
//                return;
//            }

//            byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
//            ArraySegment<byte> readOnlyData = new ArraySegment<byte>(data, 0, data.Length);

//            await socket.SendAsync(readOnlyData, WebSocketMessageType.Text, true, CancellationToken.None);
//        }


//        public virtual async Task SendTextToAllAsync(string text)
//        {
//            foreach (var pair in WebSocketConnectionManager.GetAll())
//            {
//                try
//                {
//                    if (pair.Value.State == WebSocketState.Open)
//                        await SendMessageAsync(pair.Value, message).ConfigureAwait(false);
//                }
//                catch (WebSocketException e)
//                {
//                    if (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
//                    {
//                        await OnDisconnected(pair.Value);
//                    }
//                }
//            }
//        }


//        public virtual async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, Message receivedMessage)
//        {
//            // method invocation request.
//            if (receivedMessage.MessageType == MessageType.MethodInvocation)
//            {
//                // retrieve the method invocation request.
//                InvocationDescriptor invocationDescriptor = null;
//                try
//                {
//                    invocationDescriptor = JsonConvert.DeserializeObject<InvocationDescriptor>(receivedMessage.Data, _jsonSerializerSettings);
//                    if (invocationDescriptor == null) return;
//                }
//                catch { return; } // ignore invalid data sent to the server.

//                // if the unique identifier hasn't been set then the client doesn't want a return value.
//                if (invocationDescriptor.Identifier == Guid.Empty)
//                {
//                    // invoke the method only.
//                    try
//                    {
//                        await MethodInvocationStrategy.OnInvokeMethodReceivedAsync(socket, invocationDescriptor);
//                    }
//                    catch (Exception)
//                    {
//                        // we consume all exceptions.
//                    }
//                }
//                else
//                {
//                    // invoke the method and get the result.
//                    InvocationResult invokeResult;
//                    try
//                    {
//                        // create an invocation result with the results.
//                        invokeResult = new InvocationResult()
//                        {
//                            Identifier = invocationDescriptor.Identifier,
//                            Result = await MethodInvocationStrategy.OnInvokeMethodReceivedAsync(socket, invocationDescriptor),
//                            Exception = null
//                        };
//                    }
//                    // send the exception as the invocation result if there was one.
//                    catch (Exception ex)
//                    {
//                        invokeResult = new InvocationResult()
//                        {
//                            Identifier = invocationDescriptor.Identifier,
//                            Result = null,
//                            Exception = new RemoteException(ex)
//                        };
//                    }

//                    // send a message to the client containing the result.
//                    var message = new Message()
//                    {
//                        MessageType = MessageType.MethodReturnValue,
//                        Data = JsonConvert.SerializeObject(invokeResult, _jsonSerializerSettings)
//                    };
//                    await SendMessageAsync(socket, message).ConfigureAwait(false);
//                }
//            }

//            // method return value.
//            else if (receivedMessage.MessageType == MessageType.MethodReturnValue)
//            {
//                var invocationResult = JsonConvert.DeserializeObject<InvocationResult>(receivedMessage.Data, _jsonSerializerSettings);
//                // find the completion source in the waiting list.
//                if (_waitingRemoteInvocations.ContainsKey(invocationResult.Identifier))
//                {
//                    // set the result of the completion source so the invoke method continues executing.
//                    _waitingRemoteInvocations[invocationResult.Identifier].SetResult(invocationResult);
//                    // remove the completion source from the waiting list.
//                    _waitingRemoteInvocations.Remove(invocationResult.Identifier);
//                }
//            }
//        }


//    }
//}
