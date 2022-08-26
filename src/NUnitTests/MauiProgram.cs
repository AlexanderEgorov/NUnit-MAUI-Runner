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
#if ANDROID
    private const string localhost = "10.0.2.2";
#else
    private const string localhost = "127.0.0.1";
#endif
	public TestOptions ProvideOption() {
		return new TestOptions() {
			AutoRun = true,
			TerminateAfterExecution = true,
			TcpWriterParameters = new TcpWriterInfo(localhost, 13000)
		};
	}
	public IEnumerable<Assembly> ProvideAssemblies() {
		return new List<Assembly> {
			typeof(RunnerConfig).Assembly
		};
	}
}
