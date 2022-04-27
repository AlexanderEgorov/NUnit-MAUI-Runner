using NUnit.Framework.Interfaces;

namespace NUnit.Runner.Helpers {
    internal class TestRunResult {
        private readonly List<ITestResult> _results = new List<ITestResult>();

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public TestRunResult() {
            StartTime = DateTime.Now;
        }

        public void AddResult(ITestResult result) {
            _results.Add(result);
        }

        public void CompleteTestRun() {
            EndTime = DateTime.Now;
        }

        public IReadOnlyCollection<ITestResult> TestResults => _results;
    }
}
