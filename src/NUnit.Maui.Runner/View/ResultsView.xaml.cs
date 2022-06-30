using NUnit.Runner.ViewModel;

namespace NUnit.Runner.View {
    public partial class ResultsView : ContentPage {
		internal ResultsView (ResultsViewModel model) {
            model.Navigation = Navigation;
            BindingContext = model;
            InitializeComponent();
		}

        protected override void OnAppearing() {
            base.OnAppearing();
            collectionView.SelectedItem = null;
        }

        async void ViewTest(object sender, SelectionChangedEventArgs e) {
            var result = e.CurrentSelection.FirstOrDefault() as ResultViewModel;
            if (result != null)
                await Navigation.PushAsync(new TestView(result));
        }
    }
}
