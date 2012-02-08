namespace MassTransit.Transports.Stomp.Tests
{
    using System;
    using BusConfigurators;
    using Configuration;

    public abstract class EndpointFixture
    {
        protected virtual IServiceBus SetupServiceBus(Uri uri)
        {
            return SetupServiceBus(uri, x => ConfigureServiceBus(uri, x));
        }

        protected virtual IServiceBus SetupServiceBus(Uri uri, Action<ServiceBusConfigurator> configure)
        {
            IServiceBus bus = ServiceBusFactory.New(x =>
                                                        {
                                                            x.ReceiveFrom(uri);
                                                            x.UseStomp();

                                                            configure(x);
                                                        });

            return bus;
        }

        protected virtual void ConfigureServiceBus(Uri uri, ServiceBusConfigurator configurator)
        {
        }
    }
}