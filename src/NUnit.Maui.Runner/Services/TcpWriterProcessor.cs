using NUnit.Runner.Messages;
using NUnit.Runner.Helpers;

namespace NUnit.Runner.Services {
    class TcpWriterProcessor : TestResultProcessor {
        public TcpWriterProcessor(TestOptions options)
            : base(options) { }

        public override async Task Process(ResultSummary result) {
            if (Options.TcpWriterParameters != null) {
                try {
                    await WriteResult(result);
                }
                catch (Exception exception) {
                    string message = $"Fatal error while trying to send xml result by TCP to {Options.TcpWriterParameters}\n\n{exception.Message}\n\nIs your server running?";
                    MessagingCenter.Send(new ErrorMessage(message), ErrorMessage.Name);
                }
            }

            if (Successor != null) {
                await Successor.Process(result);
            }
        }

        private async Task WriteResult(ResultSummary testResult) {
            using (var tcpWriter = new TcpWriter(Options.TcpWriterParameters)) {
                await tcpWriter.Connect().ConfigureAwait(false);
                tcpWriter.Write(testResult.GetTestXml());
            }
        }
    }
}