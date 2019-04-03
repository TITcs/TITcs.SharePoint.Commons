var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var buildNumber = EnvironmentVariable("APPVEYOR_BUILD_NUMBER");
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

Task("CopyDependencies")
	.Does(() => {
			var targetDir = string.Format("./src/TITcs.SharePoint.Commons/bin/{0}", configuration);
			var files = GetFiles("./src/packages/**/*.nupkg");

			foreach(var file in files) {

				Information(string.Format("Copying {0} to folder {1}.", file.GetFilename(), targetDir));

				CopyFile(file.FullPath, System.IO.Path.Combine(targetDir, file.GetFilename().ToString()));
			}
		});

Task("Pack")
	.IsDependentOn("CopyDependencies")
	.Does(() => {
			Information(string.Format("Packing version {0} of the package.", string.Concat("0.0.", buildNumber)));

			var nuGetPackSettings = new NuGetPackSettings {
				Id = "TITcs.SharePoint.Commons",
				Version = string.Format("0.0.{0}", buildNumber),
				Title = "Utility library for common operations in SharePoint solutions.",
				Authors = new string [] { "Marcos Natan" },
				Symbols = false,
				ProjectUrl = new Uri("https://github.com/TITcs/TITcs.SharePoint.Commons"),
				LicenseUrl = new Uri("https://github.com/TITcs/TITcs.SharePoint.Commons/blob/master/LICENSE"),
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
			var nugetKey = EnvironmentVariable("NUGET_KEY");

			NuGetPush(string.Format("{0}/TITcs.SharePoint.Commons{1}.nupkg", nugetPackagesLocation, string.Format("0.0.{0}", buildNumber)), new NuGetPushSettings {
					Source = "https://www.nuget.org/",
					ApiKey = nugetKey
				});
		});

RunTarget(target);
