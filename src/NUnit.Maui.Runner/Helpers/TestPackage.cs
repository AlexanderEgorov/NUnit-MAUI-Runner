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

        public async Task<TestRunResult> ExecuteTests() {
            var resultPackage = new TestRunResult();

            foreach (var (assembly,options) in _testAssemblies) {
                NUnitTestAssemblyRunner runner = await LoadTestAssemblyAsync(assembly, options).ConfigureAwait(false);
                ITestResult result = await Task.Run(() => runner.Run(TestListener.NULL, TestFilter.Empty)).ConfigureAwait(false);
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
    }
}
