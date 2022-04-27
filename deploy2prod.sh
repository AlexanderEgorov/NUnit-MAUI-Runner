key=$(head -n 1 .key)
package='NUnit.Maui.Runner.1.2.0.nupkg'

dotnet nuget push $package --api-key $key --source https://api.nuget.org/v3/index.json