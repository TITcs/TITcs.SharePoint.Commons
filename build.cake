var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var version = Argument<string>("version");
var projectFile = File("./src/TITcs.SharePoint.Commons/TITcs.SharePoint.Commons.csproj");
var solution = File("./src/TITcs.SharePoint.Commons.sln");

var nugetPackagesLocation = "./packages";

Task("Restore")
	.Does(() => {
		NuGetRestore(solution);
	});

Task("Clean")
	.Does(() => {
		CleanDirectories(string.Format("./src/TITcs.SharePoint.Commons/**/obj/{0}", configuration));
		CleanDirectories(string.Format("./src/TITcs.SharePoint.Commons/**/bin/{0}", configuration));
		CleanDirectory(nugetPackagesLocation);
	});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() => {
		MSBuild(solution, settings => settings.SetConfiguration(configuration));
	});

Task("Pack")
	.Does(() => {
			var nuGetPackSettings = new NuGetPackSettings {
				Id = "TITcs.SharePoint.Commons",
				Version = version,
				Title = "Utility library for common operations in SharePoint solutions.",
				Authors = new string [] { "Marcos Natan" },
				Symbols = false,
				ProjectUrl = new Uri("https://github.com/TITcs/TITcs.SharePoint.Commons"),
				LicenseUrl = new Uri("https://github.com/TITcs/TITcs.SharePoint.Commons/blob/master/LICENSE"),
				OutputDirectory = nugetPackagesLocation,
				BasePath = string.Format("./src/TITcs.SharePoint.Commons/bin/{0}", configuration),
				Properties = new Dictionary<string, string> {
					{ "Configuration", configuration }
				}
			};
			NuGetPack(projectFile, nuGetPackSettings);
		});

Task("Publish")
	.IsDependentOn("Pack")
	.Does(() => {
			var nugetKey = EnvironmentVariable("NUGET_KEY");

			NuGetPush(string.Format("{0}/TITcs.SharePoint.Commons{1}.nupkg", nugetPackagesLocation, version), new NuGetPushSettings {
					Source = "https://www.nuget.org/",
					ApiKey = nugetKey
				});
		});

RunTarget(target);
