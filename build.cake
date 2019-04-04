var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var buildNumber = EnvironmentVariable("APPVEYOR_BUILD_NUMBER", "19");
var version = string.Format("0.1.{0}", buildNumber);
var projectFile = File(".\\src\\TITcs.SharePoint.Commons\\.nuspec");
var solutionFile = File(".\\src\\TITcs.SharePoint.Commons.sln");

var nugetPackagesLocation = ".\\packages";

Task("Restore")
	.Does(() => {
		NuGetRestore(solutionFile);
	});

Task("Clean")
	.Does(() => {
		CleanDirectories(string.Format(".\\src\\TITcs.SharePoint.Commons\\**\\obj\\{0}", configuration));
		CleanDirectories(string.Format(".\\src\\TITcs.SharePoint.Commons\\**\\bin\\{0}", configuration));
		CleanDirectory(nugetPackagesLocation);
	});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() => {
		MSBuild(solutionFile, settings => settings.SetConfiguration(configuration));
	});

Task("Pack")
	.Does(() => {
			Information(string.Format("Packing version {0} of the package.", version));

			var packageId= "TITcs.SharePoint.Commons";
			var nuGetPackSettings = new NuGetPackSettings {
				Id = packageId,
				Title = packageId,
				Version = version,
				OutputDirectory = nugetPackagesLocation,
				Properties = new Dictionary<string, string> {
					{ "Configuration", configuration }
				}
			};
			NuGetPack(projectFile, nuGetPackSettings);
		});

Task("Publish")
	.IsDependentOn("Pack")
	.Does(() => {
			var nugetKey = EnvironmentVariable("NUGET_KEY", "546a65d46a5s4d6a546544345345fgdfgdfg");

			NuGetPush(string.Format("{0}\\TITcs.SharePoint.Commons.{1}.nupkg", nugetPackagesLocation, version), new NuGetPushSettings {
					Source = "https://www.nuget.org/",
					ApiKey = nugetKey
				});
		});

RunTarget(target);
