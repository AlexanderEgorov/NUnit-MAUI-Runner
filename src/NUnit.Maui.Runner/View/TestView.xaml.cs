using NUnit.Runner.ViewModel;

namespace NUnit.Runner.View {
    public partial class TestView : ContentPage {
		internal TestView(TestViewModel model) {
            BindingContext = model;
            InitializeComponent();
		}
	}
}
