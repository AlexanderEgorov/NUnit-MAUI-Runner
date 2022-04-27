using NUnit.Framework.Interfaces;
using NUnit.Runner.Extensions;

namespace NUnit.Runner.ViewModel {
    internal class ResultViewModel {
        public ResultViewModel(ITestResult result) {
            TestResult = result;
            Result = result.ResultState.Status.ToString().ToUpperInvariant();
            Name = result.Name;
            Parent = result.Test.Parent.FullName;
            Message = result.Message;
        }

        public ITestResult TestResult { get; private set; }
        public string Result { get; set; }
        public string Name { get; private set; }
        public string Parent { get; private set; }
        public string Message { get; private set; }

        public Color Color {
            get { return TestResult.ResultState.Color(); }
        }
    }
}