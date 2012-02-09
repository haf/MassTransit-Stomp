namespace MassTransit.Transports.Stomp.Configuration
{
    using System;
    using Ultralight.Client;

    public class ConnectionFactorySettings
    {
        public ConnectionFactorySettings(ConnectionFactoryDefaultSettings defaultSettings)
        {
            BuidMethod = defaultSettings.BuidMethod;
        }

        /// <summary>
        ///   Gets the buid method.
        /// </summary>
        public Func<Uri, StompClient> BuidMethod { get; set; }
    }
}