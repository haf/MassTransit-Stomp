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
    using Saga;
    using Services.Subscriptions.Server;
    using Ultralight;
    using Ultralight.Listeners;

    public abstract class given_a_stomp_bus_with_a_subscriptionservice
        : EndpointFixture
    {
        protected readonly StompServer StompServer;

        protected given_a_stomp_bus_with_a_subscriptionservice()
        {
            StompServer = new StompServer(new StompWebsocketListener("ws://localhost:8181"));
            StompServer.Start();

            LocalUri = new Uri("stomp://localhost:8181/test_queue");
            RemoteUri = new Uri("stomp://localhost:8181/test_queue_control");
            SubscriptionUri = new Uri("stomp://localhost:8181/subscriptions");

            SetupSubscriptionService();

            LocalBus = SetupServiceBus(LocalUri);
            RemoteBus = SetupServiceBus(RemoteUri);
        }

        protected IServiceBus RemoteBus { get; set; }
        protected IServiceBus LocalBus { get; set; }
        protected IServiceBus SubscriptionBus { get; set; }
        protected SubscriptionService SubscriptionService { get; set; }

        protected Uri SubscriptionUri { get; set; }
        protected Uri RemoteUri { get; set; }
        protected Uri LocalUri { get; set; }
        protected InMemorySagaRepository<SubscriptionSaga> SubscriptionSagaRepository { get; private set; }
        protected InMemorySagaRepository<SubscriptionClientSaga> SubscriptionClientSagaRepository { get; private set; }

        protected virtual void ConfigureServiceBus(Uri uri, ServiceBusConfigurator configurator)
        {
            configurator.UseControlBus();
            configurator.UseStomp();
            configurator.UseSubscriptionService("stomp://localhost:8181/subscriptions");
        }

        private void SetupSubscriptionService()
        {
            SubscriptionClientSagaRepository = SetupSagaRepository<SubscriptionClientSaga>();
            SubscriptionSagaRepository = SetupSagaRepository<SubscriptionSaga>();

            SubscriptionBus = SetupServiceBus(SubscriptionUri, x => { x.SetConcurrentConsumerLimit(1); });

            SubscriptionService = new SubscriptionService(SubscriptionBus,
                                                          SubscriptionSagaRepository,
                                                          SubscriptionClientSagaRepository);

            SubscriptionService.Start();
        }

        protected static InMemorySagaRepository<TSaga> SetupSagaRepository<TSaga>()
            where TSaga : class, ISaga
        {
            var sagaRepository = new InMemorySagaRepository<TSaga>();

            return sagaRepository;
        }

        [After]
        public void Stop()
        {
            StompServer.Stop();
        }
    }
}