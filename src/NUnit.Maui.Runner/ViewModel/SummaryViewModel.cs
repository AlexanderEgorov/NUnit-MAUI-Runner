using System.Reflection;
using System.Windows.Input;
using NUnit.Runner.Helpers;
using NUnit.Runner.View;

using NUnit.Runner.Services;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;

namespace NUnit.Runner.ViewModel {
    class SummaryViewModel : BaseViewModel {
        readonly TestPackage _testPackage;
        ResultSummary _results;
        bool _running;
        double _progress = 0;
        TestResultProcessor _resultProcessor;

        public SummaryViewModel() {
            _testPackage = new TestPackage();
            RunAllTestsCommand = new Command(
                async o => await RunAllTestsAsync(),
                o => !Running);
            RunFailedTestsCommand = new Command(
                async o => await RunFailedTestsAsync(),
                o => !Running && HasResults);
            ViewAllResultsCommand = new Command(
                async o => await Navigation.PushAsync(new ResultsView(new ResultsViewModel(_results.GetAllResults()))),
                o => !Running && HasResults);
            ViewFailedResultsCommand = new Command(
                async o => await Navigation.PushAsync(new ResultsView(new ResultsViewModel(_results.GetFailedResults()))),
                o => !Running && HasResults);
        }

        private TestOptions options;
        public TestOptions Options {
            get {
                if(options == null) {
                    options = new TestOptions();
                }
                return options;
            }
            set {
                options = value;
            }
        }
        public ResultSummary Results {
            get { return _results; }
            set {
                if (Equals(value, _results)) return;
                _results = value;
                OnPropertyChanged();
                OnPropertyChanged("HasResults");
            }
        }
        public bool Running {
            get { return _running; }
            set {
                if (value.Equals(_running)) return;
                _running = value;
                OnPropertyChanged();
                RunAllTestsCommand.ChangeCanExecute();
                RunFailedTestsCommand.ChangeCanExecute();
                ViewAllResultsCommand.ChangeCanExecute();
                ViewFailedResultsCommand.ChangeCanExecute();
            }
        }
        public bool HasResults => Results != null;
        public double Progress {
            get => _progress;
            set {
                if(_progress == value) return;
                _progress = value;
                OnPropertyChanged();
            }
        }

        public Command RunAllTestsCommand { set; get; }
        public Command RunFailedTestsCommand { set; get; }
        public Command ViewAllResultsCommand { set; get; }
        public Command ViewFailedResultsCommand { set; get; }

        public async void OnAppearing() {
            if(Options.AutoRun) {
                // Don't rerun if we navigate back
                Options.AutoRun = false;
                await RunAllTestsAsync();
                return;
            }


        }

        internal void AddTest(Assembly testAssembly, Dictionary<string, object> options = null) {
            _testPackage.AddAssembly(testAssembly, options);
        }

        async Task RunAllTestsAsync() {
            await RunTestsCoreAsync(TestFilter.Empty);
        }
        async Task RunFailedTestsAsync() {
            var set = Results.GetFailedResults().Select(x => x.FullName).ToHashSet();
            var filter = new DelegateTestFilter(x => {
                var res = set.Contains(x.FullName);
                return res;
            });
            await RunTestsCoreAsync(filter);
        }
        async Task RunTestsCoreAsync(TestFilter filter) {
            Running = true;
            Results = null;

            var progress = new Progress<double>(x => Progress = x);
            TestRunResult results = await _testPackage.ExecuteTests(filter, progress);
            ResultSummary summary = new ResultSummary(results);

            _resultProcessor = TestResultProcessor.BuildChainOfResponsability(Options);
            await _resultProcessor.Process(summary).ConfigureAwait(false);

            Device.BeginInvokeOnMainThread(() => {
                Results = summary;
                Running = false;
                Progress = 0;

                if (Options.TerminateAfterExecution)
                    TerminateWithSuccess();
            });
        }

        public static void TerminateWithSuccess()
        {
#if __IOS__
            var selector = new ObjCRuntime.Selector("terminateWithSuccess");
            UIKit.UIApplication.SharedApplication.PerformSelector(selector, UIKit.UIApplication.SharedApplication, 0);
#elif __DROID__
            System.Environment.Exit(0);
#elif WINDOWS_UWP
            Windows.UI.Xaml.Application.Current.Exit();
#endif
        }

        public class DelegateTestFilter : TestFilter {
            Func<ITest, bool> match;
            public DelegateTestFilter(Func<ITest, bool> match) {
                this.match = match;
            }
            public override bool Match(ITest test) {
                return true;
            }
            public override bool Pass(ITest test, bool negated) {
                return match(test);
            }
            public override bool IsExplicitMatch(ITest test) {
                return false;
            }
            public override TNode AddToXml(TNode parentNode, bool recursive) {
                return parentNode.AddElement("filter");
            }
        }
    }
}
