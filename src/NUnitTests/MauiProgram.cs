using System.Reflection;
using NUnit.Maui.Runner;
using NUnit.Runner.Services;

namespace NUnitTests;

public static class MauiProgram {
	public static MauiApp CreateMauiApp() {
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<NUnit.Maui.Runner.App>();
		builder.Services.AddSingleton<IRunnerConfiguration, RunnerConfig>();
		
		return builder.Build();
	}
}

class RunnerConfig : IRunnerConfiguration {
	public TestOptions ProvideOption() {
		return new TestOptions() {
			AutoRun = true,
		};
	}
	public IEnumerable<Assembly> ProvideAssemblies() {
		return new List<Assembly> {
			typeof(RunnerConfig).Assembly
		};
	}
}
