using System.Reflection;
using NUnit.Runner.Services;
using NUnit.Runner.View;

namespace NUnit.Maui.Runner;

public static class UIHost {
    public static IView Content { get => SummaryView.Instance.GetTesUI(); set => SummaryView.Instance.SetTestUI(value); }
}