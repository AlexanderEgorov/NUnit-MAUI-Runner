using System.Diagnostics;
using NUnit.Runner.Helpers;

namespace NUnit.Runner.Services {
    class XmlFileProcessor : TestResultProcessor {
        public XmlFileProcessor(TestOptions options)
            : base(options) { }

        public override async Task Process(ResultSummary result) {
            if (Options.CreateXmlResultFile == false)
                return;

            try {
                await WriteXmlResultFile(result).ConfigureAwait(false);
            }
            catch (Exception) {
                Debug.WriteLine("Fatal error while trying to write xml result file!");
                throw;
            }

            if (Successor != null) {
                await Successor.Process(result).ConfigureAwait(false);
            }
        }

        async Task WriteXmlResultFile(ResultSummary result) {
            string outputFolderName = Path.GetDirectoryName(Options.ResultFilePath);

            Directory.CreateDirectory(outputFolderName);

            using (var resultFileStream = new StreamWriter(Options.ResultFilePath, false)) {
                var xml = result.GetTestXml().ToString();
                await resultFileStream.WriteAsync(xml);
            }
        }
    }
}