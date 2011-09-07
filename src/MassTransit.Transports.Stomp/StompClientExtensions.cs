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
    using System.Threading;
    using Ultralight.Client;

    public static class StompClientExtensions
    {
        public static void WaitForSubscriptionConformation(this StompClient client, string queue)
        {
            var subscribed = false;
            var retryCount = 20;
            var message = "connected to:" + queue;
            var originalMessageHandler = client.OnMessage;

            client.OnMessage = null;
            client.OnMessage = msg => subscribed = msg.Body == message;
            
            client.Send(queue, message);

            while (!subscribed && retryCount > 0)
            {
                Thread.Sleep(1500);
                retryCount--;
            }

            client.OnMessage = originalMessageHandler;

            if (retryCount == 0)
            {
                throw new InvalidOperationException("Timeout waiting for stomp broker to respond");
            }
        }
    }
}