using System.Reflection;
using NUnit.Runner.Services;

namespace NUnit.Maui.Runner;

public interface IRunnerConfiguration {
    public TestOptions ProvideOption();
    public IEnumerable<Assembly> ProvideAssemblies();
}