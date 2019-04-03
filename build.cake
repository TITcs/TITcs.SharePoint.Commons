var target = Argument("target", "Build");
var configuration = Argument("configuration", "release");
var solution = File("./src/TITcs.SharePoint.Commons.sln");

Task("Restore")
	.Does(() => {
		NuGetRestore(solution);
	});

Task("Clean")
	.Does(() => {
		CleanDirectories(string.Format("./src/TITcs.SharePoint.Commons/**/obj/{0}", configuration));
		CleanDirectories(string.Format("./src/TITcs.SharePoint.Commons/**/bin/{0}", configuration));
	});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() => {
		MSBuild(solution, settings => settings.SetConfiguration(configuration));
	});

RunTarget(target);
