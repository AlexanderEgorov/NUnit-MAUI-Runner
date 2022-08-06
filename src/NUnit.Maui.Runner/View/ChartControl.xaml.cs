using NUnit.Runner.Helpers;

namespace NUnit.Runner.View;

public partial class ChartControl: Grid {
    private const int Radius = 50;
    
    public static readonly BindableProperty SeriesProperty = BindableProperty.Create(
        propertyName: nameof(Series),
        returnType: typeof(ResultSummary),
        declaringType: typeof(ChartControl));
    
    public ResultSummary Series
    {
        get => (ResultSummary)GetValue(SeriesProperty);
        private set {
            SetValue(SeriesProperty, value);
        }
    }

    public ChartControl() {
        InitializeComponent();
    }
    
    public void UpdateChart() {
        if (Series == null)
            return;
        
        double total = Series.TestCount;
        double passed = Series.PassCount / total;
        double errors = (Series.ErrorCount / total) + passed;
        double failure = (Series.FailureCount / total) + errors;
        double ignored = (Series.NotRunCount / total) + failure;

        figure1.Points = CreatePointsByPercentage(ignored);
        figure2.Points = CreatePointsByPercentage(failure);
        figure3.Points = CreatePointsByPercentage(errors);
        figure4.Points = CreatePointsByPercentage(passed);
    }
    
    private PointCollection CreatePointsByPercentage(double percentage) {
        var points = new PointCollection();
        double add = 2*Math.PI * (1-percentage);

        for (double a = -Math.PI; a <= Math.PI-add+0.1; a += 0.1) {
            var x = Radius * Math.Sin(a);
            var y = Radius * Math.Cos(a);
            points.Add(new Point(x, y));
        }

        return points;
    }
}