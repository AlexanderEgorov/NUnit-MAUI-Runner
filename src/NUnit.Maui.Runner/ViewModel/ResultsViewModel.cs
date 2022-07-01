using System.Collections.ObjectModel;
using NUnit.Framework.Interfaces;

namespace NUnit.Runner.ViewModel {
    class ResultsViewModel : BaseViewModel {
        public ResultsViewModel(IEnumerable<ITestResult> results) {
            Results = new ObservableCollection<ResultViewModel>(results.Select(x => new ResultViewModel(x)));
        }
        public ObservableCollection<ResultViewModel> Results { get; private set; }
    }
}
