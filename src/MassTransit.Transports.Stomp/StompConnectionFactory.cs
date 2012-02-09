namespace MassTransit.Transports.Stomp
{
    using System;
    using Magnum.Extensions;
    using log4net;

    /// <summary>
    /// Builds a <see cref="StompConnection"/>
    /// </summary>
    public class StompConnectionFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (StompConnectionFactory));

        private readonly StompClientFactory _clientFactory;

        /// <summary>
        ///   Initializes a new instance of the <see cref="StompConnectionFactory" /> class.
        /// </summary>
        /// <param name="clientFactory"> The client factory. </param>
        public StompConnectionFactory(StompClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Builds the <see cref="StompConnection"/> to connect to the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public StompConnection Build(Uri location)
        {
            var serverAddress = new UriBuilder("ws", location.Host, location.Port).Uri;

            if (Log.IsInfoEnabled)
                Log.Warn("Connecting {0}".FormatWith(location));

            return new StompConnection(_clientFactory.Build(serverAddress));
        }
    }
}