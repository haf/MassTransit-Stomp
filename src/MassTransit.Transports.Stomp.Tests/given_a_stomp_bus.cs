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

namespace MassTransit.Transports.Stomp.Tests
{
    using System;
    using BusConfigurators;
    using Configuration;
    using Magnum.TestFramework;
    using TestFramework.Fixtures;
    using Ultralight;
    using Ultralight.Listeners;

    public abstract class given_a_stomp_bus
        : LocalTestFixture<StompTransportFactory>
    {
        protected given_a_stomp_bus()
        {
            StompServer = new StompServer(new StompWebsocketListener("ws://localhost:8181"));
            StompServer.Start();

            LocalUri = new Uri("stomp://localhost:8181/test_queue");
        }

        protected override void ConfigureServiceBus(Uri uri, ServiceBusConfigurator configurator)
        {
            configurator.UseStomp();
        }

        [After]
        protected void Stop()
        {
            StompServer.Stop();
        }

        protected readonly StompServer StompServer;
    }
}