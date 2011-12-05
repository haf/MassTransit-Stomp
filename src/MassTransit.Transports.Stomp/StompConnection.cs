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
    using System.Collections.Concurrent;
    using Magnum.Extensions;
    using Ultralight;
    using Ultralight.Client;
    using log4net;

    public class StompConnection
        : Connection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (StompConnection));
        private readonly Uri _address;
        private StompClient _stompClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="StompConnection"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        public StompConnection(Uri address)
        {
            _address = address;
            Messages = new ConcurrentQueue<StompMessage>();
        }

        public ConcurrentQueue<StompMessage> Messages { get; set; }

        public void Subscribe(string path)
        {
            _stompClient.Subscribe(path);
        }

        public void Unsubscribe(string path)
        {
            _stompClient.Unsubscribe(path);
        }

        public void Send(string address, string message)
        {
            _stompClient.Send(address, message);
        }

        public void Connect()
        {
            Disconnect();

            var serverAddress = new UriBuilder("ws", _address.Host, _address.Port).Uri;

            if (Log.IsInfoEnabled)
                Log.Warn("Connecting {0}".FormatWith(_address));

            var absoluteUri = serverAddress.AbsoluteUri;

            _stompClient = StompClientFactory.Build(absoluteUri);
            _stompClient.OnMessage += m => Messages.Enqueue(m);
            _stompClient.Connect();
        }

        public void Disconnect()
        {
            try
            {
                if (_stompClient == null) return;

                if (Log.IsInfoEnabled)
                    Log.Warn("Disconnecting {0}".FormatWith(_address));

                if (_stompClient.IsConnected)
                    _stompClient.Disconnect();
            }
            catch (Exception ex)
            {
                Log.Warn("Failed to close STOMP connection.", ex);
            }
        }

        public void Dispose()
        {
        }
    }
}