using System.Text;
using NUnit.Framework.Interfaces;
using NUnit.Runner.Extensions;

namespace NUnit.Runner.ViewModel {
    internal class ResultViewModel {
        public ResultViewModel(ITestResult result) {
            Result = result.ResultState.Status.ToString().ToUpperInvariant();
            Name = result.Name;
            Parent = result.Test.Parent.FullName;
            ClassName = result.Test.Parent.Name;
            Message = result.Message;
            Duration = result.Duration;
            AssertCount = result.AssertCount;
            Color = result.ResultState.Color();

            Output = StringOrNone(result.Output);
            StackTrace = StringOrNone(result.StackTrace);

            var builder = new StringBuilder();
            IPropertyBag props = result.Test.Properties;
            foreach(string key in props.Keys) {
                foreach(var value in props[key]) {
                    builder.AppendFormat("{0} = {1}{2}", key, value, Environment.NewLine);
                }
            }
            Properties = StringOrNone(builder.ToString());
        }

        public string Result { get; set; }
        public string Name { get; private set; }
        public string ClassName { get; private set; }
        public string Parent { get; private set; }
        public string Message { get; private set; }
        public double Duration { get; private set; }
        public int AssertCount { get; private set; }
        public Color Color { get; private set; }

        public string Output { get; private set; }
        public string StackTrace { get; private set; }
        public string Properties { get; private set; }

        private string StringOrNone(string str) {
            if(string.IsNullOrWhiteSpace(str))
                return "<none>";
            return str;
        }
    }
}