using NUnit.Runner.ViewModel;

namespace NUnit.Runner.View {
    public partial class ResultsView : ContentPage {
		internal ResultsView (ResultsViewModel model) {
            model.Navigation = Navigation;
            BindingContext = model;
            InitializeComponent();
		}

        internal async void ViewTest(object sender, SelectedItemChangedEventArgs e) {
            var result = e.SelectedItem as ResultViewModel;
            if (result != null)
                await Navigation.PushAsync(new TestView(new TestViewModel(result.TestResult)));
        }
	}
}
