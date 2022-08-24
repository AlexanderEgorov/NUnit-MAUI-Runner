namespace NUnit.Runner.Services {
    public class TcpWriterInfo : IEquatable<TcpWriterInfo> {
        public TcpWriterInfo(string hostName, int port, int timeout = 10) {
            if (string.IsNullOrWhiteSpace(hostName)) {
                throw new ArgumentNullException(nameof(hostName));
            }

            if ((port <= 0) || (port > ushort.MaxValue)) {
                throw new ArgumentException("Must be between 1 and ushort.MaxValue", nameof(port));
            }

            if (timeout <= 0) {
                throw new ArgumentException("Must be positive", nameof(timeout));
            }

            Hostname = hostName;
            Port = port;
            Timeout = timeout;
        }

        public string Hostname { get; set; }

        public int Port { get; set; }

        public int Timeout { get; set; }

        public bool UseTunnelToDevice { get; set; }

        public bool Equals(TcpWriterInfo other) =>
            Hostname.Equals(other.Hostname, StringComparison.OrdinalIgnoreCase) && Port == other.Port;

        public override string ToString() => $"{Hostname}:{Port}";
    }
}
