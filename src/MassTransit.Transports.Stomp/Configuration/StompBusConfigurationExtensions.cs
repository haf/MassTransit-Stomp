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

namespace MassTransit.Transports.Stomp.Configuration
{
    using System;
    using BusConfigurators;

    public static class StompBusConfigurationExtensions
    {
        /// <summary>
        ///   Uses stomp as a transport channel.
        /// </summary>
        /// <param name="configurator"> The servicebus configurator. </param>
        /// <returns> </returns>
        public static ServiceBusConfigurator UseStomp(this ServiceBusConfigurator configurator)
        {
            return configurator.UseStomp(x => { });
        }

        /// <summary>
        ///   Uses stomp as a transport channel.
        /// </summary>
        /// <param name="configurator"> The servicebus configurator. </param>
        /// <param name="configure"> Stomp transport configuration callback. </param>
        /// <returns> </returns>
        public static ServiceBusConfigurator UseStomp(this ServiceBusConfigurator configurator, Action<ConnectionFactoryConfigurator> configure)
        {
            var factoryConfigurator = new ConnectionFactoryConfiguratorImpl(new ConnectionFactoryDefaultSettings());

            configure(factoryConfigurator);

            var connectionFactory = factoryConfigurator.CreateStompClientFactory();

            configurator.AddTransportFactory<StompTransportFactory>(configureFactory => { configureFactory.SetConnectionFactory(connectionFactory); });
            configurator.UseJsonSerializer();

            return configurator;
        }
    }
}