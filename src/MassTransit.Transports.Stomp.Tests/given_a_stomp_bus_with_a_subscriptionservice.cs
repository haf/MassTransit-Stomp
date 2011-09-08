namespace MassTransit.Transports.Stomp.Tests
{
    using System;
    using BusConfigurators;
    using Magnum.TestFramework;
    using TestFramework.Fixtures;
    using Ultralight;
    using Ultralight.Listeners;

    public class given_a_stomp_bus_with_a_subscriptionservice
        : SubscriptionServiceTestFixture<StompTransportFactory>
    {
        protected given_a_stomp_bus_with_a_subscriptionservice()
        {
            StompServer = new StompServer(new StompWsListener(new Uri("ws://localhost:8181")));
            StompServer.Start();

            LocalUri = new Uri("stomp://localhost:8181/test_queue");
            RemoteUri = new Uri("stomp://localhost:8181/test_queue_control");
            SubscriptionUri = new Uri("stomp://localhost:8181/subscriptions");
        }

        protected override void ConfigureServiceBus(Uri uri, ServiceBusConfigurator configurator)
        {
            configurator.UseControlBus();
            configurator.UseStomp();
            configurator.UseSubscriptionService("stomp://localhost:8181/subscriptions");
        }

        [After]
        public void Stop()
        {
            StompServer.Stop();
        }

        protected readonly StompServer StompServer;
    }
}