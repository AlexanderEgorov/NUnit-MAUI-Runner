#addin nuget:?package=Cake.FileHelpers&version=5.0.0

string target = Argument("target", "Build");
string configuration = Argument("configuration", "debug");
string root = "./src";
string version = "1.8";
List<string> projects = new List<string>() { "framework", "NUnit.Maui.Runner" };

Task("Clean")
    .Does(() => {
        CleanDirectory("./Build");
        foreach (string project in projects) {
            CleanDirectory($"{root}/{project}/bin");
            CleanDirectory($"{root}/{project}/obj");
        }
    });

Task("Build")
    .IsDependentOn("Clean")
    .Does(() => {
        // Do not refactor bottom line!
        ReplaceTextInFiles("./**/*", "{" + "PROJECT_VERSION" + "}", version);
        
        foreach (string project in projects) {
            DotNetBuild($"{root}/{project}", new DotNetBuildSettings {
                Configuration = configuration
            });
        }
    });

Task("NugetPack")
    .IsDependentOn("Build")
    .Does(() => {
        NuGetPack("nuget/NUnit.Maui.Runner.nuspec", new NuGetPackSettings());
    });

Task("NugetPush")
    .Does(() => {
        string key = FileReadText(".key");
        NuGetPush($"./NUnit.Maui.Runner.{version}.0.nupkg", new NuGetPushSettings {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = key
        });
    });

RunTarget(target);