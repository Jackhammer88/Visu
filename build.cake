var target = Argument("target", "Publish");
var configuration = Argument("configuration", "Release");
var solutionFolder = "./";
var mainProjectPath = "./Visu/Visu.csproj";
var outputFolder = "./visu-app/";

Task("Clean")
    .Does(()=> {
        CleanDirectory(outputFolder);
    });

Task("Restore")
        .Does(()=>{
            DotNetCoreRestore(solutionFolder);
        });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
        .Does(()=> {
            DotNetCoreBuild(solutionFolder, new DotNetCoreBuildSettings {
                NoRestore = true,
                Configuration = configuration
            });
        });

Task("Test")
    .IsDependentOn("Build")
        .Does(()=> {
            DotNetCoreTest(solutionFolder);
        });

Task("Publish")
    .IsDependentOn("Test")
        .Does(()=> {
            DotNetCorePublish(mainProjectPath, new DotNetCorePublishSettings {
                ArgumentCustomization = args=>args.Append("/p:DebugType=None"),
                Configuration = configuration,
                OutputDirectory = outputFolder,
                PublishReadyToRun = true,
                Runtime = "win-x64"
            });
        });

RunTarget(target);