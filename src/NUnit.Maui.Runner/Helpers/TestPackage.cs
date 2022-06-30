using System.Reflection;
using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Runner.Helpers {
    internal class TestPackage {
        private readonly List<(Assembly, Dictionary<string,object>)> _testAssemblies = new List<(Assembly, Dictionary<string,object>)>();

        public void AddAssembly(Assembly testAssembly, Dictionary<string,object> options = null) {
            _testAssemblies.Add( (testAssembly, options) );
        }

        public async Task<TestRunResult> ExecuteTests(TestFilter filter, IProgress<double> progress) {
            var resultPackage = new TestRunResult();

            foreach (var (assembly,options) in _testAssemblies) {
                NUnitTestAssemblyRunner runner = await LoadTestAssemblyAsync(assembly, options).ConfigureAwait(false);

                int testsCount = runner.CountTestCases(filter);
                int completedTests = 0;
                double currentProgress = 0;
                var progressListener = new TestListener(() => {
                    completedTests++;
                    double _p = completedTests / (double)testsCount;
                    if(_p - currentProgress > 0.01) {
                        currentProgress = _p;
                        progress.Report(currentProgress);
                    }
                });

                ITestResult result = await Task.Run(() => runner.Run(progressListener, filter)).ConfigureAwait(false);
                resultPackage.AddResult(result);
            }
            resultPackage.CompleteTestRun();
            return resultPackage;
        }

        private static async Task<NUnitTestAssemblyRunner> LoadTestAssemblyAsync(Assembly assembly, Dictionary<string, object> options) {
            var runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());
            await Task.Run(() => runner.Load(assembly, options ?? new Dictionary<string, object>()));
            return runner;
        }

        class TestListener : ITestListener {
            Action onTestFinished;
            public TestListener(Action onTestFinished) {
                this.onTestFinished = onTestFinished;
            }
            public void SendMessage(TestMessage message) { }
            public void TestFinished(ITestResult result) {
                this.onTestFinished();
            }
            public void TestOutput(TestOutput output) { }
            public void TestStarted(ITest test) { }
        }
    }
}
