// Licensed under the BSD license
// See the LICENSE file in the project root for more information

using System;
using System.ComponentModel;

namespace NLog.Targets.Syslog.Settings
{
    /// <inheritdoc cref="NotifyPropertyChanged" />
    /// <inheritdoc cref="IDisposable" />
    /// <summary>TCP configuration</summary>
    public class TcpConfig : NotifyPropertyChanged, IDisposable
    {
        private const string Localhost = "localhost";
        private const int DefaultPort = 514;
        private const int DefaultReconnectInterval = 500;
        private const int DefaultBufferSize = 4096;
        private string server;
        private int port;
        private TimeSpan reconnectInterval;
        private KeepAliveConfig keepAlive;
        private readonly PropertyChangedEventHandler keepAlivePropsChanged;
        private TlsConfig tls;
        private readonly PropertyChangedEventHandler tlsPropsChanged;
        private FramingMethod framing;
        private int dataChunkSize;

        /// <summary>The IP address or hostname of the Syslog server</summary>
        public string Server
        {
            get => server;
            set => SetProperty(ref server, value);
        }

        /// <summary>The port number the Syslog server is listening on</summary>
        public int Port
        {
            get => port;
            set => SetProperty(ref port, value);
        }

        /// <summary>The time interval, in milliseconds, after which a connection is retried</summary>
        public int ReconnectInterval
        {
            get => reconnectInterval.Milliseconds;
            set => SetProperty(ref reconnectInterval, TimeSpan.FromMilliseconds(value));
        }

        /// <summary>KeepAlive configuration</summary>
        public KeepAliveConfig KeepAlive
        {
            get => keepAlive;
            set => SetProperty(ref keepAlive, value);
        }

        /// <summary>Tls configuration</summary>
        public TlsConfig Tls
        {
            get => tls;
            set => SetProperty(ref tls, value);
        }

        /// <summary>Which framing method to use</summary>
        /// <remarks>If <see cref="Tls">is enabled</see> get will always return OctetCounting (RFC 5425)</remarks>
        public FramingMethod Framing
        {
            get => Tls.Enabled ? FramingMethod.OctetCounting : framing;
            set => SetProperty(ref framing, value);
        }

        /// <summary>The size of chunks in which data is split to be sent over the wire</summary>
        public int DataChunkSize
        {
            get => dataChunkSize;
            set => SetProperty(ref dataChunkSize, value);
        }

        /// <summary>Builds a new instance of the TcpProtocolConfig class</summary>
        public TcpConfig()
        {
            server = Localhost;
            port = DefaultPort;
            reconnectInterval = TimeSpan.FromMilliseconds(DefaultReconnectInterval);
            keepAlive = new KeepAliveConfig();
            keepAlivePropsChanged = (sender, args) => OnPropertyChanged(nameof(KeepAlive));
            keepAlive.PropertyChanged += keepAlivePropsChanged;
            Tls = new TlsConfig();
            tlsPropsChanged = (sender, args) => OnPropertyChanged(nameof(KeepAlive));
            tls.PropertyChanged += tlsPropsChanged;
            framing = FramingMethod.OctetCounting;
            dataChunkSize = DefaultBufferSize;
        }

        /// <inheritdoc />
        /// <summary>Disposes the instance</summary>
        public void Dispose()
        {
            keepAlive.PropertyChanged -= keepAlivePropsChanged;
            tls.PropertyChanged -= tlsPropsChanged;
        }
    }
}