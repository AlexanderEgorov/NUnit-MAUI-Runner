using System.Collections.ObjectModel;
using NUnit.Framework.Interfaces;

namespace NUnit.Runner.ViewModel {
    class ResultsViewModel : BaseViewModel {
        public ResultsViewModel(IReadOnlyCollection<ITestResult> results, bool viewAll) {
            Results = new ObservableCollection<ResultViewModel>();
            foreach (var result in results)
                AddTestResults(result, viewAll);
        }

        public ObservableCollection<ResultViewModel> Results { get; private set; }

        private void AddTestResults(ITestResult result, bool viewAll) {
            if (result.Test.IsSuite) {
                foreach (var childResult in result.Children)
                    AddTestResults(childResult, viewAll);
            }
            else if (viewAll || result.ResultState.Status != TestStatus.Passed) {
                Results.Add(new ResultViewModel(result));
            }
        }
    }
}
