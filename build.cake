var target = Argument<string>("Target");
var nugetKey = EnvironmentVariable<string>("NUGET_KEY");
var version = EnvironmentVariable<string>("VERSION");
var isOnTravis = EnvironmentVariable<bool>("TRAVIS", false);
var branch = EnvironmentVariable<string>("TRAVIS_BRANCH");

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
    if (isOnTravis)
    {
        Information("Build running on travis! branch: {0}", branch);
    }
});
