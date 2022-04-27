using NUnit.Framework.Interfaces;
using MColor = Microsoft.Maui.Graphics.Color;

namespace NUnit.Runner.Extensions {
    internal static class XamarinExtensions {
        public static Color Color(this ResultState result) {
            switch (result.Status) {
                case TestStatus.Passed:
                    return Colors.Green;
                case TestStatus.Skipped:
                    return MColor.FromRgb(206, 172, 0);    // Dark Yellow
                case TestStatus.Failed:
                    if (result == ResultState.Failure)
                        return Colors.Red;
                    if (result == ResultState.NotRunnable)
                        return MColor.FromRgb(255, 106, 0);  // Orange

                    return MColor.FromRgb(170, 0, 0); // Dark Red

                case TestStatus.Inconclusive:
                default:
                    return Colors.Gray;
            }
        }
    }
}