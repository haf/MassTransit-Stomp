// Copyright 2011 Ernst Naezer, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

namespace MassTransit.Transports.Stomp
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using Context;
    using Ultralight;
    using Util;

    public class InboundStompTransport
        : IInboundTransport
    {
        private readonly ConnectionHandler<StompConnection> _connectionHandler;
        private readonly IEndpointAddress _address;

        private bool _disposed;
        private StompSubsciption _subsciption;

        public InboundStompTransport(IEndpointAddress address, ConnectionHandler<StompConnection> connectionHandler)
        {
            _connectionHandler = connectionHandler;
            _address = address;
        }

        public IEndpointAddress Address
        {
            get { return _address; }
        }

        public void Receive(Func<IReceiveContext, Action<IReceiveContext>> callback, TimeSpan timeout)
        {
            AddConsumerBinding();

            _connectionHandler
                .Use(connection =>
                         {
                             StompMessage message;
                             if (!connection.Messages.TryDequeue(out message))
                             {
                                 Thread.Sleep(10);
                                 return;
                             }

                             using (var body = new MemoryStream(Encoding.UTF8.GetBytes(message.Body), false))
                             {
                                 var context = ReceiveContext.FromBodyStream(body);
                                 context.SetMessageId(message["id"]);
                                 context.SetInputAddress(Address);
                                 
                                 var receive = callback(context);
                                 if (receive == null)
                                 {
                                     if (SpecialLoggers.Messages.IsInfoEnabled)
                                         SpecialLoggers.Messages.InfoFormat("SKIP:{0}:{1}", Address, context.MessageId);
                                 }
                                 else
                                 {
                                     receive(context);
                                 }
                             }
                         });
        }

        private void AddConsumerBinding()
        {
            if (_subsciption != null)
                return;

            _subsciption = new StompSubsciption(_address);
            _connectionHandler.AddBinding(_subsciption);
        }

        private void RemoveConsumer()
        {
            if (_subsciption != null)
            {
                _connectionHandler.RemoveBinding(_subsciption);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                RemoveConsumer();
            }

            _disposed = true;
        }

        ~InboundStompTransport()
        {
            Dispose(false);
        }
    }
}