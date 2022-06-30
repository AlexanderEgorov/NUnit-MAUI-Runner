using NUnit.Runner.Messages;
using NUnit.Runner.ViewModel;

namespace NUnit.Runner.View {
    public partial class SummaryView : ContentPage {
        SummaryViewModel _model;

        internal SummaryView (SummaryViewModel model) {
            _model = model;
		    _model.Navigation = Navigation;
		    BindingContext = _model;
			InitializeComponent();

            MessagingCenter.Subscribe<ErrorMessage>(this, ErrorMessage.Name, error => {
                Dispatcher.Dispatch(() => errorElement.Text = error.Message);
            });
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _model.OnAppearing();
        }
    }
}
