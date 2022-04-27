using NUnit.Runner.Helpers;

namespace NUnit.Runner.Services {
    abstract class TestResultProcessor {
        protected TestResultProcessor(TestOptions options) {
            Options = options;
        }

        protected TestOptions Options { get; private set; }

        protected TestResultProcessor Successor { get; private set; }

        public abstract Task Process(ResultSummary testResult);

        public static TestResultProcessor BuildChainOfResponsability(TestOptions options) {
            var tcpWriter = new TcpWriterProcessor(options);
            var xmlFileWriter = new XmlFileProcessor(options);

            tcpWriter.Successor = xmlFileWriter;
            return tcpWriter;
        }
    }
}
