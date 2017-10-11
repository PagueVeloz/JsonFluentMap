var target = Argument<string>("Target");
var nugetKey = Enviroment<string>("NUGET_KEY");
var version = Enviroment<string>("VERSION");

Task("pack")
    .Does(() =>
{
    var msBuildSettings = new DotNetCoreMSBuildSettings();
    msBuildSettings.SetVersion(version);

    var settings = new DotNetCorePackSettings
    {
        Configuration = "Release",
        OutputDirectory = "./artifacts/",
        MSBuildSettings = msBuildSettings
    };

    DotNetCorePack("./src/*", settings);
});

Task("publish")
    .IsDependentOn("pack")
    .Does(() =>
{
    
});

Task("travis")
    .IsDependentOn("pack")
    .Does(() =>
{
    
});
