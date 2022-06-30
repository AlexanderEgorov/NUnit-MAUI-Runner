using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Runner.Extensions;

namespace NUnit.Runner.Helpers {
    internal class ResultSummary {
        private readonly TestRunResult _results;
        
        #region Constructor

        public ResultSummary(TestRunResult results) {
            _results = results;
            Initialize();
            Summarize(results.TestResults);
            StartTime = results.StartTime;
            EndTime = results.EndTime;
        }

        #endregion

        #region Public Methods
        public IEnumerable<ITestResult> GetAllResults() {
            IEnumerable<ITestResult> Flatten(IEnumerable<ITestResult> tests) {
                foreach(var test in tests) {
                    if(!test.Test.IsSuite) yield return test;
                    foreach(var children in Flatten(test.Children))
                        yield return children;
                }
            }
            return Flatten(_results.TestResults);
        }
        public IEnumerable<ITestResult> GetFailedResults() {
            return GetAllResults().Where(x => x.ResultState.Status == TestStatus.Failed);
        }

        public static XDocument ToXml(ResultSummary x) {
            var test = new XElement("test-run");
            test.Add(new XAttribute("id", "0"));
            test.Add(new XAttribute("testcasecount", x.TestCount));
            test.Add(new XAttribute("total", x.TestCount));
            test.Add(new XAttribute("passed", x.PassCount));
            test.Add(new XAttribute("failed", x.FailureCount));
            test.Add(new XAttribute("inconclusive", x.InconclusiveCount));
            test.Add(new XAttribute("skipped", x.SkipCount));
            test.Add(new XAttribute("asserts", x.AssertCount));
            test.Add(new XAttribute("result", x.OverallResult));

            test.Add(new XAttribute("xamarin-runner-version", typeof(ResultSummary).GetTypeInfo().Assembly.GetName().Version.ToString()));

            var startTime = x._results.StartTime;
            var endTime = x._results.EndTime;
            var duration = endTime.Subtract(startTime).TotalSeconds;

            test.Add(new XAttribute("start-time", startTime.ToString("u")));
            test.Add(new XAttribute("end-time", endTime.ToString("u")));
            test.Add(new XAttribute("duration", duration.ToString("0.000000", NumberFormatInfo.InvariantInfo)));

            foreach(var result in x._results.TestResults)
                test.Add(XElement.Parse(result.ToXml(true).OuterXml));

            return new XDocument(test);
        }
        #endregion

        #region Properties

        public TestStatus OverallResult { get; private set; }

        public Color OverallResultColor {
            get { return new ResultState(OverallResult).Color(); }
        }

        public int TestCount { get; private set; }

        public int AssertCount { get; private set; }

        public int RunCount { get { return PassCount + FailureCount + ErrorCount + InconclusiveCount; } }

        public int NotRunCount {
            get { return IgnoreCount + ExplicitCount + InvalidCount + SkipCount; }
        }

        public int PassCount { get; private set; }

        public int FailureCount { get; private set; }

        public int ErrorCount { get; private set; }

        public int InconclusiveCount { get; private set; }

        public int InvalidCount { get; private set; }

        public int SkipCount { get; private set; }

        public int IgnoreCount { get; private set; }

        public int ExplicitCount { get; private set; }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public TimeSpan Duration {
            get { return EndTime.Subtract(StartTime); }
        }

        #endregion

        #region Helper Methods

        private void Initialize() {
            TestCount = 0;
            PassCount = 0;
            FailureCount = 0;
            ErrorCount = 0;
            InconclusiveCount = 0;
            SkipCount = 0;
            IgnoreCount = 0;
            ExplicitCount = 0;
            InvalidCount = 0;
            OverallResult = TestStatus.Inconclusive;
        }

        private void Summarize(IEnumerable<ITestResult> results) {
            foreach (var result in results)
                Summarize(result);
        }

        private void Summarize(ITestResult result) {
            var status = TestStatus.Inconclusive;

            if (result.Test.IsSuite) {
                foreach (ITestResult r in result.Children)
                    Summarize(r);
            }
            else {
                TestCount++;
                AssertCount += result.AssertCount;
                switch (result.ResultState.Status) {
                case TestStatus.Passed:
                    PassCount++;
                    if (status == TestStatus.Inconclusive)
                        status = TestStatus.Passed;
                    break;
                case TestStatus.Failed:
                    status = TestStatus.Failed;
                    if (result.ResultState == ResultState.Failure)
                        FailureCount++;
                    else if (result.ResultState == ResultState.NotRunnable)
                        InvalidCount++;
                    else
                        ErrorCount++;
                    break;
                case TestStatus.Skipped:
                    if (result.ResultState == ResultState.Ignored)
                        IgnoreCount++;
                    else if (result.ResultState == ResultState.Explicit)
                        ExplicitCount++;
                    else
                        SkipCount++;
                    break;
                case TestStatus.Inconclusive:
                    InconclusiveCount++;
                    break;
                }

                switch (OverallResult) {
                    case TestStatus.Inconclusive:
                        OverallResult = status;
                        break;
                    case TestStatus.Passed:
                        if (status == TestStatus.Failed)
                            OverallResult = status;
                        break;
                    case TestStatus.Skipped:
                    case TestStatus.Failed:
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}