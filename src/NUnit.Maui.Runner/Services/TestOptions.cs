namespace NUnit.Runner.Services {
    public class TestOptions {
        const string OutputXmlReportName = "TestResults.xml";
        
        public TestOptions() {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            ResultFilePath = Path.Combine(path, OutputXmlReportName);
        }

        public bool AutoRun { get; set; }
        public bool TerminateAfterExecution { get; set; }
        public TcpWriterInfo TcpWriterParameters { get; set; }
        public bool CreateXmlResultFile { get; set; }
        public string ResultFilePath { get; set; }
        public string Filter { get; set; }
    }
}
