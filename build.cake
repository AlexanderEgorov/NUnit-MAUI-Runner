#addin nuget:?package=Cake.FileHelpers&version=5.0.0
#addin nuget:?package=Cake.AppleSimulator&version=0.2.0
#load "utils.cake"

string target = Argument("target", "Build");
string configuration = Argument("configuration", "debug");
string version = Argument("release-version", "1.0");


Task("Prepare")
    .Does(() => {
        CleanDirectory("./Build");
        CleanDirectory("./Artifacts");
        CleanDirectories("./**/bin");
        CleanDirectories("./**/obj");

        var options = System.Text.RegularExpressions.RegexOptions.None;
        var pattern = "\\<version\\>.*\\</version\\>";
        FilePath[] files = FindRegexInFiles($"./**/*.*", pattern, options);
        foreach (var file in files) {
            List<string> matches = FindRegexMatchesInFile(file, pattern, options);
            foreach (var match in matches) {
                ReplaceTextInFiles(file.ToString(), match, "<version>1.0</version>");
            }
        }
    });

Task("Build")
    .IsDependentOn("Prepare")
    .Does(() => {
        DotNetBuild($"./src/framework", new DotNetBuildSettings { Configuration = configuration});
        DotNetBuild($"./src/NUnit.Maui.Runner", new DotNetBuildSettings { Configuration = configuration});
    });

//TODO
Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        var settings = new DotNetBuildSettings { Configuration = configuration, OutputDirectory = "./Artifacts" };
        //var androidBundle = "./Artifacts/NUnitTests-Signed.apk";
        var iosBundle = "./Artifacts/NUnitTests.app";

        DotNetBuild("./src/NUnitTests", settings);
        foreach (var file in System.IO.Directory.GetFiles("./Artifacts")) {
            if (!System.IO.Path.GetExtension(file).Equals(".apk"))
                DeleteFile(file);
        } 

        System.Threading.Tasks.Task.Run(() => RecieveXmlReport("./Artifacts/Runner_iOS_Test_Results.xml"));
        StartProcess("dotnet", $"xharness apple run --app {iosBundle} --target ios-simulator --output-directory ./Artifacts --device {GetSimulatorUDID()}");
    
        var options = System.Text.RegularExpressions.RegexOptions.None;
        var pattern = "(total=\"8\" passed=\"4\" failed=\"4\")";
        if (FindRegexInFiles($"./Artifacts/*.xml", pattern, options).Length != 1)
            throw new Exception("Tests failed!");

        Information("All tests passed!");
    });

Task("Nuget")
    .Does(() => {
        string key = EnvironmentVariable<string>("NUGET_API_KEY", FileReadText(".key"));
        NuGetPack("nuget/NUnit.Maui.Runner.nuspec", new NuGetPackSettings { OutputDirectory = "./Artifacts" });
        NuGetPush($"./Artifacts/NUnit.Maui.Runner.{version}.0.nupkg", new NuGetPushSettings {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = key
        });
    });

RunTarget(target);


