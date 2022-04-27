# NUnit 3 MAUI Runner

## Usage
Create configuration class:
```
class RunnerConfig : IRunnerConfiguration {
    public TestOptions ProvideOption() {
        return new TestOptions() {
            // Paste NUnit options here
            // AutoRun = true
        };
    }
    public IEnumerable<Assembly> ProvideAssemblies() {
        return new List<Assembly> {
            // Paste your assemblies with tests here
        };
    }
}
```

Add NUnit App and configuration class to your MauiProgram.cs:
```
var builder = MauiApp.CreateBuilder();
  builder
      .UseMauiApp<NUnit.Maui.Runner.App>()
      .Services.AddSingleton<IRunnerConfiguration, RunnerConfig>();

return builder.Build();
```

## Contributing
We love pull requests! All NUnit projects are built and maintained entirely by the community, contributions of any kind are welcome. Not sure where to start? Have a look at our [Contributor's guide](https://github.com/nunit/nunit/blob/master/CONTRIBUTING.md).

Adding something new? We suggest posting an issue first, to run your idea by the team. 

