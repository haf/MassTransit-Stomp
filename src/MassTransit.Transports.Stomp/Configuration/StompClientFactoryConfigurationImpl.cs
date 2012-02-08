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
    using Ultralight.Client;

    /// <summary>
    /// Holds the configuration settings for <see cref="StompClientFactory"/>
    /// </summary>
    public class StompClientFactoryConfigurationImpl 
        : StompClientFactoryConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StompClientFactoryConfigurationImpl"/> class.
        /// </summary>
        /// <param name="defaultConfiguration">The default configuration.</param>
        public StompClientFactoryConfigurationImpl(StompClientFactoryDefaultConfiguration defaultConfiguration)
        {
            BuidMethod = defaultConfiguration.BuidMethod;
        }

        /// <summary>
        /// Gets the buid method.
        /// </summary>
        public Func<string, StompClient> BuidMethod { get; private set; }

        /// <summary>
        /// Override the default build method.
        /// </summary>
        /// <param name="buildMethod">The build method.</param>
        public void UseBuildMethod(Func<string, StompClient> buildMethod)
        {
            BuidMethod = buildMethod;
        }
    }
}