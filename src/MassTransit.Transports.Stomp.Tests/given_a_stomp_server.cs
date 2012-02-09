﻿// Copyright 2011 Ernst Naezer, et. al.
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

namespace MassTransit.Transports.Stomp.Tests
{
    using Magnum.TestFramework;
    using Ultralight;
    using Ultralight.Listeners;

    public abstract class given_a_stomp_server
        : EndpointFixture
    {
        protected given_a_stomp_server()
        {
            StompServer = new StompServer(new StompWebsocketListener("ws://localhost:8181"));
            StompServer.Start();
        }

        [After]
        protected void Stop()
        {
            StompServer.Stop();
            StompServer = null;
        }

        protected StompServer StompServer;
    }
}