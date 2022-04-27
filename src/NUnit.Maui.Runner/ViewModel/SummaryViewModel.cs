using System.Reflection;
using System.Windows.Input;
using NUnit.Runner.Helpers;
using NUnit.Runner.View;

using NUnit.Runner.Services;

namespace NUnit.Runner.ViewModel {
    class SummaryViewModel : BaseViewModel {
        readonly TestPackage _testPackage;
        ResultSummary _results;
        bool _running;
        TestResultProcessor _resultProcessor;

        public SummaryViewModel() {
            _testPackage = new TestPackage();
            RunTestsCommand = new Command(async o => await ExecuteTestsAync(), o => !Running);
            ViewAllResultsCommand = new Command(
                async o => await Navigation.PushAsync(new ResultsView(new ResultsViewModel(_results.GetTestResults(), true))),
                o => !HasResults);
            ViewFailedResultsCommand = new Command(
                async o => await Navigation.PushAsync(new ResultsView(new ResultsViewModel(_results.GetTestResults(), false))),
                o => !HasResults);
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

        public void OnAppearing() {
            if(Options.AutoRun) {
                // Don't rerun if we navigate back
                Options.AutoRun = false;
                RunTestsCommand.Execute(null);
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
            }
        }

        public bool HasResults => Results != null;

        public ICommand RunTestsCommand { set; get; }
        public ICommand ViewAllResultsCommand { set; get; }
        public ICommand ViewFailedResultsCommand { set; get; }

        internal void AddTest(Assembly testAssembly, Dictionary<string, object> options = null) {
            _testPackage.AddAssembly(testAssembly, options);
        }

        async Task ExecuteTestsAync() {
            Running = true;
            Results = null;
            TestRunResult results = await _testPackage.ExecuteTests();
            ResultSummary summary = new ResultSummary(results);

            _resultProcessor = TestResultProcessor.BuildChainOfResponsability(Options);
            await _resultProcessor.Process(summary).ConfigureAwait(false);

            Device.BeginInvokeOnMainThread(() => {
                Results = summary;
                Running = false;

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
    }
}
