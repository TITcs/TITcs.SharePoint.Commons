var target = Argument("target", "Build");
var configuration = Argument("configuration", "release");
var solution = File("./src/TITcs.SharePoint.Commons.sln");

Task("Restore")
	.Does(() => {
		NuGetRestore(solution);
	});

Task("Clean")
	.Does(() => {
		Information("Cleaning build directories!");
		CleanDirectories(string.Format("./src/TITcs.SharePoint.Commons/**/obj/{0}", configuration));
		CleanDirectories(string.Format("./src/TITcs.SharePoint.Commons/**/bin/{0}", configuration));
		Information("Build directories cleaned successfully!");
	});

Task("Build")
	.IsDependentOn("Restore")
	.IsDependentOn("Clean")
	.Does(() => {
		DotNetBuild(solution);
		Information("Project compiled suuccessfully!");
	});

RunTarget(target);