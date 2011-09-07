namespace MassTransit.Transports.Stomp.Tests
{
    using System;
    using BusConfigurators;
    using Magnum.TestFramework;
    using TestFramework;

    [Scenario]
    public class When_using_a_subscription_service
        : Given_a_stomp_bus
    {
        protected override void ConfigureServiceBus(Uri uri, ServiceBusConfigurator configurator)
        {
            base.ConfigureServiceBus(uri, configurator);

            configurator.UseSubscriptionService("stomp://localhost:8181/queue/subscriptions");
        }       
    }
}