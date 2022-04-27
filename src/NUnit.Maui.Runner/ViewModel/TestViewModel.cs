using System.Text;
using NUnit.Framework.Interfaces;
using NUnit.Runner.Extensions;

namespace NUnit.Runner.ViewModel {
    class TestViewModel : BaseViewModel {
        public TestViewModel(ITestResult result) {
            TestResult = result;
            Message = StringOrNone(result.Message);
            Output = StringOrNone(result.Output);
            StackTrace = StringOrNone(result.StackTrace);

            var builder = new StringBuilder();
            IPropertyBag props = result.Test.Properties;
            foreach (string key in props.Keys) {
                foreach (var value in props[key]) {
                    builder.AppendFormat("{0} = {1}{2}", key, value, Environment.NewLine);
                }
            }
            Properties = StringOrNone(builder.ToString());
        }

        public ITestResult TestResult { get; private set; }
        public string Message { get; private set; }
        public string Output { get; private set; }
        public string StackTrace { get; private set; }
        public string Properties { get; private set; }

        public Color Color {
            get { return TestResult.ResultState.Color(); }
        }

        private string StringOrNone(string str) {
            if (string.IsNullOrWhiteSpace(str))
                return "<none>";
            return str;
        }
    }
}
